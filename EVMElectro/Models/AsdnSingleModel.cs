using System;
using System.Collections.Generic;
using System.ComponentModel;
using IVMElectro.Services;

namespace IVMElectro.Models {
    class AsdnSingleModel : DataOperation, IDataErrorInfo {
        //TODO: define limitation
        string IDataErrorInfo.this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "γ":
                        if (γ < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "I1":
                        if (I1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "hp":
                        if (hp < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Ki":
                        if (Ki < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                }
                return error;
            }
        }
        #region свойства
        public double γ { get; set; }
        public double I1 { get; set; }
        public double hp { get; set; }
        public double Ki { get; set; }
        public bool P3 { get; set; } //1 - true, 0 - false 
        #endregion
        public AsdnSingleModel() {
            IsValidInputData = new Dictionary<string, bool> { { "γ", false }, { "I1", false }, { "hp", false }, { "Ki", false } };
            //инициализация
            γ = I1 = hp = Ki = 0;
            P3 = false; 
        }
        protected override void MakeInputData() => InputData = new Dictionary<string, double> { { "γ", γ }, { "I1", I1 }, { "hp", hp }, { "Ki", Ki } };
    }
}
