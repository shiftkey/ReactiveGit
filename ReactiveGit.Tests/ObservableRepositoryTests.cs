using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Xunit;

namespace ReactiveGit.Tests
{
    public class ObservableRepositoryTests
    {
        [Fact]
        public async Task CanCloneARepository()
        {
            using (var directory = TestDirectory.Create())
            {
                var cloneObserver = new ReplaySubject<Tuple<string, int>>();
                using (await ObservableRepository.Clone(
                    "https://github.com/shiftkey/rxui-design-guidelines.git",
                    directory.Path,
                    cloneObserver))
                {
                    Assert.NotEmpty(Directory.GetFiles(directory.Path));
                    var progressList = await cloneObserver.Select(x => x.Item2).ToList();
                    Assert.Equal(100, progressList.Last());
                }
            }
        }

        [Fact]
        public async Task GetProgressFromASyncOperation()
        {
            CredentialsHandler credentials = (url, usernameFromUrl, types) =>
                new UsernamePasswordCredentials
                {
                    Username = "shiftkey-tester",
                    Password = "haha-password"
                };

            var repository = new ObservableRepository(
                @"C:\Users\brendanforster\Documents\GìtHūb\testing-pushspecs",
                credentials);

            Func<int, int> translate = x => x / 3;

            var pullObserver = new ReplaySubject<Tuple<string, int>>();
            var pushObserver = new ReplaySubject<Tuple<string, int>>();

            var pullResult = await repository.Pull(pullObserver);

            Assert.NotEqual(MergeStatus.Conflicts, pullResult.Status);

            await repository.Push(pushObserver);

            var list = await pullObserver.Select(x => translate(x.Item2) * 2)
                                 .Concat(pushObserver.Select(x => 67 + translate(x.Item2)))
                                 .ToList();

            Assert.NotEmpty(list);
            Assert.Equal(100, list.Last());
        }
    }
}
