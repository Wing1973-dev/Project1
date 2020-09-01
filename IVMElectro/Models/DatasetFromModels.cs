using System.Collections.Generic;
using static IVMElectro.Services.ServiceIO;

namespace IVMElectro.Models {
    abstract class DatasetFromModels {
        //this collections are intented for keyboard input element 
        protected Dictionary<string, double> Dataset { get; set; }
        //protected Dictionary<string, bool> IsValidInputData { get; set; }
        /// <summary>
        /// Осуществляет проверку данных и готовит расчетное представление данных. Результаты проверки сообщаются пользователю
        /// </summary>
        /// <returns></returns>
        //public virtual bool Valid() {
        //    CreationDataset();
        //    foreach (KeyValuePair<string, double> item in Dataset)
        //        if (double.IsNaN(item.Value) || item.Value < 0) {
        //            ErrorReport($"Параметр расчета {item.Key} должен быть > или = 0.");
        //            IsValidInputData[item.Key] = false;
        //        }
        //        else
        //            IsValidInputData[item.Key] = true;

        //    return !IsValidInputData.ContainsValue(false);
        //}
        /// <summary>
        /// Осуществляет проверку данных и готовит расчетное представление данных
        /// </summary>
        /// <returns></returns>
        //public virtual bool ValidHidden() {
        //    CreationDataset();
        //    foreach (KeyValuePair<string, double> item in Dataset)
        //        if (double.IsNaN(item.Value) || item.Value < 0)
        //            IsValidInputData[item.Key] = false;
        //        else
        //            IsValidInputData[item.Key] = true;

        //    return !IsValidInputData.ContainsValue(false);
        //}
        protected abstract void CreationDataset();
        public Dictionary<string, double> GetDataset => Dataset;
    }
}
