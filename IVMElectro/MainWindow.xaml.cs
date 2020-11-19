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
using IVMElectro.Models.Premag;
using IVMElectro.View;
using IVMElectro.ViewModel;
using LibraryAlgorithms;
using MaterialDesignThemes.Wpf;
using NLog;

namespace IVMElectro {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        static Logger logger = LogManager.GetCurrentClassLogger();
        public MainWindow() {
            
            InitializeComponent();
            Director director = new Director();



            List<SubItem> subItemsEngine = new List<SubItem> { 
                new SubItemASDN("Асинхронный двигатель насосов", logger, director.MakeModelEMotors(new AsdnSingleBuilder())),
                new SubItemASDNRED("Асинхронный двигатель насосов\r\nс шихтованным ротором", logger, director.MakeModelEMotors(new AsdnRedSingleBuilder())) };
            List<SubItem> subItemsMagnet = new List<SubItem> {
                new SubItemPremagFlat("Электромагниты постоянного тока", logger, director.MakeModelEMagnet(new PremagFlatArmBuilder())) };

            ItemMenu itemMenu1 = new ItemMenu("Расчет электродвигателей", subItemsEngine, PackIconKind.Engine);
            ItemMenu itemMenu2 = new ItemMenu("Расчет электромагнитов", subItemsMagnet, PackIconKind.Magnet);
            ItemMenu itemMenu0 = new ItemMenu("Перечень расчетов", null, PackIconKind.ClipboardTextOutline);

            Menu.Children.Add(new UserControlMenuItem(itemMenu0, this));
            Menu.Children.Add(new UserControlMenuItem(itemMenu1, this));
            Menu.Children.Add(new UserControlMenuItem(itemMenu2, this));
        }
    }
}
