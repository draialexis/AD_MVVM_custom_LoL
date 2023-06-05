namespace View.Views
{
    public partial class ChampionsPage : ContentPage
    {
        public ChampionsPage()
        {
            InitializeComponent();
            var mainAppVm = (Application.Current as App)!.MainAppVM;
            mainAppVm.Navigation = this.Navigation;
            BindingContext = mainAppVm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Champions.SelectedItem = null;
        }

    }
}
