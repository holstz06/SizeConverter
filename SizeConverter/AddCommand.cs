using System;
using System.Windows.Input;

namespace SizeConverter
{
    public class AddCommand : ICommand
    {
        SizesViewModel viewModel;
        public AddCommand(SizesViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.AddInput(0, 0);
        }
    }
}
