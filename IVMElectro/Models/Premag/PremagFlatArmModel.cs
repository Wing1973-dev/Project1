using System.Collections.Generic;
using static IVMElectro.Services.DataSharedPremagContent;
using System.ComponentModel.DataAnnotations;

namespace IVMElectro.Models.Premag {
    public class PremagFlatArmModel : DatasetFromModels, IValidatableObject {
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
