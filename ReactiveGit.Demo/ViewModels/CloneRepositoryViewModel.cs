using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class CloneRepositoryViewModel : ReactiveObject
    {
        readonly ObservableAsPropertyHelper<string> progressText;
        readonly ObservableAsPropertyHelper<int> progressValue;
        readonly ObservableAsPropertyHelper<IObservableRepository> repository;

        public CloneRepositoryViewModel(string cloneUrl, string localDirectory)
        {
            // setup a global progress observer for the operations to use
            var progressObserver = new ReplaySubject<Tuple<string, int>>();
            progressText = progressObserver.Select(x => x.Item1)
                .ToProperty(this, x => x.ProgressText);
            progressValue = progressObserver.Select(x => x.Item2)
                .ToProperty(this, x => x.ProgressValue);

            StartClone = ReactiveCommand.CreateAsyncObservable(_ => 
                ObservableRepository.Clone(cloneUrl, localDirectory, progressObserver));

            repository = StartClone.ToProperty(this, x => x.Repository);
        }

        public IObservableRepository Repository { get { return repository.Value; } }

        public ReactiveCommand<IObservableRepository> StartClone { get; private set; }

        public int ProgressValue { get { return progressValue.Value; } }

        public string ProgressText { get { return progressText.Value; } }
    }
}
