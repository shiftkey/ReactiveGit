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

        public CloneRepositoryViewModel(string cloneUrl, string localDirectory)
        {
            var progressObserver = new ReplaySubject<Tuple<string, int>>();
            progressText = progressObserver.Select(x => x.Item1)
                .ToProperty(this, x => x.ProgressText, scheduler: RxApp.MainThreadScheduler);
            progressValue = progressObserver.Select(x => x.Item2)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.ProgressValue, scheduler: RxApp.MainThreadScheduler);

            StartClone = ReactiveCommand.CreateAsyncObservable(_ => 
                ObservableRepository.Clone(cloneUrl, localDirectory, progressObserver));
            StartClone.Subscribe(_
                => { /* clone is completed */ });

            repository = StartClone.ToProperty(this, x => x.Repository);

            Checkout = ReactiveCommand.CreateAsyncObservable(
                this.WhenAny(x => x.Repository, x => x.Value != null),
                _ => Repository.Checkout((Branch) null, progressObserver));
            Checkout.Subscribe(_
                => { /* checkout is completed */ });
        }

        public ReactiveCommand<Unit> Checkout { get; private set; }

        public IObservableRepository Repository { get { return repository.Value; } }

        public ReactiveCommand<IObservableRepository> StartClone { get; private set; }

        public int ProgressValue { get { return progressValue.Value; } }

        public string ProgressText { get { return progressText.Value; } }
    }
}
