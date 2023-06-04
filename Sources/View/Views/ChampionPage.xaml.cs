using View.AppVM;
using ViewModel;

namespace View.Views
{
    public partial class ChampionPage : ContentPage
    {
        public ChampionVM ChampionVM => championVM;
        private readonly ChampionVM championVM;

        public MainAppVM MainAppVM => mainAppVM;
        private readonly MainAppVM mainAppVM;

        public ChampionPage(ChampionVM championVM, MainAppVM mainAppVM)
        {
            this.championVM = championVM;
            this.mainAppVM = mainAppVM;
            InitializeComponent();
            BindingContext = ChampionVM;
        }
    }
}
