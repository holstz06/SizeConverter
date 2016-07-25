using System.ComponentModel;
using PropertyChanged;

namespace SizeConverter
{
    [ImplementPropertyChanged]
    public class OutputSizeModel : INotifyPropertyChanged
    {
        public double OutputSize { get; set; }
        public int OutputQuantity { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
