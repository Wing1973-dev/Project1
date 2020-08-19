using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static IVMElectro.Services.DataSharedContent;

namespace IVMElectro.Models {
    class AsdnRedSingleModel : DatasetFromModels, IValidatableObject {
        #region свойства
        #region stator parameters
        //double Da { get; set; } //private
        #endregion
        #region rotor parameters
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
        //double Dpст { get; set; }
        //double Z2 { get; set; }
        //double hp { get; set; }
        #endregion
        #endregion
        public AsdnRedSingleModel() {
            //commonModel.NotifyDa += Set_Da; 
            //commonModel.NotifyDpст += Set_Dpст; 
            //commonModel.NotifyZ2 += Set_Z2; 
            //commonModel.Notifyhp += Set_hp;
            //IsValidInputData = new Dictionary<string, bool> { { "dв", true }, { "hш", false }, { "bш", false }, { "dкп", false }, { "hр2", false }, { "aкн", false },
            //    { "aк", false }, { "bкн", false }, { "bZH", false}  };
            //инициализация
            hш = bш = dкп = hp2 = aкн = aк = bкн = bZH = 0;
            dв = 0.19; PAS = "круглый";
        }
        //internal double Get_dпн() => Math.PI * (Dpст - hp) / (Z2 - Math.PI) - bZH;
        //internal double Get_hp1() => (hp - hш - dкп - hp2 - Get_dпн()) / (1 + Math.Sin(2 * Math.PI / Z2));
        //internal double Get_dпв() => Get_dпн() + 2 * Get_hp1() * Math.Sin(2 * Math.PI / Z2);
        protected override void CreationDataset() => 
            Dataset = new Dictionary<string, double> { { "dв", dв }, { "hш", hш }, { "bш", bш }, { "dкп", dкп }, { "hр2", hp2 },  { "aкн", aкн }, { "aк", aк }, { "bкн", bкн }, { "bZH", bZH} };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();

            double _dпн = dпн(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2, bZH);
            double _hp1 = hp1(paramsModelValid.hp, hш, dкп, hp2, _dпн, paramsModelValid.Z2);
            double _dпв = dпв(_dпн, _hp1, paramsModelValid.Z2);

            if (!((0.19 <= dв) && (dв <= 0.25 * paramsModelValid.Da)))
                errors.Add(new ValidationResult($"Значение параметра dв должно принадлежать {Get_dвBounds(paramsModelValid.Da)}."));
            if (string.IsNullOrEmpty(PAS)) errors.Add(new ValidationResult("Error PAS.")); //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            if (!((0.5 <= hш) && (hш <= 1))) errors.Add(new ValidationResult(errorhш));
            if (!((1 <= bш) && (bш <= 2.5))) errors.Add(new ValidationResult(errorbш));
            if (!((Get_dкпBounds(paramsModelValid.Dpст, paramsModelValid.Z2).left <= dкп) && 
                (dкп <= Get_dкпBounds(paramsModelValid.Dpст, paramsModelValid.Z2).right)))
                errors.Add(new ValidationResult($"Значение параметра dкп должно принадлежать {dкпBoundsString(Get_dкпBounds(paramsModelValid.Dpст, paramsModelValid.Z2))}."));
            if (!((bounds_bZH(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2).left <= bZH) && 
                (bZH <= bounds_bZH(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2).right)))
                errors.Add(new ValidationResult($"Значение параметра bZH должно принадлежать {Get_bZHBounds(bounds_bZH(paramsModelValid.Dpст, paramsModelValid.hp, paramsModelValid.Z2))}."));
            if (!((3 <= hp2) && (hp2 <= 10))) errors.Add(new ValidationResult(errorhp2));
            if (!((5 <= bкн) && (bкн <= 35))) errors.Add(new ValidationResult(errorbкн));
            if (!((bounds_aкн(_dпн, _dпв, _hp1).left <= aкн) && (aкн <= 1.2 * bounds_aкн( _dпн,  _dпв, _hp1).right)))
                errors.Add(new ValidationResult($"Значение параметра aкн должно принадлежать {Get_aкнBounds(bounds_aкн(_dпн, _dпв, _hp1))}."));
            if (!((bounds_aк(dкп, hш, hp2).left <= aк) && (aк <= bounds_aк(dкп, hш, hp2).right)))
                errors.Add(new ValidationResult($"Значение параметра aк должно принадлежать {Get_aкBounds(bounds_aк(dкп, hш, hp2))}."));

            return errors;
        }

        private (double Dpст, double Z2, double hp, double Da) paramsModelValid;
        public void SetParametersForModelValidation(double Dpст, double Z2, double hp, double Da) {
            paramsModelValid.Dpст = Dpст; paramsModelValid.Z2 = Z2; paramsModelValid.hp = hp; paramsModelValid.Da = Da; 
        }



        //void Set_Da(object sender, SingleValueEventArgs e) => Da = e.Value;
        //void Set_Dpст(object sender, SingleValueEventArgs e) => Dpст = e.Value;
        //void Set_Z2(object sender, SingleValueEventArgs e) => Z2 = e.Value;
        //void Set_hp(object sender, SingleValueEventArgs e) => hp = e.Value;
        //double aкн_bound() => 0.5 * (Get_dпн() + Get_dпв()) + Get_hp1();
    }
}
