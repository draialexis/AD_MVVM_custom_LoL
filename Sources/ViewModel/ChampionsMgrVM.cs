using Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Xml.Linq;
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
        public ICommand AddChampionFormVMCommand { get; private set; }
        public ICommand UpdateChampionFormVMCommand { get; private set; }

        public ChampionsMgrVM(IDataManager dataManager)
        {
            DataManager = dataManager;

            LoadChampionsCommand = new Command(
                execute: async () => await LoadChampions(Index, Count),
                canExecute: () => DataManager is not null && DataManager.ChampionsMgr is not null
                );

            InitializeCommand = new Command(
                execute: async () =>
                {
                    await UpdateTotalItemCount();
                    LoadChampionsCommand.Execute(null);
                },
                canExecute: () => DataManager is not null && DataManager.ChampionsMgr is not null
                );

            NextPageCommand = new Command(
                execute: NextPage,
                canExecute: () => DataManager is not null && DataManager.ChampionsMgr is not null && CanNavigateNext()
                );

            PreviousPageCommand = new Command(
                execute: PreviousPage,
                canExecute: () => DataManager is not null && DataManager.ChampionsMgr is not null && CanNavigatePrevious()
                );

            UpdateChampionFormVMCommand = new Command<ChampionFormVM>(
                execute: async (ChampionFormVM championFormVM) => await UpdateChampion(championFormVM),
                canExecute:
                    (ChampionFormVM championFormVM)
                        => DataManager is not null && DataManager.ChampionsMgr is not null && championFormVM is not null
                );

            AddChampionFormVMCommand = new Command<ChampionFormVM>(
                execute: async (ChampionFormVM championFormVM) => await AddChampion(championFormVM),
                canExecute:
                    (ChampionFormVM newChampionFormVM)
                        => DataManager is not null && DataManager.ChampionsMgr is not null && newChampionFormVM is not null
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
        private int index = 1;
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

        // TODO[Add] TODO[Delete] remember to update TotalItemCount whenever the underlying data changes (addChampion or deleteChampion)
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

        public int NbAvailablePages => (TotalItemCount / Count) + 1;

        private async Task UpdateTotalItemCount()
        {
            TotalItemCount = await DataManager.ChampionsMgr.GetNbItems();
        }

        private async Task LoadChampions(int index, int count)
        {
            championsVM.Clear();

            // model's collection is 0-indexed but we're using 1 as a starting point to accomodate any view a bit better
            foreach (var champion in await DataManager.ChampionsMgr.GetItems(index - 1, count, "Name"))
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
            return Index < NbAvailablePages;
        }

        private void PreviousPage()
        {
            Index = Math.Max(1, Index - 1);
            LoadChampionsCommand.Execute(null);
        }

        private bool CanNavigatePrevious()
        {
            return Index > 1;
        }

        private async Task AddChampion(ChampionFormVM championFormVM)
        {

            if (!await ChampionExistsByName(championFormVM.ChampionVM.Name))
            {
                await DataManager.ChampionsMgr.AddItem(championFormVM.ChampionVM.Model);
                LoadChampionsCommand.Execute(null);
            }
            // TODO[Add] else find a way to warn the user that the name is already taken (from the VM or the view?)
        }

        private async Task UpdateChampion(ChampionFormVM newChampionFormVM)
        {
            await DataManager.ChampionsMgr.UpdateItem(
                await GetChampionByName(newChampionFormVM.ChampionVM.Name), 
                newChampionFormVM.ChampionVM.Model
                );
            LoadChampionsCommand.Execute(null);
        }

        public async Task<bool> ChampionExistsByName(string name)
        {
            return await GetChampionByName(name) is not null;
        }

        private async Task<Champion> GetChampionByName(string name)
        {
            if (DataManager is null || DataManager.ChampionsMgr is null || string.IsNullOrWhiteSpace(name))
            {
                Debug.WriteLine("DataManager is null || DataManager.ChampionsMgr is null || string.IsNullOrWhiteSpace(name)");
                return null;
            }
            Champion oldChampion = (
                await DataManager
                .ChampionsMgr
                .GetItemsByName(name, 0, 1)
                )
                .FirstOrDefault();
            return oldChampion;
        }
    }
}
