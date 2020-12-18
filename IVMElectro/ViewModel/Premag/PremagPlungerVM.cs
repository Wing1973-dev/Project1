﻿using System.Collections.Generic;
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
    public class PremagPlungerVM : INotifyPropertyChanged, IDataErrorInfo {
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
                    case "hфл":
                        if (Model.Plunger.hфл <= 0 || double.IsNaN(Model.Plunger.hфл))
                            error = errorhяр;
                        break;
                    case "R0":
                        if (Model.Common.R0 < 0 || double.IsNaN(Model.Common.R0))
                            error = errorR0;
                        break;
                    case "R10":
                        if (Model.Common.R10 < 0 || double.IsNaN(Model.Common.R10))
                            error = errorR10;
                        break;
                    case "R110":
                        if (Model.Plunger.R110 < 0 || double.IsNaN(Model.Plunger.R110))
                            error = errorR110;
                        break;
                    case "R1110":
                        if (Model.Plunger.R1110 < 0 || double.IsNaN(Model.Plunger.R1110))
                            error = errorR1110;
                        break;
                    case "dпз1":
                        if (Model.Plunger.dпз1 <= 0 || double.IsNaN(Model.Plunger.dпз1))
                            error = errordпз1_plngr;
                        break;
                    case "dпз2":
                        if (Model.Plunger.dпз2 <= 0 || double.IsNaN(Model.Plunger.dпз2))
                            error = errordпз2;
                        break;
                    case "dвст":
                        if (Model.Common.dвст < 0 || double.IsNaN(Model.Common.dвст))
                            error = errordвст;
                        break;
                    case "Δk1":
                        if (Model.Common.Δk1 <= 0 || double.IsNaN(Model.Common.Δk1))
                            error = errorΔk1;
                        break;
                    case "l1":
                        if (Model.Plunger.l1 < 0 || double.IsNaN(Model.Plunger.l1))
                            error = errorl1;
                        break;
                    case "l2":
                        if (Model.Plunger.l2 < 0 || double.IsNaN(Model.Plunger.l2))
                            error = errorl2;
                        break;
                }
                return error;
            }
        }
        public string Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public PremagPlungerVM(PremagCompositeModel model, Logger logger) {
            Model = model; Logger = logger;
            VariationData=new ObservableCollection<StringOfVarParametersPlunger>{
                new StringOfVarParametersPlunger { ID_culc = 1, U = 0, δ = 0, q = 0, h = 0, R1 = 0, R2 = 0, R3 = 0, qm = 0, Ws = 0, α = 0 } };
        }
        #region properties
        PremagCompositeModel Model { get; set; }
        Logger Logger { get; set; }
        public string Diagnostic { get; set; }
        #region main data
        public string Bδ { get => Model.Common.Bδ.ToString(); set { Model.Common.Bδ = StringToDouble(value); OnPropertyChanged("Bδ"); } }
        public string ρx { get => Model.Common.ρx.ToString(); set { Model.Common.ρx = StringToDouble(value); OnPropertyChanged("ρx"); } }
        public string ρГ { get => Model.Common.ρГ.ToString(); set { Model.Common.ρГ = StringToDouble(value); OnPropertyChanged("ρГ"); } }
        public string hфл { get => Model.Plunger.hфл.ToString(); set { Model.Plunger.hфл = StringToDouble(value); OnPropertyChanged("hфл"); } }
        public string R0 { get => Model.Common.R0.ToString(); set { Model.Common.R0 = StringToDouble(value); OnPropertyChanged("R0"); } }
        public string R10 { get => Model.Common.R10.ToString(); set { Model.Common.R10 = StringToDouble(value); OnPropertyChanged("R10"); } }
        public string R110 { get => Model.Plunger.R110.ToString(); set { Model.Plunger.R110 = StringToDouble(value); OnPropertyChanged("R110"); } }
        public string R1110 { get => Model.Plunger.R1110.ToString(); set { Model.Plunger.R1110 = StringToDouble(value); OnPropertyChanged("R1110"); } }
        public string dпз1 { get => Model.Plunger.dпз1.ToString(); set { Model.Plunger.dпз1 = StringToDouble(value); OnPropertyChanged("dпз1"); } }
        public string dпз2 { get => Model.Plunger.dпз2.ToString(); set { Model.Plunger.dпз2 = StringToDouble(value); OnPropertyChanged("dпз2"); } }
        public string dвст { get => Model.Common.dвст.ToString(); set { Model.Common.dвст = StringToDouble(value); OnPropertyChanged("dвст"); } }
        public string l1 { get => Model.Plunger.l1.ToString(); set { Model.Plunger.l1 = StringToDouble(value); OnPropertyChanged("l1"); } }
        public string l2 { get => Model.Plunger.l2.ToString(); set { Model.Plunger.l2 = StringToDouble(value); OnPropertyChanged("l2"); } }
        public string Δk1 { get => Model.Common.Δk1.ToString(); set { Model.Common.Δk1 = StringToDouble(value); OnPropertyChanged("Δk1"); } }
        public string MarkSteel { get => Model.Common.MarkSteel; set { Model.Common.MarkSteel = value; OnPropertyChanged("MarkSteel"); } }
        #endregion
        //variation data
        public ObservableCollection<StringOfVarParametersPlunger> VariationData { get; set; }
        //collection
        public List<string> Get_MarksOfSteel => MarksOfSteel;
        #region commands
        AlgorithmPremagPlungerEM algorithm = null;
        Dictionary<string, Dictionary<string, double>> resultCalculation = null;
        UserCommand CalculationCommand { get; set; }
        public ICommand CommandCalculation {
            get {
                if (CalculationCommand == null) CalculationCommand = new UserCommand(Calculation, CanCalculation);
                return CalculationCommand;
            }
        }
        void Calculation() {
            resultCalculation = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, Dictionary<string, double>> outputData = new Dictionary<string, Dictionary<string, double>>();
            Model.Common.CreationDataset(); Model.Plunger.CreationDataset();
            Dictionary<string, double> inputData = Model.Common.Dataset.Union(Model.Plunger.Dataset).ToDictionary(i => i.Key, i => i.Value);
            double _markSteel = 0;
            switch (MarkSteel) {
                case "09Х17Н": _markSteel = 0; break;
                case "ст. 3, ст. 10": _markSteel = 1; break;
                case "ст. 10880 (Э10)": _markSteel = 2; break;
            }
            inputData.Add("markSteel", _markSteel);
            StringBuilder report = new StringBuilder();
            foreach (StringOfVarParametersPlunger setVarData in VariationData) {
                setVarData.CreationDataset();
                algorithm = new AlgorithmPremagPlungerEM(inputData.Union(setVarData.Dataset).ToDictionary(i => i.Key, i => i.Value));
                algorithm.Run();
                if (algorithm.SolutionIsDone) {
                    report.AppendLine($"Расчет №{setVarData.ID_culc} - завершен успешно");
                    resultCalculation.Add($"Расчет №{ setVarData.ID_culc}", algorithm.GetResult);
                }
                else
                    report.AppendLine($@"Расчет №{setVarData.ID_culc} - прерван. Смотри содержимое файла logs\*.log");
            }
            //foreach (string item in algorithm.Logging)
            //    Logger.Error(item);
            Diagnostic = report.ToString();
            OnPropertyChanged("Diagnostic");
        }
        bool CanCalculation() {
            //validation of variation string
            bool strVarParam = true;
            foreach (StringOfVarParametersPlunger str in VariationData) {
                str.SetParametersForModelValidation(Model.Common.R0, Model.Common.R10, Model.Plunger.R110, Model.Plunger.R1110,
                    Model.Plunger.hфл, Model.Plunger.l1, Model.Plunger.l2);
                var results = new List<ValidationResult>();
                var context = new ValidationContext(str);
                strVarParam = Validator.TryValidateObject(str, context, results, true);
                if (!strVarParam) break;
            }

            var resultsCommon = new List<ValidationResult>(); var resulstPlunger = new List<ValidationResult>();
            var contextCommon = new ValidationContext(Model.Common); var contextPlunger = new ValidationContext(Model.Plunger);
            return Validator.TryValidateObject(Model.Common, contextCommon, resultsCommon, true) &&
                Validator.TryValidateObject(Model.Plunger, contextPlunger, resulstPlunger, true) && strVarParam;
        }

        UserCommand ViewResultCommand { get; set; }
        public ICommand CommandViewResult {
            get {
                if (ViewResultCommand == null) ViewResultCommand = new UserCommand(ViewResult, CanViewResult);
                return ViewResultCommand;
            }
        }
        void ViewResult() {
            if (resultCalculation != null) {
                List<(double δ, double Fтм)> plot = null;
                bool isPlot = true;
                StringOfVarParametersPlunger test = VariationData[0];
                for (int i = 1; i < VariationData.Count; i++) {
                    isPlot = test.PartialEquality(VariationData[i]);
                    if (!isPlot) break;
                }
                if (isPlot) {
                    plot = new List<(double δ, double Fтм)>();
                    for (int i = 0; i < resultCalculation.Count; i++) {
                        plot.Add((VariationData[i].δ, resultCalculation.ElementAt(i).Value["Fтм"]));
                    }
                }
            }
        }
        bool CanViewResult() => algorithm != null && algorithm.SolutionIsDone;
        #endregion
        #endregion
    }
}