using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using View.Views;
using ViewModel;

namespace View.AppVM
{
    public partial class MainAppVM : ObservableObject
    {
        public INavigation Navigation { get; set; }

        public ChampionsMgrVM ChampionsMgrVM => (Application.Current as App)!.ChampionsMgrVM;

        // Navigation is initialized in ChampionsPage.xaml.cs, which is the entrypoint for the Champions tab
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainAppVM() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [RelayCommand(CanExecute = nameof(CanFindChampion))]
        private async Task NavToSelectChampion(ChampionVM selectedChampion)
        {
            if (Navigation is null) return;
            await Navigation.PushAsync(new ChampionPage(selectedChampion, this));
        }

        private bool CanFindChampion(ChampionVM champion)
        {
            return CanFindChampionMgr() && champion is not null;
        }

        [RelayCommand(CanExecute = nameof(CanFindChampionMgr))]

        private async Task NavToAddChampion()
        {
            string result =
                await (Application.Current as App)!.MainPage!
                    .DisplayPromptAsync("Nouveau champion", "Choisissons d'abord un nom :", cancel: "Annuler", accept: "Confirmer", maxLength: 255);

            if (string.IsNullOrWhiteSpace(result)) return;

            if (Navigation is null) return;
            await Navigation.PushModalAsync(new ChampionFormPage(new(null, result), this));
        }

        private bool CanFindChampionMgr()
        {
            return ChampionsMgrVM is not null;
        }

        [RelayCommand(CanExecute = nameof(CanFindChampion))]
        private async Task NavToUpdateChampion(ChampionVM championToUpdate)
        {
            if (Navigation is null) return;
            await Navigation.PushModalAsync(new ChampionFormPage(new(championToUpdate), this));
        }

        [RelayCommand(CanExecute = nameof(CanFindChampionMgr))]
        private void NavToAllChampionsAfterDeleting(ChampionVM championVM)
        {
            ChampionsMgrVM.DeleteChampionCommand.Execute(championVM);
            NavToAllChampionsAfterEditingCommand.Execute(true);
        }

        [RelayCommand(CanExecute = nameof(CanUpsertChampion))]
        private void NavToAllChampionsAfterUpserting(ChampionFormVM championFormVM)
        {
            ChampionsMgrVM.UpsertChampionFormVMCommand.Execute(championFormVM);
            NavToAllChampionsAfterEditingCommand.Execute(false);
        }

        private bool CanUpsertChampion(ChampionFormVM championFormVM)
        {
            return CanFindChampionMgr() && championFormVM is not null;
        }

        [RelayCommand]
        private async Task NavToAllChampionsAfterEditing(bool didDelete)
        {
            // https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation#relative-routes
            // navigate to ChampionsPage and start a new navigation stack from scratch
            await Shell.Current.GoToAsync("//ChampionsPage");
        }

        [RelayCommand]
        private void NavBackToChampionAfterCancelingEdit()
        {
            if (Navigation is null) return;
            Navigation.PopModalAsync();
        }
    }
}
