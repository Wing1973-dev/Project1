using IVMElectro.View;
using IVMElectro.ViewModel;
using NLog;

namespace IVMElectro.Models.MainWindow {
    public class SubItemASDN : SubItem {
        AsdnCompositeModel modelAsdn { get; set; }
        Logger Logger { get; set; }
        public SubItemASDN(string name, AsdnCompositeModel model, Logger logger) : base(name) { 
            modelAsdn = model; Logger = logger;
        }
        
        public override void MakeWindowCalculation() => WindowCalculation = new ASDNView {
            DataContext = new AsdnSingleViewModel(modelAsdn, Logger)
        };
    }
}
