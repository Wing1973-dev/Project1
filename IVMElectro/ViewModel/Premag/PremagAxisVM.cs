using System;
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

namespace IVMElectro.ViewModel.Premag {
    public class PremagAxisVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public PremagAxisVM(Logger logger) {
            VariationDataMainData = new ObservableCollection<PremagAxisMainDataModel> {
                new PremagAxisMainDataModel{ ID_slot = 1, Bδ = 0, ρx = 0, ρГ = 0, hяр = 0, hяк = 0, R0 = 0, R10 = 0, dпз1 = 0, dвст = 0, Δk1 = 0 }
            };
            VariationDataUpMagnets = new ObservableCollection<StringOfVarParametersAxis> {
                new StringOfVarParametersAxis{ ID_culc = 1, ID_slot=1, U = 0, δ = 0, q = 0, h = 0, R1 = 0, R2 = 0, R3 = 0, qm = 0, Ws = 0 }
            };
            VariationDataDownMagnets = new ObservableCollection<StringOfVarParametersAxis> {
                new StringOfVarParametersAxis{ ID_culc = 1, ID_slot=1, U = 0, δ = 0, q = 0, h = 0, R1 = 0, R2 = 0, R3 = 0, qm = 0, Ws = 0 }
            };
        }
        #region properties
        string diagnostic = string.Empty;
        public string Diagnostic { get => diagnostic; set { diagnostic = value; OnPropertyChanged("Diagnostic"); } }
        Logger Logger { get; set; }

        string markSteel = "09Х17Н";
        public string MarkSteel { get => markSteel; set { markSteel = value; OnPropertyChanged("MarkSteel"); } }
        //collection
        public List<string> Get_MarksOfSteel => MarksOfSteel;
        //variation data Main Data
        public ObservableCollection<PremagAxisMainDataModel> VariationDataMainData { get; set; }
        //variation data Up Magnets
        public ObservableCollection<StringOfVarParametersAxis> VariationDataUpMagnets { get; set; }
        //variation data Down Magnets
        public ObservableCollection<StringOfVarParametersAxis> VariationDataDownMagnets { get; set; }
        #region commands
        AlgorithmPremagFlatEM algorithm = null;
        Dictionary<string, Dictionary<string, Dictionary<string, double>>> commonResult = null;
        Dictionary<int, double> FтмSumUp, FтмSumDwn; //key - № расчета

