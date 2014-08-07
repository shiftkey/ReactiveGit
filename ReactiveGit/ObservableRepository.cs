using System;
using System.Reactive;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public class ObservableRepository : IDisposable
    {
        readonly Repository _repository;
        readonly Credentials _credentials;

        public ObservableRepository(string directory)
            : this(directory, null) { }

        public ObservableRepository(string directory, Credentials credentials)
        {
            _repository = new Repository(directory);
            _credentials = credentials;
        }

        public IObservable<MergeResult> Pull(
            IObserver<Tuple<string, int>> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);
            var options = new PullOptions
            {
                FetchOptions = new FetchOptions
                {
                    TagFetchMode = TagFetchMode.None,
                    Credentials = _credentials,
                    OnTransferProgress = progress =>
                    {
                        var p = (50 * progress.ReceivedObjects) / progress.TotalObjects;
                        observer.OnNext(Tuple.Create("", p));
                        return true;
                    }
                },
                MergeOptions = new MergeOptions
                {
                    OnCheckoutProgress = (s, completedSteps, totalSteps) =>
                    {
                        var progress = 50 + (50 * completedSteps) / totalSteps;
                        observer.OnNext(Tuple.Create(s, progress));
                    }
                }
            };

            return Observable.Start(() =>
            {
                var result = _repository.Network.Pull(signature, options);

                observer.OnNext(Tuple.Create("pull completed", 100));
                observer.OnCompleted();

                return result;
            });
        }

        public IObservable<Unit> Push(IObserver<Tuple<string, int>> observer)
        {
            var branch = _repository.Head;

            var options = new PushOptions
            {
                Credentials = _credentials,
                OnPushTransferProgress = (current, total, bytes) =>
                {
                    var progress = 0;
                    if (total != 0)
                    {
                        progress = 50 + (50 * current) / total;
                    }

                    observer.OnNext(Tuple.Create("", progress));

                    return true;
                }
            };

            return Observable.Start(() =>
            {
                _repository.Network.Push(branch, options);

                observer.OnNext(Tuple.Create("push completed", 100));
                observer.OnCompleted();
            });
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
