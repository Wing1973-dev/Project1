using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static IVMElectro.Services.DataSharedPremagContent;

namespace IVMElectro.Models.Premag {
    public class PremagAxisMainDataModel : DatasetFromModels, IValidatableObject, ICloneable {
        #region fields
        public int ID_slot { get; set; }
        public double Bδ { get; set; }
        public double ρx { get; set; }
        public double ρГ { get; set; }
        public double hяр { get; set; }
        public double hяк { get; set; }
        public double R0 { get; set; }
        public double R10 { get; set; } //R'0
        public double dпз1 { get; set; }
        public double dвст { get; set; }
        public double Δk1 { get; set; }
        //public string MarkSteel { get; set; }
        #endregion
        public PremagAxisMainDataModel() {
            Bδ = ρx = ρГ = hяр = hяк = R0 = R10 = dпз1 = dвст = Δk1 = 0;
            //MarkSteel = "09Х17Н";
        }
        public override void CreationDataset() => Dataset = new Dictionary<string, double> {
            { "Bδ", Bδ } , { "ρx", ρx }, { "ρГ", ρГ }, { "hяр", hяр }, { "hяк", hяк }, { "R0", R0 }, { "R10", R10 }, { "dпз1", dпз1 }, { "dвст", dвст }, { "Δk1", Δk1 } };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (Bδ <= 0) errors.Add(new ValidationResult(errorBδ));
            if (ρx <= 0) errors.Add(new ValidationResult(errorρx));
            if (ρГ <= 0) errors.Add(new ValidationResult(errorρГ));
            if (hяр <= 0) errors.Add(new ValidationResult(errorBδ));
            if (hяк <= 0) errors.Add(new ValidationResult(errorρx));
            if (R0 < 0) errors.Add(new ValidationResult(errorR0));
            if (R10 < 0) errors.Add(new ValidationResult(errorR10));
            if (dпз1 < 0) errors.Add(new ValidationResult(errordпз1));
            if (dвст < 0) errors.Add(new ValidationResult(errordвст));
            if (Δk1 <= 0) errors.Add(new ValidationResult(errorΔk1));
            //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            //if (string.IsNullOrEmpty(MarkSteel)) errors.Add(new ValidationResult("Error MarkSteel"));
            return errors;
        }

        public object Clone() => new PremagAxisMainDataModel {   ID_slot = ID_slot, Bδ = Bδ, ρx = ρx, ρГ = ρГ,   hяр = hяр,  hяк = hяк,
            R0 = R0,  R10 = R10,dпз1 = dпз1, dвст = dвст, Δk1 = Δk1
        };
    }
}
