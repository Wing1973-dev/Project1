using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace IVMElectro.Models.Premag {
    public class StringOfVarParametersAxis : StringOfVarParameters, ICloneable, IEquatable<StringOfVarParametersAxis> {
        #region properties
        public int ID_slot { get; set; }
        #endregion
        public StringOfVarParametersAxis() { }
        public StringOfVarParametersAxis(XElement input) : base(input) {
            if (input.Element("ID_slot") != null) ID_slot = Convert.ToInt32(input.Element("ID_slot").Value.Trim());
        }
        public new object Clone() => new StringOfVarParametersAxis {
            ID_culc = ID_culc,
            ID_slot = ID_slot,
            U = U,
            δ = δ,
            q = q,
            h = h,
            R1 = R1,
            R2 = R2,
            R3 = R3,
            qm = qm,
            Ws = Ws
        };
        public bool Equals([AllowNull] StringOfVarParametersAxis other) {
            return ID_culc == other.ID_culc && ID_slot == other.ID_slot;
        }
        public override int GetHashCode() => ID_culc * 73 + ID_slot;

        public override XElement Serialise() {
            XElement resultObject = base.Serialise();
            resultObject.Add(new XElement("ID_slot", ID_slot));
            return resultObject;
        }
    }
}
