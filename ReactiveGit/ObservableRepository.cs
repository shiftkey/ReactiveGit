using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace ReactiveGit
{
    /// <summary>
    /// A LibGit2Sharp-based repository which encapsulates asynchronous operations
    /// </summary>
    public partial class ObservableRepository
        : IObservableRepository
    {
        readonly Repository _repository;
        readonly CredentialsHandler _credentialsHandler;

        /// <summary>
        /// Create an <c>ObservableRepository</c> from a folder on disk
        /// </summary>
        /// <param name="directory">The repository root folder</param>
        /// <remarks>
        /// Do not use this constructor if you require authenticated access to the
        /// remote repository
        /// </remarks>
        public ObservableRepository(string directory)
            : this(directory, null) { }

        /// <summary>
        /// Create an <c>ObservableRepository</c> from a folder on disk, 
        /// with a credential handler for authentication
        /// </summary>
        /// <param name="directory">The repository root folder</param>
        /// <param name="credentialsHandler">Credential provider</param>
        public ObservableRepository(string directory, CredentialsHandler credentialsHandler)
        {
            _repository = new Repository(directory);
            _credentialsHandler = credentialsHandler;
        }

        /// <inheritdoc />
        public IRepository Inner { get { return _repository; } }

        /// <summary>
        /// Dispose the resources held by the repository
        /// </summary>
        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
