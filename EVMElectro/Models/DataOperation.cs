using System.Collections.Generic;
using static IVMElectro.Services.ServiceIO;

namespace IVMElectro.Models {
    abstract class DataOperation {
        protected Dictionary<string, double> InputData { get; set; }
        protected Dictionary<string, bool> IsValidInputData { get; set; }
        public string Error { get; }
        /// <summary>
        /// Осуществляет проверку данных и готовит расчетное представление данных. Результаты проверки сообщаются пользователю
        /// </summary>
        /// <returns></returns>
        public virtual bool Valid() {
            MakeInputData();
            foreach (KeyValuePair<string, double> item in InputData)
                if (double.IsNaN(item.Value) || item.Value < 0) {
                    ErrorReport($"Параметр расчета {item.Key} должен быть > или = 0.");
                    IsValidInputData[item.Key] = false;
                }
                else
                    IsValidInputData[item.Key] = true;

            return !IsValidInputData.ContainsValue(false);
        }
        /// <summary>
        /// Осуществляет проверку данных и готовит расчетное представление данных
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidHidden() {
            MakeInputData();
            foreach (KeyValuePair<string, double> item in InputData)
                if (double.IsNaN(item.Value) || item.Value < 0)
                    IsValidInputData[item.Key] = false;
                else
                    IsValidInputData[item.Key] = true;

            return !IsValidInputData.ContainsValue(false);
        }
        protected abstract void MakeInputData();
        public Dictionary<string, double> Get_InputData {
            get {
                if (IsValidInputData.ContainsValue(false) || InputData == null) return null;
                return InputData;
            }
        }
    }
}
