using System.Windows;
using System.Windows.Controls;
using IVMElectro.ViewModel.Premag;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for AxisMDView.xaml
    /// </summary>
    public partial class AxisMDView : Window {
        public AxisMDView() => InitializeComponent();
        private void Exit_Click(object sender, RoutedEventArgs e) {
            switch (((Button)sender).Name) {
                case "OK":
                    if (((PremagAxisMDVM)DataContext).CanOK())
                        ((PremagAxisMDVM)DataContext).IsOK = true;
                    break;
            }
            Close();
        }
    }
}
