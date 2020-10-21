using System.Collections.Generic;
using static IVMElectro.Services.DataSharedPremagContent;
using System.ComponentModel.DataAnnotations;

namespace IVMElectro.Models {
    public class PremagCommonModel : DatasetFromModels, IValidatableObject {
        
        //public string this[string columnName] {
        //    get {
        //        string error = string.Empty;
        //        switch (columnName) {
        //            case "Bδ":
        //                if (Bδ <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //            case "ρx":
        //                if (ρx <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //            case "ρГ":
        //                if (ρГ <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //            case "R0":
        //                if (R0 < 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > или = 0.";
        //                }
        //                break;
        //            case "dпз1":
        //                if (dпз1 < 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > или = 0.";
        //                }
        //                break;
        //            case "dвст":
        //                if (dвст < 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > или = 0.";
        //                }
        //                break;
        //            case "Δk1":
        //                if (Δk1 <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //            case "dм":
        //                if (dм <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //            case "dиз":
        //                if (dиз <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //        }
        //        return error;
        //    }

        //}
        #region fields
        public double Bδ { get; set; }
        public double ρx { get; set; }
        public double ρГ { get; set; }
        public double R0 { get; set; }
        public double R10 { get; set; } //R0'
        public double dвст { get; set; }
        public double Δk1 { get; set; }
        public double dм { get; set; }
        public double dиз { get; set; }
        public string MarkSteel { get; set; }
        #endregion
        public PremagCommonModel() {
            //инициализация
            Bδ = ρx = ρГ = R0 = R10 = dвст = Δk1 = dм = dиз = 0;
            MarkSteel = "09X17H";
        }
        public override void CreationDataset() => Dataset = new Dictionary<string, double> {
            { "Bδ", Bδ } , { "ρx", ρx }, { "ρГ", ρГ }, { "R0", R0 }, { "R0'", R0 }, { "dвст", dвст }, { "Δk1", Δk1 },
        { "dм", dм }, { "dиз", dиз } };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (Bδ <= 0) errors.Add(new ValidationResult(errorBδ));
            if (ρx <= 0) errors.Add(new ValidationResult(errorρx));
            if (ρГ <= 0) errors.Add(new ValidationResult(errorρГ));
            if (R0 < 0) errors.Add(new ValidationResult(errorR0));
            if (R10 < 0) errors.Add(new ValidationResult(errorR10));
            if (dвст < 0) errors.Add(new ValidationResult(errordвст));
            if (Δk1 <= 0) errors.Add(new ValidationResult(errorΔk1));
            if (dм < 0) errors.Add(new ValidationResult(errorρГ));
            if (dиз < 0) errors.Add(new ValidationResult(errorR0));
            //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            if (string.IsNullOrEmpty(MarkSteel)) errors.Add(new ValidationResult("Error MarkSteel"));
            return errors;
        }
    }
}
