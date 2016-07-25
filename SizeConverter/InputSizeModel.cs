using System.ComponentModel;
using PropertyChanged;

namespace SizeConverter
{
    [ImplementPropertyChanged]
    public class InputSizeModel : INotifyPropertyChanged
    {
        public double InputSize { get; set; }

        int _InputQuantity;
        public int InputQuantity
        {
            get { return _InputQuantity; }
            set
            {
                PreviousQuantity = _InputQuantity;
                _InputQuantity = value;
                OnPropertyChanged("InputQuantity");
            }
        }
        public int PreviousQuantity { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
