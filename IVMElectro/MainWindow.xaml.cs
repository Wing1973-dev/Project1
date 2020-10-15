using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IVMElectro.Models;
using IVMElectro.Models.MainWindow;
using IVMElectro.View;
using IVMElectro.ViewModel;
using LibraryAlgorithm;
using MaterialDesignThemes.Wpf;

namespace IVMElectro {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Director director = new Director();


            //List<SubItem> subItemsEngine = new List<SubItem> { new SubItem("Асинхронный двигатель насосов"), 
            //    new SubItem("Асинхронный двигатель насосов\r\nс шихтованным ротором") };

            List<SubItem> subItemsEngine = new List<SubItem> { new SubItemASDN("Асинхронный двигатель насосов", director.MakeModelEM(new AsdnSingleBuilder())),
                new SubItem("Асинхронный двигатель насосов\r\nс шихтованным ротором") };

            ItemMenu itemMenu1 = new ItemMenu("Расчет электродвигателей", subItemsEngine, PackIconKind.Engine);
            List<SubItem> subItemsMagnet = new List<SubItem> { new SubItem("Электромагниты постоянного тока") };
            ItemMenu itemMenu2 = new ItemMenu("Расчет электромагнитов", subItemsMagnet, PackIconKind.Magnet);
            ItemMenu itemMenu0 = new ItemMenu("Перечень расчетов", null, PackIconKind.ClipboardTextOutline);

            Menu.Children.Add(new UserControlMenuItem(itemMenu0, this));
            Menu.Children.Add(new UserControlMenuItem(itemMenu1, this));
            Menu.Children.Add(new UserControlMenuItem(itemMenu2, this));
        }
    }
}
