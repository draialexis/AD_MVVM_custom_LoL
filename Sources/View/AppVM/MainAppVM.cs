using System.Diagnostics;
using System.Windows.Input;
using View.Views;
using ViewModel;

namespace View.AppVM
{
    public class MainAppVM
    {
        public INavigation Navigation { get; set; }

        public ChampionsMgrVM ChampionsMgrVM => (Application.Current as App).ChampionsMgrVM;

        public ICommand NavToSelectChampionCommand { get; private set; }
        public ICommand NavToAddChampionCommand { get; private set; }
        public ICommand NavToUpdateChampionCommand { get; private set; }
        public ICommand NavToAllChampionsAfterUpdatingCommand { get; private set; }
        public ICommand NavToAllChampionsCommand { get; private set; }
        public ICommand NavBackCommand { get; private set; }

        public MainAppVM()
        {
            NavToSelectChampionCommand = new Command<ChampionVM>(
                execute: NavToSelectChampion,
                canExecute: (ChampionVM selectedChampion) => ChampionsMgrVM is not null && selectedChampion is not null
                );

            NavToAddChampionCommand = new Command(
                execute: NavToAddChampion,
                canExecute: () => ChampionsMgrVM is not null
                ); // TODO[Add] when champion name is an argument, set canExecute to false when name is null or blank

            NavToUpdateChampionCommand = new Command<ChampionVM>(
                execute: NavToUpdateChampion,
                canExecute: (ChampionVM championToUpdate) => ChampionsMgrVM is not null && championToUpdate is not null
                );

            NavToAllChampionsAfterUpdatingCommand = new Command<ChampionFormVM>(
                execute: NavToAllChampionsAfterUpdating,
                canExecute: (ChampionFormVM championFormVM) => ChampionsMgrVM is not null && championFormVM is not null
                );

            NavToAllChampionsCommand = new Command(NavToAllChampions);

            NavBackCommand = new Command(NavBack);
        }

        // FIXME make all nav methods async and use async Task / await ?

        private void NavToSelectChampion(ChampionVM selectedChampion)
        {
            Navigation?.PushAsync(new ChampionPage(selectedChampion, this));
        }

        private void NavToAddChampion()
        {
            Navigation?.PushAsync(new ChampionFormPage(new(null, "TODO[Add] make this champion name into a user-chosen argument when tackling 'Add'"), ChampionsMgrVM, this));
        }

        private void NavToUpdateChampion(ChampionVM championToUpdate)
        {
            Navigation?.PushAsync(new ChampionFormPage(new(championToUpdate), ChampionsMgrVM, this));
        }

        private void NavToAllChampionsAfterUpdating(ChampionFormVM championFormVM)
        {
            ChampionsMgrVM.UpdateChampionFormVMCommand.Execute(championFormVM);
            NavToAllChampionsCommand.Execute(null);
        }

        private void NavToAllChampions()
        {
            Navigation?.PushAsync(new ChampionsPage());
        }

        private void NavBack()
        {
            Navigation?.PopAsync();
        }
    }
}
