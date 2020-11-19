using IVMElectro.View;
using IVMElectro.ViewModel;
using NLog;

namespace IVMElectro.Models.MainWindow {
    public class SubItemASDN : SubItem {
        AsdnCompositeModel Model { get; set; }
        public SubItemASDN(string name, Logger logger, AsdnCompositeModel model) : base(name, logger) => Model = model;
        public override void MakeWindowCalculation() => WindowCalculation = new ASDNView {
            DataContext = new AsdnSingleViewModel(Model, Logger)
        };
    }
}
