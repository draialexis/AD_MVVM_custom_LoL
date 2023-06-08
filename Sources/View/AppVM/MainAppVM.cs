using System.Windows.Input;
using View.Views;
using ViewModel;

namespace View.AppVM
{
    public class MainAppVM
    {
        public INavigation Navigation { get; set; }

        public ChampionsMgrVM ChampionsMgrVM => (Application.Current as App)!.ChampionsMgrVM;

        public ICommand NavToSelectChampionCommand { get; private set; }
        public ICommand NavToAddChampionCommand { get; private set; }
        public ICommand NavToUpdateChampionCommand { get; private set; }
        public ICommand NavToAllChampionsAfterDeletingCommand { get; private set; }
        public ICommand NavToAllChampionsAfterUpsertingCommand { get; private set; }
        public ICommand NavToAllChampionsAfterEditingCommand { get; private set; }
        public ICommand NavBackToChampionAfterCancelingEditCommand { get; private set; }

        // Navigation is initialized in ChampionsPage.xaml.cs, which is the entrypoint for the Champions tab
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainAppVM()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            NavToSelectChampionCommand = new Command<ChampionVM>(
                execute: async (ChampionVM selectedChampion) => await NavToSelectChampion(selectedChampion),
                canExecute: (ChampionVM selectedChampion) => ChampionsMgrVM is not null && selectedChampion is not null
                );

            NavToAddChampionCommand = new Command(
                execute: async () => await NavToAddChampion(),
                canExecute: () => ChampionsMgrVM is not null
                );

            NavToUpdateChampionCommand = new Command<ChampionVM>(
                execute: async (ChampionVM championToUpdate) => await NavToUpdateChampion(championToUpdate),
                canExecute: (ChampionVM championToUpdate) => ChampionsMgrVM is not null && championToUpdate is not null
                );

            NavToAllChampionsAfterDeletingCommand = new Command<ChampionVM>(
                execute: NavToAllChampionsAfterDeleting,
                canExecute: (ChampionVM championVM) => ChampionsMgrVM is not null
                );

            NavToAllChampionsAfterUpsertingCommand = new Command<ChampionFormVM>(
                execute: NavToAllChampionsAfterUpserting,
                canExecute: (ChampionFormVM championFormVM) => ChampionsMgrVM is not null && championFormVM is not null
                );

            NavToAllChampionsAfterEditingCommand = new Command<bool>(NavToAllChampionsAfterEditing);

            NavBackToChampionAfterCancelingEditCommand = new Command(NavBackToChampionAfterCancelingEdit);
        }

        private async Task NavToSelectChampion(ChampionVM selectedChampion)
        {
            if (Navigation is null) return;
            await Navigation.PushAsync(new ChampionPage(selectedChampion, this));
        }

        private async Task NavToAddChampion()
        {
            string result =
                await (Application.Current as App)!.MainPage!
                    .DisplayPromptAsync("Nouveau champion", "Choisissons d'abord un nom :", cancel: "Annuler", accept: "Confirmer", maxLength: 255);

            if (string.IsNullOrWhiteSpace(result)) return;

            if (Navigation is null) return;
            await Navigation.PushModalAsync(new ChampionFormPage(new(null, result), this));
        }

        private async Task NavToUpdateChampion(ChampionVM championToUpdate)
        {
            if (Navigation is null) return;
            await Navigation.PushModalAsync(new ChampionFormPage(new(championToUpdate), this));
        }

        private void NavToAllChampionsAfterDeleting(ChampionVM championVM)
        {
            ChampionsMgrVM.DeleteChampionCommand.Execute(championVM);
            NavToAllChampionsAfterEditingCommand.Execute(true);
        }

        private void NavToAllChampionsAfterUpserting(ChampionFormVM championFormVM)
        {
            ChampionsMgrVM.UpsertChampionFormVMCommand.Execute(championFormVM);
            NavToAllChampionsAfterEditingCommand.Execute(false);
        }

        private async void NavToAllChampionsAfterEditing(bool didDelete)
        {
            if (Navigation is null) return;
            if (!didDelete)
            {
                await Navigation.PopModalAsync();
            }
            await Navigation.PushAsync(new ChampionsPage());
        }

        private void NavBackToChampionAfterCancelingEdit()
        {
            if (Navigation is null) return;
            Navigation.PopModalAsync();
        }
    }
}
