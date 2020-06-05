using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IVMElectro.Models {
    class PremagCommonModel : DataOperation, IDataErrorInfo {
        public string this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "Bδ":
                        if (Bδ <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "ρx":
                        if (ρx <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "ρГ":
                        if (ρГ <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "R0":
                        if (R0 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "dпз1":
                        if (dпз1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "dвст":
                        if (dвст < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "Δk1":
                        if (Δk1 <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "dм":
                        if (dм <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "dиз":
                        if (dиз <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                }
                return error;
            }

        }
        #region fields
        public double Bδ { get; set; }
        public double ρx { get; set; }
        public double ρГ { get; set; }
        public double R0 { get; set; }
        public double dпз1 { get; set; }
        public double dвст { get; set; }
        public double Δk1 { get; set; }
        public double dм { get; set; }
        public double dиз { get; set; }
        //public DataTable VariationalParameters { get; set; }
        //не валидируемые
        public string MarkSteel { get; set; }
        #endregion
        public PremagCommonModel() {
            IsValidInputData = new Dictionary<string, bool> { { "Bδ", false } , { "ρx", false }, { "ρГ", false }, { "R0", false }, { "dпз1", false },
            { "dвст", false }, { "Δk1", false }, { "dм", false }, { "dиз", false } };
            //инициализация
            Bδ = ρx = ρГ = R0 = dпз1 = dвст = Δk1 = dм = dиз = 0;
            MarkSteel = "09X17H"; 
        }
        protected override void MakeInputData() => InputData = new Dictionary<string, double> {
            { "Bδ", Bδ } , { "ρx", ρx }, { "ρГ", ρГ }, { "R0", R0 }, { "dпз1", dпз1 }, { "dвст", dвст }, { "Δk1", Δk1 }, { "dм", dм }, { "dиз", dиз } };
    }
}
