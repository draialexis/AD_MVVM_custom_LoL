using Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VMToolkit;

namespace ViewModel
{
    public class ChampionsMgrVM : PropertyChangeNotifier
    {
        private IDataManager DataManager { get; set; }

        public ICommand LoadChampionsCommand { get; private set; }
        public ICommand InitializeCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }

        public ChampionsMgrVM(IDataManager dataManager)
        {
            DataManager = dataManager;

            LoadChampionsCommand = new Command(
                execute: async () => await LoadChampions(Index, Count),
                canExecute: () => DataManager is not null
                );

            InitializeCommand = new Command(
                execute: async () =>
                {
                    await UpdateTotalItemCount();
                    LoadChampionsCommand.Execute(null);
                },
                canExecute: () => DataManager is not null
                );

            NextPageCommand = new Command(
                execute: NextPage,
                canExecute: CanNavigateNext
                );

            PreviousPageCommand = new Command(
                execute: PreviousPage,
                canExecute: CanNavigatePrevious
                );
        }

        public int Index
        {
            get => index;
            set
            {
                if (index == value) return;
                index = value;
                OnPropertyChanged();
                (NextPageCommand as Command)?.ChangeCanExecute();
                (PreviousPageCommand as Command)?.ChangeCanExecute();
            }

        }
        private int index = 0;
        public int Count
        {
            get => count;
            set
            {
                if (count == value) return;
                count = value;
                OnPropertyChanged();
                (NextPageCommand as Command)?.ChangeCanExecute();
                (PreviousPageCommand as Command)?.ChangeCanExecute();
            }
        }
        private int count = 5;

        public ReadOnlyObservableCollection<ChampionVM> ChampionsVM
        {
            get => new(championsVM);
        }

        private readonly ObservableCollection<ChampionVM> championsVM = new();

        // TODO remember to update TotalItemCount whenever the underlying data changes (addChampion or deleteChampion)
        public int TotalItemCount
        {
            get => totalItemCount;
            set
            {
                if (totalItemCount == value) return;
                totalItemCount = value;
                OnPropertyChanged();
                (NextPageCommand as Command)?.ChangeCanExecute();
            }
        }
        private int totalItemCount;

        private async Task UpdateTotalItemCount()
        {
            TotalItemCount = await DataManager.ChampionsMgr.GetNbItems();
        }

        private async Task LoadChampions(int index, int count)
        {
            championsVM.Clear();

            foreach (var champion in await DataManager.ChampionsMgr.GetItems(index, count))
            {
                championsVM.Add(new ChampionVM(champion));
            }

            OnPropertyChanged(nameof(ChampionsVM));
        }

        private void NextPage()
        {
            Index += 1;
            LoadChampionsCommand.Execute(null);
        }

        private bool CanNavigateNext()
        {
            return ((Index + 1) * Count) < TotalItemCount;
        }

        private void PreviousPage()
        {
            Index = Math.Max(0, Index - 1);
            LoadChampionsCommand.Execute(null);
        }

        private bool CanNavigatePrevious()
        {
            return Index > 0;
        }
    }
}
