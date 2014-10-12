using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class RepositoryViewModel : ReactiveObject
    {
        readonly ObservableRepository repository;

        readonly ObservableAsPropertyHelper<string> progressText;
        readonly ObservableAsPropertyHelper<int> progressValue;
        private ObservableAsPropertyHelper<bool> isExecuting;

        public RepositoryViewModel(string directory)
        {
            repository = new ObservableRepository(directory);

            // setup a global progress observer for the operations to use
            var progressObserver = new ReplaySubject<Tuple<string, int>>();
            progressText = progressObserver.Select(x => x.Item1)
                .ToProperty(this, x => x.ProgressText);
            progressValue = progressObserver.Select(x => x.Item2)
                .ToProperty(this, x => x.ProgressValue);

            Checkout = ReactiveCommand.CreateAsyncObservable(_ =>
                repository.Checkout("master", progressObserver));

            isExecuting = this.WhenAnyObservable(
                x => x.Checkout.IsExecuting)
                .ToProperty(this, x => x.IsExecuting);
        }

        public bool IsExecuting { get { return isExecuting.Value; } }

        public int ProgressValue { get { return progressValue.Value; } }

        public string ProgressText { get { return progressText.Value; } }

        public ReactiveCommand<Unit> Checkout { get; private set; }

    }
}
