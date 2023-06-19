using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace ViewModel
{
    public partial class SkillVM : ObservableObject
    {
        [ObservableProperty]
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
            set => SetProperty(Model.Description, value, Model, (s, d) => s.Description = d);
        }
    }
}