        UserCommand CalculationCommand { get; set; }
        public ICommand CommandCalculation {
            get {
                if (CalculationCommand == null) CalculationCommand = new UserCommand(Calculation, CanCalculation);
                return CalculationCommand;
            }
        }
        void Calculation() {
            FтмSumUp = new Dictionary<int, double>(); FтмSumDwn = new Dictionary<int, double>();
            double _markSteel = 0;
            switch (MarkSteel) {
                case "09Х17Н": _markSteel = 0; break;
                case "ст. 3, ст. 10": _markSteel = 1; break;
                case "ст. 10880 (Э10)": _markSteel = 2; break;
            }
            StringBuilder report = new StringBuilder();
            commonResult = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            foreach (PremagAxisMainDataModel slot in VariationDataMainData) {
                Dictionary<string, Dictionary<string, double>> resultUpVarData = new Dictionary<string, Dictionary<string, double>>();
                Dictionary<string, Dictionary<string, double>> resultDownVarData = new Dictionary<string, Dictionary<string, double>>();
                slot.CreationDataset(); //get data for calculation
                Dictionary<string, double> inputData = slot.Dataset; inputData.Add("markSteel", _markSteel);
                //Up EM
                var setVarDataUP = VariationDataUpMagnets.Where(i => i.ID_slot == slot.ID_slot);
                foreach (StringOfVarParametersAxis setVarData in setVarDataUP) {
                    setVarData.CreationDataset(); //get data for calculation
                    algorithm = new AlgorithmPremagFlatEM(inputData.Union(setVarData.Dataset).ToDictionary(i => i.Key, i => i.Value));
                    algorithm.Run();
                    if (algorithm.SolutionIsDone) {
                        report.AppendLine($"Расчет №{setVarData.ID_culc} для верхнего ЭМ №{slot.ID_slot} - завершен успешно");
                        resultUpVarData.Add($"Расчет №{ setVarData.ID_culc}  для верхнего ЭМ №{slot.ID_slot}", algorithm.GetResult);
                        if (FтмSumUp.ContainsKey(setVarData.ID_culc))
                            FтмSumUp[setVarData.ID_culc] += algorithm.GetResult["Fтм"];
                        else
                            FтмSumUp.Add(setVarData.ID_culc, algorithm.GetResult["Fтм"]);
                    }
                    else
                        report.AppendLine($@"Расчет №{setVarData.ID_culc} для верхнего ЭМ №{slot.ID_slot} - прерван. Смотри содержимое файла logs\*.log");
                }
                //Down EM
                var setVarDataDWN = VariationDataDownMagnets.Where(i => i.ID_slot == slot.ID_slot);
                foreach (StringOfVarParametersAxis setVarData in setVarDataDWN) {
                    setVarData.CreationDataset(); //get data for calculation
                    algorithm = new AlgorithmPremagFlatEM(inputData.Union(setVarData.Dataset).ToDictionary(i => i.Key, i => i.Value));
                    algorithm.Run();
                    if (algorithm.SolutionIsDone) {
                        report.AppendLine($"Расчет №{setVarData.ID_culc} для нижнего ЭМ №{slot.ID_slot} - завершен успешно");
                        resultDownVarData.Add($"Расчет №{ setVarData.ID_culc}  для нижнего ЭМ №{slot.ID_slot}", algorithm.GetResult);
                        if (FтмSumDwn.ContainsKey(setVarData.ID_culc))
                            FтмSumDwn[setVarData.ID_culc] += algorithm.GetResult["Fтм"];
                        else
                            FтмSumDwn.Add(setVarData.ID_culc, algorithm.GetResult["Fтм"]);
                    }
                    else
                        report.AppendLine($@"Расчет №{setVarData.ID_culc} для нижнего ЭМ №{slot.ID_slot} - прерван. Смотри содержимое файла logs\*.log");
                }
                commonResult.Add($"Слот №{slot.ID_slot}", resultUpVarData);
                commonResult[$"Слот №{slot.ID_slot}"] = commonResult[$"Слот №{slot.ID_slot}"].Union(resultDownVarData).ToDictionary(i => i.Key, i => i.Value);
            }
            Diagnostic = report.ToString();
            //OnPropertyChanged("Diagnostic");
        }
        bool CanCalculation() {
            bool isStrVarParam = true, isSlot = true; 
            foreach (PremagAxisMainDataModel slot in VariationDataMainData) {
                var resultsSlot = new List<ValidationResult>();
                var contextSlot = new ValidationContext(slot);
                isSlot = Validator.TryValidateObject(slot, contextSlot, resultsSlot);
                if (!isSlot) break;
                //validation of variation string
                //Up EM
                foreach (StringOfVarParametersAxis setVarData in VariationDataUpMagnets) {
                    setVarData.SetParametersForModelValidation(slot.R0, slot.R10, slot.dпз1, slot.dвст);
                    var results = new List<ValidationResult>();
                    var context = new ValidationContext(setVarData);
                    isStrVarParam = Validator.TryValidateObject(setVarData, context, results, true);
                    if (!isStrVarParam) break;
                }
                //Down EM
                if (isStrVarParam) {
                    foreach (StringOfVarParametersAxis setVarData in VariationDataDownMagnets) {
                        setVarData.SetParametersForModelValidation(slot.R0, slot.R10, slot.dпз1, slot.dвст);
                        var results = new List<ValidationResult>();
                        var context = new ValidationContext(setVarData);
                        isStrVarParam = Validator.TryValidateObject(setVarData, context, results, true);
                        if (!isStrVarParam) break;
                    }
                }
                if (!isStrVarParam) break;
            }
            return isSlot && isStrVarParam;
        }

        UserCommand ViewResultCommand { get; set; }
        public ICommand CommandViewResult {
            get {
                if (ViewResultCommand == null) ViewResultCommand = new UserCommand(ViewResult, CanViewResult);
                return ViewResultCommand;
            }
        }
        void ViewResult() {
            if (commonResult != null || commonResult.Count > 0) {

                
            }
        }
        bool CanViewResult() => algorithm != null && algorithm.SolutionIsDone;
        #endregion
        #endregion
    }
}
