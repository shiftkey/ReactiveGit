using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        public IObservable<Unit> Checkout(
            Branch branch,
            IObserver<Tuple<string, int>> observer)
        {
           var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = (path, completedSteps, totalSteps) =>
                {
                    var progress = (100 * completedSteps) / totalSteps;
                    observer.OnNext(Tuple.Create(path, progress));
                }
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(branch, options, signature);

                observer.OnNext(Tuple.Create("pull completed", 100));
                observer.OnCompleted();

            }, Scheduler.Default);
        }

        public IObservable<Unit> Checkout(
            Commit commit,
            IObserver<Tuple<string, int>> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = (path, completedSteps, totalSteps) =>
                {
                    var progress = (100 * completedSteps) / totalSteps;
                    observer.OnNext(Tuple.Create(path, progress));
                }
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(commit, options, signature);

                observer.OnNext(Tuple.Create("pull completed", 100));
                observer.OnCompleted();

            }, Scheduler.Default);
        }

        public IObservable<Unit> Checkout(
            string commitOrBranchSpec,
            IObserver<Tuple<string, int>> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = (path, completedSteps, totalSteps) =>
                {
                    var progress = (100 * completedSteps) / totalSteps;
                    observer.OnNext(Tuple.Create(path, progress));
                }
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(commitOrBranchSpec, options, signature);

                observer.OnNext(Tuple.Create("pull completed", 100));
                observer.OnCompleted();

            }, Scheduler.Default);
        }

    }
}
