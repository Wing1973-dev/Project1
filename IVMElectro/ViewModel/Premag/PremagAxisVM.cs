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

        // Записать параметр в файл с результатом расчета
        private void WriteParamToResultFile(string param, string caption, Dictionary<string, Dictionary<string, double>> calcs, string filter)
        {
            sw.WriteLine("<tr><td>" + caption + "</td>");

            foreach (var calc in calcs.Keys.Where(key => key.Contains(filter)))
            {
                sw.WriteLine("<td>" + calcs[calc][param].ToString("F5") + "</td>");
            }

            sw.WriteLine("</tr>");
        }

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
            if (commonResult != null || commonResult.Count > 0)
            {

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

                sw.WriteLine("<script type='text/javascript' src='js/jquery-3.5.1.min.js'></script>");

                sw.WriteLine("<script>");
                sw.WriteLine("$(function(){");
                sw.WriteLine("$('.view-source .hide').hide();");
                sw.WriteLine("$a = $('.view-source a');");
                sw.WriteLine("$a.on('click', function(event) {");
                sw.WriteLine("event.preventDefault();");
                sw.WriteLine("$a.not(this).next().slideUp(500);");
                sw.WriteLine("$(this).next().slideToggle(500);");
                sw.WriteLine("});");
                sw.WriteLine("});");
                sw.WriteLine("</script>");

                sw.WriteLine("</head>");
                sw.WriteLine("<body><div class='mx-auto' style='width: 1024px;'>");

                sw.WriteLine("<h1>Результаты расчета</h1>");

                sw.WriteLine("<h2>Электромагнит осевого электромагнитного подшипника</h2>");

                    foreach (var x in commonResult.Keys)
                    {
                        sw.WriteLine("<h3>" + (x).ToString() + "</h3>");

                        sw.WriteLine("<div class='view-source'><a href='#'>Верхние электромагниты</a>");

                        Dictionary<string, Dictionary<string, double>> calcs = commonResult[x];

                        sw.WriteLine("<div class='hide'>");
                        sw.WriteLine("<table class='table table-striped table-fit'>");

                        // Делаем шапку таблицы

                        sw.WriteLine("<tr><td>Параметр</td>");
                        int calc_number = 1;
                        foreach (var calc in calcs.Keys.Where(key => key.Contains("верхнего")))
                        {
                            sw.WriteLine("<td>Расчет " + (calc_number++).ToString() + "</td>");
                        }
                        sw.WriteLine("</tr>");

                        WriteParamToResultFile("Sзаз", "S<sub>заз</sub>,&nbsp;мм<sup>2</sup>", calcs, "верхнего");
                        WriteParamToResultFile("Sзаз1", "S<sub>заз1</sub>,&nbsp;мм<sup>2</sup>", calcs, "верхнего");
                        WriteParamToResultFile("Sзаз2", "S<sub>заз2</sub>,&nbsp;мм<sup>2</sup>", calcs, "верхнего");
                        WriteParamToResultFile("Sяр", "S<sub>яр</sub>,&nbsp;мм<sup>2</sup>", calcs, "верхнего");
                        WriteParamToResultFile("Sяк", "S<sub>як</sub>,&nbsp;мм<sup>2</sup>", calcs, "верхнего");
                        WriteParamToResultFile("lяр", "l<sub>яр</sub>,&nbsp;мм", calcs, "верхнего");
                        WriteParamToResultFile("lяк", "l<sub>як</sub>,&nbsp;мм", calcs, "верхнего");
                        WriteParamToResultFile("lпол", "l<sub>пол</sub>,&nbsp;мм", calcs, "верхнего");
                        WriteParamToResultFile("ν", "ν", calcs, "верхнего");
                        WriteParamToResultFile("lср", "l<sub>ср</sub>,&nbsp;мм", calcs, "верхнего");
                        WriteParamToResultFile("ls", "l<sub>s</sub>,&nbsp;мм", calcs, "верхнего");
                        WriteParamToResultFile("r20", "r<sub>20</sub>,&nbsp;Ом", calcs, "верхнего");
                        WriteParamToResultFile("rГ", "r<sub>Г</sub>,&nbsp;Ом", calcs, "верхнего");
                        WriteParamToResultFile("I", "I,&nbsp;А", calcs, "верхнего");
                        WriteParamToResultFile("Fм", "F<sub>м</sub>,&nbsp;А", calcs, "верхнего");
                        WriteParamToResultFile("Qм", "Q<sub>м</sub>,&nbsp;мм<sup>2</sup>", calcs, "верхнего");
                        WriteParamToResultFile("Kм", "K<sub>м</sub>", calcs, "верхнего");
                        WriteParamToResultFile("Фδ", "Ф<sub>δ</sub>,&nbsp;Вб", calcs, "верхнего");
                        WriteParamToResultFile("Bδ", "B<sub>δ</sub>,&nbsp;Тл", calcs, "верхнего");
                        WriteParamToResultFile("Fδ", "F<sub>δ</sub>,&nbsp;А", calcs, "верхнего");
                        WriteParamToResultFile("Фяр", "Ф<sub>яp</sub>,&nbsp;Вб", calcs, "верхнего");
                        WriteParamToResultFile("Bяр", "B<sub>яр</sub>,&nbsp;Тл", calcs, "верхнего");
                        WriteParamToResultFile("Fяр", "F<sub>яр</sub>,&nbsp;А", calcs, "верхнего");
                        WriteParamToResultFile("Фяк", "Ф<sub>як</sub>,&nbsp;Вб", calcs, "верхнего");
                        WriteParamToResultFile("Bяк", "B<sub>як</sub>,&nbsp;Тл", calcs, "верхнего");
                        WriteParamToResultFile("Fяк", "F<sub>як</sub>,&nbsp;А", calcs, "верхнего");
                        WriteParamToResultFile("Фp", "Ф<sub>p</sub>,&nbsp;Вб", calcs, "верхнего");
                        WriteParamToResultFile("Bp1", "B<sub>p1</sub>,&nbsp;Тл", calcs, "верхнего");
                        WriteParamToResultFile("Bp2", "B<sub>p2</sub>,&nbsp;Тл", calcs, "верхнего");
                        WriteParamToResultFile("Fp1", "F<sub>p1</sub>&nbsp;A", calcs, "верхнего");
                        WriteParamToResultFile("Fp2", "F<sub>p2</sub>&nbsp;A", calcs, "верхнего");
                        WriteParamToResultFile("F", "F,&nbsp;А", calcs, "верхнего");
                        WriteParamToResultFile("Wp", "W<sub>p</sub>,&nbsp;кгс∙см", calcs, "верхнего");
                        WriteParamToResultFile("Fтм", "F<sub>тм</sub>,&nbsp;Н", calcs, "верхнего");
                        WriteParamToResultFile("P", "P,&nbsp;Вт", calcs, "верхнего");
                        WriteParamToResultFile("Δt", "Δt,&nbsp;°С", calcs, "верхнего");
                        WriteParamToResultFile("Kt", "Вт/см<sup>2</sup>&nbsp;°С", calcs, "верхнего");
                        sw.WriteLine("</table>");
                        sw.WriteLine("</div>");                        

                        sw.WriteLine("<div class='view-source'><a href='#'>Нижние электромагниты</a>");
                        sw.WriteLine("<div class='hide'>");
                        sw.WriteLine("<table class='table table-striped table-fit'>");

                        // Делаем шапку таблицы

                        sw.WriteLine("<tr><td>Параметр</td>");
                        calc_number = 1;
                        foreach (var calc in calcs.Keys.Where(key => key.Contains("нижнего")))
                        {
                            sw.WriteLine("<td>Расчет " + (calc_number++).ToString() + "</td>");
                        }
                        sw.WriteLine("</tr>");

                        WriteParamToResultFile("Sзаз", "S<sub>заз</sub>,&nbsp;мм<sup>2</sup>", calcs, "нижнего");
                        WriteParamToResultFile("Sзаз1", "S<sub>заз1</sub>,&nbsp;мм<sup>2</sup>", calcs, "нижнего");
                        WriteParamToResultFile("Sзаз2", "S<sub>заз2</sub>,&nbsp;мм<sup>2</sup>", calcs, "нижнего");
                        WriteParamToResultFile("Sяр", "S<sub>яр</sub>,&nbsp;мм<sup>2</sup>", calcs, "нижнего");
                        WriteParamToResultFile("Sяк", "S<sub>як</sub>,&nbsp;мм<sup>2</sup>", calcs, "нижнего");
                        WriteParamToResultFile("lяр", "l<sub>яр</sub>,&nbsp;мм", calcs, "нижнего");
                        WriteParamToResultFile("lяк", "l<sub>як</sub>,&nbsp;мм", calcs, "нижнего");
                        WriteParamToResultFile("lпол", "l<sub>пол</sub>,&nbsp;мм", calcs, "нижнего");
                        WriteParamToResultFile("ν", "ν", calcs, "нижнего");
                        WriteParamToResultFile("lср", "l<sub>ср</sub>,&nbsp;мм", calcs, "нижнего");
                        WriteParamToResultFile("ls", "l<sub>s</sub>,&nbsp;мм", calcs, "нижнего");
                        WriteParamToResultFile("r20", "r<sub>20</sub>,&nbsp;Ом", calcs, "нижнего");
                        WriteParamToResultFile("rГ", "r<sub>Г</sub>,&nbsp;Ом", calcs, "нижнего");
                        WriteParamToResultFile("I", "I,&nbsp;А", calcs, "нижнего");
                        WriteParamToResultFile("Fм", "F<sub>м</sub>,&nbsp;А", calcs, "нижнего");
                        WriteParamToResultFile("Qм", "Q<sub>м</sub>,&nbsp;мм<sup>2</sup>", calcs, "нижнего");
                        WriteParamToResultFile("Kм", "K<sub>м</sub>", calcs, "нижнего");
                        WriteParamToResultFile("Фδ", "Ф<sub>δ</sub>,&nbsp;Вб", calcs, "нижнего");
                        WriteParamToResultFile("Bδ", "B<sub>δ</sub>,&nbsp;Тл", calcs, "нижнего");
                        WriteParamToResultFile("Fδ", "F<sub>δ</sub>,&nbsp;А", calcs, "нижнего");
                        WriteParamToResultFile("Фяр", "Ф<sub>яp</sub>,&nbsp;Вб", calcs, "нижнего");
                        WriteParamToResultFile("Bяр", "B<sub>яр</sub>,&nbsp;Тл", calcs, "нижнего");
                        WriteParamToResultFile("Fяр", "F<sub>яр</sub>,&nbsp;А", calcs, "нижнего");
                        WriteParamToResultFile("Фяк", "Ф<sub>як</sub>,&nbsp;Вб", calcs, "нижнего");
                        WriteParamToResultFile("Bяк", "B<sub>як</sub>,&nbsp;Тл", calcs, "нижнего");
                        WriteParamToResultFile("Fяк", "F<sub>як</sub>,&nbsp;А", calcs, "нижнего");
                        WriteParamToResultFile("Фp", "Ф<sub>p</sub>,&nbsp;Вб", calcs, "нижнего");
                        WriteParamToResultFile("Bp1", "B<sub>p1</sub>,&nbsp;Тл", calcs, "нижнего");
                        WriteParamToResultFile("Bp2", "B<sub>p2</sub>,&nbsp;Тл", calcs, "нижнего");
                        WriteParamToResultFile("Fp1", "F<sub>p1</sub>&nbsp;A", calcs, "нижнего");
                        WriteParamToResultFile("Fp2", "F<sub>p2</sub>&nbsp;A", calcs, "нижнего");
                        WriteParamToResultFile("F", "F,&nbsp;А", calcs, "нижнего");
                        WriteParamToResultFile("Wp", "W<sub>p</sub>,&nbsp;кгс∙см", calcs, "нижнего");
                        WriteParamToResultFile("Fтм", "F<sub>тм</sub>,&nbsp;Н", calcs, "нижнего");
                        WriteParamToResultFile("P", "P,&nbsp;Вт", calcs, "нижнего");
                        WriteParamToResultFile("Δt", "Δt,&nbsp;°С", calcs, "нижнего");
                        WriteParamToResultFile("Kt", "Вт/см<sup>2</sup>&nbsp;°С", calcs, "нижнего");

                        sw.WriteLine("</table>");
                        sw.WriteLine("</div>");
                    }

                sw.WriteLine("<h3>Суммарное значение F<sub>тм</sub> для верхних электромагнитов</h3>");

                sw.WriteLine("<table class='table table-striped table-fit'>");
                sw.WriteLine("<tr>");

                for (int i = 0; i < FтмSumUp.Count; i++)
                {
                    sw.WriteLine("<td>Расчет " + (i + 1).ToString() + "</td>");
                }
                
                sw.WriteLine("</tr><tr>");                

                for (int i = 0; i < FтмSumUp.Count; i++)
                {
                    sw.WriteLine("<td>" + FтмSumUp.ElementAt(i).Value.ToString("F5") + "</td>");
                }
                sw.WriteLine("</tr>");
                sw.WriteLine("</table>");

                sw.WriteLine("<h3>Суммарное значение F<sub>тм</sub> для нижних электромагнитов</h3>");

                sw.WriteLine("<table class='table table-striped table-fit'>");
                sw.WriteLine("<tr>");

                for (int i = 0; i < FтмSumDwn.Count; i++)
                {
                    sw.WriteLine("<td>Расчет " + (i + 1).ToString() + "</td>");
                }

                sw.WriteLine("</tr><tr>");

                for (int i = 0; i < FтмSumDwn.Count; i++)
                {
                    sw.WriteLine("<td>" + FтмSumDwn.ElementAt(i).Value.ToString("F5") + "</td>");
                }
                sw.WriteLine("</tr>");
                sw.WriteLine("</table>");

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
