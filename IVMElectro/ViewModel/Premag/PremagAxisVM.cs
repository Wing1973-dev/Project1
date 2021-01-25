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
using System.IO;
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
        StreamWriter sw; // Поток для записи в файл с результатом расчета
        public string Diagnostic { get; set; }
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
            OnPropertyChanged("Diagnostic");
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

        // Записать параметр в файл с результатом расчета
        private void WriteParamToResultFile(string param, string caption)
        {
            sw.WriteLine("<tr><td>" + caption + ":</td>");            
            sw.WriteLine("<td>" + param + "</td>");            
            sw.WriteLine("</tr>");
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


                string file_name = Directory.GetCurrentDirectory() + "\\report_" + Path.GetFileNameWithoutExtension(IVMElectro.Services.ServiceIO.FileName) + ".html";

                // Создаем поток для записи в файл
                sw = new StreamWriter(file_name);

                sw.WriteLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<meta http-equiv='content-type' content='text/html; charset=UTF-8' />");

                sw.WriteLine("<style>");
                sw.WriteLine(".table td, .table th {");
                sw.WriteLine("padding: .75rem;");
                sw.WriteLine("vertical-align: top;");
                sw.WriteLine("border-top: 1px solid #dee2e6;}");
                sw.WriteLine("table {");
                sw.WriteLine("border-collapse: collapse;}");
                sw.WriteLine(".table-striped tbody tr:nth-of-type(odd){ background-color:rgba(0, 0, 0, .05)}");
                sw.WriteLine(".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6{");
                sw.WriteLine("font-family: sans-serif;}");
                sw.WriteLine(".ml -auto, .mx-auto { margin-left: auto!important; }");
                sw.WriteLine(".mr -auto, .mx-auto { margin-right: auto!important; }");
                sw.WriteLine("</style>");

                sw.WriteLine("<style>.table-fit { width: 1px;} h2 {background-color: #d9d9d9;} h3 {background-color: #ccccff}</style>");

                sw.WriteLine("<title>Результаты расчета</title>");
                sw.WriteLine("<style>.table-fit { width: 1px;} h2 {background-color: #d9d9d9;} h3 {background-color: #ccccff}</style>");

                sw.WriteLine("</head>");
                sw.WriteLine("<body><div class='mx-auto' style='width: 1024px;'>");

                sw.WriteLine("<h1>Результаты расчета</h1>");

                sw.WriteLine("<h2>Электромагнит осевого электромагнитного подшипника</h2>");                

                for (int i = 0; i < commonResult.Count; i++)
                {
                    sw.WriteLine("<h3>ЭМ " + (i + 1).ToString() + "</h3>");

                    foreach (var x in commonResult.Keys)
                    {
                        sw.WriteLine("<p>" + (x).ToString() + "</p>");

                        Dictionary<string, Dictionary<string, double>> calcs = commonResult[x];

                        foreach (var calc in calcs.Keys)
                        {
                            sw.WriteLine("<p>" + (calc).ToString() + "</p>");

                            Dictionary<string, double> parameters_of_calc = calcs[calc];

                            sw.WriteLine("<table class='table table-striped table-fit'>");

                            WriteParamToResultFile(parameters_of_calc["Sзаз"].ToString("F5"), "S<sub>заз</sub>,&nbsp;мм<sup>2</sup>");
                            WriteParamToResultFile(parameters_of_calc["Sзаз1"].ToString("F5"), "S<sub>заз1</sub>,&nbsp;мм<sup>2</sup>");
                            WriteParamToResultFile(parameters_of_calc["Sзаз2"].ToString("F5"), "S<sub>заз2</sub>,&nbsp;мм<sup>2</sup>");
                            WriteParamToResultFile(parameters_of_calc["Sяр"].ToString("F5"), "S<sub>яр</sub>,&nbsp;мм<sup>2</sup>");
                            WriteParamToResultFile(parameters_of_calc["Sяк"].ToString("F5"), "S<sub>як</sub>,&nbsp;мм<sup>2</sup>");
                            WriteParamToResultFile(parameters_of_calc["lяр"].ToString("F5"), "l<sub>яр</sub>,&nbsp;мм");
                            WriteParamToResultFile(parameters_of_calc["lяк"].ToString("F5"), "l<sub>як</sub>,&nbsp;мм");
                            WriteParamToResultFile(parameters_of_calc["lпол"].ToString("F5"), "l<sub>пол</sub>,&nbsp;мм");
                            WriteParamToResultFile(parameters_of_calc["ν"].ToString("F5"), "ν");
                            WriteParamToResultFile(parameters_of_calc["lср"].ToString("F5"), "l<sub>ср</sub>,&nbsp;мм");
                            WriteParamToResultFile(parameters_of_calc["ls"].ToString("F5"), "l<sub>s</sub>,&nbsp;мм");
                            WriteParamToResultFile(parameters_of_calc["r20"].ToString("F5"), "r<sub>20</sub>,&nbsp;Ом");
                            WriteParamToResultFile(parameters_of_calc["rГ"].ToString("F5"), "r<sub>Г</sub>,&nbsp;Ом");
                            WriteParamToResultFile(parameters_of_calc["I"].ToString("F5"), "I,&nbsp;А");
                            WriteParamToResultFile(parameters_of_calc["Fм"].ToString("F5"), "F<sub>м</sub>,&nbsp;А");
                            WriteParamToResultFile(parameters_of_calc["Qм"].ToString("F5"), "Q<sub>м</sub>,&nbsp;мм<sup>2</sup>");
                            WriteParamToResultFile(parameters_of_calc["Kм"].ToString("F5"), "K<sub>м</sub>");
                            WriteParamToResultFile(parameters_of_calc["Фδ"].ToString("F5"), "Ф<sub>δ</sub>,&nbsp;Мкс");
                            WriteParamToResultFile(parameters_of_calc["Bδ"].ToString("F5"), "B<sub>δ</sub>,&nbsp;Гс");
                            WriteParamToResultFile(parameters_of_calc["Fδ"].ToString("F5"), "F<sub>δ</sub>,&nbsp;А");
                            WriteParamToResultFile(parameters_of_calc["Фяр"].ToString("F5"), "Ф<sub>яp</sub>,&nbsp;Мкс");
                            WriteParamToResultFile(parameters_of_calc["Bяр"].ToString("F5"), "B<sub>яр</sub>,&nbsp;Гс");
                            WriteParamToResultFile(parameters_of_calc["Fяр"].ToString("F5"), "F<sub>яр</sub>,&nbsp;А");
                            WriteParamToResultFile(parameters_of_calc["Фяк"].ToString("F5"), "Ф<sub>як</sub>,&nbsp;Мкс");
                            WriteParamToResultFile(parameters_of_calc["Bяк"].ToString("F5"), "B<sub>як</sub>,&nbsp;Гс");
                            WriteParamToResultFile(parameters_of_calc["Fяк"].ToString("F5"), "F<sub>як</sub>,&nbsp;А");
                            WriteParamToResultFile(parameters_of_calc["Фp"].ToString("F5"), "Ф<sub>p</sub>,&nbsp;Мкс");
                            WriteParamToResultFile(parameters_of_calc["Bp1"].ToString("F5"), "B<sub>p1</sub>,&nbsp;Гс");
                            WriteParamToResultFile(parameters_of_calc["Bp2"].ToString("F5"), "B<sub>p2</sub>,&nbsp;Гс");
                            WriteParamToResultFile(parameters_of_calc["Fp1"].ToString("F5"), "F<sub>p1</sub>&nbsp;A");
                            WriteParamToResultFile(parameters_of_calc["Fp2"].ToString("F5"), "F<sub>p2</sub>&nbsp;A");
                            WriteParamToResultFile(parameters_of_calc["F"].ToString("F5"), "F,&nbsp;А");
                            WriteParamToResultFile(parameters_of_calc["Wp"].ToString("F5"), "W<sub>p</sub>,&nbsp;кгс∙см");
                            WriteParamToResultFile(parameters_of_calc["Fтм"].ToString("F5"), "F<sub>тм</sub>,&nbsp;кг");
                            WriteParamToResultFile(parameters_of_calc["P"].ToString("F5"), "P,&nbsp;Вт");
                            WriteParamToResultFile(parameters_of_calc["Δt"].ToString("F5"), "Δt,&nbsp;°С");
                            WriteParamToResultFile(parameters_of_calc["Kt"].ToString("F5"), "Вт/см<sup>2</sup>&nbsp;°С");

                            sw.WriteLine("</table>");
                        }
                    }                                        
                }               

                sw.WriteLine("</div></body>");
                sw.WriteLine("</html>");

                // Закрываем поток для записи в файл
                sw.Close();

                Services.ServiceIO.LaunchBrowser(file_name);

            }
        }
        bool CanViewResult() => algorithm != null && algorithm.SolutionIsDone;
        #endregion
        #endregion
    }
}
