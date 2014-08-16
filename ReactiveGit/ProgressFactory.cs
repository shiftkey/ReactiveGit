using System;
using LibGit2Sharp.Handlers;

namespace ReactiveGit
{
    internal static class ProgressFactory
    {
        public static CheckoutProgressHandler CreateHandler(
            IObserver<Tuple<string, int>> observer,
            int start = 0,
            int count = 100)
        {
            return (path, completedSteps, totalSteps) =>
            {
                var progress = start + (count * completedSteps) / totalSteps;
                observer.OnNext(Tuple.Create(path, progress));
            };
        }
    }
}