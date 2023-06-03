using VMToolkit;

namespace ViewModel
{
    public class CharacteristicVM : PropertyChangeNotifier
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }
}
