using System;
using System.Collections.Generic;
using System.ComponentModel;
using IVMElectro.Models;
using IVMElectro.Commands;
using System.Windows.Input;
using static LibraryAlgorithms.Services.ServiceDT;
using IVMElectro.Services.Directories.WireDirectory;
using static IVMElectro.Services.DataSharedASDNContent;
using System.Data;
using IVMElectro.Services.Directories;
using System.ComponentModel.DataAnnotations;
using NLog;
using System.Linq;
using LibraryAlgorithms;
using System.IO;

namespace IVMElectro.ViewModel {
    class AsdnRedSingleViewModel : INotifyPropertyChanged, IDataErrorInfo {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        string IDataErrorInfo.this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "ΔГ1":
                        if (Model.Common.ΔГ1 < 0 || double.IsNaN(Model.Common.ΔГ1))
                            error = errorΔГ1;
                        break;
                    case "ΔГ2":
                        if (!((0 <= Model.Common.ΔГ2) && (Model.Common.ΔГ2 <= 5)))
                            error = errorΔГ2;
                        break;
                    case "Di":
                        if (!((50 < Model.Common.Di) && (Model.Common.Di <= 800)))
                            error = errorDi;
                        break;
                    case "Dpст":
                        if (Model.Common.Dpст < 0 || double.IsNaN(Model.Common.Dpст))
                            error = errorDpст;
                        break;
                    case "Da":
                        //diapason problem
                        if ((Get_DaBounds(Model.Common.Di).left == Get_DaBounds(Model.Common.Di).right) ||
                        double.IsNaN(Get_DaBounds(Model.Common.Di).left) || double.IsNaN(Get_DaBounds(Model.Common.Di).right))
                            error = errorDa;
                        else if (!((Get_DaBounds(Model.Common.Di).left <= Model.Common.Da) && (Model.Common.Da < Get_DaBounds(Model.Common.Di).right)))
                            error = $"Значение параметра {columnName} должно принадлежать {DaBounds}.";
                        break;
                    case "Pмех":
                        if (!((0 <= Model.Common.Pмех) && (Model.Common.Pмех <= PмехBoundRight(Model.Common.P12))))
                            error = $"Значение параметра {columnName} должно принадлежать {PмехBounds}.";
                        break;
                    case "P12":
                        if (!((50 <= Model.Common.P12) && (Model.Common.P12 <= 2e5))) {
                            error = errorP12;
                        }
                        break;
                    case "f1":
                        if (!((10 <= Model.Common.f1) && (Model.Common.f1 <= 400)))
                            error = errorf1;
                        break;
                    case "a2":
                        if (!((0 <= Model.Common.a2) && (Model.Common.a2 <= 30)))
                            error = errora2;
                        break;
                    case "bП":
                        if (Model.Common.bП < 0 || double.IsNaN(Model.Common.bП))
                            error = errorbП;
                        break;
                    case "bП1":
                        if (Model.Common.bП1 < 0 || double.IsNaN(Model.Common.bП1))
                            error = errorbП1;
                        break;
                    case "β":
                        if (!((0.5 <= Model.Common.β) && (Model.Common.β <= 0.95)))
                            error = errorβ;
                        break;
                    case "bП2":
                        if (PAS == "прямоугольный" || PAS == "двойная клетка")
                            if ((Model.AsdnRedSingle.bП2 < 0) || double.IsNaN(Model.AsdnRedSingle.bП2))
                                error = errorbП2RED;
                        break;
                    case "bк":
                        if (PAS == "прямоугольный" || PAS == "двойная клетка") {
                            if (!((Model.AsdnRedSingle.bП2 <= Model.AsdnRedSingle.bк) && (Model.AsdnRedSingle.bк <= 5 * Model.AsdnRedSingle.bП2)))
                                error = $"Значение параметра {columnName} должно принадлежать {bкBounds}.";
                        }
                        else if (Model.AsdnRedSingle.bк <= 0 || double.IsNaN(Model.AsdnRedSingle.bк))
                            error = errorbкRED;
                        //parameter is always needed
                        break;
                    case "U1":
                        if (!((48 <= Model.Common.U1) && (Model.Common.U1 <= 660)))
                            error = errorU1;
                        break;
                    case "h1":
                        if (Model.Common.h1 < 0 || double.IsNaN(Model.Common.h1))
                            error = errorh1;
                        break;
                    case "h2":
                        if (Model.Common.h2 < 0 || double.IsNaN(Model.Common.h2))
                            error = errorh2;
                        break;
                    case "h3":
                        if (!((0 <= Model.Common.h3) && (Model.Common.h3 <= 5)))
                            error = errorh3;
                        break;
                    case "h4":
                        if (!((5 <= Model.Common.h4) && (Model.Common.h4 <= 50)))
                            error = errorh4;
                        break;
                    case "h5":
                        if (!((0.1 <= Model.Common.h5) && (Model.Common.h5 <= 5)))
                            error = errorh5;
                        break;
                    case "h6":
                        if (!((0 <= Model.Common.h6) && (Model.Common.h6 <= 20)))
                            error = errorh6;
                        break;
                    case "h7":
                        if (!((0 <= Model.Common.h7) && (Model.Common.h7 <= 2)))
                            error = errorh7;
                        break;
                    case "h8":
                        if (!((0 <= Model.Common.h8) && (Model.Common.h8 <= 20)))
                            error = errorh8;
                        break;
                    case "li":
                        if (double.IsNaN(Get_liBounds(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di).left) ||
                            double.IsNaN(Get_liBounds(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di).right))
                            error = errordiapason;
                        else if (!((Get_liBounds(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di).left <= Model.Common.li) &&
                            (Model.Common.li <= Get_liBounds(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di).right)))
                            error = $"Значение параметра li должно принадлежать [{Get_liBounds(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di).left} : " +
                                $"{Get_liBounds(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di).right}].";
                        break;
                    case "ρ1x":
                        if (!((0.002 <= Model.Common.ρ1x) && (Model.Common.ρ1x <= 0.05)))
                            error = errorρ1x;
                        break;
                    case "ρ1Г":
                        if (!((Model.Common.ρ1x <= Model.Common.ρ1Г) && (Model.Common.ρ1Г <= 0.1235)))
                            error = $"Значение параметра расчета {columnName} должно принадлежать {ρ1ГBounds}.";
                        break;
                    case "ρ2Г":
                        if (!((0.01 <= Model.Common.ρ2Г) && (Model.Common.ρ2Г <= 0.2)))
                            error = errorρ2Г;
                        break;
                    case "ac":
                        if (!(Model.Common.dиз < Model.Common.ac))
                            error = $"Значение параметра расчета {columnName} должно должно быть > {Model.Common.dиз}.";
                        break;
                    case "Kfe1":
                        if (!((0.9 <= Model.Common.Kfe1) && (Model.Common.Kfe1 <= 1)))
                            error = errorKfe1;
                        break;
                    case "Kfe2":
                        if (!((0.9 <= Model.Common.Kfe2) && (Model.Common.Kfe2 <= 1)))
                            error = errorKfe2;
                        break;
                    case "Δкр":
                        if (!((3 <= Model.Common.Δкр) && (Model.Common.Δкр <= 40)))
                            error = errorΔкр;
                        break;
                    case "d1":
                        if (!((0 <= Model.Common.d1) && (Model.Common.d1 <= bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1))))
                            error = $"Значение параметра расчета {columnName} должно принадлежать {d1Bounds}.";
                        break;
                    case "y1":
                        //diapason problem
                        if (0.5 * Model.Common.Z1 / Convert.ToDouble(Model.Common.p) <= 1)
                            error = errory1;
                        else if (!((1 <= Model.Common.y1) && (Model.Common.y1 <= 0.5 * Model.Common.Z1 / Convert.ToDouble(Model.Common.p))))
                            error = $"Значение параметра расчета {columnName} должно принадлежать {y1Bounds}.";
                        break;
                    case "B":
                        if (!((0 <= Model.Common.B) && (Model.Common.B <= 20)))
                            error = errorB;
                        break;
                    case "cз":
                        if (!((0 <= Model.Common.cз) && (Model.Common.cз <= 200)))
                            error = errorcз;
                        break;
                    case "bz1":
                        if (!((3.5 <= Model.Common.bz1) && (Model.Common.bz1 <= 15))) 
                            error = errorbz1;
                        break;
                    case "W1":
                        if ((Model.Common.W1 < 0) || !int.TryParse(W1, out _))
                            error = errorW1;
                        else if (Model.Common.W1 % 2 != 0)
                            error = errorW1parity;
                        break;
                    case "Wc":
                        if ((Model.Common.Wc < 0) || !int.TryParse(Wc, out _))
                            error = errorWc;
                        break;
                    case "hp":
                        if ((Model.Common.hp < 0) || double.IsNaN(Model.Common.hp))
                            error = errorhp;
                        break;
                    case "dв":
                        if ((Model.AsdnRedSingle.dв < 0) || double.IsNaN(Model.AsdnRedSingle.dв))
                            error = errordв;
                        break;
                    case "ρРУБ":
                        if ((Model.Common.ρРУБ < 0) || double.IsNaN(Model.Common.ρРУБ))
                            error = errorρРУБ;
                        break;
                    case "Z2":
                        if ((Model.Common.Z2 < 0) || !int.TryParse(Z2, out _))
                            error = errorZ2;
                        break;
                    case "Z1":
                        if ((Model.Common.Z1 <= 0) || !int.TryParse(Z1, out _)) 
                            error = errorZ1;
                        break;
                    case "K2":
                        if ((Model.Common.K2 < 0) || double.IsNaN(Model.Common.K2))
                            error = errorK2;
                        break;
                    case "qГ":
                        if ((Model.Common.qГ < 0) || double.IsNaN(Model.Common.qГ))
                            error = errorqГ;
                        break;
                    case "dиз":
                        if ((Model.Common.dиз < 0) || double.IsNaN(Model.Common.dиз))
                            error = errordиз;
                        break;
                    case "a1":
                        if ((Model.Common.a1 < 0) || !int.TryParse(a1, out _)) 
                            error = errora1;
                        break;
                    case "aк":
                        if (PAS == "двойная клетка") {
                            if (bounds_aк(Model.AsdnRedSingle.dкп, Model.AsdnRedSingle.hш, Model.AsdnRedSingle.hp2).left ==
                                bounds_aк(Model.AsdnRedSingle.dкп, Model.AsdnRedSingle.hш, Model.AsdnRedSingle.hp2).right)
                                error = errordiapason;
                            else if (!((bounds_aк(Model.AsdnRedSingle.dкп, Model.AsdnRedSingle.hш, Model.AsdnRedSingle.hp2).left <= Model.AsdnRedSingle.aк) &&
                            (Model.AsdnRedSingle.aк <= bounds_aк(Model.AsdnRedSingle.dкп, Model.AsdnRedSingle.hш, Model.AsdnRedSingle.hp2).right)))
                                error = $"Значение параметра {columnName} должно принадлежать {aкBounds}.";
                        }
                        else if (Model.AsdnRedSingle.aк <= 0 || double.IsNaN(Model.AsdnRedSingle.aк))
                            error = erroraкRED;
                        //parameter is always needed
                        break;
                    case "p10_50":
                        if ((Model.Common.p10_50 < 0) || double.IsNaN(Model.Common.p10_50))
                            error = errorp10_50;
                        break;
                    case "hш":
                        if (!((0.5 <= Model.AsdnRedSingle.hш) && (Model.AsdnRedSingle.hш <= 1)))
                            error = errorhш;
                        break;
                    case "bш":
                        if (!((1 <= Model.AsdnRedSingle.bш) && (Model.AsdnRedSingle.bш <= 2.5)))
                            error = errorbш;
                        break;
                    case "dкп":
                        if (PAS == "круглый" || PAS == "двойная клетка") {
                            if (double.IsNaN(Get_dкпBounds(Model.Common.Dpст, Model.Common.Z2).left) ||
                                double.IsNaN(Get_dкпBounds(Model.Common.Dpст, Model.Common.Z2).right))
                                error = errordкп;
                            else if (Get_dкпBounds(Model.Common.Dpст, Model.Common.Z2).left >= Get_dкпBounds(Model.Common.Dpст, Model.Common.Z2).right)
                                error = errordкп;
                            else if (!((Get_dкпBounds(Model.Common.Dpст, Model.Common.Z2).left <= Model.AsdnRedSingle.dкп) &&
                            (Model.AsdnRedSingle.dкп <= Get_dкпBounds(Model.Common.Dpст, Model.Common.Z2).right)))
                                        error = $"Значение параметра {columnName} должно принадлежать {dкпBounds}.";
                        } //all other options (прямоугольный, грушевидный) are blocked
                        break;
                    case "bZH":
                        if (double.IsNaN(bounds_bZH(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2).left) ||
                            double.IsNaN(bounds_bZH(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2).right))
                            error = errorbZH;
                        else if (bounds_bZH(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2).left ==
                            bounds_bZH(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2).right)
                            error = errorbZH;
                        else if (!((bounds_bZH(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2).left <= Model.AsdnRedSingle.bZH) &&
                            (Model.AsdnRedSingle.bZH <= bounds_bZH(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2).right)))
                            error = $"Значение параметра {columnName} должно принадлежать {bZHBounds}.";
                        break;
                    case "hр2":
                        if (PAS == "двойная клетка") 
                            if (!((3 <= Model.AsdnRedSingle.hp2) && (Model.AsdnRedSingle.hp2 <= 10)))
                                error = errorhp2RED;
                        break;
                    case "bкн":
                        if (PAS == "двойная клетка")
                            if (!((5 <= Model.AsdnRedSingle.bкн) && (Model.AsdnRedSingle.bкн <= 35)))
                                error = errorbкн;
                        break;
                    case "aкн":
                        if (PAS == "двойная клетка")
                            if (!((bounds_aкн(Model.AsdnRedSingle.dпн, Model.AsdnRedSingle.dпв, Model.AsdnRedSingle.hp1).left <= Model.AsdnRedSingle.aкн) && 
                                (Model.AsdnRedSingle.aкн <= 1.2 * bounds_aкн(Model.AsdnRedSingle.dпн, Model.AsdnRedSingle.dпв, Model.AsdnRedSingle.hp1).right)))
                                error = $"Значение параметра {columnName} должно принадлежать {aкнBounds}.";
                        break;
                    case "dпн":
                        if (PAS == "двойная клетка" || PAS == "грушевидный")
                            if (double.IsNaN(Model.AsdnRedSingle.dпн) || Model.AsdnRedSingle.dпн <= 0) error = errordпн;
                        break;
                    case "dпв":
                        if (PAS == "двойная клетка" || PAS == "грушевидный")
                            if (double.IsNaN(Model.AsdnRedSingle.dпв) || Model.AsdnRedSingle.dпв <= 0) error = errordпв;
                        break;
                    case "hp1":
                        if (PAS == "прямоугольный")
                            if (double.IsNaN(Model.AsdnRedSingle.hp1) || Model.AsdnRedSingle.hp1 <= 0) error = errorhp1;
                        break;
                }
                return error;
            }
        }
        //double dпн => dпн(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2, Model.AsdnRedSingle.bZH);
        //double dпв => dпв(Model.Common.Z2, Model.Common.Dpст, Model.AsdnRedSingle.hш, Model.AsdnRedSingle.bZH);
        //double hp1 => hp1(dпв, Model.AsdnRedSingle.dпн, Model.Common.Z2);
        
        public string Error { get; }
        public AsdnRedSingleViewModel(AsdnCompositeModel model, Logger logger) {
            Model = model; Logger = logger;
            WireDirectory = new List<Wire> { 
                new WireПНЭТ_имид { Id = 1, NameWire = "ПНЭТ_имид – ТУ 16-505.489-78" }, 
                new WireПСДК_Л { Id = 2, NameWire = "ПСДК_Л – ТУ 16.К71-129-91" },
                new WireПСДКТ_Л{Id = 3, NameWire = "ПСДКТ_Л – ТУ 16.К71-129-91" }, 
                new WireПЭЭИД2{ Id = 4, NameWire = "ПЭЭИД2 – ТУ 16.К71-250-95" } };
            WireDirectory.ForEach(i => i.CreateTable());
            MarkSteelPartitionlDirectory = new List<SteelProperties> { new SteelProperties { Id = 1, Name = "ХН78Т", Value = 1.16 }, new SteelProperties {Id = 2, Name = "ВТ-1", Value =  0.47 },
                new SteelProperties {Id = 3, Name = "ПТ-7М", Value = 1.08 } };
            MarkSteelStatorDirectory = new List<SteelProperties> { new SteelProperties { Id = 1, Name = "1412", Value = 1.94 }, new SteelProperties { Id = 2, Name = "2412", Value = 1.38 },
                new SteelProperties {Id = 3, Name = "2411", Value =  1.7 }, new SteelProperties {Id = 4, Name = "1521", Value = 19.6 } };
        }
        #region properties 
        public AsdnCompositeModel Model { get; set; }
        Logger Logger { get; set; }
        string diagnostic = string.Empty;
        public string Diagnostic { get => diagnostic; set { diagnostic = value; OnPropertyChanged("Diagnostic"); } }
        #region machine parameters
        public string P12 { get => Model.Common.P12.ToString(); set { Model.Common.P12 = StringToDouble(value); OnPropertyChanged("P12"); } }
        public string U1 { get => Model.Common.U1.ToString(); set { Model.Common.U1 = StringToDouble(value); OnPropertyChanged("U1"); } }
        public string f1 { get => Model.Common.f1.ToString(); set { Model.Common.f1 = StringToDouble(value); OnPropertyChanged("f1"); } }
        public string p { get => Model.Common.p.ToString(); set { Model.Common.p = StringToInt(value); OnPropertyChanged("p"); } } 
        public string Pмех { get => Model.Common.Pмех.ToString(); set { Model.Common.Pмех = StringToDouble(value); OnPropertyChanged("Pмех"); } }
        public string PмехBounds { get => $"[0 : {PмехBoundRight(Model.Common.P12)}]"; } //label
        #endregion
        #region stator parameters
        public string Di { get => Model.Common.Di.ToString(); set { Model.Common.Di = StringToDouble(value); OnPropertyChanged("Di"); } }
        public string ΔГ1 { get => Model.Common.ΔГ1.ToString(); set { Model.Common.ΔГ1 = StringToDouble(value); OnPropertyChanged("ΔГ1"); } }
        public string Z1 { get => Model.Common.Z1.ToString(); set { Model.Common.Z1 = StringToInt(value); OnPropertyChanged("Z1"); } }
        public string q1 { get => Math.Round(Model.Common.q1, 3).ToString(); } //label
        public string Da { get => Model.Common.Da.ToString(); set { Model.Common.Da = StringToDouble(value); OnPropertyChanged("Da"); } }
        public string DaBounds { get => $"[{Math.Round(Get_DaBounds(Model.Common.Di).left, 2)} : {Math.Round(Get_DaBounds(Model.Common.Di).right, 2)})"; } //label
        public string a1 { get => Model.Common.a1.ToString(); set { Model.Common.a1 = StringToInt(value); OnPropertyChanged("a1"); } }
        public string a2 { get => Model.Common.a2.ToString(); set { Model.Common.a2 = StringToInt(value); OnPropertyChanged("a2"); } }
        public string Δкр { get => Model.Common.Δкр.ToString(); set { Model.Common.Δкр = StringToDouble(value); OnPropertyChanged("Δкр"); } }
        public string dиз { get => Model.Common.dиз.ToString(); set { Model.Common.dиз = StringToDouble(value); OnPropertyChanged("dиз"); } }
        public string qГ { get => Model.Common.qГ.ToString(); set { Model.Common.qГ = StringToDouble(value); OnPropertyChanged("qГ"); } }
        public string bz1 { get => Model.Common.bz1.ToString(); set { Model.Common.bz1 = StringToDouble(value); OnPropertyChanged("bz1"); } }
        public string h8 { get => Model.Common.h8.ToString(); set { Model.Common.h8 = StringToDouble(value); OnPropertyChanged("h8"); } }
        public string h7 { get => Model.Common.h7.ToString(); set { Model.Common.h7 = StringToDouble(value); OnPropertyChanged("h7"); } }
        public string h6 { get => Model.Common.h6.ToString(); set { Model.Common.h6 = StringToDouble(value); OnPropertyChanged("h6"); } }
        public string bП1 { get => Model.Common.bП1.ToString(); set { Model.Common.bП1 = StringToDouble(value); OnPropertyChanged("bП1"); } }
        public string bП1_calc { get => Math.Round(Model.Common.bП1_calc, 3).ToString(); } //label
        public string h5 { get => Model.Common.h5.ToString(); set { Model.Common.h5 = StringToDouble(value); OnPropertyChanged("h5"); } }
        public string h3 { get => Model.Common.h3.ToString(); set { Model.Common.h3 = StringToDouble(value); OnPropertyChanged("h3"); } }
        public string h4 { get => Model.Common.h4.ToString(); set { Model.Common.h4 = StringToDouble(value); OnPropertyChanged("h4"); } }
        public string ac { get => Model.Common.ac.ToString(); set { Model.Common.ac = StringToDouble(value); OnPropertyChanged("ac"); } }
        public string acBounds { get => $"({Math.Round(Model.Common.dиз, 1)} : {Math.Round(2 * Model.Common.dиз, 1)})"; } //label
        public string bПН { get => Model.Common.bПН.ToString(); } //label
        public string h1 { get => Model.Common.h1.ToString(); set { Model.Common.h1 = StringToDouble(value); OnPropertyChanged("h1"); } } //dep 51
        public string h1_calc { get => Math.Round(Model.Common.h1_calc, 3).ToString(); } //label
        public string h2 { get => Model.Common.h2.ToString(); set { Model.Common.h2 = StringToDouble(value); OnPropertyChanged("h2"); } } //dep 51
        public string h2_calc { get => Math.Round(Model.Common.h2_calc, 3).ToString(); } //label
        public string li { get => Model.Common.li.ToString(); set { Model.Common.li = StringToDouble(value); OnPropertyChanged("li"); } }
        public string liBounds { get => Get_liBounds_string(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di); } //label
        public string cз { get => Model.Common.cз.ToString(); set { Model.Common.cз = StringToDouble(value); OnPropertyChanged("cз"); } }
        public string bП { get => Model.Common.bП.ToString(); set { Model.Common.bП = StringToDouble(value); OnPropertyChanged("bП"); } } 
        public string bП_calc { get => Math.Round(Model.Common.bП_calc, 3).ToString(); } //label
        public string W1 { get => Model.Common.W1.ToString(); set { Model.Common.W1 = StringToInt(value); OnPropertyChanged("W1"); } }
        public string Wc { get => Model.Common.Wc.ToString(); set { Model.Common.Wc = StringToInt(value); OnPropertyChanged("Wc"); } }
        public string Wc_calc { get => Math.Round(Model.Common.Wc_calc, 3).ToString(); } //label
        public string Kзап { get => Math.Round(Model.Common.Kзап, 3).ToString(); } //label
        public string y1 { get => Model.Common.y1.ToString(); set { Model.Common.y1 = StringToDouble(value); OnPropertyChanged("y1"); } }
        public string y1Bounds { get => $"[1 : {Math.Round(0.5 * Model.Common.Z1 / Convert.ToDouble(Model.Common.p), 3)}]"; } //label
        public string β { get => Math.Round(Model.Common.β, 3).ToString(); } //label
        public string K2 { get => Model.Common.K2.ToString(); set { Model.Common.K2 = StringToDouble(value); OnPropertyChanged("K2"); } }
        public string d1 { get => Model.Common.d1.ToString(); set { Model.Common.d1 = StringToDouble(value); OnPropertyChanged("d1"); } }
        public string d1Bounds { get => $"[0 : {Math.Round(bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1), 2)}]"; } //label
        public string Kfe1 { get => Model.Common.Kfe1.ToString(); set { Model.Common.Kfe1 = StringToDouble(value); OnPropertyChanged("Kfe1"); } }
        public string ρ1x { get => Model.Common.ρ1x.ToString(); set { Model.Common.ρ1x = StringToDouble(value); OnPropertyChanged("ρ1x"); } }
        public string ρРУБ { get => Model.Common.ρРУБ.ToString(); set { Model.Common.ρРУБ = StringToDouble(value); OnPropertyChanged("ρРУБ"); } }
        public string ρ1Г { get => Model.Common.ρ1Г.ToString(); set { Model.Common.ρ1Г = StringToDouble(value); OnPropertyChanged("ρ1Г"); } }
        public string ρ1ГBounds { get => $"[{Math.Round(Model.Common.ρ1x, 4)} : 0.1235]"; } //label
        public string B { get => Model.Common.B.ToString(); set { Model.Common.B = StringToDouble(value); OnPropertyChanged("B"); } }
        public string p10_50 { get => Model.Common.p10_50.ToString(); set { Model.Common.p10_50 = StringToDouble(value); OnPropertyChanged("p10_50"); } }
        public string PR { get => Model.Common.PR; set { Model.Common.PR = value; OnPropertyChanged("PR"); } }
        #endregion
        #region rotor parameters
        public string ΔГ2 { get => Model.Common.ΔГ2.ToString(); set { Model.Common.ΔГ2 = StringToDouble(value); OnPropertyChanged("ΔГ2"); } }
        public string Dpст { get => Model.Common.Dpст.ToString(); set { Model.Common.Dpст = StringToDouble(value); OnPropertyChanged("Dpст"); } }
        public string DpстBounds => $"[{Math.Round(Model.Common.DpстBoundCalculation - 5, 2)} : {Math.Round(Model.Common.DpстBoundCalculation - 0.1, 2)})";  //label
        public string bСК { get => Model.Common.bСК; set { Model.Common.bСК = value; OnPropertyChanged("bСК"); } } 
        public string bП2 { get => Model.AsdnRedSingle.bП2.ToString(); set { Model.AsdnRedSingle.bП2 = StringToDouble(value); OnPropertyChanged("bП2"); } }
        public string Z2 { get => Model.Common.Z2.ToString(); set { Model.Common.Z2 = StringToInt(value); OnPropertyChanged("Z2"); } }
        public string dв { get => Model.AsdnRedSingle.dв.ToString(); set { Model.AsdnRedSingle.dв = StringToDouble(value); OnPropertyChanged("dв"); } }
        public string dвBounds => Get_dвBounds(Model.Common.Da);  //label
        public string hp { get => Model.Common.hp.ToString(); set { Model.Common.hp = StringToDouble(value); OnPropertyChanged("hp"); } }
        public string hpBounds { get => Get_hpBounds(Model.Common.Dpст, Model.Common.ΔГ2); } //label
        public string bк { get => Model.AsdnRedSingle.bк.ToString(); set { Model.AsdnRedSingle.bк = StringToDouble(value); OnPropertyChanged("bк"); } }
        public string bкBounds => Get_bкBounds(Model.AsdnRedSingle.bП2);  //label
        public string ρ2Г { get => Model.Common.ρ2Г.ToString(); set { Model.Common.ρ2Г = StringToDouble(value); OnPropertyChanged("ρ2Г"); } }
        public string Kfe2 { get => Model.Common.Kfe2.ToString(); set { Model.Common.Kfe2 = StringToDouble(value); OnPropertyChanged("Kfe2"); } }
        public string PAS { get => Model.AsdnRedSingle.PAS; set { Model.AsdnRedSingle.PAS = value; OnPropertyChanged("PAS"); } }
        public string hш { get => Model.AsdnRedSingle.hш.ToString(); set { Model.AsdnRedSingle.hш = StringToDouble(value); OnPropertyChanged("hш"); } }
        public string bш { get => Model.AsdnRedSingle.bш.ToString(); set { Model.AsdnRedSingle.bш = StringToDouble(value); OnPropertyChanged("bш"); } }
        public string dкп { get => Model.AsdnRedSingle.dкп.ToString(); set { Model.AsdnRedSingle.dкп = StringToDouble(value); OnPropertyChanged("dкп"); } }
        public string dкпBounds => dкпBoundsString(Get_dкпBounds(Model.Common.Dpст, Model.Common.Z2));
        public string bZH { get => Model.AsdnRedSingle.bZH.ToString(); set { Model.AsdnRedSingle.bZH = StringToDouble(value); OnPropertyChanged("bZH"); } }
        public string bZHBounds => Get_bZHBounds(bounds_bZH(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2));
        public string hр2 { get => Model.AsdnRedSingle.hp2.ToString(); set { Model.AsdnRedSingle.hp2 = StringToDouble(value); OnPropertyChanged("hр2"); } }
        public string bкн { get => Model.AsdnRedSingle.bкн.ToString(); set { Model.AsdnRedSingle.bкн = StringToDouble(value); OnPropertyChanged("bкн"); } }
        public string aкн { get => Model.AsdnRedSingle.aкн.ToString(); set { Model.AsdnRedSingle.aкн = StringToDouble(value); OnPropertyChanged("aкн"); } }
        public string aкнBounds => Get_aкнBounds(bounds_aкн(Model.AsdnRedSingle.dпн, Model.AsdnRedSingle.dпв, Model.AsdnRedSingle.hp1));
        public string aк { get => Model.AsdnRedSingle.aк.ToString(); set { Model.AsdnRedSingle.aк = StringToDouble(value); OnPropertyChanged("aк"); } }
        public string aкBounds => Get_aкBounds(bounds_aк(Model.AsdnRedSingle.dкп, Model.AsdnRedSingle.hш, Model.AsdnRedSingle.hp2));
        public string dпн { get => Model.AsdnRedSingle.dпн.ToString(); set { Model.AsdnRedSingle.dпн = StringToDouble(value); OnPropertyChanged("dпн"); } }
        public string dпн_calc { get => Math.Round(dпн(Model.Common.Dpст, Model.Common.hp, Model.Common.Z2, Model.AsdnRedSingle.bZH), 3).ToString(); } //label
        public string dпв { get => Model.AsdnRedSingle.dпв.ToString(); set { Model.AsdnRedSingle.dпв = StringToDouble(value); OnPropertyChanged("dпв"); } }
        public string dпв_calc { get => Math.Round(dпв(Model.Common.Z2, Model.Common.Dpст, Model.AsdnRedSingle.hш, Model.AsdnRedSingle.bZH), 3).ToString(); } //label
        public string hp1 { get => Model.AsdnRedSingle.hp1.ToString(); set { Model.AsdnRedSingle.hp1 = StringToDouble(value); OnPropertyChanged("hp1"); } }
        public string hp1_calc { get => Math.Round(hp1(Model.AsdnRedSingle.dпв, Model.AsdnRedSingle.dпн, Model.Common.Z2), 3).ToString(); } //label
        #endregion
        #region collection
        /// <summary>
        /// Число пар полюсов
        /// </summary>
        public List<string> Get_collection_p => p_Collection;
        /// <summary>
        /// Рекомендуемые значения Z1
        /// </summary>
        public List<string> Get_collectionZ1 => Z1Collection(Model.Common.Di, Model.Common.p);
        /// <summary>
        /// Рекомендуемые значения а1
        /// </summary>
        public List<string> Get_collection_a1 => a1Collection(Model.Common.p, Model.Common.Z1);
        public List<Wire> WireDirectory { get; set; } //dиз, qГ
        /// <summary>
        /// Таблица K2
        /// </summary>
        public DataTable Get_tableK2 => K2Table;
        public List<SteelProperties> MarkSteelPartitionlDirectory { get; set; } //ρРУБ collection
        public List<SteelProperties> MarkSteelStatorDirectory { get; set; } //p10_50 collection
        public List<string> Get_collectionPAS => new List<string> { "круглый", "прямоугольный", "грушевидный", "двойная клетка" };
        public List<string> Get_collectionPR => new List<string> { "трапецеидальный", "круглый" }; //признак формы паза статора
        /// <summary>
        /// Тип паза ротора
        /// </summary>
        public List<string> Get_collection_bСК => bСК_Collection;
        public ContentZ2 Get_collectionZ2 => Z2Object(Model.Common.p, Model.Common.Z1, Model.Common.bСК);
        #endregion
        #region command
        AlgorithmASDNRED algorithm;
        UserCommand CalculationCommand { get; set; }
        public ICommand CommandCalculation {
            get {
                if (CalculationCommand == null) CalculationCommand = new UserCommand(Calculation, CanCalculation);
                return CalculationCommand;
            }
        }
        public void Calculation() {
            Model.Common.CreationDataset(); Model.AsdnRedSingle.CreationDataset();
            Dictionary<string, double> _inputAlgorithm = Model.Common.GetDataset.Union(Model.AsdnRedSingle.GetDataset).ToDictionary(i => i.Key, i => i.Value);
            _inputAlgorithm.Add("bСК", Model.Common.bСК == "скошенные" ? 1 : 0);
            //_inputAlgorithm.Add("dпн", dпн); 
            //_inputAlgorithm.Add("hр1", hp1); 
            //_inputAlgorithm.Add("dпв", dпв);

            double _pas = 0;
            switch (PAS) {
                case "круглый": _pas = 1; break;
                case "прямоугольный": _pas = 2; break;
                case "грушевидный": _pas = 3; break;
                case "двойная клетка": _pas = 4; break;
            }
            _inputAlgorithm.Add("PAS", _pas);
            algorithm = new AlgorithmASDNRED(_inputAlgorithm); algorithm.Run();
            foreach (string item in algorithm.Logging)
                Logger.Error(item);

            Diagnostic = algorithm.SolutionIsDone ? "Расчет завершен успешно" : @"Расчет прерван. Смотри содержимое файла logs\*.log";
        }
        public bool CanCalculation() {
            Model.AsdnRedSingle.SetParametersForModelValidation(Model.Common.Dpст, Model.Common.Z2, Model.Common.hp, Model.Common.Da);
            var resultsCommon = new List<ValidationResult>(); var resulstAsdnRed = new List<ValidationResult>();
            var contextCommon = new ValidationContext(Model.Common); var contextAsdnRed = new ValidationContext(Model.AsdnRedSingle);
            return Validator.TryValidateObject(Model.Common, contextCommon, resultsCommon) &&
                Validator.TryValidateObject(Model.AsdnRedSingle, contextAsdnRed, resulstAsdnRed);
        }
        #region CommonResult
        UserCommand ViewResultCommand { get; set; }
        public ICommand CommandViewResult {
            get {
                if (ViewResultCommand == null) ViewResultCommand = new UserCommand(ViewResult, CanViewResult);
                return ViewResultCommand;
            }
        }
        void ViewResult() {
            //initial data
            Dictionary<string, double> machineData = new Dictionary<string, double> {
                { "P'2", Model.Common.P12}, { "U1", Model.Common.U1}, { "f1", Model.Common.f1}, { "p", Model.Common.p}, { "Pмех", Model.Common.Pмех}
            };
            Dictionary<string, double> statorData = new Dictionary<string, double> {
                {"Di", Model.Common.Di }, {"ΔГ1", Model.Common.ΔГ1 }, {"Z1", Model.Common.Z1 }, {"q1", Model.Common.q1 }, {"Da", Model.Common.Da },
                {"a1", Model.Common.a1 }, {"a2", Model.Common.a2 }, {"Δкр", Model.Common.Δкр }, {"dиз", Model.Common.dиз }, {"qГ", Model.Common.qГ },
                {"bz1", Model.Common.bz1 }, {"h8", Model.Common.h8 }, {"h7", Model.Common.h7 }, {"h6", Model.Common.h6 }, {"bП1", Model.Common.bП1 },
                {"h5", Model.Common.h5 }, {"h3", Model.Common.h3 }, {"h4", Model.Common.h4 }, {"ac", Model.Common.ac },
                {"bПН", Model.Common.bПН }, {"h1", Model.Common.h1 }, {"h2", Model.Common.h2 }, {"li", Model.Common.li }, {"cз", Model.Common.cз },
                {"bП", Model.Common.bП }, {"W1", Model.Common.W1 }, {"Wc", Model.Common.Wc }, {"Kзап", Model.Common.Kзап }, {"y1", Model.Common.y1 },
                {"β", Model.Common.β }, {"K2", Model.Common.K2 }, {"d1", Model.Common.d1 }, {"Kfe1", Model.Common.Kfe1 }, {"ρ1x", Model.Common.ρ1x },
                {"ρРУБ", Model.Common.ρРУБ }, {"ρ1Г", Model.Common.ρ1Г }, {"B", Model.Common.B }, {"p10_50", Model.Common.p10_50 }
            };
            Dictionary<string, double> rotorData = new Dictionary<string, double> {
                {"ΔГ2", Model.Common.ΔГ2 }, {"Dpст", Model.Common.Dpст }, {"bСК", Model.Common.bСК == "скошенные" ? 1 : 0 }, {"bП2", Model.AsdnRedSingle.bП2 },
                {"hp", Model.Common.hp }, {"dв", Model.AsdnRedSingle.dв }, {"Z2", Model.Common.Z2 }, {"bк", Model.AsdnRedSingle.bк }, {"ρ2Г", Model.Common.ρ2Г },
                {"Kfe2", Model.Common.Kfe2 }, {"hш", Model.AsdnRedSingle.hш }, {"bш", Model.AsdnRedSingle.bш }, {"dкп", Model.AsdnRedSingle.dкп },
                {"bZH", Model.AsdnRedSingle.bZH }, {"hp2", Model.AsdnRedSingle.hp2 }, {"bкн", Model.AsdnRedSingle.bкн }, {"aкн", Model.AsdnRedSingle.aкн },
                {"aк", Model.AsdnRedSingle.aк }
            };


            string file_name = Directory.GetCurrentDirectory() + "\\report_" + Path.GetFileNameWithoutExtension(Services.ServiceIO.FileName) + ".html";

            // Создаем поток для записи в файл
            StreamWriter sw = new StreamWriter(file_name);

            sw.WriteLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<meta http-equiv='content-type' content='text/html; charset=UTF-8' />");

            sw.WriteLine("<link href = 'css/bootstrap.min.css' rel='stylesheet'>");

            sw.WriteLine("<title>Результаты расчета</title>");
            sw.WriteLine("<style>.table-fit { width: 1px;} h2 {background-color: #d9d9d9;} h3 {background-color: #ccccff}</style>");

            sw.WriteLine("</head>");
            sw.WriteLine("<body><div class='mx-auto' style='width: 1024px;'>");

            sw.WriteLine("<h1>Исходные данные</h1>");

            sw.WriteLine("<h2>Параметры машины</h2>");

            sw.WriteLine("<table class='table table-striped table-fit'>");
            sw.WriteLine("<tr><td><NOBR>Мощность на роторе P'<sub>2</sub>,&nbsp;Вт:</NOBR></td><td>" + machineData["P'2"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Фазное напряжение U<sub>1</sub>,&nbsp;В:</NOBR></td><td>" + machineData["U1"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Частота питающей сети f<sub>1</sub>,&nbsp;Гц:</NOBR></td><td>" + machineData["f1"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число пар полюсов p:</NOBR></td><td>" + machineData["p"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Механические потери P<sub>мех</sub>,&nbsp;Вт:</NOBR></td><td>" + machineData["Pмех"].ToString() + "</td></tr>");

            sw.WriteLine("</table>");

            sw.WriteLine("<h2>Параметры статора</h2>");

            sw.WriteLine("<table class='table table-striped table-fit'>");
            sw.WriteLine("<tr><td><NOBR>Диаметр расточки D<sub>i</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["Di"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Толщина перегородки Δ<sub>Г1</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["ΔГ1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число зубцов Z<sub>1</sub>,&nbsp;шт:</NOBR></td><td>" + statorData["Z1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число пазов на полюс и фазу q<sub>1</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["q1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Диаметр наружный D<sub>a</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["Da"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число параллельных ветвей обмотки a<sub>1</sub>,&nbsp;шт:</NOBR></td><td>" + statorData["a1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число параллельных цепей обмотки a<sub>2</sub>,&nbsp;шт:</NOBR></td><td>" + statorData["a2"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Суммарная толщина крайнего и нажимного листов Δ<sub>кр</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["Δкр"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Диаметр изолированного провода d<sub>из</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["dиз"].ToString() + " </td></tr>");            
            sw.WriteLine("<tr><td><NOBR>Ширина зубца статора b<sub>z1</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["bz1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота силового клина паза h<sub>8</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h8"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота щели между клиньями паза h<sub>7</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h7"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота нажимного клина обмотки h<sub>6</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h6"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина паза у клина b<sub>П1</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["bП1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота изоляции от клина до обмотки h<sub>5</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h5"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Толщина изоляции между слоями обмотки h<sub>3</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h3"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота верхнего слоя обмотки h<sub>4</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h4"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина открытия паза a<sub>c</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["ac"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина силового клина паза b<sub>ПН</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["bПН"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота зубца статора h<sub>1</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота нижнего слоя обмотки h<sub>2</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["h2"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Длина пакета l<sub>i</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["li"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Суммарная длина свеса перегородки c<sub>з</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["cз"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина паза у дна b<sub>П</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["bП"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число витков в фазе обмотки W<sub>1</sub>,&nbsp;шт:</NOBR></td><td>" + statorData["W1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число эффективных проводников в секции W<sub>c</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["Wc"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Коэффициент K<sub>зап</sub>:</NOBR></td><td>" + statorData["Kзап"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Шаг обмотки по пазам y<sub>1</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["y1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Коэффициент β:</NOBR></td><td>" + statorData["β"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Коэффициент длины лобовой части обмотки K<sub>2</sub>:</NOBR></td><td>" + statorData["K2"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Диаметр полукруглого паза под клин d<sub>1</sub>,&nbsp;мм:</NOBR></td><td>" + statorData["d1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Коэффициент заполнения пакета сталью K<sub>fe1</sub>:</NOBR></td><td>" + statorData["Kfe1"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Удельное сопротивление материала обмотки в холодном состоянии ρ<sub>1x</sub>,&nbsp;Ом∙мм<sup>2</sup>/м:</NOBR></td><td>" + statorData["ρ1x"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Удельное сопротивление материала перегородки ρ<sub>РУБ</sub>,&nbsp;Ом∙мм<sup>2</sup>/м:</NOBR></td><td>" + statorData["ρРУБ"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Удельное сопротивление материала обмотки в рабочем состоянии ρ<sub>1Г</sub>,&nbsp;Ом∙мм<sup>2</sup>/м:</NOBR></td><td>" + statorData["ρ1Г"].ToString() + " </td></tr>");
            sw.WriteLine("<tr><td><NOBR>Прямой участок лобовой части обмотки B,&nbsp;мм:</NOBR></td><td>" + statorData["B"].ToString() + " </td></tr>");            
            sw.WriteLine("<tr><td><NOBR>Удельные потери стали p<sub>10/50</sub>,&nbsp;Вт/кг:</NOBR></td><td>" + statorData["p10_50"].ToString() + " </td></tr>");
            sw.WriteLine("</table>");

            sw.WriteLine("<h2>Параметры ротора</h2>");

            sw.WriteLine("<table class='table table-striped table-fit'>");
            sw.WriteLine("<tr><td><NOBR>Толщина магнитной гильзы Δ<sub>Г2</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["ΔГ2"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Диаметр по стали магнитопровода D<sub>pст</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["Dpст"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Тип пазов b<sub>СК</sub>:</NOBR></td><td>" + rotorData["bСК"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина прямоугольного паза b<sub>П2</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["bП2"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Число пазов Z<sub>2</sub>,&nbsp;шт:</NOBR></td><td>" + rotorData["Z2"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота паза h<sub>p</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["hp"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Внутренний диаметр сверления d<sub>в</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["dв"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина кольца к.з. клетки b<sub>к</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["bк"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Удельное сопротивление к.з. клетки в рабочем состоянии ρ<sub>2Г</sub>,&nbsp;Ом∙мм<sup>2</sup>/м:</NOBR></td><td>" + rotorData["ρ2Г"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Коэффициент заполнения пакета сталью K<sub>fe2</sub>:</NOBR></td><td>" + rotorData["Kfe2"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота шлица паза h<sub>ш</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["hш"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина шлица паза b<sub>ш</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["bш"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Диаметр круглого паза d<sub>кп</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["dкп"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина зубца b<sub>ZH</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["bZH"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Диаметр низа грушевидного паза d<sub>пн</sub>,&nbsp;мм:</NOBR></td><td>" + "???" + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Диаметр верха грушевидного паза d<sub>пв</sub>,&nbsp;мм:</NOBR></td><td>" + "???" + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота нижней части паза h<sub>p1</sub>,&nbsp;мм:</NOBR></td><td>" + "???" + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота между клетками h<sub>p2</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["hp2"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота кольца к.з. обмотки a<sub>к</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["aк"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Ширина нижнего кольца к.з. клетки b<sub>кн</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["bкн"].ToString() + "</td></tr>");
            sw.WriteLine("<tr><td><NOBR>Высота нижнего кольца к.з. клетки a<sub>кн</sub>,&nbsp;мм:</NOBR></td><td>" + rotorData["aкн"].ToString() + "</td></tr>");
            sw.WriteLine("</table>");

            sw.WriteLine("</div></body>");
            sw.WriteLine("</html>");


            // Закрываем поток для записи в файл
            sw.Close();

            Services.ServiceIO.LaunchBrowser(file_name);
        }
        bool CanViewResult() => algorithm != null && algorithm.SolutionIsDone;
        #endregion
        #region HeatResult
        UserCommand ViewHeatResultCommand { get; set; }
        public ICommand CommandViewHeatResult {
            get {
                if (ViewHeatResultCommand == null) ViewHeatResultCommand = new UserCommand(ViewHeatResult, CanViewHeatResult);
                return ViewHeatResultCommand;
            }
        }
        void ViewHeatResult() {
            SteelProperties steelPartition = MarkSteelPartitionlDirectory.FirstOrDefault(s => s.Value == Model.Common.ρРУБ);

        }
        bool CanViewHeatResult() => algorithm != null && algorithm.SolutionIsDone;
        #endregion
        #region StatorRotorResult
        UserCommand ViewStatorRotorResultCommand { get; set; }
        public ICommand CommandViewStatorRotorResult {
            get {
                if (ViewResultCommand == null) ViewResultCommand = new UserCommand(ViewStatorRotorResult, CanViewStatorRotorResult);
                return ViewResultCommand;
            }
        }
        void ViewStatorRotorResult()
        {
        
        }

        bool CanViewStatorRotorResult() => algorithm != null && algorithm.SolutionIsDone;
        #endregion
        #endregion
        #endregion
    }
}
