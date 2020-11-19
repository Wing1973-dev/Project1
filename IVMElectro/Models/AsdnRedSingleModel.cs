using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static IVMElectro.Services.DataSharedASDNContent;

namespace IVMElectro.Models {
    public class AsdnRedSingleModel : DatasetFromModels, IValidatableObject {
        #region свойства
        public double dв { get; set; } //диаметр вала 
        public string PAS { get; set; } //is defined in the interface (not checked) (1 - круглый, 2 - прямоугольный, 3 - грушевидный, 4 - двойная клетка)
        public double hш { get; set; }
        public double bш { get; set; }
        public double dкп { get; set; }
        public double bZH { get; set; }
        public double hp2 { get; set; }
        public double bкн { get; set; }
        public double aкн { get; set; }
        public double aк { get; set; } //высота кольца к.з. клетки
        public double bП2 { get; set; } //ширина прямоугольного паза ротора
        public double bк { get; set; }
        #endregion
        public AsdnRedSingleModel() {
            hш = bш = dкп = hp2 = aкн = aк = bкн = bZH = bП2 = bк = 0;
            dв = 0.19; PAS = "круглый";
        }
        public override void CreationDataset() => 
            Dataset = new Dictionary<string, double> { { "dв", dв }, { "hш", hш }, { "bш", bш }, { "dкп", dкп }, { "hр2", hp2 },  { "aкн", aкн }, { "aк", aк }, 
                { "bкн", bкн }, { "bП2", bП2 }, { "bк", bк } };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();

            double _dпн = dпн(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2, bZH);
            double _dпв = dпв(paramsModelValid.Z2, paramsModelValid.Dpст, hш, bZH);
            double _hp1 = hp1(_dпв, _dпн, paramsModelValid.Z2);

            if (!((0.19 <= dв) && (dв <= 0.25 * paramsModelValid.Da)))
                errors.Add(new ValidationResult($"Значение параметра dв должно принадлежать {Get_dвBounds(paramsModelValid.Da)}."));
            if (string.IsNullOrEmpty(PAS)) errors.Add(new ValidationResult("Error PAS.")); //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            if (!((0.5 <= hш) && (hш <= 1))) errors.Add(new ValidationResult(errorhш));
            if (!((1 <= bш) && (bш <= 2.5))) errors.Add(new ValidationResult(errorbш));
            if (!((bounds_bZH(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2).left <= bZH) &&
                (bZH <= bounds_bZH(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2).right)))
                errors.Add(
                    new ValidationResult(
                        $"Значение параметра bZH должно принадлежать {Get_bZHBounds(bounds_bZH(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2))}."));


            //parameters depending on the type of rotor
            if (PAS == "двойная клетка") {
                if (!((bounds_aк(dкп, hш, hp2).left <= aк) && (aк <= bounds_aк(dкп, hш, hp2).right)))
                    errors.Add(new ValidationResult($"Значение параметра aк должно принадлежать {Get_aкBounds(bounds_aк(dкп, hш, hp2))}."));
            }
            else if (double.IsNaN(aк) || aк <= 0) errors.Add(new ValidationResult(erroraкRED));

            if (PAS == "прямоугольный" || PAS == "двойная клетка") {
                if (!((bП2 <= bк) && (bк <= 5 * bП2)))
                    errors.Add(new ValidationResult($"Значение параметра bк должно принадлежать {Get_bкBounds(bП2)}."));
            }
            else if (double.IsNaN(bк) || bк <= 0) errors.Add(new ValidationResult(errorbкRED));

            if (PAS == "круглый" || PAS == "двойная клетка")
                if (!((Get_dкпBounds(paramsModelValid.Dpст, paramsModelValid.Z2).left <= dкп) &&
                    (dкп <= Get_dкпBounds(paramsModelValid.Dpст, paramsModelValid.Z2).right)))
                    errors.Add(
                        new ValidationResult($"Значение параметра dкп должно принадлежать {dкпBoundsString(Get_dкпBounds(paramsModelValid.Dpст, paramsModelValid.Z2))}."));
            
            if (PAS == "двойная клетка") {
                if (!((3 <= hp2) && (hp2 <= 10))) errors.Add(new ValidationResult(errorhp2RED));
                if (!((5 <= bкн) && (bкн <= 35))) errors.Add(new ValidationResult(errorbкн));
                if (!((bounds_aкн(_dпн, _dпв, _hp1).left <= aкн) && (aкн <= 1.2 * bounds_aкн(_dпн, _dпв, _hp1).right)))
                    errors.Add(new ValidationResult($"Значение параметра aкн должно принадлежать {Get_aкнBounds(bounds_aкн(_dпн, _dпв, _hp1))}."));
            } 
            
            if (PAS == "прямоугольный" || PAS == "двойная клетка")
                if (double.IsNaN(bП2) || bП2 <= 0) errors.Add(new ValidationResult(errorbП2RED));

            return errors;
        }

        private (double Dpст, double Z2, double hp, double Da) paramsModelValid;
        public void SetParametersForModelValidation(double Dpст, double Z2, double hp, double Da) {
            paramsModelValid.Dpст = Dpст; paramsModelValid.Z2 = Z2; paramsModelValid.hp = hp; paramsModelValid.Da = Da; 
        }
    }
}
