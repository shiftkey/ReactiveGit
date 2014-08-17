using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

namespace ReactiveGit.Tests
{
    public class TestDirectory : IDisposable
    {
        public TestDirectory()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
        }

        public string Path { get; private set; }

        public static TestDirectory Create()
        {
            return new TestDirectory();
        }

        public async void Dispose()
        {
            await Observable.Start(
                () => Directory.Delete(Path, true))
            .Retry(3);
        }
    }
}
