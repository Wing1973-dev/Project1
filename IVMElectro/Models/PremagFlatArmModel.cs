using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using static IVMElectro.Services.ServiceIO;
using static LibraryAlgorithm.Services.ServiceDT;

namespace IVMElectro.Models {
    class PremagFlatArmModel : DatasetFromModels {
        public string this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "hяр":
                        if (hяр <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "hяк":
                        if (hяк <= 0) {
                            error = $"Параметр расчета {columnName} должен быть > 0.";
                        }
                        break;
                    case "R0ʹ":
                        if (R0ʹ < 0) {
                            error = $"Параметр расчета {columnName} должен быть > или = 0.";
                        }
                        break;
                }
                return error;
            }
        }
        #region fields
        public double hяр { get; set; }
        public double hяк { get; set; }
        public double R0ʹ { get; set; }
        public DataTable VariationalParameters { get; set; }
        #endregion
        public PremagFlatArmModel() {
            //IsValidInputData = new Dictionary<string, bool> { { "hяр", false } , { "hяк", false }, { "R0ʹ", false } };
            //инициализация
            hяр = hяк = R0ʹ = 0;
            VariationalParameters = new DataTable();
            DataColumn U = new DataColumn("U B", typeof(string)), δ = new DataColumn("δ мм", typeof(string)), q = new DataColumn("q %", typeof(string)),
                h = new DataColumn("h мм", typeof(string)), R1 = new DataColumn("R1 мм", typeof(string)), R2 = new DataColumn("R2 мм", typeof(string)),
                R3 = new DataColumn("R3 мм", typeof(string)), qm = new DataColumn("qm мм²", typeof(string)), Ws = new DataColumn("Ws", typeof(string));
            DataColumn ID = new DataColumn("№п/п", typeof(int)); ID.Unique = true; ID.AllowDBNull = false; ID.AutoIncrement = true;
            ID.AutoIncrementSeed = 1; ID.AutoIncrementStep = 1;
            VariationalParameters.Columns.AddRange(new DataColumn[] { ID, U, δ, q, h, R1, R2, R3, qm, Ws });
        }
        public override void CreationDataset() => Dataset = new Dictionary<string, double> { { "hяр", hяр } , { "hяк", hяк }, { "R0ʹ", R0ʹ } };
        //TODO edit here
        public bool IsValidVariationalParameters() {
            if (VariationalParameters == null || VariationalParameters.Rows.Count == 0) {
                ErrorReport("Режимы эксплуатации не определены.");
                return false;
            }
            int countrow = 1;
            foreach (DataRow row in VariationalParameters.Rows) {
                
                if (StringToDouble(row["TF °C"].ToString()) < 180 || StringToDouble(row["TF °C"].ToString()) > 340) {
                    ErrorReport($"Недопустимое значение данных для температуры облучения {countrow}-ого режима эксплуатации");
                    return false;
                }
                if (StringToDouble(row["F нейт/м²"].ToString()) < 0) {
                    ErrorReport($"Недопустимое значение данных для флюенса {countrow}-ого режима эксплуатации");
                    return false;
                }
                countrow++;
            }
            return true;
        }
    }
}
