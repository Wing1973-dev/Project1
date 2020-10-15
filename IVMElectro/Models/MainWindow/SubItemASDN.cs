using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using IVMElectro.View;
using IVMElectro.ViewModel;

namespace IVMElectro.Models.MainWindow {
    public class SubItemASDN : SubItem {
        AsdnCompositeModel modelAsdn { get; set; }
        public SubItemASDN(string name, AsdnCompositeModel model) : base(name) =>  modelAsdn = model;
        
        public override void MakeWindowCalculation() => WindowCalculation = new ASDNView {
            DataContext = new AsdnSingleViewModel(modelAsdn)
        };
    }
}
