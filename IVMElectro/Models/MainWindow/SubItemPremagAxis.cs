using IVMElectro.View.PREMAG;
using IVMElectro.ViewModel.Premag;
using NLog;

namespace IVMElectro.Models.MainWindow {
    public class SubItemPremagAxis : SubItem {
        public SubItemPremagAxis(string name, Logger logger) : base(name, logger) {
        }

        public override void MakeWindowCalculation() => WindowCalculation = new AxisView {
            DataContext = new PremagAxisVM(Logger)
        };
    }
}
