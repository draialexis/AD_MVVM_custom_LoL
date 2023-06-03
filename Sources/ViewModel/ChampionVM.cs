using Model;
using VMToolkit;

namespace ViewModel
{
    public class ChampionVM : PropertyChangeNotifier
    {

        public Champion Model
        {
            get => model;
            set
            {
                if (model is not null && model.Equals(value)) return;
                model = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Icon));
                OnPropertyChanged(nameof(Bio));
                OnPropertyChanged(nameof(Class));
                OnPropertyChanged(nameof(Image));
            }
        }
        private Champion model;

        public ChampionVM(Champion model)
        {
            Model = model;
        }

        public string Name
        {
            get => Model?.Name;
        }

        public string Icon
        {
            get => Model?.Icon;
            set
            {
                if (Model == null || Model.Icon.Equals(value)) return;
                Model.Icon = value;
                OnPropertyChanged();
            }
        }

        public string Bio
        {
            get => Model?.Bio;
            set
            {
                if (Model == null || Model.Bio.Equals(value)) return;
                Model.Bio = value;
                OnPropertyChanged();
            }
        }

        public string Class
        {
            get => Model?.Class.ToString();
        }

        public string Image
        {
            get => Model?.Image.Base64;
            set
            {
                if (Model == null || Model.Image.Base64.Equals(value)) return;
                Model.Image.Base64 = value;
                OnPropertyChanged();
            }
        }

        public List<CharacteristicVM> Characteristics
        {
            get
            {
                if (Model?.Characteristics == null) return new();

                List<CharacteristicVM> characteristics = new();

                foreach (var characteristic in Model.Characteristics)
                {
                    characteristics.Add(new CharacteristicVM { Key = characteristic.Key, Value = characteristic.Value });
                }

                return characteristics;
            }
            // TODO set?
        }

    }
}
