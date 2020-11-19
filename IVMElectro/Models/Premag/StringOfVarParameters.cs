using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static IVMElectro.Services.DataSharedPremagContent;

namespace IVMElectro.Models.Premag {
    public class StringOfVarParameters: DatasetFromModels, IValidatableObject, ICloneable {
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
        public StringOfVarParameters() { }
        public StringOfVarParameters(XElement input) {
            if (input.Element("ID") != null) ID = Convert.ToInt32(input.Element("ID").Value.Trim());
            if (input.Element("U") != null) U = Convert.ToDouble(input.Element("U").Value.Trim());
            if (input.Element("δ") != null) δ = Convert.ToDouble(input.Element("δ").Value.Trim());
            if (input.Element("q") != null) q = Convert.ToDouble(input.Element("q").Value.Trim());
            if (input.Element("h") != null) h = Convert.ToDouble(input.Element("h").Value.Trim());
            if (input.Element("R1") != null) R1 = Convert.ToDouble(input.Element("R1").Value.Trim());
            if (input.Element("R2") != null) R2 = Convert.ToDouble(input.Element("R2").Value.Trim());
            if (input.Element("R3") != null) R3 = Convert.ToDouble(input.Element("R3").Value.Trim());
            if (input.Element("qm") != null) qm = Convert.ToDouble(input.Element("qm").Value.Trim());
            if (input.Element("Ws") != null) Ws = Convert.ToDouble(input.Element("Ws").Value.Trim());
        }
        public override void CreationDataset() => Dataset = new Dictionary<string, double> {
            { "U", U }, { "δ", δ }, { "q" , q}, { "h", h }, { "R1", R1 }, { "R2", R2 }, { "R3", R3 }, { "qm", qm }, { "Ws", Ws } };

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
            this.R0 = R0; this.R10 = R10; this.dпз1 = dпз1; this.dвст = dвст;
        }

        public object Clone() => new StringOfVarParameters { ID = ID, U = U, δ = δ, q = q, h = h, R1 = R1, R2 = R2, R3 = R3, qm = qm, Ws = Ws };
        public XElement Serialise() => new XElement($"StringOfVarParameters{ID}",
            new XElement("ID", ID),
            new XElement("U", U),
            new XElement("δ", δ),
            new XElement("q", q),
            new XElement("h", h),
            new XElement("R1", R1),
            new XElement("R2", R2),
            new XElement("R3", R3),
            new XElement("qm", qm),
            new XElement("Ws", Ws)
            );

    }
}
