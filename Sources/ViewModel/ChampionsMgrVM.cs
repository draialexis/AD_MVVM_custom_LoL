using Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ICommand UpsertChampionFormVMCommand { get; private set; }

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

            UpsertChampionFormVMCommand = new Command<ChampionFormVM>(
                execute: async (ChampionFormVM championFormVM) => await UpsertChampion(championFormVM),
                canExecute:
                    (ChampionFormVM championFormVM)
                        => DataManager is not null && DataManager.ChampionsMgr is not null && championFormVM is not null
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

        // TODO[Delete] remember to update TotalItemCount whenever the underlying data changes (addChampion or deleteChampion)
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

        public int NbAvailablePages
        {
            get
            {
                int nbPagesRoundedDown = TotalItemCount / Count;
                bool isCleanDivision = TotalItemCount % Count == 0 || TotalItemCount == 0;

                return nbPagesRoundedDown + (isCleanDivision ? 0 : 1);
            }
        }

        private async Task UpdateTotalItemCount()
        {
            TotalItemCount = await DataManager.ChampionsMgr.GetNbItems();
        }

        private async Task LoadChampions(int index, int count)
        {
            championsVM.Clear();

            // model's collection is 0-indexed but this VM is using 1 as a starting point to accomodate any view a bit better
            foreach (
                var champion in
                from champion in await DataManager.ChampionsMgr.GetItems(index - 1, count, "Name")
                where champion is not null
                select champion
                )
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

        private async Task UpsertChampion(ChampionFormVM championFormVM)
        {
            Champion? preExistingChampion = await GetChampionByUniqueName(championFormVM.ChampionVM.Name);

            if (preExistingChampion is null)
            {
                await DataManager.ChampionsMgr.AddItem(championFormVM.ChampionVM.Model);
                await UpdateTotalItemCount();
            }
            else
            {
                await DataManager.ChampionsMgr.UpdateItem(preExistingChampion, championFormVM.ChampionVM.Model);
            }
            LoadChampionsCommand.Execute(null);
        }

        private async Task<Champion?> GetChampionByUniqueName(string name)
        {
            if (DataManager is null || DataManager.ChampionsMgr is null || string.IsNullOrWhiteSpace(name))
            {
                Debug.WriteLine("DataManager is null || DataManager.ChampionsMgr is null || string.IsNullOrWhiteSpace(name)");
                return null;
            }
            Champion? preExistingChampion = (await DataManager.ChampionsMgr.GetItemsByName(name, 0, 1)).FirstOrDefault();
            return preExistingChampion;
        }
    }
}
