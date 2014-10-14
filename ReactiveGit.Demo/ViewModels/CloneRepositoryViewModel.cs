using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LibGit2Sharp;
using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class CloneRepositoryViewModel : ReactiveObject
    {
        readonly ObservableAsPropertyHelper<string> progressText;
        readonly ObservableAsPropertyHelper<int> progressValue;
        readonly ObservableAsPropertyHelper<IObservableRepository> repository;
        readonly ObservableAsPropertyHelper<bool> isCloning;

        public CloneRepositoryViewModel(string cloneUrl, string localDirectory)
        {
            IsEmpty = true;
            Branches = new ReactiveList<BranchViewModel>();

            var progressObserver = new ReplaySubject<Tuple<string, int>>();
            progressText = progressObserver.Select(x => x.Item1)
                .ToProperty(this, x => x.ProgressText, scheduler: RxApp.MainThreadScheduler);
            progressValue = progressObserver.Select(x => x.Item2)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.ProgressValue, scheduler: RxApp.MainThreadScheduler);

            Clone = ReactiveCommand.CreateAsyncObservable(_ => 
                ObservableRepository.Clone(cloneUrl, localDirectory, progressObserver));
            Clone.Subscribe(_
                =>
            {
                IsEmpty = false; 
                // TODO: extract branches from underlying repository
            });

            isCloning = Clone.IsExecuting.ToProperty(this, x => x.IsCloning);

            repository = Clone.ToProperty(this, x => x.Repository);

            Checkout = ReactiveCommand.CreateAsyncObservable(
                this.WhenAny(x => x.Repository, x => x.Value != null),
                _ => Repository.Checkout((Branch) null, progressObserver));
            Checkout.Subscribe(_
                => { /* checkout is completed */ });
        }

        bool isEmpty;
        public bool IsEmpty
        {
            get { return isEmpty; }
            private set { this.RaiseAndSetIfChanged(ref isEmpty, value); }
        }

        public bool IsCloning { get { return isCloning.Value; } }

        public ReactiveCommand<Unit> Checkout { get; private set; }

        public IObservableRepository Repository { get { return repository.Value; } }

        public ReactiveCommand<IObservableRepository> Clone { get; private set; }

        public int ProgressValue { get { return progressValue.Value; } }

        public string ProgressText { get { return progressText.Value; } }

        public ReactiveList<BranchViewModel> Branches { get; private set; }

        BranchViewModel selectedBranch;
        public BranchViewModel SelectedBranch
        {
            get { return selectedBranch; }
            set { this.RaiseAndSetIfChanged(ref selectedBranch, value); }
        }
    }
}
