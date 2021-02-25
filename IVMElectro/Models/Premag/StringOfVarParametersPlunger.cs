using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static IVMElectro.Services.DataSharedPremagContent;
using static LibraryAlgorithms.Services.ServiceDT;

namespace IVMElectro.Models.Premag {
    public class StringOfVarParametersPlunger: StringOfVarParameters, IValidatableObject, ICloneable {
        #region properties fo validation
        public double R110 { get; private set; }
        public double R1110 { get; private set; }
        public double hфл { get; private set; }
        public double l1 { get; private set; }
        public double l2 { get; private set; }
        #endregion
        #region properties
        public double α { get; set; }
        #endregion
        public StringOfVarParametersPlunger() { }
        public StringOfVarParametersPlunger(XElement input) : base(input) {
            if (input.Element("α") != null) α = StringToDouble(input.Element("α").Value.Trim());
        }
        public override void CreationDataset() {
            base.CreationDataset(); Dataset.Add("α", α);
        }
        public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (U <= 0) errors.Add(new ValidationResult(errorU));
            if (δ <= 0) errors.Add(new ValidationResult(errorδ));
            if (q <= 0) errors.Add(new ValidationResult(errorq));
            if (h <= 0) errors.Add(new ValidationResult(errorh));
            if (R1 <= 0) errors.Add(new ValidationResult(errorR1));
            if (R1 <= R0) errors.Add(new ValidationResult(errorR1R0));
            if (R1 <= R10) errors.Add(new ValidationResult(errorR1R10));
            if (R1 <= R110) errors.Add(new ValidationResult(errorR1R110));
            if (R1 <= R1110) errors.Add(new ValidationResult(errorR1R1110));
            if (R2 <= 0) errors.Add(new ValidationResult(errorR2));
            if (R2 <= R1) errors.Add(new ValidationResult(errorR2R1));
            if (R3 <= 0) errors.Add(new ValidationResult(errorR3));
            if (R3 <= R2) errors.Add(new ValidationResult(errorR3R2));
            if (qm <= 0) errors.Add(new ValidationResult(errorqm));
            if (Ws <= 0) errors.Add(new ValidationResult(errorWs));
            if (α < 0) errors.Add(new ValidationResult(errorα));
            if (!(h + 0.5 * hфл * Math.PI >= δ * Math.Cos(Math.PI * α / 180) * Math.Cos(Math.PI * α / 180) + l1 + l2)) 
                errors.Add(new ValidationResult(errorhhфлδαl1l2));
            return errors;
        }
        public override void SetParametersForModelValidation(params double[] outside) {
            R0 = outside[0]; R10 = outside[1]; R110 = outside[2]; R1110 = outside[3]; hфл = outside[4]; l1 = outside[5]; l2 = outside[6];
        }
        public new object Clone() => new StringOfVarParametersPlunger {
            ID_culc = ID_culc,
            U = U,
            δ = δ,
            q = q,
            h = h,
            R1 = R1,
            R2 = R2,
            R3 = R3,
            qm = qm,
            Ws = Ws,
            α = α
        };
        public override XElement Serialise() {
            XElement resultObject = base.Serialise();
            resultObject.Add(new XElement("α", α));
            return resultObject;
        }
        public override bool PartialEquality(StringOfVarParameters test) => base.PartialEquality(test) && α == ((StringOfVarParametersPlunger)test).α;
    }
}
