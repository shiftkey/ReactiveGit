using System;
using System.Reactive;
using LibGit2Sharp;

namespace ReactiveGit
{
    public interface IObservableRepository : IDisposable
    {
        IObservable<Unit> Push(IObserver<Tuple<string, int>> observer);
        IObservable<MergeResult> Pull(IObserver<Tuple<string, int>> observer);

        IObservable<Unit> Checkout(
            Commit commit,
            IObserver<Tuple<string, int>> observer);
        IObservable<Unit> Checkout(
            Branch branch,
            IObserver<Tuple<string, int>> observer);
        IObservable<Unit> Checkout(
            string commitOrBranchSpec,
            IObserver<Tuple<string, int>> observer);
    }
}
