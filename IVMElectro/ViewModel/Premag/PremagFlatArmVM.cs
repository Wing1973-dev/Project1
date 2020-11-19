using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Input;
using IVMElectro.Commands;
using IVMElectro.Models.Premag;
using LibraryAlgorithms;
using NLog;
using static IVMElectro.Services.DataSharedPremagContent;
using static LibraryAlgorithms.Services.ServiceDT;

namespace IVMElectro.ViewModel.Premag {
    public class PremagFlatArmVM : INotifyPropertyChanged, IDataErrorInfo {
        public string this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "Bδ":
                        if (Model.Common.Bδ <= 0 || double.IsNaN(Model.Common.Bδ))
                            error = errorBδ;
                        break;
                    case "ρx":
                        if (Model.Common.ρx <= 0 || double.IsNaN(Model.Common.ρx))
                            error = errorρx;
                        break;
                    case "ρГ":
                        if (Model.Common.ρГ <= 0 || double.IsNaN(Model.Common.ρГ))
                            error = errorρГ;
                        break;
                    case "hяр":
                        if (Model.FlatArm.hяр <= 0 || double.IsNaN(Model.FlatArm.hяр))
                            error = errorhяр;
                        break;
                    case "hяк":
                        if (Model.FlatArm.hяк <= 0 || double.IsNaN(Model.FlatArm.hяк))
                            error = errorhяк;
                        break;
                    case "R0":
                        if (Model.Common.R0 < 0 || double.IsNaN(Model.Common.R0))
                            error = errorR0;
                        break;
                    case "R10":
                        if (Model.Common.R10 < 0 || double.IsNaN(Model.Common.R10))
                            error = errorR10;
                        break;
                    case "dпз1":
                        if (Model.FlatArm.dпз1 < 0 || double.IsNaN(Model.FlatArm.dпз1))
                            error = errordпз1;
                        break;
                    case "dвст":
                        if (Model.Common.dвст < 0 || double.IsNaN(Model.Common.dвст))
                            error = errordвст;
                        break;
                    case "Δk1":
                        if (Model.Common.Δk1 <= 0 || double.IsNaN(Model.Common.Δk1))
                            error = errorΔk1;
                        break;
                }
                return error;
            }
        }
        public string Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        public PremagFlatArmVM(PremagCompositeModel model, Logger logger) {
            Model = model; Logger = logger;
            VariationData = new ObservableCollection<StringOfVarParameters> { 
                new StringOfVarParameters { ID = 1, U = 0, δ = 0, q = 0, h = 0, R1 = 0, R2 = 0, R3 = 0, qm = 0, Ws = 0 } };
        }
        #region properties
        PremagCompositeModel Model { get; set; }
        Logger Logger { get; set; }
        public string Diagnostic { get; set; }
        #region main data
        public string Bδ { get => Model.Common.Bδ.ToString(); set { Model.Common.Bδ = StringToDouble(value); OnPropertyChanged("Bδ"); } }
        public string ρx { get => Model.Common.ρx.ToString(); set { Model.Common.ρx = StringToDouble(value); OnPropertyChanged("ρx"); } }
        public string ρГ { get => Model.Common.ρГ.ToString(); set { Model.Common.ρГ = StringToDouble(value); OnPropertyChanged("ρГ"); } }
        public string hяр { get => Model.FlatArm.hяр.ToString(); set { Model.FlatArm.hяр = StringToDouble(value); OnPropertyChanged("hяр"); } }
        public string hяк { get => Model.FlatArm.hяк.ToString(); set { Model.FlatArm.hяк = StringToDouble(value); OnPropertyChanged("hяк"); } }
        public string R0 { get => Model.Common.R0.ToString(); set { Model.Common.R0 = StringToDouble(value); OnPropertyChanged("R0"); } }
        public string R10 { get => Model.Common.R10.ToString(); set { Model.Common.R10 = StringToDouble(value); OnPropertyChanged("R10"); } }
        public string dпз1 { get => Model.FlatArm.dпз1.ToString(); set { Model.FlatArm.dпз1 = StringToDouble(value); OnPropertyChanged("dпз1"); } }
        public string dвст { get => Model.Common.dвст.ToString(); set { Model.Common.dвст = StringToDouble(value); OnPropertyChanged("dвст"); } }
        public string Δk1 { get => Model.Common.Δk1.ToString(); set { Model.Common.Δk1 = StringToDouble(value); OnPropertyChanged("Δk1"); } }
        public string MarkSteel { get => Model.Common.MarkSteel; set { Model.Common.MarkSteel = value; OnPropertyChanged("MarkSteel"); } } 
        #endregion
        //variation data
        public ObservableCollection<StringOfVarParameters> VariationData { get; set; }
        //collection
        public List<string> Get_MarksOfSteel => MarksOfSteel;
        #region commands
        AlgorithmFlatEM algorithm = null;
        Dictionary<string, Dictionary<string, double>> resultCalculation = null;
        UserCommand CalculationCommand { get; set; }
        public ICommand CommandCalculation {
            get {
                if (CalculationCommand == null) CalculationCommand = new UserCommand(Calculation, CanCalculation);
                return CalculationCommand;
            }
        }
        void Calculation() {
            Dictionary<string, Dictionary<string, double>> outputData = new Dictionary<string, Dictionary<string, double>>();
            Model.Common.CreationDataset(); Model.FlatArm.CreationDataset();
            Dictionary<string, double> inputData = Model.Common.Dataset.Union(Model.FlatArm.Dataset).ToDictionary(i => i.Key, i => i.Value);
            double _markSteel = 0;
            switch (MarkSteel) {
                case "09Х17Н": _markSteel = 0; break;
                case "ст. 3, ст. 10": _markSteel = 1; break;
                case "ст. 10880 (Э10)": _markSteel = 2; break;
            }
            inputData.Add("markSteel", _markSteel);
            StringBuilder report = new StringBuilder();
            foreach (StringOfVarParameters setVarData in VariationData) {
                setVarData.CreationDataset();
                algorithm = new AlgorithmFlatEM(inputData.Union(setVarData.Dataset).ToDictionary(i => i.Key, i => i.Value));
                algorithm.Run();
                if (algorithm.SolutionIsDone) {
                    report.AppendLine($"Расчет №{setVarData.ID} - завершен успешно");
                    resultCalculation.Add($"Расчет №{ setVarData.ID}", algorithm.GetResult);
                }
                else
                    report.AppendLine($@"Расчет №{setVarData.ID} - прерван. Смотри содержимое файла logs\*.log");
            }
            //foreach (string item in algorithm.Logging)
            //    Logger.Error(item);

            Diagnostic = report.ToString();
            OnPropertyChanged("Diagnostic");
        }
        bool CanCalculation() {
            //validation of variation string
            bool strVarParam = true;
            foreach (StringOfVarParameters str in VariationData) {
                str.SetParametersForModelValidation(Model.Common.R0, Model.Common.R10, Model.FlatArm.dпз1, Model.Common.dвст);
                var results = new List<ValidationResult>();
                var context = new ValidationContext(str);
                strVarParam = Validator.TryValidateObject(str, context, results, true);
                if (!strVarParam) break;
            }

            var resultsCommon = new List<ValidationResult>(); var resulstFA = new List<ValidationResult>();
            var contextCommon = new ValidationContext(Model.Common); var contextFA = new ValidationContext(Model.FlatArm);
            return Validator.TryValidateObject(Model.Common, contextCommon, resultsCommon, true) &&
                Validator.TryValidateObject(Model.FlatArm, contextFA, resulstFA, true) && strVarParam;
        }

        UserCommand ViewResultCommand { get; set; }
        public ICommand CommandViewResult {
            get {
                if (ViewResultCommand == null) ViewResultCommand = new UserCommand(ViewResult, CanViewResult);
                return ViewResultCommand;
            }
        }
        void ViewResult() {
            if (resultCalculation != null) { }
        }
        bool CanViewResult() => algorithm != null && algorithm.SolutionIsDone;
        #endregion

        #endregion
    }
}
