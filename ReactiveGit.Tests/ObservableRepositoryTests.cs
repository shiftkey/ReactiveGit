using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;
using Xunit;

namespace ReactiveGit.Tests
{
    public class ObservableRepositoryTests
    {
        [Fact]
        public async Task CanPullSomeRepository()
        {
            Credentials credentials = new UsernamePasswordCredentials
            {
                Username = "shiftkey-tester",
                Password = "haha-password"
            }; 

            var repository = new ObservableRepository(
                @"C:\Users\brendanforster\Documents\GìtHūb\atom-dark-syntax",
                credentials);

            var pullObserver = Observer.Create<Tuple<string, int>>(
                next => { Console.WriteLine("progress: " + next.Item2); },
                () => { Console.WriteLine("it is done"); })
                .NotifyOn(DefaultScheduler.Instance);

            var result = await repository.Pull(pullObserver);

            Assert.NotEqual(MergeStatus.Conflicts, result.Status);
        }
    }
}
