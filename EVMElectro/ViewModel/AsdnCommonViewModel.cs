using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IVMElectro.Models;
using IVMElectro.Commands;
using System.Windows.Input;

namespace IVMElectro.ViewModel {
    abstract class AsdnCommonViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        public AsdnCommonViewModel(AsdnCompositeModel model) => Model = model; 
        #region properties
        public AsdnCompositeModel Model { get; set; }
        #region Common
        public double ΔГ1 { get => Model.Common.ΔГ1; set { Model.Common.ΔГ1 = value; OnPropertyChanged("ΔГ1"); } }
        public double ΔГ2 { get => Model.Common.ΔГ2; set { Model.Common.ΔГ2 = value; OnPropertyChanged("ΔГ2"); } }
        public double Di { get => Model.Common.Di; set { Model.Common.Di = value; OnPropertyChanged("Di"); } }
        public double Dp { get => Model.Common.Dp; set { Model.Common.Dp = value; OnPropertyChanged("Dp"); } }
        public double Da { get => Model.Common.Da; set { Model.Common.Da = value; OnPropertyChanged("Da"); } }
        public double Z1 { get => Model.Common.Z1; set { Model.Common.Z1 = value; OnPropertyChanged("Z1"); } }
        public double Z2 { get => Model.Common.Z2; set { Model.Common.Z2 = value; OnPropertyChanged("Z2"); } }
        public int p { get => Model.Common.p; set { Model.Common.p = value; OnPropertyChanged("p"); } }
        public double Pмех { get => Model.Common.Pмех; set { Model.Common.Pмех = value; OnPropertyChanged("Pмех"); } }
        public double Pʹ2 { get => Model.Common.P12; set { Model.Common.P12 = value; OnPropertyChanged("Pʹ2"); } }
        public double p10_50 { get => Model.Common.p10_50; set { Model.Common.p10_50 = value; OnPropertyChanged("p10_50"); } }
        public double f1 { get => Model.Common.f1; set { Model.Common.f1 = value; OnPropertyChanged("f1"); } }
        public double dиз { get => Model.Common.dиз; set { Model.Common.dиз = value; OnPropertyChanged("dиз"); } }
        public double a1 { get => Model.Common.a1; set { Model.Common.a1 = value; OnPropertyChanged("a1"); } }
        public double a2 { get => Model.Common.a2; set { Model.Common.a2 = value; OnPropertyChanged("a2"); } }
        public double αδ { get => Model.Common.αδ; set { Model.Common.αδ = value; OnPropertyChanged("αδ"); } }
        public double bП { get => Model.Common.bП; set { Model.Common.bП = value; OnPropertyChanged("bП"); } }
        public double bП1 { get => Model.Common.bП1; set { Model.Common.bП1 = value; OnPropertyChanged("bП1"); } }
        public double bc { get => Model.Common.bc; set { Model.Common.bc = value; OnPropertyChanged("bc"); } }
        public double bк { get => Model.Common.bк; set { Model.Common.bк = value; OnPropertyChanged("bк"); } }
        public double S { get => Model.Common.S; set { Model.Common.S = value; OnPropertyChanged("S"); } }
        public double W1 { get => Model.Common.W1; set { Model.Common.W1 = value; OnPropertyChanged("W1"); } }
        public double U1 { get => Model.Common.U1; set { Model.Common.U1 = value; OnPropertyChanged("U1"); } }
        public double h1 { get => Model.Common.h1; set { Model.Common.h1 = value; OnPropertyChanged("h1"); } }
        public double h2 { get => Model.Common.h2; set { Model.Common.h2 = value; OnPropertyChanged("h2"); } }
        public double h3 { get => Model.Common.h3; set { Model.Common.h3 = value; OnPropertyChanged("h3"); } }
        public double h4 { get => Model.Common.h4; set { Model.Common.h4 = value; OnPropertyChanged("h4"); } }
        public double h6 { get => Model.Common.h6; set { Model.Common.h6 = value; OnPropertyChanged("h6"); } }
        public double li { get => Model.Common.li; set { Model.Common.li = value; OnPropertyChanged("li"); } }
        public double ρ1x { get => Model.Common.ρ1x; set { Model.Common.ρ1x = value; OnPropertyChanged("ρ1x"); } }
        public double ρ1Г { get => Model.Common.ρ1Г; set { Model.Common.ρ1Г = value; OnPropertyChanged("ρ1Г"); } }
        public double ρ2Г { get => Model.Common.ρ2Г; set { Model.Common.ρ2Г = value; OnPropertyChanged("ρ2Г"); } }
        public double qГ { get => Model.Common.qГ; set { Model.Common.qГ = value; OnPropertyChanged("qГ"); } }
        public double ac { get => Model.Common.ac; set { Model.Common.ac = value; OnPropertyChanged("ac"); } }
        public double aк { get => Model.Common.aк; set { Model.Common.aк = value; OnPropertyChanged("aк"); } }
        public double Kfe1 { get => Model.Common.Kfe1; set { Model.Common.Kfe1 = value; OnPropertyChanged("Kfe1"); } }
        public double Kfe2 { get => Model.Common.Kfe2; set { Model.Common.Kfe2 = value; OnPropertyChanged("Kfe2"); } }
        public double Δкр { get => Model.Common.Δкр; set { Model.Common.Δкр = value; OnPropertyChanged("Δкр"); } }
        public double d1 { get => Model.Common.d1; set { Model.Common.d1 = value; OnPropertyChanged("d1"); } }
        public double h5 { get => Model.Common.h5; set { Model.Common.h5 = value; OnPropertyChanged("h5"); } }
        public double h7 { get => Model.Common.h7; set { Model.Common.h7 = value; OnPropertyChanged("h7"); } }
        public double h8 { get => Model.Common.h8; set { Model.Common.h8 = value; OnPropertyChanged("h8"); } }
        public double bП2 { get => Model.Common.bП2; set { Model.Common.bП2 = value; OnPropertyChanged("bП2"); } }
        public double bПН { get => Model.Common.bПН; set { Model.Common.bПН = value; OnPropertyChanged("bПН"); } }
        public double y1 { get => Model.Common.y1; set { Model.Common.y1 = value; OnPropertyChanged("y1"); } }
        public double ρРУБ { get => Model.Common.ρРУБ; set { Model.Common.ρРУБ = value; OnPropertyChanged("ρРУБ"); } }
        public double B { get => Model.Common.B; set { Model.Common.B = value; OnPropertyChanged("B"); } }
        public double cз { get => Model.Common.cз; set { Model.Common.cз = value; OnPropertyChanged("cз"); } }
        public double dв { get => Model.Common.dв; set { Model.Common.dв = value; OnPropertyChanged("dв"); } }
        public string bСК { get => Model.Common.bСК ? "да" : "нет"; set => Model.Common.bСК = value == "да"; }
        public string markSteelStator { get => Model.Common.markSteelStator; set => Model.Common.markSteelStator = value; }
        #endregion
        #region collection
        /// <summary>
        /// Марка метала статора
        /// </summary>
        public List<string> Get_marksSteelStator => new List<string> { "1412", "2412", "2411", "1521" };
        /// <summary>
        /// Наличие скоса паза ротора
        /// </summary>
        public List<string> Get_bСК => new List<string> { "да", "нет" };
        #endregion
        #region commands
        CalculationCommand CalculationCommand { get; set; }
        public ICommand CommandCalculation {
            get {
                if (CalculationCommand == null) CalculationCommand = new CalculationCommand(Calculation, CanCalculation);
                return CalculationCommand;
            }
        }
        public abstract void Calculation();
        public abstract bool CanCalculation();
        #endregion
        #endregion
    }
}
