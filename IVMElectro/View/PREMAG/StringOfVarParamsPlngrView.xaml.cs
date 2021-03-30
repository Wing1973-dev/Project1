using System.Windows;
using System.Windows.Controls;
using IVMElectro.ViewModel.Premag;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for StringOfVarParamsPlngrView.xaml
    /// </summary>
    public partial class StringOfVarParamsPlngrView : Window {
        public StringOfVarParamsPlngrView() => InitializeComponent();
        private void Exit_Click(object sender, RoutedEventArgs e) {
            switch (((Button)sender).Name) {
                case "OK":
                    if (((StringOfVarParamsPlngrVM)DataContext).CanOK()) {
                        ((StringOfVarParamsPlngrVM)DataContext).IsOK = true;
                        Close();
                    }
                    break;
                case "CANSEL":
                    ((StringOfVarParamsPlngrVM)DataContext).IsOK = false;
                    Close();
                    break;
            }
        }
    }
}
