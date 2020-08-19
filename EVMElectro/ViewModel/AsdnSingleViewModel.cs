﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using IVMElectro.Models;
using IVMElectro.Commands;
using System.Windows.Input;
using static IVMElectro.Services.ServiceDT;
using System.Data;
using IVMElectro.Services.Directories.WireDirectory;
using IVMElectro.Services.Directories;
using static IVMElectro.Services.DataSharedContent;
using System.ComponentModel.DataAnnotations;

namespace IVMElectro.ViewModel {
    abstract class AsdnSingleViewModel : INotifyPropertyChanged, IDataErrorInfo {
        #region string error
        private const string errora2 = "Значение параметра a2 должено принадлежать [0 : 30].";
        
        
        

        
        
        
        
        
        
        
        
        
        
        
        
        private const string errorB = "Значение параметра B должено принадлежать [0 : 20].";
        
        
        
        
        
        
        
        
        
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        string IDataErrorInfo.this[string columnName] {
            get {
                string error = string.Empty;
                switch (columnName) {
                    case "ΔГ1":
                        if ((!(0 <= Model.Common.ΔГ1) && (Model.Common.ΔГ1 <= 0)))
                            error = errorΔГ1;
                        break;
                    case "ΔГ2":
                        if (!((0 <= Model.Common.ΔГ2) && (Model.Common.ΔГ2 <= 5))) 
                            error = errorΔГ2;
                        break;
                    case "Di":
                        if (!((50 < Model.Common.Di) && (Model.Common.Di <= 1100)))
                            error = errorDi;
                        break;
                    case "Dpст":
                        if (!((Model.Common.DpстBoundCalculation - 5 <= Model.Common.Dpст) && (Model.Common.Dpст < Model.Common.DpстBoundCalculation - 0.1))) 
                            error = $"Значение параметра Dp.ст должно принадлежать {DpстBounds}.";
                        break;
                    case "Da":
                        if (!((Get_DaBounds(Model.Common.Di).left <= Model.Common.Da) && (Model.Common.Da < Get_DaBounds(Model.Common.Di).right))) 
                            error = $"Значение параметра {columnName} должно принадлежать {DaBounds}.";
                        break;
                    case "Pмех":
                        if (!((0 <= Model.Common.Pмех) && (Model.Common.Pмех <= PмехBoundRight(Model.Common.P12)))) 
                            error = $"Значение параметра {columnName} должно принадлежать {PмехBounds}.";
                        break;
                    case "P12":
                        if (!((50 <= Model.Common.P12) && (Model.Common.P12 <= 2e5))) 
                            error = errorP12;
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
                        if (bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1) < 0 || 
                            double.IsNaN(bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1))) 
                            error = errorbП1;
                        break;
                    case "β":
                        if (!((0.5 <= Model.Common.β) && (Model.Common.β <= 0.95))) 
                            error = errorβ;
                        break;
                    case "bП2":
                        if (!((2 <= Model.Common.bП2) && (Model.Common.bП2 <= 6))) 
                            error = errorbП2;
                        break;
                    case "bПН":
                        if (!((0 <= Model.Common.bПН) && (Model.Common.bПН <= bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1)))) 
                            error = $"Значение параметра {columnName} должно принадлежать {bПНBounds}.";
                        break;
                    case "bк":
                        if (!((Model.Common.bП2 <= Model.Common.bк) && (Model.Common.bк <= 5 * Model.Common.bП2))) 
                            error = $"Значение параметра {columnName} должно принадлежать {bкBounds}.";
                        break;
                    case "U1":
                        if (!((48 <= Model.Common.U1) && (Model.Common.U1 <= 660)))
                            error = errorU1;
                        break;
                    case "h1":
                        if (Model.Common.h1 < 0 || double.IsNaN(Model.Common.h1)) 
                            error = errorh1;
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
                        if (!((0.1 <= Model.Common.h5) && (Model.Common.h5 <= 0))) 
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
                        if (!((0.5 <= Model.Common.li) && (Model.Common.li >= 3))) 
                            error = errorli;
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
                        if (!((Model.Common.dиз < Model.Common.ac) && (Model.Common.ac < 2 * Model.Common.dиз))) 
                            error = $"Значение параметра расчета {columnName} должно принадлежать {acBounds}.";
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
                        if (!((1 <= Model.Common.y1) && (Model.Common.y1 <= 0.5 * Model.Common.Z1 / Convert.ToDouble(Model.Common.p)))) 
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
                    case "Kзап":
                        if (!((0.36 <= Model.Common.Kзап) && (Model.Common.Kзап <= 0.76))) 
                            error = errorKзап;
                        break;
                    case "hp":
                        if (!((0.125 * Get_Dp(Model.Common.Dpст, Model.Common.ΔГ2) <= Model.Common.hp) && (Model.Common.hp <= 0.375 * Get_Dp(Model.Common.Dpст, Model.Common.ΔГ2)))) 
                            error = $"Значение параметра {columnName} должно принадлежать {hpBounds}.";
                        break;
                    case "dв":
                        if (!((0 <= Model.AsdnSingle.dв) && (Model.AsdnSingle.dв <= 0.5 * (0.5 * Get_Dp(Model.Common.Dpст, Model.Common.ΔГ2) - Model.Common.hp)))) 
                            error = $"Значение параметра {columnName} должно принадлежать {dвBounds}.";
                        break;
                    case "ρРУБ":
                        if ( (Model.Common.ρРУБ < 0) || double.IsNaN(Model.Common.ρРУБ)) 
                            error = errorρРУБ;
                        break;
                    case "Z2":
                        if ((Model.Common.Z2 < 0) || !int.TryParse(Model.Common.Z2.ToString(), out _)) 
                            error = errorZ2;
                        break;
                    case "Z1":
                        if ((Model.Common.Z1 < 0) || !int.TryParse(Model.Common.Z1.ToString(), out _)) 
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
                        if (!((1.05 * Model.Common.hp <= Model.AsdnSingle.aк) && (Model.AsdnSingle.aк <= 1.5 * Model.Common.hp))) 
                            error = $"Значение параметра {columnName} должно принадлежать {aкBounds}.";
                        break;
                    case "γ":
                        if ((Model.AsdnSingle.γ < 0) || double.IsNaN(Model.AsdnSingle.γ)) 
                            error = errorγ;
                        break;
                    case "p10_50":
                        if ((Model.Common.p10_50 < 0) || double.IsNaN(Model.Common.p10_50)) 
                            error = errorp10_50;
                        break;
                }
                return error;
            }
        }
        public string Error { get; }
        public AsdnSingleViewModel(AsdnCompositeModel model) { 
            Model = model;
            WireDirectory = new List<Wire> { new WireПНЭД_имид { Id = 1,Name = "ПНЭД_имид" }, new WireПСДК_Л { Id = 2, Name = "ПСДК_Л" },
            new WireПСДКТ_Л{Id = 3,Name = "ПСДКТ_Л" }, new WireПЭЭИД2{ Id = 4, Name = "ПЭЭИД2" } };
            MarkSteelRotorDirectory = new List<SteelProperties> { new SteelProperties { Id = 1, Name = "09Х17Н", Value = 10000 }, new SteelProperties { Id = 2, Name = "Ст.3", Value = 50000 },
            new SteelProperties{Id = 3, Name = "20Х13", Value = 16700 }};
            MarkSteelPartitionlDirectory = new List<SteelProperties> { new SteelProperties { Id = 1, Name = "ХН78Т", Value = 1.16 }, new SteelProperties {Id = 2, Name = "ВТ-1", Value =  0.47 },
                new SteelProperties {Id = 3, Name = "ПТ-7М", Value = 1.08 } };
            MarkSteelStatorDirectory = new List<SteelProperties> { new SteelProperties { Id = 1, Name = "1412", Value = 1.94 }, new SteelProperties { Id = 2, Name = "2412", Value = 1.38 },
                new SteelProperties {Id = 3, Name = "2411", Value =  1.7 }, new SteelProperties {Id = 4, Name = "1521", Value = 19.6 } };
        }
        #region properties
        public AsdnCompositeModel Model { get; set; }
        #region machine parameters
        public string P12 { get => Model.Common.P12.ToString(); set { Model.Common.P12 = StringToDouble(value); OnPropertyChanged("P12"); } }
        public string U1 { get => Model.Common.U1.ToString(); set { Model.Common.U1 = StringToDouble(value); OnPropertyChanged("U1"); } }
        public string f1 { get => Model.Common.f1.ToString(); set { Model.Common.f1 = StringToDouble(value); OnPropertyChanged("f1"); } }
        public string p { get => Model.Common.p.ToString(); set { Model.Common.p = StringToInt(value); } } //no need to sync with the interface
        public string Pмех { get => Model.Common.Pмех.ToString(); set { Model.Common.Pмех = StringToDouble(value); OnPropertyChanged("Pмех"); } }
        public string PмехBounds { get => $"[0 : {PмехBoundRight(Model.Common.P12)}]"; } //label
        #endregion
        #region stator parameters
        public string Di { get => Model.Common.Di.ToString(); set { Model.Common.Di = StringToDouble(value); OnPropertyChanged("Di"); } }
        public string ΔГ1 { get => Model.Common.ΔГ1.ToString(); set { Model.Common.ΔГ1 = StringToDouble(value); OnPropertyChanged("ΔГ1"); } }
        public string Z1 { get => Model.Common.Z1.ToString(); set { Model.Common.Z1 = StringToInt(value); OnPropertyChanged("Z1"); } }
        public string Da { get => Model.Common.Da.ToString(); set { Model.Common.Da = StringToDouble(value); OnPropertyChanged("Da"); } }
        //рекомендуемые границы
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
        public string bП1 { get => bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1).ToString(); } //label
        public string h5 { get => Model.Common.h5.ToString(); set { Model.Common.h5 = StringToDouble(value); OnPropertyChanged("h5"); } }
        public string h3 { get => Model.Common.h3.ToString(); set { Model.Common.h3 = StringToDouble(value); OnPropertyChanged("h3"); } }
        public string h4 { get => Model.Common.h4.ToString(); set { Model.Common.h4 = StringToDouble(value); OnPropertyChanged("h4"); } }
        public string ac { get => Model.Common.ac.ToString(); set { Model.Common.ac = StringToDouble(value); OnPropertyChanged("ac"); } }
        public string acBounds { get => $"({Model.Common.dиз} : {2 * Model.Common.dиз})"; } //label
        public string bПН { get => Model.Common.bПН.ToString(); set { Model.Common.bПН = StringToDouble(value); OnPropertyChanged("bПН"); } }
        public string bПНBounds { get => $"[0 : {Math.Round(bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1), 2)}]"; } //label
        public string h1 { get => Model.Common.h1.ToString(); } //label
        public string li { get => Model.Common.li.ToString(); set { Model.Common.li = StringToDouble(value); OnPropertyChanged("li"); } }
        public string liBounds { get => Get_liBounds(Model.Common.U1, I1(Model.Common.P12, Model.Common.U1), Model.Common.Di); } //label
        public string cз { get => Model.Common.cз.ToString(); set { Model.Common.cз = StringToDouble(value); OnPropertyChanged("cз"); } }
        public string bП { get => Model.Common.bП.ToString(); } //label
        public string Kзап { get => Model.Common.Kзап.ToString(); set { Model.Common.Kзап = StringToDouble(value); OnPropertyChanged("Kзап"); } }
        public string y1 { get => Model.Common.y1.ToString(); set { Model.Common.y1 = StringToDouble(value); OnPropertyChanged("y1"); } }
        public string y1Bounds { get => $"[1 : {0.5 * Model.Common.Z1 / Convert.ToDouble(Model.Common.p)}]"; } //label
        public string β { get => Model.Common.β.ToString(); } //label
        public string K2 { get => Model.Common.K2.ToString(); set { Model.Common.K2 = StringToDouble(value); OnPropertyChanged("K2"); } }
        public string d1 { get => Model.Common.d1.ToString(); set { Model.Common.d1 = StringToDouble(value); OnPropertyChanged("d1"); } }
        public string d1Bounds { get => $"[0 : {Math.Round(bП1Calc(Model.Common.Di, Model.Common.h8, Model.Common.h7, Model.Common.h6, Model.Common.bz1, Model.Common.Z1), 2)}]"; } //label
        public string Kfe1 { get => Model.Common.Kfe1.ToString(); set { Model.Common.Kfe1 = StringToDouble(value); OnPropertyChanged("Kfe1"); } }
        public string ρ1x { get => Model.Common.ρ1x.ToString(); set { Model.Common.ρ1x = StringToDouble(value); OnPropertyChanged("ρ1x"); } }
        public string ρРУБ { get => Model.Common.ρРУБ.ToString(); set { Model.Common.ρРУБ = StringToDouble(value); OnPropertyChanged("ρРУБ"); } }
        public string ρ1Г { get => Model.Common.ρ1Г.ToString(); set { Model.Common.ρ1Г = StringToDouble(value); OnPropertyChanged("ρ1Г"); } }
        public string ρ1ГBounds { get => $"[{Model.Common.ρ1x} : 0.1235]"; } //label
        public string B { get => Model.Common.B.ToString(); set { Model.Common.B = StringToDouble(value); OnPropertyChanged("B"); } }
        public string PЗ { get => Model.AsdnSingle.P3.ToString(); set => Model.AsdnSingle.P3 = StringToInt(value); } //no need to sync with the interface
        //public string MarkSteelStator { get => Model.Common.markSteelStator; set => Model.Common.markSteelStator = value; } //no need to sync with the interface
        public string p10_50 { get => Model.Common.p10_50.ToString(); set {  Model.Common.p10_50 = StringToDouble(value); OnPropertyChanged("p10_50"); } }
        #endregion
        #region rotor parameters
        public string ΔГ2 { get => Model.Common.ΔГ2.ToString(); set { Model.Common.ΔГ2 = StringToDouble(value); OnPropertyChanged("ΔГ2"); } }
        public string Dpст { get => Model.Common.Dpст.ToString(); set { Model.Common.Dpст = StringToDouble(value); OnPropertyChanged("Dpст"); } }
        public string DpстBounds { get => $"[{Math.Round(Model.Common.DpстBoundCalculation - 5, 2)} : {Math.Round(Model.Common.DpстBoundCalculation - 0.1, 2)})"; } //label
        public string bСК { get => Model.Common.bСК; set => Model.Common.bСК = value; } //no need to sync with the interface
        public string bП2 { get => Model.Common.bП2.ToString(); set { Model.Common.bП2 = StringToDouble(value); OnPropertyChanged("bП2"); } }
        public string hp { get => Model.Common.hp.ToString(); set { Model.Common.hp = StringToDouble(value); OnPropertyChanged("hp"); } }
        public string hpBounds { get => Get_hpBounds(Model.Common.Dpст, Model.Common.ΔГ2); } //label
        public string dв { get => Model.AsdnSingle.dв.ToString(); set { Model.AsdnSingle.dв = StringToDouble(value); OnPropertyChanged("dв"); } }
        public string dвBounds { get => Get_dвBounds(Model.Common.Dpст, Model.Common.ΔГ2, Model.Common.hp); } //label
        public string Z2 { get => Model.Common.Z2.ToString(); set { Model.Common.Z2 = StringToInt(value); OnPropertyChanged("Z2"); } }
        public string bк { get => Model.Common.bк.ToString(); set { Model.Common.bк = StringToDouble(value); OnPropertyChanged("bк"); } }
        public string bкBounds { get => Get_bкBounds(Model.Common.bП2); } //label
        public string aк { get => Model.AsdnSingle.aк.ToString(); set { Model.AsdnSingle.aк = StringToDouble(value); OnPropertyChanged("aк"); } }
        public string aкBounds { get => Get_aкBounds(Model.Common.hp); } //label
        public string γ { get => Model.AsdnSingle.γ.ToString(); set { Model.AsdnSingle.γ = StringToDouble(value); OnPropertyChanged("γ"); } }
        public string ρ2Г { get => Model.Common.ρ2Г.ToString(); set { Model.Common.ρ2Г = StringToDouble(value); OnPropertyChanged("ρ2Г"); } }
        public string Kfe2 { get => Model.Common.Kfe2.ToString(); set { Model.Common.Kfe2 = StringToDouble(value); OnPropertyChanged("Kfe2"); } }
        #endregion
        #region collection
        /// <summary>
        /// Марка метала статора
        /// </summary>
        //public List<string> Get_collectionMarksSteelStator => new List<string> { "1412", "2412", "2411", "1521" };
        /// <summary>
        /// Тип паза ротора
        /// </summary>
        public List<string> Get_collection_bСК => bСК_Collection;
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
        /// <summary>
        /// Таблица K2
        /// </summary>
        public DataTable Get_tableK2 => K2Table;
        //public DataTable Get_table_ρРУБ => ρРУБTable();
        public List<string> Get_collectionPЗ => new List<string> { "0", "1" };
        public List<string> Get_collectionZ2 => Z2Collection(Model.Common.p, Model.Common.Z1, Model.Common.bСК);
        public List<Wire> WireDirectory { get; set; } //dиз, qГ
        public List<SteelProperties> MarkSteelRotorDirectory { get; set; } //γ collection
        public List<SteelProperties> MarkSteelPartitionlDirectory { get; set; } //ρРУБ collection
        public List<SteelProperties> MarkSteelStatorDirectory { get; set; } //p10_50 collection
        #endregion
        #region commands
        CalculationCommand CalculationCommand { get; set; }
        public ICommand CommandCalculation {
            get {
                if (CalculationCommand == null) CalculationCommand = new CalculationCommand(Calculation, CanCalculation);
                return CalculationCommand;
            }
        }
        public void Calculation() { }
        public bool CanCalculation() {
            Model.AsdnSingle.SetParametersForModelValidation(Model.Common.Dpст, Model.Common.ΔГ2, Model.Common.hp);
            var resultsCommon = new List<ValidationResult>(); var resulstAsdn = new List<ValidationResult>();
            var contextCommon = new ValidationContext(Model.Common); var contextAsdn = new ValidationContext(Model.AsdnSingle);

            //return Model.AsdnSingle != null && Model.AsdnSingle.ValidHidden() && Model.Common.ValidHidden();
            return Validator.TryValidateObject(Model.Common, contextCommon, resultsCommon, true) &&
                Validator.TryValidateObject(Model.AsdnSingle, contextAsdn, resulstAsdn, true);
        }
        #endregion
        //double PмехBoundUp() => Math.Round(0.5 * Model.Common.P12, 2);
        //List<string> a1Collection() {
        //    List<string> sequence = new List<string>() { "1" };
        //    double add;
        //    //K = [2 : p-1]
        //    for (int i = 2; i < Model.Common.p; i++) {
        //        add = Math.Round(Model.Common.Z1 / 3 / Convert.ToDouble(i) / Convert.ToDouble(Model.Common.p));
        //        if (add >= 1 && add <= Model.Common.p) sequence.Add(add.ToString());
        //    }
        //    sequence.Add($"{Model.Common.p}");
        //    sequence.Add($"{2 * Model.Common.p}");
        //    return sequence;
        //}
        //double bП1Calc() => (Math.PI * (Model.Common.Di + 2 * (Model.Common.h8 + Model.Common.h7 + Model.Common.h6)) - Model.Common.bz1) / Model.Common.Z1;
        //DataTable K2Table() {
        //    DataTable table = new DataTable();
        //    DataColumn p = new DataColumn("Число пар полюсов", typeof(string)), NoneInsulating = new DataColumn("β ≥ 0,8\r\nЛобовые части секции\r\nне изолированы", typeof(string)),
        //        Insulating = new DataColumn("β ≥ 0,8\r\nЛобовые части секции\r\nизолированы", typeof(string)),
        //        β = new DataColumn("0,5 ≤ β < 0,8", typeof(string));
        //    table.Columns.AddRange(new DataColumn[] { p, NoneInsulating, Insulating, β });
        //    DataRow row = table.NewRow();
        //    row.ItemArray = new object[] { "2", "1,2", "1,45", "1,55" };
        //    table.Rows.Add(row);
        //    row = table.NewRow();
        //    row.ItemArray = new object[] { "4", "1,3", "1,55", "1,75" };
        //    table.Rows.Add(row);
        //    row = table.NewRow();
        //    row.ItemArray = new object[] { "6", "1,4", "1,75", "1,9" };
        //    table.Rows.Add(row);
        //    row = table.NewRow();
        //    row.ItemArray = new object[] { "8", "1,5", "1,9", "2" };
        //    table.Rows.Add(row);

        //    return table;
        //}
        DataTable ρРУБTable() {
            DataTable table = new DataTable();
            DataColumn material = new DataColumn("Материал\r\nперегородки ", typeof(string)), Values = new DataColumn("Значение", typeof(string));
            table.Columns.AddRange(new DataColumn[] { material, Values });
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { "ХН78Т", "1,16" };
            table.Rows.Add(row);
            row = table.NewRow();
            row.ItemArray = new object[] { "ВТ-1", "0,47" };
            table.Rows.Add(row);
            row = table.NewRow();
            row.ItemArray = new object[] { "ПТ-7М", "1,08" };
            table.Rows.Add(row);

            return table;
        }
        //(double left, double right) bounds_Da => (left: 1.4 * Model.Common.Di, right: 2 * Model.Common.Di);
        //double Dp => Model.Common.Dpст + Model.Common.ΔГ2;
        
        #endregion
    }
}
