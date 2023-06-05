using Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModel
{
    public class ChampionFormVM
    {
        public ChampionVM ChampionVM { get; private set; }

        public ICommand AddCharacteristicCommand { get; private set; }
        public ICommand UpdateCharacteristicCommand { get; private set; }
        public ICommand DeleteCharacteristicCommand { get; private set; }
        public ICommand UpsertIconCommand { get; private set; }
        public ICommand UpsertImageCommand { get; private set; }

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
                canExecute: (Tuple<string, int> tuple) =>
                    ChampionVM is not null && tuple is not null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item2 >= 0
                );

            UpdateCharacteristicCommand = new Command<CharacteristicVM>(
                execute: UpdateCharacteristic,
                canExecute: (CharacteristicVM characteristic) =>
                ChampionVM is not null && characteristic is not null && !string.IsNullOrWhiteSpace(characteristic.Key)
                );

            DeleteCharacteristicCommand = new Command<CharacteristicVM>(
                execute: DeleteCharacteristic,
                canExecute: (CharacteristicVM characteristic) =>
                ChampionVM is not null && characteristic is not null && !string.IsNullOrWhiteSpace(characteristic.Key)
                );

            UpsertIconCommand = new Command<byte[]>(
                execute: UpsertIcon,
                canExecute: (byte[] imageBytes) => ChampionVM is not null && imageBytes.Any()
                );

            UpsertImageCommand = new Command<byte[]>(
                execute: UpsertImage,
                canExecute: (byte[] imageBytes) => ChampionVM is not null && imageBytes.Any()
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

        public ReadOnlyCollection<string> AllClasses => allClasses;
        private static readonly ReadOnlyCollection<string> allClasses = new(Enum.GetNames(typeof(ChampionClass)).ToList());
    }
}
