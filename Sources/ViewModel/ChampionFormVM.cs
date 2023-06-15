using Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModel
{
    public class ChampionFormVM
    {
        public ChampionVM ChampionVM
        {
            get => championVM;
            private set
            {
                if (championVM is not null && championVM.Equals(value)) return;
                championVM = value;
                (AddCharacteristicCommand as Command)?.ChangeCanExecute();
                (UpdateCharacteristicCommand as Command)?.ChangeCanExecute();
                (DeleteCharacteristicCommand as Command)?.ChangeCanExecute();
                (UpsertIconCommand as Command)?.ChangeCanExecute();
                (UpsertImageCommand as Command)?.ChangeCanExecute();
                (AddSkillCommand as Command)?.ChangeCanExecute();
                (UpdateSkillCommand as Command)?.ChangeCanExecute();
                (DeleteSkillCommand as Command)?.ChangeCanExecute();
            }
        }
        private ChampionVM championVM;

        public ICommand AddCharacteristicCommand { get; private set; }
        public ICommand UpdateCharacteristicCommand { get; private set; }
        public ICommand DeleteCharacteristicCommand { get; private set; }
        public ICommand UpsertIconCommand { get; private set; }
        public ICommand UpsertImageCommand { get; private set; }
        public ICommand AddSkillCommand { get; private set; }
        public ICommand UpdateSkillCommand { get; private set; }
        public ICommand DeleteSkillCommand { get; private set; }

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

            AddCharacteristicCommand = new Command<Tuple<string, int>>(
                execute: AddCharacteristic,
                canExecute: tuple =>
                    ChampionVM is not null && tuple is not null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item2 >= 0
                );

            UpdateCharacteristicCommand = new Command<CharacteristicVM>(
                execute: UpdateCharacteristic,
                canExecute: characteristic =>
                    ChampionVM is not null && characteristic is not null && !string.IsNullOrWhiteSpace(characteristic.Key)
                );

            DeleteCharacteristicCommand = new Command<CharacteristicVM>(
                execute: DeleteCharacteristic,
                canExecute: characteristic =>
                    ChampionVM is not null && characteristic is not null && !string.IsNullOrWhiteSpace(characteristic.Key)
                );

            UpsertIconCommand = new Command<byte[]>(
                execute: UpsertIcon,
                canExecute: imageBytes => ChampionVM is not null && imageBytes.Any()
                );

            UpsertImageCommand = new Command<byte[]>(
                execute: UpsertImage,
                canExecute: imageBytes => ChampionVM is not null && imageBytes.Any()
                );

            AddSkillCommand = new Command<Tuple<string, string, string?>>(
                execute: tuple => AddSkill(tuple.Item1, tuple.Item2, tuple.Item3),
                canExecute: tuple =>
                    ChampionVM is not null &&
                    tuple is not null &&
                    !string.IsNullOrWhiteSpace(tuple.Item1) &&
                    !string.IsNullOrWhiteSpace(tuple.Item2)
                );

            UpdateSkillCommand = new Command<SkillVM>(
                execute: UpdateSkill,
                canExecute: skill =>
                    ChampionVM is not null && skill is not null && !string.IsNullOrWhiteSpace(skill.Type) && !string.IsNullOrWhiteSpace(skill.Description)
                );

            DeleteSkillCommand = new Command<SkillVM>(
                execute: DeleteSkill,
                    canExecute: skill => ChampionVM is not null && skill is not null
                );
        }

        private void AddCharacteristic(Tuple<string, int> tuple)
        {
            ChampionVM.AddCharacteristic(tuple);
        }

        private void UpdateCharacteristic(CharacteristicVM characteristic)
        {
            ChampionVM.UpdateCharacteristic(characteristic);
        }

        private void DeleteCharacteristic(CharacteristicVM characteristic)
        {
            ChampionVM.RemoveCharacteristic(characteristic);
        }

        private void UpsertIcon(byte[] imageBytes)
        {
            ChampionVM.Icon = Convert.ToBase64String(imageBytes);
        }

        private void UpsertImage(byte[] imageBytes)
        {
            ChampionVM.Image = Convert.ToBase64String(imageBytes);
        }

        private void AddSkill(string name, string type, string? description)
        {
            ChampionVM.AddSkill(name, type, description);
        }


        private void UpdateSkill(SkillVM skill)
        {
            ChampionVM.UpdateSkill(skill);
        }

        private void DeleteSkill(SkillVM skill)
        {
            ChampionVM.RemoveSkill(skill);
        }

        public ReadOnlyCollection<string> AllClasses => allClasses;
        private static readonly ReadOnlyCollection<string> allClasses = new(Enum.GetNames(typeof(ChampionClass)).ToList());

        public ReadOnlyCollection<string> AllSkillTypes => allTypes;
        private static readonly ReadOnlyCollection<string> allTypes = new(Enum.GetNames(typeof(SkillType)).ToList());
    }
}
