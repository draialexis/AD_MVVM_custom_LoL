using ViewModel;

namespace View.Views
{
    public partial class ChampionPage : ContentPage
    {
        public ChampionVM ChampionVM => championVM;
        private readonly ChampionVM championVM;

        public ChampionPage(ChampionVM championVM)
        {
            this.championVM = championVM;
            InitializeComponent();
            BindingContext = ChampionVM;
        }

        private void OnUpdateChampionRequested(object sender, EventArgs e)
        {
            Navigation?.PushAsync(new ChampionFormPage(ChampionVM));
        }
    }
}
