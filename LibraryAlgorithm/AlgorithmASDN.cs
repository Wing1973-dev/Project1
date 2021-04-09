
using System;
using System.Collections.Generic;

namespace LibraryAlgorithms {
    //расчет параметров мащины, магнитной цепи, холостой ход, номинальный режим
    //abstract: все переменные глобальной видимости типа передаются в функции неявно (экономия параметров функций)
    //требуется следить за использованием таких параметров при вызове локальных функций
    public class AlgorithmASDN {
        private const double Ki = 0.8;
        #region input data
        private double Z1, Z2, p, W1, a1, a2, y1, Di, Dp, f1, h1, h2, h3, h4, h5, h6, h7, h8, bП, bП1, bП2, ΔГ1, ΔГ2, hp, ac, li, Da,
            B, Δкр, ρ1x, ρ1Г, ρ2Г, ρРУБ, qГ, bк, aк,
            dиз, d1, bПН, cз, Kfe1, Kfe2, I1, U1, S = 0.1, //S хх
            αδ = 0.65, γ, dв, p10_50, Pмех,
            Pʹ2, bСК, K2;
        private bool P3; //тип обмотки (1 - true, 0 - false)
        //материал статора
        private enum MarkSteelStator {
            Сталь1412,
            Сталь2411,
            Сталь2412,
            Сталь1521
        }
        private MarkSteelStator mrkStlStr;
        //markSteelRotor; //материал ротора (не использутся за неименеем данных по кривой намагничивания (прил.4))  
        #endregion
        #region output data
        #region геометрические размеры и параметры машины
        private double lp, hʹj2, bZ2MIN, bZ2MAX, qс, qк, rʹ2, xʹ2; //параметры ротора
        private double hj1, Kзап, bZ1MAX, bZ1MIN, bZ1СР, m1, L, lB, r1x, lc; //параметры статора
        private double qИЗ, Sп, Sʹп, q1, Wc, np, nэл, β, k1; //характеристики обмотки
        #endregion
        #region расчет магнитной цепи
        private double Bz1, Bj1, Kδ1, Fz1, Fj1, r1Г, x1; //статор
        private double Bz2, Bj2, Kδ2, Fz2, Fj2; //ротор
        private double BδM, Fδ; //зазор
        private double Iμ, xμ, Kδ, ΣF; //прочее
        #endregion
        #region холостой ход
        private double Pz1, Pz1_round, Pj1, Pj1_round, PГ, PГ_round, Pпов1, Pпов1_round, Pпул1, Pпул1_round; //статор
        private double PFe2, PFe2_round, Pпов2, Pпов2_round, Pпул2, Pпул2_round; //ротор
        private double E1, P0, W0, I0А, IХХА, IXX, cosφ0; //прочее
        #endregion
        #region номинальный режим
        private double I1A, I1R, I1Н, I1Н_round, PЭ1, PЭ1_round, Δi1; //статор
        private double rʹʹ2ЭН, xʹʹ2ЭН, Iʹʹ2Н, nН, PЭ2, PЭ2_round, Δi2, ΔiК, r2ст, x2ст; //ротор
        private double MН, SН, E1нр, cosφН, cosφН_round, ηЭЛ, ηЭЛ_round, P1, P1_round, SK, SK_round, c1, rʹ1, xʹ1; //прочее
        #endregion
        //перегрузочная способность
        private double E1M, I1M, Iʹʹ2M, PM, MM, KM, SM, n2, cosφM;
        //пусковой режим
        private double Iʹʹ2П, I1П, I1П_round, MП, KП, KI, E1П;
        #endregion
        private double rʹʹ2Э, xʹʹ2Э;
        public AlgorithmASDN(Dictionary<string, double> input) {
            Pʹ2 = input["P12"]; U1 = input["U1"]; f1 = input["f1"]; p = input["p"]; Pмех = input["Pмех"];

            Di = input["Di"]; ΔГ1 = input["ΔГ1"]; Z1 = input["Z1"]; Da = input["Da"]; a1 = input["a1"]; a2 = input["a2"]; Δкр = input["Δкр"];
            dиз = input["dиз"]; qГ = input["qГ"]; h8 = input["h8"]; h7 = input["h7"]; h6 = input["h6"]; h5 = input["h5"]; h3 = input["h3"];
            h4 = input["h4"]; ac = input["ac"]; bПН = input["bПН"]; li = input["li"]; cз = input["cз"]; y1 = input["y1"]; K2 = input["K2"];
            d1 = input["d1"]; Kfe1 = input["Kfe1"]; ρ1x = input["ρ1x"]; ρРУБ = input["ρРУБ"]; ρ1Г = input["ρ1Г"]; B = input["B"]; 
            P3 = input["P3"] != 0; p10_50 = input["p10_50"]; β = input["β"];
            W1 = input["W1"]; Wc = input["Wc"];

            ΔГ2 = input["ΔГ2"]; bСК = input["bСК"] == 1 ? Math.PI * Di / Z1 : 0; bП2 = input["bП2"]; Z2 = input["Z2"]; hp = input["hp"]; dв = input["dв"];
            bк = input["bк"]; aк = input["aк"]; γ = input["γ"]; ρ2Г = input["ρ2Г"]; Kfe2 = input["Kfe2"];

            bП1 = input["bП1"]; Dp = input["Dpст"] + ΔГ2;  h1 = input["h1"]; h2 = input["h2"];  bП = input["bП"]; 
            I1 = Pʹ2 / 1.95 / U1; 
            
            if (p10_50 == 1.94) mrkStlStr = MarkSteelStator.Сталь1412;
            else if (p10_50 == 1.38) mrkStlStr = MarkSteelStator.Сталь2412;
            else if (p10_50 == 1.7) mrkStlStr = MarkSteelStator.Сталь2411;
            else if (p10_50 == 19.6) mrkStlStr = MarkSteelStator.Сталь1521;

            SolutionIsDone = false; Logging = new List<string>();
        }
        
