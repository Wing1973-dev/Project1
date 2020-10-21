using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static IVMElectro.Services.DataSharedPremagContent;

namespace IVMElectro.Models.Premag {
    public class StringOfVarParameters: IValidatableObject {
        //(double R0, double R10, double dпз1, double dвст) paramsForModelValid;
        #region properties fo validation
        public double R0 { get; private set; }
        public double R10 { get; private set; }
        public double dпз1 { get; private set; }
        public double dвст { get; private set; }
        #endregion
        #region properties
        public int ID { get; set; }
        public double U { get; set; }
        public double δ { get; set; }
        public double q { get; set; }
        public double h { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double R3 { get; set; }
        public double qm { get; set; }
        public double Ws { get; set; } 
        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (U <= 0) errors.Add(new ValidationResult(errorU));
            if (δ <= 0) errors.Add(new ValidationResult(errorδ));
            if (q <= 0) errors.Add(new ValidationResult(errorq));
            if (h <= 0) errors.Add(new ValidationResult(errorh));
            if (R1 <= 0) errors.Add(new ValidationResult(errorR1));
            if (R1 <= R0) errors.Add(new ValidationResult(errorR1R0));
            if (R1 <= R10) errors.Add(new ValidationResult(errorR1R10));
            if (R2 <= 0) errors.Add(new ValidationResult(errorR2));
            if (R2 <= R1) errors.Add(new ValidationResult(errorR2R1));
            if (R2 <= dпз1) errors.Add(new ValidationResult(errorR2dпз1));
            if (R2 <= dвст) errors.Add(new ValidationResult(errorR2dвст));
            if (R3 <= 0) errors.Add(new ValidationResult(errorR3));
            if (R3 <= R2) errors.Add(new ValidationResult(errorR3R2));
            if (qm <= 0) errors.Add(new ValidationResult(errorqm));
            if (Ws <= 0) errors.Add(new ValidationResult(errorWs));
            return errors;
        }

        public void SetParametersForModelValidation(double R0, double R10, double dпз1, double dвст) {
            //ParamsForModelValid.R0 = R0; ParamsForModelValid.R10 = R10; ParamsForModelValid.dпз1 = dпз1; ParamsForModelValid.dвст = dвст;
            this.R0 = R0; this.R10 = R10; this.dпз1 = dпз1; this.dвст = dвст;
        }
    }
}
