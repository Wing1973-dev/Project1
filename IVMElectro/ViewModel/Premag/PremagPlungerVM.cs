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
using System.IO;
using System;

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
        StreamWriter sw; // Поток для записи в файл с результатом расчета
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


        // Записать параметр в файл с результатом расчета
        private void WriteParamToResultFile(string param, string caption)
        {
            sw.WriteLine("<tr><td>" + caption + ":</td>");
            for (int i = 0; i < resultCalculation.Count; i++)
            {
                sw.WriteLine("<td>" + resultCalculation.ElementAt(i).Value[param].ToString("F5")  + "</td>");
            }
            sw.WriteLine("</tr>");
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

                string file_name = Directory.GetCurrentDirectory() + "\\report_" + Path.GetFileNameWithoutExtension(IVMElectro.Services.ServiceIO.FileName) + ".html";

                // Создаем поток для записи в файл
                sw = new StreamWriter(file_name);

                sw.WriteLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<meta http-equiv='content-type' content='text/html; charset=UTF-8' />");

                sw.WriteLine("<link href = 'css/bootstrap.min.css' rel='stylesheet'>");

                sw.WriteLine("<title>Результаты расчета</title>");
                sw.WriteLine("<style>.table-fit { width: 1px;} h2 {background-color: #d9d9d9;} h3 {background-color: #ccccff}</style>");

                sw.WriteLine("</head>");
                sw.WriteLine("<body><div class='mx-auto' style='width: 1024px;'>");

                sw.WriteLine("<h1>Результаты расчета</h1>");

                sw.WriteLine("<h2>Электромагнит постоянного тока с плунжером</h2>");

                sw.WriteLine("<table class='table table-striped table-fit'>");

                sw.WriteLine("<tr><td><b>Параметр</b></td>");

                for (int i = 0; i < VariationData.Count; i++)
                {
                    sw.WriteLine("<td><b>Расчет&nbsp;" + (i + 1).ToString() + "</b></td>");
                }
                sw.WriteLine("</tr>");

                WriteParamToResultFile("δэ", "δ<sub>э</sub>,&nbsp;мм");
                WriteParamToResultFile("S1", "S<sub>1</sub>,&nbsp;мм<sup>2</sup>");
                WriteParamToResultFile("S11", "S<sub>11</sub>,&nbsp;мм<sup>2</sup>");
                WriteParamToResultFile("S12", "S<sub>12</sub>,&nbsp;мм<sup>2</sup>");
                WriteParamToResultFile("Sстоп", "S<sub>стоп</sub>,&nbsp;мм<sup>2</sup>");
                WriteParamToResultFile("Sкор", "S<sub>кор</sub>,&nbsp;мм<sup>2</sup>");
                WriteParamToResultFile("Sпз", "S<sub>пз</sub>,&nbsp;мм<sup>2</sup>");
                WriteParamToResultFile("lпл.ст", "l<sub>пл.ст</sub>,&nbsp;мм");
                WriteParamToResultFile("lк", "l<sub>к</sub>,&nbsp;мм");
                WriteParamToResultFile("lфл", "l<sub>фл</sub>,&nbsp;мм");
                WriteParamToResultFile("ν1", "ν<sub>1</sub>,&nbsp;");
                WriteParamToResultFile("ν2", "ν<sub>2</sub>,&nbsp;");
                WriteParamToResultFile("lср", "l<sub>ср</sub>,&nbsp;мм");
                WriteParamToResultFile("ls", "l<sub>s</sub>,&nbsp;мм");
                WriteParamToResultFile("r20", "r<sub>20</sub>,&nbsp;Ом");
                WriteParamToResultFile("rГ", "r<sub>Г</sub>,&nbsp;Ом");
                WriteParamToResultFile("I", "I,&nbsp;А");
                WriteParamToResultFile("Fм", "F<sub>м</sub>,&nbsp;А");
                WriteParamToResultFile("Qм", "Q<sub>м</sub>,&nbsp;мм<sup>2</sup>");
                WriteParamToResultFile("Kм", "K<sub>м</sub>");
                WriteParamToResultFile("Фδ", "Ф<sub>δ</sub>,&nbsp;Мкс");
                WriteParamToResultFile("Bδ", "B<sub>δ</sub>,&nbsp;Гс");
                WriteParamToResultFile("Fδ", "F<sub>δ</sub>,&nbsp;А");
                WriteParamToResultFile("Фp", "Ф<sub>p</sub>,&nbsp;Мкс");
                WriteParamToResultFile("Bp1", "B<sub>p1</sub>,&nbsp;Гс");
                WriteParamToResultFile("Bp11", "B<sub>p11</sub>,&nbsp;Гс");
                WriteParamToResultFile("Bp12", "B<sub>p12</sub>,&nbsp;Гс");
                WriteParamToResultFile("Fp", "F<sub>p</sub>,&nbsp;А");
                WriteParamToResultFile("Фк", "Ф<sub>к</sub>,&nbsp;Мкс");
                WriteParamToResultFile("Bк", "B<sub>к</sub>,&nbsp;Гс");
                WriteParamToResultFile("Fк", "F<sub>к</sub>,&nbsp;А");
                WriteParamToResultFile("Fфл", "F<sub>фл</sub>,&nbsp;А");
                WriteParamToResultFile("Фпз", "Ф<sub>пз</sub>,&nbsp;Мкс");
                WriteParamToResultFile("Bпз", "B<sub>пз</sub>,&nbsp;Гс");
                WriteParamToResultFile("Fпз", "F<sub>пз</sub>,&nbsp;А");
                WriteParamToResultFile("F", "F,&nbsp;А");
                WriteParamToResultFile("Wp", "W<sub>p</sub>,&nbsp;кгс∙см");
                WriteParamToResultFile("Fтм", "F<sub>тм</sub>,&nbsp;кг");
                WriteParamToResultFile("P", "P,&nbsp;Вт");
                WriteParamToResultFile("Kt", "Вт/см<sup>2</sup>&nbsp;°С");
                WriteParamToResultFile("Δt", "Δ<sub>t</sub>,&nbsp;°С");

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
