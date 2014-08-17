using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        public IObservable<MergeResult> Pull(
            IObserver<Tuple<string, int>> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);
            var isCancelled = false;

            var options = new PullOptions
            {
                FetchOptions = new FetchOptions
                {
                    TagFetchMode = TagFetchMode.None,
                    Credentials = _credentials,
                    OnTransferProgress = progress =>
                    {
                        // TODO: how should we signal for the "indexing objects" events
                        var p = (50 * progress.ReceivedObjects) / progress.TotalObjects;
                        observer.OnNext(Tuple.Create("", p));
                        return !isCancelled;
                    }
                },
                MergeOptions = new MergeOptions
                {
                    OnCheckoutProgress = ProgressFactory.CreateHandler(observer, start:50, count:50)
                }
            };

            return Observable.Create<MergeResult>(subj =>
            {
                var sub = Observable.Start(() =>
                {
                    var result = _repository.Network.Pull(signature, options);

                    observer.OnNext(Tuple.Create("pull completed", 100));
                    observer.OnCompleted();

                    return result;
                }, Scheduler.Default).Subscribe(subj);

                return new CompositeDisposable(
                    sub,
                    Disposable.Create(() =>
                    {
                        isCancelled = true;
                        observer.OnCompleted();
                    }));
            });
        }
    }
}
