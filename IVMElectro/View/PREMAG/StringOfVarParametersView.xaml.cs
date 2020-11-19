using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IVMElectro.ViewModel.Premag;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for StringOfVarParametersView.xaml
    /// </summary>
    public partial class StringOfVarParametersView : Window {
        public StringOfVarParametersView() {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            switch (((Button)sender).Name) {
                case "OK":
                    if (((StringOfVarParametersVM)DataContext).CanOK())
                        ((StringOfVarParametersVM)DataContext).IsOK = true;
                    break;
            }
            Close();
        }
    }
}
