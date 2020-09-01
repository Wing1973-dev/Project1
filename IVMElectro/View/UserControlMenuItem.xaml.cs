using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IVMElectro.Models.MainWindow;
//using IVMElectro.View;

namespace IVMElectro.View {
    /// <summary>
    /// Interaction logic for UserControlMenuItem.xaml
    /// </summary>
    public partial class UserControlMenuItem : UserControl {
        /// <summary>
        /// Owner window of calculation
        /// </summary>
        Window WindowOwner { get; set; }
        /// <summary>
        /// Window of calculation
        /// </summary>
        Window WindowCalculation { get; set; }
        public UserControlMenuItem(ItemMenu itemMenu, Window owner) {
            InitializeComponent();
            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
            DataContext = itemMenu; WindowOwner = owner; 
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            WindowCalculation = new ASDNView();
            WindowCalculation.Title = ((TextBlock)sender).Text;
            WindowCalculation.Owner = WindowOwner;
            WindowCalculation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            WindowCalculation.Show();
        }
    }
}
