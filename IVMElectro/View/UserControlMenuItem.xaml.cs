using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IVMElectro.Models.MainWindow;

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
        ItemMenu ItemsMenu { get; set; }
        public UserControlMenuItem(ItemMenu itemMenu, Window owner) {
            InitializeComponent();
            ItemsMenu = itemMenu;
            ExpanderMenu.Visibility = ItemsMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = ItemsMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
            DataContext = ItemsMenu; WindowOwner = owner;
        }
        /// <summary>
        /// Generator of models and their views
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            foreach (SubItem subItem in ItemsMenu.SubItems) {
                if (((TextBlock)sender).Text == subItem.Name) {
                    subItem.MakeWindowCalculation();
                    if (subItem.WindowCalculation != null) {
                        subItem.WindowCalculation.Title = ((TextBlock)sender).Text;
                        subItem.WindowCalculation.Owner = WindowOwner;
                        subItem.WindowCalculation.Show();
                    }
                    break;
                }
            }
        }

        private void ExpanderMenu_Collapsed(object sender, RoutedEventArgs e) {
            for (int i = 0; i < ListViewMenu.ItemContainerGenerator.Items.Count; i++)
                ((ListViewItem)ListViewMenu.ItemContainerGenerator.ContainerFromIndex(i)).IsSelected = false;
        }

        private void ListViewItemMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

        }
    }
}
