using IVMElectro.Models.Premag;
using IVMElectro.View.PREMAG;
using IVMElectro.ViewModel.Premag;
using NLog;

namespace IVMElectro.Models.MainWindow {
    public class SubItemPremagPlunger : SubItem {
        PremagCompositeModel Model { get; set; }
        public SubItemPremagPlunger(string name, Logger logger, PremagCompositeModel model) : base(name, logger) => Model = model;
        public override void MakeWindowCalculation() => WindowCalculation = new PlungerView {
            DataContext = new PremagPlungerVM(Model, Logger)
        };
    }
}
