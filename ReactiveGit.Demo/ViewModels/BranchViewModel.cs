using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class BranchViewModel : ReactiveObject
    {
        string name;
        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }
    }
}