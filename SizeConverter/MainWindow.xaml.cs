using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SizeConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textbox = (sender as TextBox);

            if(textbox != null)
            {
                if(!textbox.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }
        }
    }
}
