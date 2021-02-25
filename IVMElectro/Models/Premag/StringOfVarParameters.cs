using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static IVMElectro.Services.DataSharedPremagContent;
using static LibraryAlgorithms.Services.ServiceDT;

namespace IVMElectro.Models.Premag {
    public class StringOfVarParameters: DatasetFromModels, IValidatableObject, ICloneable {
        #region properties fo validation
        public double R0 { get; protected set; }
        public double R10 { get; protected set; }
        public double dпз1 { get; private set; }
        public double dвст { get; private set; }
        #endregion
        #region properties
        public int ID_culc { get; set; }
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
            if (input.Element("ID_culc") != null) ID_culc = Convert.ToInt32(input.Element("ID_culc").Value.Trim());
            if (input.Element("U") != null) U = StringToDouble(input.Element("U").Value.Trim());
            if (input.Element("δ") != null) δ = StringToDouble(input.Element("δ").Value.Trim());
            if (input.Element("q") != null) q = StringToDouble(input.Element("q").Value.Trim());
            if (input.Element("h") != null) h = StringToDouble(input.Element("h").Value.Trim());
            if (input.Element("R1") != null) R1 = StringToDouble(input.Element("R1").Value.Trim());
            if (input.Element("R2") != null) R2 = StringToDouble(input.Element("R2").Value.Trim());
            if (input.Element("R3") != null) R3 = StringToDouble(input.Element("R3").Value.Trim());
            if (input.Element("qm") != null) qm = StringToDouble(input.Element("qm").Value.Trim());
            if (input.Element("Ws") != null) Ws = StringToDouble(input.Element("Ws").Value.Trim());
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
        
        public virtual void SetParametersForModelValidation(params double[] outside) {
            R0 = outside[0]; R10 = outside[1]; dпз1 = outside[2]; dвст = outside[3];
        }

        public object Clone() => new StringOfVarParameters { ID_culc = ID_culc, U = U, δ = δ, q = q, h = h, R1 = R1, R2 = R2, R3 = R3, qm = qm, Ws = Ws };
        public virtual XElement Serialise() => new XElement($"StringOfVarParameters{ID_culc}",
            new XElement("ID_culc", ID_culc),
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
        public virtual bool PartialEquality(StringOfVarParameters test) =>
            U == test.U && q == test.q && h == test.h && R1 == test.R1 && R2 == test.R2 && R3 == test.R3 && qm == test.qm && Ws == test.Ws;
    }
}
