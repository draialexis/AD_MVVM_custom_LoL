using Model;
using VMToolkit;

namespace ViewModel
{
    public class SkillVM : PropertyChangeNotifier
    {
        public Skill Model
        {
            get => model;
            set
            {
                if (model is not null && model.Equals(value)) return;
                model = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(Description));
            }
        }
        private Skill model;

        public SkillVM(Skill model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public string Name
        {
            get => Model.Name;
        }

        public string Type
        {
            get => Model.Type.ToString();
        }

        public string Description
        {
            get => Model.Description;
            set
            {
                if (Model == null || Model.Description.Equals(value)) return;
                Model.Description = value;
                OnPropertyChanged();
            }
        }
    }
}
