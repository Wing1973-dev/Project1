using System.Collections.Generic;
using static IVMElectro.Services.DataSharedPremagContent;
using System.ComponentModel.DataAnnotations;

namespace IVMElectro.Models.Premag {
    public class PremagPlungerModel : DatasetFromModels, IValidatableObject {
        const string errordпз1 = "Значение параметра dпз1 должно быть > 0.";
        
        #region fields
        public double hфл { get; set; }
        public double R110 { get; set; } //R''0
        public double R1110 { get; set; } //R'''0
        public double dпз1 { get; set; }
        public double dпз2 { get; set; }
        public double l1 { get; set; }
        public double l2 { get; set; }
        #endregion
        public PremagPlungerModel() {
            hфл = R110 = R1110 = dпз1 = dпз2 = l1 = l2 = 0;
        }
        public override void CreationDataset() => Dataset = new Dictionary<string, double> { { "hфл", hфл }, { "R110", R110 }, 
            { "R1110", R1110 }, { "dпз1", dпз1 }, { "dпз2", dпз2 }, { "l1", l1 }, { "l2", l2 } };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (hфл <= 0) errors.Add(new ValidationResult(errorhфл));
            if (R110 < 0) errors.Add(new ValidationResult(errorR110));
            if (R1110 < 0) errors.Add(new ValidationResult(errorR1110));
            if (dпз1 <= 0) errors.Add(new ValidationResult(errordпз1));
            if (dпз2 <= 0) errors.Add(new ValidationResult(errordпз2));
            if (l1 < 0) errors.Add(new ValidationResult(errorl1));
            if (l2 < 0) errors.Add(new ValidationResult(errorl2));
            return errors;
        }

    }
}
