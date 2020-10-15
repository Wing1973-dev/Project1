using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.ComponentModel;

namespace IVMElectro.Models.MainWindow {
    /// <summary>
    /// <c>ItemMenu</c> represents the first level menu items
    /// <list type="bullet">
    /// <item>This type contains a description of the calculation</item>
    /// </list>
    /// <list type="number">
    /// <item>Расчет электромагнитов</item>
    /// <item>Расчет электродвигателей</item>
    /// </list>
    /// </summary>
    public class ItemMenu {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="header">NameWire of element menu</param>
        /// <param name="subItems">Collection of SubItem</param>
        /// <param name="iconKind">icon</param>
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind iconKind) {
            Header = header; SubItems = subItems; IconKind = iconKind; 
        }
        public string Header { get; private set; }
        public PackIconKind IconKind { get; private set; }
        public List<SubItem> SubItems { get; private set; }
       
    }
}