        //Расчетные алгоритмы: 
        //расчет параметров мащины и магнитной цепи, холостой ход, номинальный режим
        public void Run() {
            
            SolutionIsDone = true;
        }
        
        #region результаты работы
        //B: Гс -> Тл
        //F: кг -> Н
        //геометрические размеры и параметры машины
        public Dictionary<string, Dictionary<string, double>> Get_DataMachine => SolutionIsDone ?
                    new Dictionary<string, Dictionary<string, double>>() {
                        { "ротор", new Dictionary<string, double>() {
                                { "lp", Math.Round(lp, 5) },
                                { "hʹj2", Math.Round(hʹj2, 5) },
                                { "bZ2MIN", Math.Round(bZ2MIN, 5) },
                                { "bZ2MAX", Math.Round(bZ2MAX, 5) },
                                { "qс", Math.Round(qс, 5) },
                                { "qк", Math.Round(qк, 5) },
                                { "Pʹ2", Math.Round(Pʹ2, 5) },
                                { "rʹ2", Math.Round(rʹ2, 5) },
                                { "xʹ2", Math.Round(xʹ2, 5) }
                            }
                        },
                        { "статор", new Dictionary<string, double>() {
                                { "hj1", Math.Round(hj1, 5) },
                                { "hZ1", Math.Round(h2 + 2 * h3 + h4 + h5 + h6 + h7 + h8, 5) },
                                { "Kз", Math.Round(Kзап, 5) },
                                { "bZ1MAX", Math.Round(bZ1MAX, 5) },
                                { "bZ1MIN", Math.Round(bZ1MIN, 5) },
                                { "bZ1СР", Math.Round(bZ1СР, 5) },
                                { "lГ", Math.Round(li + cз, 5) },
                                { "m1", Math.Round(m1, 5) },
                                { "L", Math.Round(L, 5) },
                                { "lB", Math.Round(lB, 5) },
                                { "r1x", Math.Round(r1x, 5) },
                                { "lc", Math.Round(lc, 5) }
                            }
                        },
                        { "общие параметры", new Dictionary<string, double>() {
                                { "Tлп", Math.Round(0.165 * (L - 2 * (li + 2 * Δкр)) * PЭ1 / Z1 / nэл / qГ, 5) }
                            }
                        },
                        { "обмотка", new Dictionary<string, double>() {
                                { "qИЗ", Math.Round(qИЗ, 5) },
                                { "Sп", Math.Round(Sп, 5) },
                                { "Sʹп", Math.Round(Sʹп, 5) },
                                { "q1", Math.Round(q1, 5) },
                                { "Wc", Math.Round(Wc, 5) },
                                { "np", Math.Round(np, 5) },
                                { "nэл", Math.Round(nэл, 5) },
                                { "WЭФ", Math.Round(W1 * k1) },
                                { "β", Math.Round(β, 5) },
                                { "k1", Math.Round(k1, 5) }
                            }
                        }
                    } : null;
        //расчет магнитной цепи (Bz1 * 1e-4, Bj1 * 1e-4, Bz2 * 1e-4, Bj2 * 1e-4, BδM * 1e-4)
        public Dictionary<string, Dictionary<string, double>> Get_MagneticCircuit => SolutionIsDone ?
                    new Dictionary<string, Dictionary<string, double>>() {
                        { "ротор", new Dictionary<string, double>() {
                                { "Bz2", Math.Round(Bz2 * 1e-4 * 1e-4, 5) },
                                { "Bj2", Math.Round(Bj2 * 1e-4 * 1e-4, 5) },
                                { "Kδ2", Math.Round(Kδ2, 5) },
                                { "Fz2", Math.Round(Fz2, 5) },
                                { "Fj2", Math.Round(Fj2, 5) }
                            }
                        },
                        { "статор", new Dictionary<string, double>() {
                                { "Bz1", Math.Round(Bz1 * 1e-4 * 1e-4, 5) },
                                { "Bj1", Math.Round(Bj1 * 1e-4 * 1e-4, 5) },
                                { "Kδ1", Math.Round(Kδ1, 5) },
                                { "Fz1", Math.Round(Fz1, 5) },
                                { "Fj1", Math.Round(Fj1, 5) },
                                { "r1Г", Math.Round(r1Г, 5) },
                                { "x1", Math.Round(x1, 5) }
                            }
                        },
                        { "зазор", new Dictionary<string, double>() {
                                { "BδM", Math.Round(BδM * 1e-4 * 1e-4, 5) },
                                { "Fδ", Math.Round(Fδ, 5) }
                            }
                        },
                        { "прочее", new Dictionary<string, double>() {
                                { "Iμ", Math.Round(Iμ, 5) },
                                { "xμ", Math.Round(xμ, 5) },
                                { "Kδ", Math.Round(Kδ, 5) },
                                { "ΣF", Math.Round(ΣF, 5) }
                            }
                        }
                    } : null;
        //холостой ход
        public Dictionary<string, Dictionary<string, double>> Get_Idle => SolutionIsDone ?
                    new Dictionary<string, Dictionary<string, double>>() {
                        { "ротор", new Dictionary<string, double>() {
                                { "PFe2", Math.Round(PFe2, 5) },
                                { "PFe2 окр", Math.Round(PFe2_round, 5) },
                                { "Pпов2", Math.Round(Pпов2, 5) },
                                { "Pпов2 окр", Math.Round(Pпов2_round, 5) },
                                { "Pпул2", Math.Round(Pпул2, 5) },
                                { "Pпул2 окр", Math.Round(Pпул2_round, 5) }
                            }
                        },
                        { "статор", new Dictionary<string, double>() {
                                { "Pz1", Math.Round(Pz1, 5) },
                                { "Pz1 окр", Math.Round(Pz1_round, 5) },
                                { "Pj1", Math.Round(Pj1, 5) },
                                { "Pj1 окр", Math.Round(Pj1_round, 5) },
                                { "PГ", Math.Round(PГ, 5) },
                                { "PГ окр", Math.Round(PГ_round, 5) },
                                { "Pпов1", Math.Round(Pпов1, 5) },
                                { "Pпов1 окр", Math.Round(Pпов1_round, 5) },
                                { "Pпул1", Math.Round(Pпул1, 5) },
                                { "Pпул1 окр", Math.Round(Pпул1_round, 5) }
                            }
                        },
                        { "прочее", new Dictionary<string, double>() {
                                { "E1", Math.Round(E1, 5) },
                                { "P0", Math.Round(P0, 5) },
                                { "W0", Math.Round(W0, 5) },
                                { "I0А", Math.Round(I0А, 5) },
                                { "IХХА", Math.Round(IХХА, 5) },
                                { "IXX", Math.Round(IXX, 5) },
                                { "cosφ0", Math.Round(cosφ0, 5) }
                            }
                        }
                    } : null;
        //номинальный режим (SK, SK окр - кВ*А, MН * 9.8)
        public Dictionary<string, Dictionary<string, double>> Get_NominalRating => SolutionIsDone ?
                    new Dictionary<string, Dictionary<string, double>>() {
                        { "ротор", new Dictionary<string, double>() {
                                { "rʹʹ2Э", Math.Round(rʹʹ2ЭН, 5) },
                                { "xʹʹ2Э", Math.Round(xʹʹ2ЭН, 5) },
                                { "Iʹʹ2Н", Math.Round(Iʹʹ2Н, 5) },
                                { "nН", Math.Round(nН, 5) },
                                { "PЭ2", Math.Round(PЭ2, 5) },
                                { "PЭ2 окр", Math.Round(PЭ2_round, 5) },
                                { "Δi2", Math.Round(Δi2, 5) },
                                { "ΔiК", Math.Round(ΔiК, 5) },
                                { "r2ст", Math.Round(r2ст, 5) },
                                { "x2ст", Math.Round(x2ст, 5) }
                            }
                        },
                        { "статор", new Dictionary<string, double>() {
                                { "I1A", Math.Round(I1A, 5) },
                                { "I1R", Math.Round(I1R, 5) },
                                { "I1Н", Math.Round(I1Н, 5) },
                                { "I1Н окр", Math.Round(I1Н_round, 5) },
                                { "PЭ1", Math.Round(PЭ1, 5) },
                                { "PЭ1 окр", Math.Round(PЭ1_round, 5) },
                                { "Δi1", Math.Round(Δi1, 5) }
                            }
                        },
                        { "прочее", new Dictionary<string, double>() {
                                { "MН", Math.Round(MН * 9.8, 5) },
                                { "SН", Math.Round(SН, 5) },
                                { "E1нр", Math.Round(E1нр, 5) },
                                { "cosφН", Math.Round(cosφН, 5) },
                                { "cosφН окр", Math.Round(cosφН_round, 5) },
                                { "ηЭЛ", Math.Round(ηЭЛ, 5) },
                                { "ηЭЛ окр", Math.Round(ηЭЛ_round, 5) },
                                { "P1", Math.Round(P1, 5) },
                                { "P1 окр", Math.Round(P1_round, 5) },
                                { "SK", Math.Round(SK, 5) },
                                { "SK окр", Math.Round(SK_round, 5) },
                                { "A", Math.Round(60 * W1 * I1Н / Math.PI / Di, 5) },
                                { "c1", Math.Round(c1, 5) },
                                { "rʹ1", Math.Round(rʹ1, 5) },
                                { "xʹ1", Math.Round(xʹ1, 5) }
                            }
                        }
                    } : null;
        //перегрузочная способность (MM * 9.8)
        public Dictionary<string, double> Get_OverloadCapacity => SolutionIsDone ?
                    new Dictionary<string, double>() {
                        {"E1M", Math.Round(E1M, 5) },
                        {"I1M", Math.Round(I1M, 5) },
                        {"Iʹʹ2M", Math.Round(Iʹʹ2M, 5) },
                        {"PM", Math.Round(PM, 5) },
                        {"MM", Math.Round(MM * 9.8, 5) },
                        {"KM", Math.Round(KM, 5) },
                        {"SM", Math.Round(SM, 5) },
                        {"n2", Math.Round(n2, 5) },
                        {"cosφM", Math.Round(cosφM, 5) }
                    } : null;
        //пусковой режим (MП * 9.8)
        public Dictionary<string, double> Get_StartingConditions => SolutionIsDone ?
                    new Dictionary<string, double>() {
                        { "Iʹʹ2П", Math.Round(Iʹʹ2П, 5) },
                        { "I1П", Math.Round(I1П, 5) },
                        { "I1П окр", Math.Round(I1П_round, 5) },
                        { "MП", Math.Round(MП * 9.8, 5) },
                        { "KП", Math.Round(KП, 5) },
                        { "KI", Math.Round(KI, 5) },
                        { "E1П", Math.Round(E1П, 5) }
                    } : null;
        //тепловой расчет
        public Dictionary<string, double> Get_HeatCalculation => SolutionIsDone ?
            new Dictionary<string, double>() {
                        { "P1 окр", Math.Round(P1_round*0.001, 5) },
                        { "Pʹ2", Math.Round(Pʹ2*0.001, 5) },
                        { "Pмех", Math.Round(Pмех, 5) },
                        { "Pʹ2-Pмех", Math.Round(Pʹ2-Pмех, 5) },
                        { "U1", Math.Round(U1, 5) },
                        { "f1", Math.Round(f1, 5) },
                        { "I1Н окр", Math.Round(I1Н_round, 5) },
                        { "n", Math.Round(nН, 5) },
                { "PЭ1 окр", Math.Round(PЭ1_round*0.001, 5) },
                { "Pj1 окр", Math.Round(Pj1_round*0.001, 5) },
                { "Зубцы железа", Math.Round(Pz1_round + Pпов1_round + Pпул1_round * 0.001, 5) }, 
                { "PГ окр", Math.Round(PГ_round * 0.001, 5) },
                { "Ротор", Math.Round(PЭ2_round + PFe2_round + Pпов2_round + Pпул2_round * 0.001, 5) },
                { "Суммарные электрические потери",
                Math.Round(PЭ1_round*0.001, 5) + Math.Round(Pj1_round*0.001, 5) + Math.Round(Pz1_round + Pпов1_round + Pпул1_round * 0.001, 5) +
                    Math.Round(PГ_round * 0.001, 5) + Math.Round(PЭ2_round + PFe2_round + Pпов2_round + Pпул2_round * 0.001, 5) },
                { "Dp", Dp  },
                { "Di", Di  },
                { "Da", Da  },
                { "li", li  },
                { "2 * Δкр", 2 * Δкр  }, 
                { "Z1", Z1  },
                { "ΔГ1", ΔГ1  }
            } : null;
        public Dictionary<string, string> Get_HeatCalculationStringData => SolutionIsDone ?
            new Dictionary<string, string>() {
                { "Материал железа статора", mrkStlStr.ToString()  }
            } : null;
        public Dictionary<string, double> Get_StatorRotorCalculation => SolutionIsDone ?
            new Dictionary<string, double>() {
                { "Da", Da  },
                { "Di", Di  },
                { "Dp", Dp  },
                { "D'p", Dp +  ΔГ2 },
                { "li", li  },
                { "Kл", Δкр  },
                { "F", Math.Round(lB, 5)  },
                { "B", B  },
                { "ΔГ1", ΔГ1  },
                { "ΔГ2", ΔГ2  },
                { "hz2", hp  },
                { "aк", aк  },
                { "bк", bк  },
                { "dв", dв  },
                { "bП2", bП2  },
                { "Z1", Z1  },
                { "Z2", Z2  },
                { "hZ1", Math.Round(h2 + 2 * h3 + h4 + h5 + h6 + h7 + h8, 5)  },
                { "bП", bП  },
                { "bП1", bП1  },
                { "bПН", bПН  },
                { "h2", h2  },
                { "h3", h3  },
                { "h4", h4  },
                { "h5", h5  },
                { "h6", h6  },
                { "h7", h7  },
                { "h8", h8  },
                { "ΔИЗ", h3  },
                { "ac", ac  },
                { "Провод обмоточный", Math.Round(qГ/dиз, 5)  },
                { "Pʹ2", Math.Round(Pʹ2, 5) },
                { "P1", Math.Round(P1_round, 5) },
                { "SK", Math.Round(SK_round, 5) },
                { "I1Н", Math.Round(I1Н, 5) },
                { "nН", Math.Round(nН, 5) },
                { "I1П", Math.Round(I1П, 5) }
            } : null;
        #endregion
        //solution is done
        public bool SolutionIsDone { get; private set; }
        public List<string> Logging { get; private set; }
    }
}
