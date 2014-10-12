using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        /// <inheritdoc />
        public IObservable<Unit> Push(IObserver<Tuple<string, int>> observer)
        {
            var branch = _repository.Head;

            var isCancelled = false;
            var options = new PushOptions
            {
                CredentialsProvider = _credentialsHandler,
                OnPushTransferProgress = (current, total, bytes) =>
                {
                    var progress = 0;
                    if (total != 0)
                    {
                        progress = 50 + (50 * current) / total;
                    }

                    observer.OnNext(Tuple.Create("", progress));

                    return !isCancelled;
                }
            };

            return Observable.Create<Unit>(subj =>
            {
                var sub = Observable.Start(() =>
                {
                    _repository.Network.Push(branch, options);

                    observer.OnNext(Tuple.Create("push completed", 100));
                    observer.OnCompleted();
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
