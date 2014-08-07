using System;
using System.Reactive;
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
                @"C:\Users\brendanforster\Documents\GìtHūb\testing-pushspecs",
                credentials);

            var progress = 0;

            var pullObserver = Observer.Create<Tuple<string, int>>(
                next =>
                {
                    progress = (next.Item2 * 2) / 3;
                });

            var pullResult = await repository.Pull(pullObserver);

            Assert.Equal(66, progress);

            Assert.NotEqual(MergeStatus.Conflicts, pullResult.Status);

            var pushObserver = Observer.Create<Tuple<string, int>>(
                next =>
                {
                    progress = 67 + (next.Item2 / 3);
                });

            await repository.Push(pushObserver);

            Assert.Equal(100, progress);
        }
    }
}
