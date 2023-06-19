using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ViewModel
{
    public partial class ChampionsMgrVM : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoadChampionsCommand))]
        [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(UpsertChampionFormVMCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteChampionCommand))]
        private IDataManager dataManager;


        public ChampionsMgrVM(IDataManager dataManager)
        {
            DataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        private bool MgrIsNotNull()
        {
            return DataManager is not null && DataManager.ChampionsMgr is not null;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int index = 1;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int count = 5;

        public ReadOnlyObservableCollection<ChampionVM> ChampionsVM
        {
            get => new(championsVM);
        }
        private readonly ObservableCollection<ChampionVM> championsVM = new();

        public int TotalItemCount
        {
            get => totalItemCount;
            set
            {
                if (totalItemCount == value) return;
                if (totalItemCount == 0 && value == 1)
                {
                    Index = 1;
                    OnPropertyChanged(nameof(Index));
                }
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
                bool noRemainderOrNoItems = TotalItemCount % Count == 0 || TotalItemCount == 0;

                return nbPagesRoundedDown + (noRemainderOrNoItems ? 0 : 1);
            }
        }

        [RelayCommand(CanExecute = nameof(MgrIsNotNull))]
        private async Task Initialize()
        {
            await UpdateTotalItemCount();
            LoadChampionsCommand.Execute(null);
        }

        [RelayCommand(CanExecute = nameof(MgrIsNotNull))]
        private async Task UpdateTotalItemCount()
        {
            TotalItemCount = await DataManager.ChampionsMgr.GetNbItems();
            OnPropertyChanged(nameof(NbAvailablePages));
            if (Index > NbAvailablePages)
            {
                Index = NbAvailablePages;
            }
        }

        [RelayCommand(CanExecute = nameof(MgrIsNotNull))]
        private async Task LoadChampions()
        {
            championsVM.Clear();

            // model's collection is 0-indexed but this VM is using 1 as a starting point to accomodate any view a bit better
            foreach (
                var champion in
                from champion in await DataManager.ChampionsMgr.GetItems(Index - 1, Count, "Name")
                where champion is not null
                select champion
                )
            {
                championsVM.Add(new ChampionVM(champion));
            }

            OnPropertyChanged(nameof(ChampionsVM));
        }


        [RelayCommand(CanExecute = nameof(CanNavigateNext))]
        private void NextPage()
        {
            Index += 1;
            LoadChampionsCommand.Execute(null);
        }

        private bool CanNavigateNext()
        {
            return MgrIsNotNull() && Index < NbAvailablePages;
        }

        [RelayCommand(CanExecute = nameof(CanNavigatePrevious))]
        private void PreviousPage()
        {
            Index = Math.Max(1, Index - 1);
            LoadChampionsCommand.Execute(null);
        }

        private bool CanNavigatePrevious()
        {
            return MgrIsNotNull() && Index > 1;
        }

        [RelayCommand(CanExecute = nameof(CanUpsertChampionFormVM))]
        private async Task UpsertChampionFormVM(ChampionFormVM championFormVM)
        {
            Champion? preExistingChampion = await GetChampionByUniqueName(championFormVM.ChampionVM.Name);

            if (preExistingChampion is null)
            {
                if (await DataManager.ChampionsMgr.AddItem(championFormVM.ChampionVM.Model) is not null)
                {
                    await UpdateTotalItemCount();
                }
            }
            else
            {
                await DataManager.ChampionsMgr.UpdateItem(preExistingChampion, championFormVM.ChampionVM.Model);
            }
            LoadChampionsCommand.Execute(null);
        }

        private bool CanUpsertChampionFormVM(ChampionFormVM championFormVM)
        {
            return MgrIsNotNull() && championFormVM is not null;
        }

        [RelayCommand(CanExecute = nameof(CanDeleteChampion))]
        private async Task DeleteChampion(ChampionVM championVM)
        {
            if (await DataManager.ChampionsMgr.DeleteItem(championVM.Model))
            {
                await UpdateTotalItemCount();
                LoadChampionsCommand.Execute(null);
            }
        }

        private bool CanDeleteChampion(ChampionVM championVM)
        {
            return MgrIsNotNull() && championVM is not null;
        }

        private async Task<Champion?> GetChampionByUniqueName(string name)
        {
            if (!MgrIsNotNull() || string.IsNullOrWhiteSpace(name))
            {
                Debug.WriteLine("DataManager is null || DataManager.ChampionsMgr is null || string.IsNullOrWhiteSpace(name)");
                return null;
            }
            Champion? preExistingChampion = (await DataManager.ChampionsMgr.GetItemsByName(name, 0, 1)).FirstOrDefault();
            return preExistingChampion;
        }
    }
}
