using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using IVMElectro.Models.Premag;
using static LibraryAlgorithms.Services.ServiceDT;
using static IVMElectro.Services.DataSharedPremagContent;
using IVMElectro.Commands;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace IVMElectro.ViewModel.Premag {
    public class StringOfVarParametersVM : INotifyPropertyChanged, IDataErrorInfo {
        public string this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "U":
                        if (Model.U <= 0 || double.IsNaN(Model.U))
                            error = errorU;
                        break;
                    case "δ":
                        if (Model.δ <= 0 || double.IsNaN(Model.δ))
                            error = errorδ;
                        break;
                    case "q":
                        if (Model.q <= 0 || double.IsNaN(Model.q))
                            error = errorq;
                        break;
                    case "h":
                        if (Model.h <= 0 || double.IsNaN(Model.h))
                            error = errorh;
                        break;
                    case "R1":
                        if (Model.R1 <= 0 || double.IsNaN(Model.R1))
                            error = errorR1;
                        if (Model.R1 <= Model.R0)
                            error = errorR1R0;
                        if (Model.R1 <= Model.R10)
                            error = errorR1R10;
                        break;
                    case "R2":
                        if (Model.R2 <= 0 || double.IsNaN(Model.R2))
                            error = errorR2;
                        if (Model.R2 <= Model.R1)
                            error = errorR2R1;
                        if (Model.R2 <= Model.dпз1)
                            error = errorR2dпз1;
                        if (Model.R2 <= Model.dвст)
                            error = errorR2dвст;
                        break;
                    case "R3":
                        if (Model.R3 <= 0 || double.IsNaN(Model.R3))
                            error = errorR3;
                        if (Model.R3 <= Model.R2)
                            error = errorR3R2;
                        break;
                    case "qm":
                        if (Model.qm <= 0 || double.IsNaN(Model.qm))
                            error = errorqm;
                        break;
                    case "Ws":
                        if (Model.Ws <= 0 || double.IsNaN(Model.Ws))
                            error = errorWs;
                        break;
                }
                return error;
            }

        }
        public string Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public StringOfVarParametersVM(StringOfVarParameters model) => Model = model;
        #region properties
        public StringOfVarParameters Model { get; private set; }
        public string ID { get => Model.ID.ToString(); set { Model.ID = StringToInt(value); } }
        public string U { get => Model.U.ToString(); set { Model.U = StringToDouble(value); OnPropertyChanged("U"); } }
        public string δ { get => Model.δ.ToString(); set { Model.δ = StringToDouble(value); OnPropertyChanged("δ"); } }
        public string q { get => Model.q.ToString(); set { Model.q = StringToDouble(value); OnPropertyChanged("q"); } }
        public string h { get => Model.h.ToString(); set { Model.h = StringToDouble(value); OnPropertyChanged("h"); } }
        public string R1 { get => Model.R1.ToString(); set { Model.R1 = StringToDouble(value); OnPropertyChanged("R1"); } }
        public string R2 { get => Model.R2.ToString(); set { Model.R2 = StringToDouble(value); OnPropertyChanged("R2"); } }
        public string R3 { get => Model.R3.ToString(); set { Model.R3 = StringToDouble(value); OnPropertyChanged("R3"); } }
        public string qm { get => Model.qm.ToString(); set { Model.qm = StringToDouble(value); OnPropertyChanged("qm"); } }
        public string Ws { get => Model.Ws.ToString(); set { Model.Ws = StringToDouble(value); OnPropertyChanged("Ws"); } }
        public bool IsOK { get; set; } = false;
        #endregion
        //command
        UserCommand OKCommand { get; set; }
        public ICommand CommandOK {
            get {
                if (OKCommand == null) OKCommand = new UserCommand(OK, CanOK);
                return OKCommand;
            }
        }
        void OK() => IsOK = true;
        //the parameters required for validation must be provided by the cod that uses the given class
        public bool CanOK() {
            var resultsCommon = new List<ValidationResult>(); var contextCommon = new ValidationContext(Model);
            return Validator.TryValidateObject(Model, contextCommon, resultsCommon, true);
        }
    }
}
