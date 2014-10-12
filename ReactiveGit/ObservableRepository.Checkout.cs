using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        /// <inheritdoc />
        public IObservable<Unit> Checkout(
            Branch branch,
            IObserver<Tuple<string, int>> observer)
        {
           var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandler(observer)
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(branch, options, signature);
                SignalCompleted(observer);
            }, Scheduler.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> Checkout(
            Commit commit,
            IObserver<Tuple<string, int>> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandler(observer)
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(commit, options, signature);
                SignalCompleted(observer);
            }, Scheduler.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> Checkout(
            string commitOrBranchSpec,
            IObserver<Tuple<string, int>> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandler(observer)
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(commitOrBranchSpec, options, signature);
                SignalCompleted(observer);
            }, Scheduler.Default);
        }

        static void SignalCompleted(IObserver<Tuple<string, int>> observer)
        {
            observer.OnNext(Tuple.Create("checkout completed", 100));
            observer.OnCompleted();
        }
    }
}
