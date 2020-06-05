using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IVMElectro.Models;
using IVMElectro.Commands;
using System.Windows.Input;

namespace IVMElectro.ViewModel {
    class AsdnRedSingleViewModel : AsdnCommonViewModel {
        public AsdnRedSingleViewModel(AsdnCompositeModel model) : base(model) { }
        #region properties AsdnRedSingle
        public double dкп { get => Model.AsdnRedSingle.dкп; set { Model.AsdnRedSingle.dкп = value; OnPropertyChanged("dкп"); } }
        public double dпв { get => Model.AsdnRedSingle.dпв; set { Model.AsdnRedSingle.dпв = value; OnPropertyChanged("dпв"); } }
        public double dпн { get => Model.AsdnRedSingle.dпн; set { Model.AsdnRedSingle.dпн = value; OnPropertyChanged("dпн"); } }
        public double hр1 { get => Model.AsdnRedSingle.hр1; set { Model.AsdnRedSingle.hр1 = value; OnPropertyChanged("hр1"); } }
        public double hр2 { get => Model.AsdnRedSingle.hр2; set { Model.AsdnRedSingle.hр2 = value; OnPropertyChanged("hр2"); } }
        public double hш { get => Model.AsdnRedSingle.hш; set { Model.AsdnRedSingle.hш = value; OnPropertyChanged("hш"); } }
        public double bш { get => Model.AsdnRedSingle.bш; set { Model.AsdnRedSingle.bш = value; OnPropertyChanged("bш"); } }
        public double aкн { get => Model.AsdnRedSingle.aкн; set { Model.AsdnRedSingle.aкн = value; OnPropertyChanged("aкн"); } }
        public double bкн { get => Model.AsdnRedSingle.bкн; set { Model.AsdnRedSingle.bкн = value; OnPropertyChanged("bкн"); } }
        #endregion
        #region command
        public override void Calculation() { }
        public override bool CanCalculation() => Model.AsdnRedSingle != null && Model.AsdnRedSingle.ValidHidden() && Model.Common.ValidHidden();
        #endregion
    }
}
