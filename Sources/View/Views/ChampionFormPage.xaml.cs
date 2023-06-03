using ViewModel;

namespace View.Views
{
    public partial class ChampionFormPage : ContentPage
    {
        public ChampionVM? ChampionVM => championVM;
        private readonly ChampionVM? championVM;

        public ChampionFormPage(ChampionVM? championVM)
        {
            this.championVM = championVM;
            InitializeComponent();
            BindingContext = ChampionVM;
        }
    }
}
