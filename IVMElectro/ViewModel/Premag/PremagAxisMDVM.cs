using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using IVMElectro.Commands;
using IVMElectro.Models.Premag;
using static IVMElectro.Services.DataSharedPremagContent;
using static LibraryAlgorithms.Services.ServiceDT;

namespace IVMElectro.ViewModel.Premag {
    public class PremagAxisMDVM : INotifyPropertyChanged, IDataErrorInfo {
        public string this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "Bδ":
                        if (Model.Bδ <= 0 || double.IsNaN(Model.Bδ))
                            error = errorBδ;
                        break;
                    case "ρx":
                        if (Model.ρx <= 0 || double.IsNaN(Model.ρx))
                            error = errorρx;
                        break;
                    case "ρГ":
                        if (Model.ρГ <= 0 || double.IsNaN(Model.ρГ))
                            error = errorρГ;
                        break;
                    case "hяр":
                        if (Model.hяр <= 0 || double.IsNaN(Model.hяр))
                            error = errorhяр;
                        break;
                    case "hяк":
                        if (Model.hяк <= 0 || double.IsNaN(Model.hяк))
                            error = errorhяк;
                        break;
                    case "R0":
                        if (Model.R0 < 0 || double.IsNaN(Model.R0))
                            error = errorR0;
                        break;
                    case "R10":
                        if (Model.R10 < 0 || double.IsNaN(Model.R10))
                            error = errorR10;
                        break;
                    case "dпз1":
                        if (Model.dпз1 < 0 || double.IsNaN(Model.dпз1))
                            error = errordпз1;
                        break;
                    case "dвст":
                        if (Model.dвст < 0 || double.IsNaN(Model.dвст))
                            error = errordвст;
                        break;
                    case "Δk1":
                        if (Model.Δk1 <= 0 || double.IsNaN(Model.Δk1))
                            error = errorΔk1;
                        break;
                }
                return error;
            }
        }
        public string Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public PremagAxisMDVM(PremagAxisMainDataModel model) => Model = model;

        #region properties
        public PremagAxisMainDataModel Model { get; private set; }
        public string ID_slot { get => Model.ID_slot.ToString(); set { Model.ID_slot = StringToInt(value); } }
        public string Bδ { get => Model.Bδ.ToString(); set { Model.Bδ = StringToDouble(value); OnPropertyChanged("Bδ"); } }
        public string ρx { get => Model.ρx.ToString(); set { Model.ρx = StringToDouble(value); OnPropertyChanged("ρx"); } }
        public string ρГ { get => Model.ρГ.ToString(); set { Model.ρГ = StringToDouble(value); OnPropertyChanged("ρГ"); } }
        public string hяр { get => Model.hяр.ToString(); set { Model.hяр = StringToDouble(value); OnPropertyChanged("hяр"); } }
        public string hяк { get => Model.hяк.ToString(); set { Model.hяк = StringToDouble(value); OnPropertyChanged("hяк"); } }
        public string R0 { get => Model.R0.ToString(); set { Model.R0 = StringToDouble(value); OnPropertyChanged("R0"); } }
        public string R10 { get => Model.R10.ToString(); set { Model.R10 = StringToDouble(value); OnPropertyChanged("R10"); } } //R'0
        public string dпз1 { get => Model.dпз1.ToString(); set { Model.dпз1 = StringToDouble(value); OnPropertyChanged("dпз1"); } }
        public string dвст { get => Model.dвст.ToString(); set { Model.dвст = StringToDouble(value); OnPropertyChanged("dвст"); } }
        public string Δk1 { get => Model.Δk1.ToString(); set { Model.Δk1 = StringToDouble(value); OnPropertyChanged("Δk1"); } }
        public bool IsOK { get; set; } = false;
        #endregion
        #region command
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
        #endregion
    }
}
