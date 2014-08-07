using System;
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
                        // surface the progress from libgit2
                        var progress = 50 + (50 * completedSteps) / totalSteps;
                        observer.OnNext(Tuple.Create(s, progress));
                    }
                }
            };

            // this is a blocking call, hence all the ceremony before
            return Observable.Start(() => _repository.Network.Pull(signature, options))
                .Finally(() =>
                {
                    // ensure the observable signals even when the branch is up to date
                    observer.OnNext(Tuple.Create("pull completed", 100));
                    observer.OnCompleted();
                });
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
