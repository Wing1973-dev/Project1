using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using IVMElectro.Commands;
using IVMElectro.Models.Premag;
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
                    case "dм":
                        if (Model.Common.dм < 0 || double.IsNaN(Model.Common.dм))
                            error = errordм;
                        break;
                    case "dиз":
                        if (Model.Common.dиз < 0 || double.IsNaN(Model.Common.dиз))
                            error = errordиз;
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
        public string dм { get => Model.Common.dм.ToString(); set { Model.Common.dм = StringToDouble(value); OnPropertyChanged("dм"); } }
        public string dиз { get => Model.Common.dиз.ToString(); set { Model.Common.dиз = StringToDouble(value); OnPropertyChanged("dиз"); } }
        #endregion
        //variation data
        public ObservableCollection<StringOfVarParameters> VariationData { get; set; }
        //collection
        public List<string> Get_MarkOfSteel => MarkOfSteel;
        #region commands
        //AlgorithmASDN algorithm;
        UserCommand CalculationCommand { get; set; }
        public ICommand CommandCalculation {
            get {
                if (CalculationCommand == null) CalculationCommand = new UserCommand(Calculation, CanCalculation);
                return CalculationCommand;
            }
        }
        void Calculation() {
            Model.Common.CreationDataset(); 
            //Model.AsdnSingle.CreationDataset();
            //Dictionary<string, double> _inputAlgorithm = Model.Common.GetDataset.Union(Model.AsdnSingle.GetDataset).ToDictionary(i => i.Key, i => i.Value);
            //_inputAlgorithm.Add("bП1", bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1));
            //_inputAlgorithm.Add("h1", Model.Common.h1); _inputAlgorithm.Add("h2", Model.Common.h2); _inputAlgorithm.Add("bП", Model.Common.bП);
            //_inputAlgorithm.Add("P3", Model.AsdnSingle.P3); _inputAlgorithm.Add("bСК", Model.Common.bСК == "скошенные" ? 1 : 0);
            //_inputAlgorithm.Add("β", Model.Common.β); _inputAlgorithm.Add("Sсл", Model.Common.Sсл);
            //algorithm = new AlgorithmASDN(_inputAlgorithm); algorithm.Run();
            //foreach (string item in algorithm.Logging)
            //    Logger.Error(item);

            //Diagnostic = algorithm.SolutionIsDone ? "Расчет завершен успешно" : @"Расчет прерван. Смотри содержимое файла logs\*.log";
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
        void ViewResult() { }
        bool CanViewResult() => false;
        //=> (algorithm != null && algorithm.SolutionIsDone);
        #endregion

        #endregion
    }
}
