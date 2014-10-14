using System;
using System.Linq;
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
        readonly ObservableAsPropertyHelper<IObservableRepository> repositoryObs;
        readonly ObservableAsPropertyHelper<bool> isCloningObs;

        public CloneRepositoryViewModel(string cloneUrl, string localDirectory)
        {
            IsEmpty = true;
            Branches = new ReactiveList<BranchViewModel>();

            // omg this hack sucks
            Progress = new ReplaySubject<Tuple<string, int>>();

            var anyProgress = this.WhenAnyObservable(x => x.Progress);
            progressText = anyProgress.Select(x => x.Item1)
                .ToProperty(this, x => x.ProgressText, scheduler: RxApp.MainThreadScheduler);
            progressValue = anyProgress.Select(x => x.Item2)
                .ToProperty(this, x => x.ProgressValue, scheduler: RxApp.MainThreadScheduler);

            Clone = ReactiveCommand.CreateAsyncObservable(_ =>
            {
                Progress = new ReplaySubject<Tuple<string, int>>();
                return ObservableRepository.Clone(cloneUrl, localDirectory, Progress);
            });

            isCloningObs = Clone.IsExecuting.ToProperty(this, x => x.IsCloning);

            Clone.Subscribe(_ => { IsEmpty = false; });

            repositoryObs = Clone.ToProperty(this, x => x.Repository);

            this.WhenAnyValue(x => x.Repository)
                .Where(x => x != null)
                .Subscribe(RefreshBranches);

            Checkout = ReactiveCommand.CreateAsyncObservable(
                this.WhenAny(x => x.SelectedBranch, x => x != null),
                _ =>
                {
                    Progress = new ReplaySubject<Tuple<string, int>>();
                    var branch = Repository.Inner.Branches[SelectedBranch.Name];
                    return Repository.Checkout(branch, Progress);
                });
            Checkout.Subscribe(_
                => RefreshBranches(Repository));
        }

        ISubject<Tuple<string, int>> progress;
        public ISubject<Tuple<string, int>> Progress
        {
            get { return progress; }
            private set { this.RaiseAndSetIfChanged(ref progress, value); }
        }

        void RefreshBranches(IObservableRepository repo)
        {
            Branches.Clear();
            foreach (var branch in repo.Inner.Branches
                .Select(x => new BranchViewModel {Name = x.Name, CanonicalName = x.CanonicalName}))
            {
                Branches.Add(branch);
            }
        }

        bool isEmpty;
        public bool IsEmpty
        {
            get { return isEmpty; }
            private set { this.RaiseAndSetIfChanged(ref isEmpty, value); }
        }

        public bool IsCloning { get { return isCloningObs.Value; } }

        public ReactiveCommand<Unit> Checkout { get; private set; }

        public IObservableRepository Repository { get { return repositoryObs.Value; } }

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
