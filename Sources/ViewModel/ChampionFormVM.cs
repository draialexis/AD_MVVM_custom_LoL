using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public partial class ChampionFormVM : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCharacteristicCommand))]
        [NotifyCanExecuteChangedFor(nameof(UpdateCharacteristicCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteCharacteristicCommand))]
        [NotifyCanExecuteChangedFor(nameof(UpsertIconCommand))]
        [NotifyCanExecuteChangedFor(nameof(UpsertImageCommand))]
        [NotifyCanExecuteChangedFor(nameof(AddSkillCommand))]
        [NotifyCanExecuteChangedFor(nameof(UpdateSkillCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteSkillCommand))]
        private ChampionVM championVM;

        public ChampionFormVM(ChampionVM? championVM, string? championName = null)
        {
            if (championVM == null)
            {
                if (string.IsNullOrWhiteSpace(championName))
                {
                    throw new ArgumentNullException(nameof(championName));
                }
                ChampionVM = new ChampionVM(new Champion(championName)) { Class = ChampionClass.Unknown.ToString() };
            }
            else
            {
                // Getting a clone, to be able to make users actively save their changes
                ChampionVM = new ChampionVM(championVM);
            }
        }

        [RelayCommand(CanExecute = nameof(CanAddCharacteristic))]
        private void AddCharacteristic(Tuple<string, int> tuple)
        {
            ChampionVM.AddCharacteristic(tuple);
        }

        private bool CanAddCharacteristic(Tuple<string, int> tuple)
        {
            return ChampionVM is not null
                && tuple is not null
                && !string.IsNullOrWhiteSpace(tuple.Item1)
                && tuple.Item2 >= 0;
        }

        [RelayCommand(CanExecute = nameof(CanFindCharacteristicInChampion))]
        private void UpdateCharacteristic(CharacteristicVM characteristic)
        {
            ChampionVM.UpdateCharacteristic(characteristic);
        }

        [RelayCommand(CanExecute = nameof(CanFindCharacteristicInChampion))]
        private void DeleteCharacteristic(CharacteristicVM characteristic)
        {
            ChampionVM.RemoveCharacteristic(characteristic);
        }

        [RelayCommand(CanExecute = nameof(CanInsertImageBytes))]
        private void UpsertIcon(byte[] imageBytes)
        {
            ChampionVM.Icon = Convert.ToBase64String(imageBytes);
        }

        [RelayCommand(CanExecute = nameof(CanInsertImageBytes))]
        private void UpsertImage(byte[] imageBytes)
        {
            ChampionVM.Image = Convert.ToBase64String(imageBytes);
        }

        [RelayCommand(CanExecute = nameof(CanAddSkill))]
        private void AddSkill(Tuple<string, string, string?> tuple)
        {
            ChampionVM.AddSkill(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        private bool CanAddSkill(Tuple<string, string, string?> tuple)
        {
            return ChampionVM is not null
                && tuple is not null
                && !string.IsNullOrWhiteSpace(tuple.Item1)
                && !string.IsNullOrWhiteSpace(tuple.Item2);
        }

        [RelayCommand(CanExecute = nameof(CanUpdateSkill))]
        private void UpdateSkill(SkillVM skill)
        {
            ChampionVM.UpdateSkill(skill);
        }

        private bool CanUpdateSkill(SkillVM skill)
        {
            return ChampionVM is not null
                && skill is not null
                && !string.IsNullOrWhiteSpace(skill.Type)
                && !string.IsNullOrWhiteSpace(skill.Description);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSkill))]
        private void DeleteSkill(SkillVM skill)
        {
            ChampionVM.RemoveSkill(skill);
        }

        private bool CanDeleteSkill(SkillVM skill)
        {
            return ChampionVM is not null && skill is not null;
        }

        private bool CanFindCharacteristicInChampion(CharacteristicVM characteristic)
        {
            return ChampionVM is not null
                && characteristic is not null
                && !string.IsNullOrWhiteSpace(characteristic.Key);
        }

        private bool CanInsertImageBytes(byte[] imageBytes)
        {
            return ChampionVM is not null && imageBytes.Any();
        }

        public ReadOnlyCollection<string> AllClasses => allClasses;
        private static readonly ReadOnlyCollection<string> allClasses = new(Enum.GetNames(typeof(ChampionClass)).ToList());

        public ReadOnlyCollection<string> AllSkillTypes => allTypes;
        private static readonly ReadOnlyCollection<string> allTypes = new(Enum.GetNames(typeof(SkillType)).ToList());
    }
}
