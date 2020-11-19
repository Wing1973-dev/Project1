using IVMElectro.View;
using IVMElectro.ViewModel;
using NLog;

namespace IVMElectro.Models.MainWindow {
    public class SubItemASDNRED : SubItem {
        AsdnCompositeModel Model { get; set; }
        public SubItemASDNRED(string name, Logger logger, AsdnCompositeModel model) : base(name, logger) => Model = model;
        public override void MakeWindowCalculation() => WindowCalculation = new ASDNREDView {
            DataContext = new AsdnRedSingleViewModel(Model, Logger)
        };
    }
}
