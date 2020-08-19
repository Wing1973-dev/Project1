using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static IVMElectro.Services.DataSharedContent;

namespace IVMElectro.Models {
    class AsdnSingleModel : DatasetFromModels, IValidatableObject {
        #region свойства
        #region stator parameters
        public int P3 { get; set; } //input in the interface (not checked)  (for algorithm: true -> 1, false ->0 )
        #endregion
        #region rotor parameters
        public double dв { get; set; } //внутренний диаметр сверления
        public double aк { get; set; } //высота кольца к.з. обмотки
        public double γ { get; set; } 
        //double hp { get; set; } //private
        #endregion
        #endregion
        //public AsdnSingleModel(AsdnCommonModel commonModel) {
        //    //commonModel.Notifyhp += Set_hp;
        //    IsValidInputData = new Dictionary<string, bool> { { "dв", true }, { "aк", false } };
        //    dв = aк = 0; P3 = 0;
        //}
        public AsdnSingleModel() {
            //IsValidInputData = new Dictionary<string, bool> { { "dв", true }, { "aк", false }, { "γ", false } };
            dв = aк = γ = 0; P3 = 0;
        }
        protected override void CreationDataset() => Dataset = new Dictionary<string, double> { { "dв", dв }, { "aк", aк }, { "γ", γ } };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (!(P3 == 0 || P3 == 1)) errors.Add(new ValidationResult("Error P3")); //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            if (!((0 <= dв) && (dв <= 0.5 * (0.5 * Get_Dp(parametersForModelValidation.Dpст, parametersForModelValidation.ΔГ2) - parametersForModelValidation.hp))))
                errors.Add(new ValidationResult($"Значение параметра dв должно принадлежать " +
                    $"{Get_dвBounds(parametersForModelValidation.Dpст, parametersForModelValidation.ΔГ2, parametersForModelValidation.hp)}."));
            if (!((1.05 * parametersForModelValidation.hp <= aк) && (aк <= 1.5 * parametersForModelValidation.hp)))
                errors.Add(new ValidationResult($"Значение параметра aк должно принадлежать {Get_aкBounds(parametersForModelValidation.hp)}."));
            if ((γ < 0) || double.IsNaN(γ)) errors.Add(new ValidationResult(errorγ));

            return errors;
        }

        private (double Dpст, double ΔГ2, double hp) parametersForModelValidation;
        public void SetParametersForModelValidation(double Dpст, double ΔГ2, double hp) { 
            parametersForModelValidation.Dpст = Dpст; parametersForModelValidation.ΔГ2 = ΔГ2; parametersForModelValidation.hp = hp;
        }
        //void Set_hp(object sender, SingleValueEventArgs e) => hp = e.Value;

    }
}
