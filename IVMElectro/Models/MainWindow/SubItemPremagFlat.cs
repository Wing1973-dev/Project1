using IVMElectro.Models.Premag;
using IVMElectro.View.PREMAG;
using IVMElectro.ViewModel.Premag;
using NLog;

namespace IVMElectro.Models.MainWindow {
    public class SubItemPremagFlat : SubItem {
        PremagCompositeModel Model { get; set; }
        public SubItemPremagFlat(string name, Logger logger, PremagCompositeModel model) : base(name, logger) => Model = model;
        public override void MakeWindowCalculation() => WindowCalculation = new FlatArmView {
            DataContext = new PremagFlatArmVM(Model, Logger)
        };
    }
}
