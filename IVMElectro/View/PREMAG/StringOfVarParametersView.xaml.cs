using System.Windows;
using System.Windows.Controls;
using IVMElectro.ViewModel.Premag;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for StringOfVarParametersView.xaml
    /// </summary>
    public partial class StringOfVarParametersView : Window {
        public StringOfVarParametersView() => InitializeComponent();

        private void Exit_Click(object sender, RoutedEventArgs e) {
            switch (((Button)sender).Name) {
                case "OK":
                    if (((StringOfVarParamsVM)DataContext).CanOK()) {
                        ((StringOfVarParamsVM)DataContext).IsOK = true;
                        Close();
                    }
                    break;
                case "CANSEL":
                    ((StringOfVarParamsVM)DataContext).IsOK = false;
                    Close();
                    break;
            }
        }
    }
}
