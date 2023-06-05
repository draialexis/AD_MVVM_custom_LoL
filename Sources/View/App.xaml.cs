using View.AppVM;
using ViewModel;

namespace View
{
    public partial class App : Application
    {
        public ChampionsMgrVM ChampionsMgrVM { get; set; }
        public MainAppVM MainAppVM { get; set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            MainAppVM = services.GetService<MainAppVM>()!;
            ChampionsMgrVM = services.GetService<ChampionsMgrVM>()!;
            ChampionsMgrVM.InitializeCommand.Execute(null);
            MainPage = new AppShell();
        }
    }
}
