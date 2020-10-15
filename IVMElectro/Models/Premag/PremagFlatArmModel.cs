using System.Collections.Generic;
using static IVMElectro.Services.DataSharedPremagContent;
using System.ComponentModel.DataAnnotations;

namespace IVMElectro.Models.Premag {
    public class PremagFlatArmModel : DatasetFromModels, IValidatableObject {
        const string errordпз1 = "Значение параметра dпз1 должно быть ≥ 0.";
        //public string this[string columnName] {
        //    get {
        //        string error = string.Empty;
        //        switch (columnName) {
        //            case "hяр":
        //                if (hяр <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //            case "hяк":
        //                if (hяк <= 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > 0.";
        //                }
        //                break;
        //            case "R0ʹ":
        //                if (R0ʹ < 0) {
        //                    error = $"Параметр расчета {columnName} должен быть > или = 0.";
        //                }
        //                break;
        //        }
        //        return error;
        //    }
        //}
        #region fields
        public double hяр { get; set; }
        public double hяк { get; set; }
        public double dпз1 { get; set; }
        #endregion
        public PremagFlatArmModel() {
            hяр = hяк = dпз1 = 0;
        }
        public override void CreationDataset() => Dataset = new Dictionary<string, double> { { "hяр", hяр } , { "hяк", hяк }, { "dпз1", dпз1 } };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (hяр <= 0) errors.Add(new ValidationResult(errorBδ));
            if (hяк <= 0) errors.Add(new ValidationResult(errorρx));
            if (dпз1 < 0) errors.Add(new ValidationResult(errordпз1));
            return errors;
        }
    }
}
