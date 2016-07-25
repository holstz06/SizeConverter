using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;
using System.Windows.Input;
using System;

namespace SizeConverter
{
    [ImplementPropertyChanged]
    public class SizesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<InputSizeModel> InputModels { get; set; } = new ObservableCollection<InputSizeModel>();
        public ObservableCollection<OutputSizeModel> OutputModels { get; set; } = new ObservableCollection<OutputSizeModel>();

        public AddCommand AddCommand { get; private set; }

        public bool Panels { get; set; } = false;
        public bool Shelves { get; set; } = true;

        public SizesViewModel()
        {
            AddCommand = new AddCommand(this);
        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Adds a new input shelving size/quantity to the collection.
        /// </summary>
        /// 
        /// <param name="Size">
        ///     The size of the shelving
        /// </param>
        /// 
        /// <param name="Quantity">
        ///     The quantity of the shelving
        /// </param>
        /// 
        /// <returns>
        ///     The Input Size Model that was created
        /// </returns>
        ///////////////////////////////////////////////////////////////////////
        public InputSizeModel AddInput(double Size, int Quantity)
        {
            InputSizeModel input = new InputSizeModel()
            {
                InputSize = Size,
                InputQuantity = Quantity
            };
            input.PropertyChanged += InputSize_PropertyChanged;
            InputModels.Add(input);

            return input;

        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Finds the appropriate size for the shelving unit. (e.g. if the input shelf
        ///     size = 22, then the corrent output size would be 24)
        /// </summary>
        /// 
        /// <param name="InputSize">
        ///     The size of the shelving unit as inputed
        /// </param>
        /// 
        /// <param name="InputQuantity">
        ///     The quantity of the shelving unit as inputed
        /// </param>
        /// 
        /// <returns>
        ///     The key value pair of the size/quantity of the shelving
        /// </returns>
        ///////////////////////////////////////////////////////////////////////
        public KeyValuePair<double, int> FindSize(double InputSize, int InputQuantity)
        {
            // Selete the sizes to go off (panels of shelves)
            double[] shelvingSizes = null;
            if (Panels)
                shelvingSizes = PanelSizes;
            if (Shelves)
                shelvingSizes = ShelfSizes;

            double outputSize = 0;

            // Locate the nearest shelving size (e.g. if input size = 22, panel size = 24)
            if (shelvingSizes != null)
            {
                foreach (double shelvingSize in shelvingSizes)
                {
                    if (InputSize <= shelvingSize)
                    {
                        outputSize = shelvingSize;
                        break;
                    }
                }
            }

            return new KeyValuePair<double, int>(outputSize, InputQuantity);
        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Adds an output model to the collection
        /// </summary>
        /// 
        /// <param name="InputSize">
        ///     The shelving size that was inputed from the user (should be categorical)
        /// </param>
        /// 
        /// <param name="InputQuantity">
        ///     The quantity of shelving that is inputed from the user (should be categorical)
        /// </param>
        ///////////////////////////////////////////////////////////////////////
        public void AddOutput(double InputSize, int InputQuantity)
        {
            // Search through each of our output values and find a matching out
            // If there is a matching output, increase its quantity and indicate there
            // was a matching output
            bool outputFound = false;
            foreach (OutputSizeModel output in OutputModels)
            {
                if (output.OutputSize == InputSize)
                {
                    output.OutputQuantity += InputQuantity;
                    outputFound = true;
                }
            }

            // If there was no output found, then we need to create a new output
            if (!outputFound)
            {
                OutputSizeModel newOutput = new OutputSizeModel()
                {
                    OutputSize = InputSize,
                    OutputQuantity = InputQuantity
                };

                OutputModels.Add(newOutput);
            }
        }

        //=======================================================
        // INotifyPropertyChanged Members
        //=======================================================
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InputSize_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InputSizeModel input = (InputSizeModel)sender;
            var inputMeasurement = FindSize(input.InputSize, input.InputQuantity);

            switch (e.PropertyName)
            {
                case "InputSize":
                    AddOutput(inputMeasurement.Key, inputMeasurement.Value - input.PreviousQuantity); break;
                case "InputQuantity":
                    AddOutput(inputMeasurement.Key, inputMeasurement.Value - input.PreviousQuantity);
                    if(input.PreviousQuantity == 0)
                        AddInput(0, 0);
                    break;
            }
        }

        //=======================================================
        // Readonly lists of panel/shelf sizes
        //=======================================================
        public readonly double[] PanelSizes = { 24, 30, 36, 48, 60, 72, 84, 96 };
        public readonly double[] ShelfSizes = { 12, 18, 24, 30, 36, 42 };
    }

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
