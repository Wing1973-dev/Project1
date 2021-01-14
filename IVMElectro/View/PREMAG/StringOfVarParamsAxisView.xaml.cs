using System.Windows;
using System.Windows.Controls;
using IVMElectro.ViewModel.Premag;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for StringOfVarParamsAxisView.xaml
    /// </summary>
    public partial class StringOfVarParamsAxisView : Window {
        public StringOfVarParamsAxisView() => InitializeComponent();
        private void Exit_Click(object sender, RoutedEventArgs e) {
            switch (((Button)sender).Name) {
                case "OK":
                    //if (((StringOfVarParamsAxisVM)DataContext).CanOK())
                    //    ((StringOfVarParamsAxisVM)DataContext).IsOK = true;
                    ((StringOfVarParamsAxisVM)DataContext).IsOK = ((StringOfVarParamsAxisVM)DataContext).CanOK();
                    break;
            }
            Close();
        }

    }
}
