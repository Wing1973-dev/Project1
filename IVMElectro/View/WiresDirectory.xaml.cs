using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IVMElectro.Services.Directories.WireDirectory;

namespace IVMElectro.View {
    /// <summary>
    /// Interaction logic for WiresDirectory.xaml
    /// </summary>
    public partial class WiresDirectory : Window {
        public WiresDirectory(List<Wire> wires) {
            InitializeComponent();
            for (int i = 0; i < mainTabControl.Items.Count; i++) 
                ((TabItem)mainTabControl.Items[i]).DataContext = wires[i];
        }
    }
}
