using System;
using System.Windows.Input;

namespace SizeConverter
{
    public class RemoveCommand : ICommand
    {
        SizesViewModel viewModel;
        public RemoveCommand(SizesViewModel viewModel)
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
            if (viewModel.InputModels.Count > 0)
                viewModel.Remove();
        }
    }
}
