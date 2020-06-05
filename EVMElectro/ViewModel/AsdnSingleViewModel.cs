using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IVMElectro.Models;
using IVMElectro.Commands;
using System.Windows.Input;

namespace IVMElectro.ViewModel {
    class AsdnSingleViewModel : AsdnCommonViewModel {
        public AsdnSingleViewModel(AsdnCompositeModel model) : base(model) { }
        #region properties AsdnSingle
        public bool P3 { get => Model.AsdnSingle.P3; set { Model.AsdnSingle.P3 = value; OnPropertyChanged("P3"); } } //<CheckBox IsChecked="{Binding P3}">
        public double Ki { get => Model.AsdnSingle.Ki; set { Model.AsdnSingle.Ki = value; OnPropertyChanged("Ki"); } }
        public double hp { get => Model.AsdnSingle.hp; set { Model.AsdnSingle.hp = value; OnPropertyChanged("hp"); } }
        public double I1 { get => Model.AsdnSingle.I1; set { Model.AsdnSingle.I1 = value; OnPropertyChanged("I1"); } }
        public double γ { get => Model.AsdnSingle.γ; set { Model.AsdnSingle.γ = value; OnPropertyChanged("γ"); } } 
        #endregion
        #region commands
        public override void Calculation() { }
        public override bool CanCalculation() => Model.AsdnSingle != null && Model.Common.ValidHidden() && Model.AsdnSingle.ValidHidden();
        #endregion
    }
}
