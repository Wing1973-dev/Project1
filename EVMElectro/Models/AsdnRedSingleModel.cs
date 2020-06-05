using System;
using System.Collections.Generic;
using System.ComponentModel;
using IVMElectro.Services;

namespace IVMElectro.Models {
    class AsdnRedSingleModel : DataOperation, IDataErrorInfo {
        //TODO: define limitation
        string IDataErrorInfo.this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "dкп":
                        if (dкп < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "dпв":
                        if (dпв < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "dпн":
                        if (dпн < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "hр1":
                        if (hр1 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "hр2":
                        if (hр2 < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "hш":
                        if (hш < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bш":
                        if (bш < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "aкн":
                        if (aкн < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                    case "bкн":
                        if (bкн < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                }
                return error;
            }
        }
        #region свойства
        public int PAS { get; set; }//red {1 - круглый, 2 - прямоугольный, 3 - грушевидный, 4 - двойная клетка}
        #region red artifact
        public double dкп { get; set; }
        public double dпв { get; set; }
        public double dпн { get; set; }
        public double hр1 { get; set; }
        public double hр2 { get; set; }
        public double hш { get; set; }
        public double bш { get; set; }
        public double aкн { get; set; }
        public double bкн { get; set; } 
        #endregion
        #endregion
        public AsdnRedSingleModel() {
            IsValidInputData = new Dictionary<string, bool> { { "dкп", false }, { "dпв", false }, { "dпн", false }, { "hр1", false },
                { "hр2", false }, { "hш", false }, { "bш", false }, { "aкн", false }, { "bкн", false }  };
            //инициализация
            dкп = dпв = dпн = hр1 = hр2 = hш = bш = aкн = bкн = 0;
            PAS = 1;
        }
        protected override void MakeInputData() => InputData = new Dictionary<string, double> { { "dкп", dкп }, { "dпв", dпв },
            { "dпн", dпн }, { "hр1", hр1 }, { "hр2", hр2 }, { "hш", hш }, { "bш", bш }, { "aкн", aкн }, { "bкн", bкн } };
    }
}
