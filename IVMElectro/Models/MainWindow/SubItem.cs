using System.Windows;

namespace IVMElectro.Models.MainWindow {
    /// <summary>
    /// <c>SubItem</c> represents the second level menu items
    /// <list type="bullet">
    /// <item>This type contains a description of the sub-calculation</item>
    /// </list>
    /// <list type="number">
    /// <item>ASDN</item>
    /// <item>ASDNRED</item>
    /// <item>PREMAG</item>
    /// </list>
    /// </summary>
    public class SubItem {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">This is name of the sub-calculation</param>
        public SubItem(string name) => Name = name;
        public string Name { get; private set; }
        public Window WindowCalculation { get; set; }
        public virtual void MakeWindowCalculation() => WindowCalculation = null; 
    }
}
