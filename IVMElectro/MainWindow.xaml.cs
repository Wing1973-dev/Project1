using System.Collections.Generic;
using System.Windows;
using IVMElectro.Models;
using IVMElectro.Models.MainWindow;
using IVMElectro.Models.Premag;
using IVMElectro.View;
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
                new SubItemASDNRED("Асинхронный двигатель насосов\r\n с шихтованным ротором", logger, director.MakeModelEMotors(new AsdnRedSingleBuilder())) };
            List<SubItem> subItemsMagnet = new List<SubItem> {
                new SubItemPremagFlat("Электромагнит постоянного тока\r\n с плоским якорем", logger, director.MakeModelEMagnet(new PremagFlatArmBuilder())),
                new SubItemPremagPlunger("Электромагнит постоянного тока\r\n с плунжером", logger, director.MakeModelEMagnet(new PremagPlungerBuilder())),
                new SubItemPremagAxis("Электромагнит осевого\r\n электромагнитного подшипника", logger)
            };

            ItemMenu itemMenu1 = new ItemMenu("Расчет электродвигателей", subItemsEngine, PackIconKind.Engine);
            ItemMenu itemMenu2 = new ItemMenu("Расчет электромагнитов", subItemsMagnet, PackIconKind.Magnet);
            ItemMenu itemMenu0 = new ItemMenu("Перечень расчетов", null, PackIconKind.ClipboardTextOutline);

            Menu.Children.Add(new UserControlMenuItem(itemMenu0, this));
            Menu.Children.Add(new UserControlMenuItem(itemMenu1, this));
            Menu.Children.Add(new UserControlMenuItem(itemMenu2, this));
        }
    }
}
