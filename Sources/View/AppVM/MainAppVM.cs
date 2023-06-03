using System.Windows.Input;
using View.Views;
using ViewModel;

namespace View.AppVM
{
    public class MainAppVM
    {
        public INavigation Navigation { get; set; }

        public ChampionsMgrVM ChampionsMgrVM => (Application.Current as App).ChampionsMgrVM;

        public ICommand SelectChampionCommand { get; private set; }

        public MainAppVM()
        {
            SelectChampionCommand = new Command<ChampionVM>(
                execute: OnChampionSelected,
                canExecute: (ChampionVM selectedChampion) => ChampionsMgrVM is not null && selectedChampion is not null
                );
        }

        private void OnChampionSelected(ChampionVM selectedChampion)
        {
            Navigation?.PushAsync(new ChampionPage(selectedChampion));
        }
    }

}
