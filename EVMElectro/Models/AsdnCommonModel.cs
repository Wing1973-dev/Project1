using System.Collections.Generic;
using System.ComponentModel;

namespace IVMElectro.Models {
    class AsdnCommonModel : DataOperation, IDataErrorInfo {
        string IDataErrorInfo.this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "ΔГ1":
                        if (ΔГ1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "ΔГ2":
                        if (ΔГ2 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Di":
                        if (Di < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Dp":
                        if (Dp < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Da":
                        if (Da < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Z1":
                        if (Z1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Z2":
                        if (Z2 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "p":
                        if (p <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "Pмех":
                        if (Pмех < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "P12":
                        if (P12 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "p10_50":
                        if (p10_50 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "f1":
                        if (f1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "dиз":
                        if (dиз < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "a1":
                        if (a1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "a2":
                        if (a2 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "αδ":
                        if (αδ < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bП":
                        if (bП < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bП1":
                        if (bП1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bП2":
                        if (bП2 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bПН":
                        if (bПН < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bc":
                        if (bc < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bк":
                        if (bк < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "S":
                        if (S < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "W1":
                        if (W1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "U1":
                        if (U1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h1":
                        if (h1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h2":
                        if (h2 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h3":
                        if (h3 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h4":
                        if (h4 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h5":
                        if (h5 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h6":
                        if (h6 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h7":
                        if (h7 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "h8":
                        if (h8 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "li":
                        if (li < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "ρ1x":
                        if (ρ1x < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "ρ1Г":
                        if (ρ1Г < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "ρ2Г":
                        if (ρ2Г < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "qГ":
                        if (qГ < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "ac":
                        if (ac < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "aк":
                        if (aк < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Kfe1":
                        if (Kfe1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Kfe2":
                        if (Kfe2 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Δкр":
                        if (Δкр < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "d1":
                        if (d1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "y1":
                        if (y1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "ρРУБ":
                        if (ρРУБ < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "B":
                        if (B < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "cз":
                        if (cз < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "dв":
                        if (dв < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                }
                return error;
            }
        }
        #region fields
        public double ΔГ1 { get; set; }
        public double ΔГ2 { get; set; }
        public double Di { get; set; }
        public double Dp { get; set; }
        public double Da { get; set; }
        public double Z1 { get; set; }
        public double Z2 { get; set; }
        /// <summary>
        /// число пар полюсов
        /// </summary>
        public int p { get; set; }
        public double Pмех { get; set; }
        public double P12 { get; set; }
        public double p10_50 { get; set; }
        public double f1 { get; set; }
        public double dиз { get; set; }
        public double a1 { get; set; }
        public double a2 { get; set; }
        public double αδ { get; set; }
        public double bП { get; set; }
        public double bП1 { get; set; }
        public double bП2 { get; set; }
        public double bПН { get; set; }
        public double bc { get; set; }
        public double bк { get; set; }
        public double S { get; set; }
        public double W1 { get; set; }
        public double U1 { get; set; }
        public double h1 { get; set; }
        public double h2 { get; set; }
        public double h3 { get; set; }
        public double h4 { get; set; }
        public double h5 { get; set; }
        public double h6 { get; set; }
        public double h7 { get; set; }
        public double h8 { get; set; }
        public double li { get; set; }
        public double ρ1x { get; set; }
        public double ρ1Г { get; set; }
        public double ρ2Г { get; set; }
        public double qГ { get; set; }
        public double ac { get; set; }
        public double aк { get; set; }
        public double Kfe1 { get; set; }
        public double Kfe2 { get; set; }
        public double Δкр { get; set; }
        public double d1 { get; set; }
        public double y1 { get; set; }
        public double ρРУБ { get; set; }
        public double B { get; set; }
        public double cз { get; set; }
        public double dв { get; set; }
        //не валидируемые
        public string markSteelStator { get; set; }
        public bool bСК { get; set; } //да - true, нет - false
        #endregion
        public AsdnCommonModel() {
            IsValidInputData = new Dictionary<string, bool> { { "ΔГ1", false } , { "ΔГ2", false }, { "Di", false }, { "Dp", false }, { "Da", false },
            { "Z1", false }, { "Z2", false }, { "p", false }, { "Pмех", false }, { "Pʹ2", false }, { "p10_50", false }, { "f1", false }, { "dиз", false },
            { "a1", false }, { "a2", false }, { "αδ", false }, { "bП", false }, { "bП1", false }, { "bП2", false }, { "bПН", false }, { "bc", false }, { "bк", false }, 
            { "S", false }, { "W1", false }, { "U1", false }, { "h1", false }, { "h2", false }, { "h3", false }, { "h4", false }, { "h5", false }, { "h6", false },
            { "h7", false }, { "h8", false }, { "li", false }, { "ρ1x", false }, { "ρ1Г", false }, { "ρ2Г", false }, { "qГ", false }, { "ac", false }, { "aк", false }, 
            { "Kfe1", false }, { "Kfe2", false }, { "Δкр", false }, { "d1", false }, { "y1", false }, { "ρРУБ", false }, { "B", false }, { "cз", false }, { "dв", false }  };
            //инициализация
            ΔГ1 = ΔГ2 = Di = Dp = Da = Z1 = Z2 = Pмех = P12 = p10_50 = f1 = dиз = a1 = a2 = αδ = bП = bП1 = bc = bк = S = W1 = U1 = h1 = h2 = h3 = h4 = h6 = li = ρ1x =
                ρ1Г = ρ2Г = qГ = ac = aк = Kfe1 = Kfe2 = Δкр = d1 = h5 = h7 = h8 = bП2 = bПН = y1 = ρРУБ = B = cз = dв = p = 0;
            markSteelStator = "2412"; bСК = false;
        }
        protected override void MakeInputData() => InputData = new Dictionary<string, double> {
                { "ΔГ1", ΔГ1 } , { "ΔГ2", ΔГ2 }, { "Di", Di }, { "Dp", Dp }, { "Da", Da }, { "Z1", Z1 }, { "Z2", Z2 }, { "p", p },
                { "Pмех", Pмех }, { "Pʹ2", P12 }, { "p10_50", p10_50 }, { "f1", f1 }, { "dиз", dиз }, { "a1", a1 }, { "a2", a2 }, { "αδ", αδ },
                { "bП", bП }, { "bП1", bП1 }, { "bc", bc }, { "bк", bк }, { "S", S }, { "W1", W1 }, { "U1", U1 }, { "h1", h1 }, { "h2", h2 },
            { "h3", h3 }, { "h4", h4 }, { "h6", h6 }, { "li", li }, { "ρ1x", ρ1x }, { "ρ1Г", ρ1Г }, { "ρ2Г", ρ2Г }, { "qГ", qГ }, { "ac", ac },
            { "aк", aк }, { "Kfe1", Kfe1 }, { "Kfe2", Kfe2 }, { "Δкр", Δкр },  { "d1", d1 }, { "h5", h5 }, { "h7", h7 }, { "h8", h8 },
            { "bП2", bП2 }, { "bПН", bПН }, { "y1", y1 }, { "ρРУБ", ρРУБ }, { "B", B }, { "cз", cз }, { "dв", dв } };
    }
}
