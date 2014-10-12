using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace ReactiveGit
{
    public partial class ObservableRepository
        : IObservableRepository
    {
        readonly Repository _repository;
        readonly CredentialsHandler _credentialsHandler;

        public ObservableRepository(string directory)
            : this(directory, null) { }

        public ObservableRepository(string directory, CredentialsHandler credentialsHandler)
        {
            _repository = new Repository(directory);
            _credentialsHandler = credentialsHandler;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
