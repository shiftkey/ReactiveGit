using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
        : IObservableRepository
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

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
