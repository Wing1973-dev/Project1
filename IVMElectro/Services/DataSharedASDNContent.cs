using System;
using System.Collections.Generic;
using System.Data;
using IVMElectro.Services.Directories;

namespace IVMElectro.Services {
    static class DataSharedASDNContent {
        #region string error
        public const string errorP12 = "Значение параметра Pʹ2 должно принадлежать [50 : 200000].";
        public const string errorU1 = "Значение параметра U1 должно принадлежать [48 : 660].";
        public const string errorf1 = "Значение параметра f1 должно принадлежать [10 : 400].";
        public const string errorDi = "Значение параметра Di должно принадлежать (50 : 1100].";
        public const string errorDa = "Ошибочное значение параметра Da. Ошибочные значения диапазона.";
        public const string errorDрст = "Ошибочное значение параметра Dp.ст. Ошибочные значения диапазона.";
        public const string errorΔГ1 = "Значение параметра ΔГ1 должно принадлежать [0 : 10].";
        public const string errorZ1 = "Ошибочное значение параметра Z1.";
        public const string errora1 = "Ошибочное значение параметра a1.";
        public const string errora2 = "Значение параметра a2 должно принадлежать [0 : 30].";
        public const string errorac = "Ошибочное значение параметра ac. Ошибочные значения диапазона.";
        public const string errordв = "Ошибочное значение параметра dв. Ошибочные значения диапазона.";
        public const string errordкп = "Ошибочное значение параметра dкп. Ошибочные значения диапазона.";
        public const string errorbZH = "Ошибочное значение параметра bZH. Ошибочные значения диапазона.";
        public const string errorΔкр = "Значение параметра Δкр должено принадлежать [3 : 40].";
        public const string errordиз = "Ошибочное значение параметра dиз.";
        public const string errorqГ = "Ошибочное значение параметра qГ.";
        public const string errorbz1 = "Значение параметра bz1 должно принадлежать [3,5 : 15].";
        public const string errorh8 = "Значение параметра h8 должно принадлежать [0 : 20].";
        public const string errorh7 = "Значение параметра h7 должно принадлежать [0 : 2].";
        public const string errorh6 = "Значение параметра h6 должно принадлежать [0 : 20].";
        public const string errorbП1 = "Ошибочное значение параметра bП1.";
        public const string errorbП2RED = "Ошибочное значение параметра bП2.";
        public const string errorh5 = "Значение параметра h5 должено принадлежать [0,1 : 5].";
        public const string errorh3 = "Значение параметра h3 должено принадлежать [0 : 5].";
        public const string errorh4 = "Значение параметра h4 должено принадлежать [5 : 50].";
        public const string errorh1 = "Ошибочное значение параметра h1.";
        public const string errorli = "Значение параметра li должно принадлежать [0,5 : 3].";
        public const string errorcз = "Значение параметра cз должно принадлежать [0 : 200].";
        public const string errorbП = "Ошибочное значение параметра bП.";
        public const string errorKзап = "Значение параметра Kзап должно принадлежать [0,36 : 0,76].";
        public const string errorβ = "Значение параметра β должно принадлежать [0,5 : 0,95].";
        public const string errorK2 = "Ошибочное значение параметра K2.";
        public const string errorKfe1 = "Значение параметра Kfe должно принадлежать [0,9 : 1].";
        public const string errorρ1x = "Значение параметра ρ1x должно принадлежать [0,002 : 0.05].";
        public const string errorρРУБ = "Ошибочное значение параметра ρРУБ.";
        public const string errorB = "Значение параметра B должно принадлежать [0 : 20].";
        public const string errorp10_50 = "Ошибочное значение параметра p10_50.";
        public const string errorΔГ2 = "Значение параметра ΔГ2 должно принадлежать [0 : 5].";
        public const string errorbП2ASDN = "Значение параметра bП2 должно принадлежать [2 : 6].";
        public const string errorZ2 = "Ошибочное значение параметра Z2.";
        public const string errorρ2Г = "Значение параметра ρ2Г должено принадлежать [0,01 : 0,2].";
        public const string errorKfe2 = "Значение параметра Kfe2 должено принадлежать [0,9 : 1].";
        public const string errorγ = "Ошибочное значение параметра γ.";
        public const string errorhш = "Значение параметра hш должно принадлежать [0,5 : 1].";
        public const string errorbш = "Значение параметра bш должно принадлежать [1 : 2,5].";
        public const string errorhp2RED = "Значение параметра hp2 должно принадлежать [3 : 10]."; //для двойной клетки
        public const string errorhp2 = "Ошибочное значение параметра hp2.";
        public const string errorbкн = "Значение параметра bкн должно принадлежать [5 : 35].";
        public const string erroraкRED = "Ошибочное значение параметра aк.";
        public const string errorbкRED = "Ошибочное значение параметра bк.";
        public const string errory1 = "Ошибочное значение параметра y1. Ошибочные значения диапазона.";
        #endregion
        public static double bП1Calc(double Di, double h8, double h7, double h6, double bz1, double Z1) => Math.Round((Math.PI * (Di + 2 * (h8 + h7 + h6)) - bz1) / Z1, 3);
        public static List<string> p_Collection => new List<string> { "1", "2", "3", "4", "5", "6", "7", "8" };
        public static List<string> Z1Collection(double Di, int p) {
            int start = Convert.ToInt32(Math.Round(Math.PI * Di / 20 )),
                end = Convert.ToInt32(2 * Math.Round(Math.PI * Di / 10 ));

            List<string> bounds = new List<string> {
                start.ToString()
            };
            for (int i = start + 1; i < end; i++) {
                if (i % (6 * p) == 0)
                    bounds.Add(i.ToString());
            }
            if (end != 0)
                bounds.Add(end.ToString());

            return bounds;
        }
        public static List<string> a1Collection(int p, int Z1) {
            List<string> sequence = new List<string>() { "1" };
            double add;
            //K = [2 : p-1]
            for (int i = 2; i < p; i++) {
                add = Math.Round(Z1 / 3 / Convert.ToDouble(i) / Convert.ToDouble(p));
                if (add > 1 && add < p) {
                    if (sequence.Contains(add.ToString())) continue;
                    sequence.Add(add.ToString());
                }
            }
            if (!sequence.Contains(p.ToString()))
                sequence.Add(p.ToString());
            sequence.Add($"{2 * p}");
            return sequence;
        }
        public static double PмехBoundRight(double P12) => Math.Round(0.5 * P12, 2);
        public static string Get_liBounds(double U1, double I1, double Di)
            => $"[{Math.Round(3 * U1 * I1 * 30 * 1e3 / 0.75 / 4e4 / Math.PI / Di, 3)} : " +
            $"{Math.Round(3 * U1 * I1 * 30 * 1e3 / 0.5 / 1.5e3 / Math.PI / Di, 3)}]"; //label
        public static DataTable K2Table {
            get {
                DataTable table = new DataTable();
                DataColumn p = new DataColumn("Число пар полюсов", typeof(string)), NoneInsulating = new DataColumn("β ≥ 0,8\r\nЛобовые части секции\r\nне изолированы", typeof(string)),
                    Insulating = new DataColumn("β ≥ 0,8\r\nЛобовые части секции\r\nизолированы", typeof(string)),
                    β = new DataColumn("0,5 ≤ β < 0,8", typeof(string));
                table.Columns.AddRange(new DataColumn[] { p, NoneInsulating, Insulating, β });
                DataRow row = table.NewRow();
                row.ItemArray = new object[] { "2", "1,2", "1,45", "1,55" };
                table.Rows.Add(row);
                row = table.NewRow();
                row.ItemArray = new object[] { "4", "1,3", "1,55", "1,75" };
                table.Rows.Add(row);
                row = table.NewRow();
                row.ItemArray = new object[] { "6", "1,4", "1,75", "1,9" };
                table.Rows.Add(row);
                row = table.NewRow();
                row.ItemArray = new object[] { "8", "1,5", "1,9", "2" };
                table.Rows.Add(row);

                return table;
            }
        }
        public static List<string> bСК_Collection => new List<string> { "прямые", "скошенные" };
        public static List<string> Z2Collection( int p, int Z1, string bСК) {
            switch (p) {
                case 1: {
                        if (Z1 <= 12)
                            return bСК == "прямые" ? new List<string> { "9", "15" } : null;
                        else if (Z1 <= 18)
                            return bСК == "прямые" ? new List<string> { "11", "15", "21", "22" } : new List<string> { "26", "31", "33", "34", "35" };
                        else if (Z1 <= 24)
                            return bСК == "прямые" ? new List<string> { "19", "32" } : new List<string> { "18", "20", "26", "31", "33", "34", "35" };
                        else if (Z1 <= 30)
                            return bСК == "прямые" ? new List<string> { "22", "38" } : new List<string> { "20", "21", "23", "24", "37", "39", "40" };
                        else if (Z1 <= 36)
                            return bСК == "прямые" ? new List<string> { "26", "28", "44", "46" } : new List<string> { "25", "27", "29", "43", "45", "47" };
                        else if (Z1 <= 42)
                            return bСК == "прямые" ? new List<string> { "32", "33", "34", "50", "52" } : null;
                        else if (Z1 <= 48)
                            return bСК == "прямые" ? new List<string> { "38", "40", "56", "58" } : new List<string> { "37", "39", "41", "55", "57", "59" };
                    }
                    break;
                case 2: {
                        if (Z1 <= 12)
                            return bСК == "прямые" ? new List<string> { "9" } : new List<string> { "15" };
                        else if (Z1 <= 18)
                            return bСК == "прямые" ? new List<string> { "10", "14" } : new List<string> { "18", "22" };
                        else if (Z1 <= 24)
                            return bСК == "прямые" ? new List<string> { "17" } : new List<string> { "16", "18", "30", "33", "34", "35", "36" };
                        else if (Z1 <= 36)
                            return bСК == "прямые" ? new List<string> { "26", "44", "46" } : new List<string> { "27", "28", "30", "34", "45", "48" };
                        else if (Z1 <= 42)
                            return bСК == "прямые" ? new List<string> { "52", "54" } : new List<string> { "34", "53" };
                        else if (Z1 <= 48)
                            return bСК == "прямые" ? new List<string> { "34", "38", "56", "58", "62", "64" } : new List<string> { "40", "57", "59" };
                        else if (Z1 <= 60)
                            return bСК == "прямые" ? new List<string> { "50", "52", "68", "70", "74" } : new List<string> { "48", "49", "51", "56", "64", "69", "71" };
                        else if (Z1 <= 72)
                            return bСК == "прямые" ? new List<string> { "62", "64", "80", "82", "86" } : new List<string> { "61", "63", "68", "76", "81", "83" };
                    }
                    break;
                case 3: {
                        if (Z1 <= 36)
                            return bСК == "прямые" ? new List<string> { "26", "46" } : new List<string> { "33", "47", "49", "50" };
                        else if (Z1 <= 54)
                            return bСК == "прямые" ? new List<string> { "44", "64", "66", "68" } : new List<string> { "42", "43", "51", "65", "67" };
                        else if (Z1 <= 72)
                            return bСК == "прямые" ? new List<string> { "56", "58", "62", "82", "84", "86", "88" } :
                                new List<string> { "57", "59", "60", "61", "83", "85", "87", "90" };
                        else if (Z1 <= 90)
                            return bСК == "прямые" ? new List<string> { "74", "76", "78", "80", "100", "102", "104" } :
                                new List<string> { "75", "77", "79", "101", "103", "105" };
                    }
                    break;
                case 4: {
                        if (Z1 <= 48)
                            return bСК == "прямые" ? new List<string> { "36", "44", "62", "64" } : new List<string> { "35", "44", "61", "63", "65" };
                        else if (Z1 <= 72)
                            return bСК == "прямые" ? new List<string> { "56", "58", "86", "88", "90" } : new List<string> { "56", "57", "59", "85", "87", "89" };
                        else if (Z1 <= 84)
                            return bСК == "прямые" ? new List<string> { "66", "70", "98", "100", "102", "104" } : null;
                        else if (Z1 <= 96)
                            return bСК == "прямые" ? new List<string> { "78", "82", "110", "112", "114" } : new List<string> { "79", "80", "81", "83", "109", "111", "113" };
                    }
                    break;
                case 5: {
                        if (Z1 <= 60)
                            return bСК == "прямые" ? new List<string> { "44", "46", "74", "76" } : new List<string> { "57", "69", "77", "78", "79" };
                        else if (Z1 <= 90)
                            return bСК == "прямые" ? new List<string> { "68", "72", "74", "76", "104", "106", "108", "110", "112", "114" } :
                                new List<string> { "70", "71", "73", "87", "93", "107", "109" };
                        else if (Z1 <= 120)
                            return bСК == "прямые" ? new List<string> { "86", "88", "92", "94", "96", "98", "102", "104", "106", "134", "136", "138", "140", "142", "114", "146" } :
                                new List<string> { "99", "101", "103", "117", "123", "137", "139" };
                    }
                    break;
                case 6: {
                        if (Z1 <= 72)
                            return bСК == "прямые" ? new List<string> { "56", "64", "80", "88" } : new List<string> { "69", "75", "80", "89", "91", "92" };
                        else if (Z1 <= 90)
                            return bСК == "прямые" ? new List<string> { "68", "70", "74", "88", "98", "106", "108", "110" } : new List<string> { "86", "87", "93", "94" };
                        else if (Z1 <= 108)
                            return bСК == "прямые" ? new List<string> { "86", "88", "92", "100", "116", "124", "128", "130", "132" } :
                                new List<string> { "84", "89", "91", "104", "105", "111", "112", "127" };
                        else if (Z1 <= 144)
                            return bСК == "прямые" ? new List<string> { "124", "128", "136", "152", "160", "164", "166", "168", "170", "172" } :
                                new List<string> { "125", "127", "141", "147", "161", "163" };
                    }
                    break;
                case 7: {
                        if (Z1 <= 84)
                            return bСК == "прямые" ? new List<string> { "74", "94", "102", "104", "106" } : new List<string> { "75", "77", "79", "89", "91", "93", "103" };
                        else if (Z1 <= 126)
                            return bСК == "прямые" ? new List<string> { "106", "108", "116", "136", "144", "146", "148", "150", "152", "154", "158" } :
                                new List<string> { "107", "117", "119", "121", "131", "133", "135", "145" };
                    }
                    break;
                case 8: {
                        if (Z1 <= 96)
                            return bСК == "прямые" ? new List<string> { "84", "86", "106", "108", "116", "118" } : new List<string> { "90", "102" };
                        else if (Z1 <= 126)
                            return bСК == "прямые" ? new List<string> { "120", "122", "124", "132", "134", "156", "154", "164", "166", "168", "170", "172" } :
                                new List<string> { "138", "150" };
                    }
                    break;
            }
            return null;
        }
        public static ContentZ2 Z2Object(int p, int Z1, string bСК) {
            switch (p) {
                case 1: {
                        if (Z1 <= 12)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "9", "15" } }
                                 : new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string>() };
                        else if (Z1 <= 18)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "11", "15", "21", "22" } } : 
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "26", "31", "33", "34", "35" } };
                        else if (Z1 <= 24)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "19", "32" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "18", "20", "26", "31", "33", "34", "35" } };
                        else if (Z1 <= 30)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "22", "38" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "20", "21", "23", "24", "37", "39", "40" } };
                        else if (Z1 <= 36)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "26", "28", "44", "46" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "25", "27", "29", "43", "45", "47" } };
                        else if (Z1 <= 42)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "32", "33", "34", "50", "52" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string>() };
                        else if (Z1 <= 48)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "38", "40", "56", "58" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "37", "39", "41", "55", "57", "59" } };
                    }
                    break;
                case 2: {
                        if (Z1 <= 12)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "9" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "15" } };
                        else if (Z1 <= 18)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "10", "14" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "18", "22" } };
                        else if (Z1 <= 24)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "17" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "16", "18", "30", "33", "34", "35", "36" } };
                        else if (Z1 <= 36)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "26", "44", "46" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "27", "28", "30", "34", "45", "48" } };
                        else if (Z1 <= 42)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "52", "54" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "34", "53" } };
                        else if (Z1 <= 48)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "34", "38", "56", "58", "62", "64" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "40", "57", "59" } };
                        else if (Z1 <= 60)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "50", "52", "68", "70", "74" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "48", "49", "51", "56", "64", "69", "71" } };
                        else if (Z1 <= 72)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "62", "64", "80", "82", "86" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "61", "63", "68", "76", "81", "83" } };
                    }
                    break;
                case 3: {
                        if (Z1 <= 36)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "26", "46" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "33", "47", "49", "50" } };
                        else if (Z1 <= 54)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "44", "64", "66", "68" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "42", "43", "51", "65", "67" } };
                        else if (Z1 <= 72)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "56", "58", "62", "82", "84", "86", "88" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "57", "59", "60", "61", "83", "85", "87", "90" } };
                        else if (Z1 <= 90)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "74", "76", "78", "80", "100", "102", "104" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "75", "77", "79", "101", "103", "105" } };
                    }
                    break;
                case 4: {
                        if (Z1 <= 48)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "36", "44", "62", "64" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "35", "44", "61", "63", "65" } };
                        else if (Z1 <= 72)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "56", "58", "86", "88", "90" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "56", "57", "59", "85", "87", "89" } };
                        else if (Z1 <= 84)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "66", "70", "98", "100", "102", "104" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string>() };
                        else if (Z1 <= 96)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "78", "82", "110", "112", "114" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "79", "80", "81", "83", "109", "111", "113" } };
                    }
                    break;
                case 5: {
                        if (Z1 <= 60)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "44", "46", "74", "76" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "57", "69", "77", "78", "79" } };
                        else if (Z1 <= 90)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "68", "72", "74", "76", "104", "106", "108", "110", "112", "114" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "70", "71", "73", "87", "93", "107", "109" } };
                        else if (Z1 <= 120)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК,  p = p,  Z1 = Z1,
                                Z2 = new List<string> { "86", "88", "92", "94", "96", "98", "102", "104", "106", "134", "136", "138", "140", "142", "114", "146" }
                            } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "99", "101", "103", "117", "123", "137", "139" } };
                    }
                    break;
                case 6: {
                        if (Z1 <= 72)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "56", "64", "80", "88" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "69", "75", "80", "89", "91", "92" } };
                        else if (Z1 <= 90)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "68", "70", "74", "88", "98", "106", "108", "110" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "86", "87", "93", "94" } };
                        else if (Z1 <= 108)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "86", "88", "92", "100", "116", "124", "128", "130", "132" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "84", "89", "91", "104", "105", "111", "112", "127" } };
                        else if (Z1 <= 144)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1,
                                Z2 = new List<string> { "124", "128", "136", "152", "160", "164", "166", "168", "170", "172" }
                            } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "125", "127", "141", "147", "161", "163" } };
                    }
                    break;
                case 7: {
                        if (Z1 <= 84)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "74", "94", "102", "104", "106" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "75", "77", "79", "89", "91", "93", "103" } };
                        else if (Z1 <= 126)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1,
                                Z2 = new List<string> { "106", "108", "116", "136", "144", "146", "148", "150", "152", "154", "158" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "107", "117", "119", "121", "131", "133", "135", "145" } };
                    }
                    break;
                case 8: {
                        if (Z1 <= 96)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "84", "86", "106", "108", "116", "118" } } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "90", "102" } };
                        else if (Z1 <= 126)
                            return bСК == "прямые" ? new ContentZ2 { bck = bСК, p = p, Z1 = Z1,
                                Z2 = new List<string> { "120", "122", "124", "132", "134", "156", "154", "164", "166", "168", "170", "172" }
                            } :
                                new ContentZ2 { bck = bСК, p = p, Z1 = Z1, Z2 = new List<string> { "138", "150" } };
                    }
                    break;
            }
            return null;
        }
        public static double I1(double P12, double U1) => P12 / 3 / 0.65 / U1;
        public static (double left, double right) Get_DaBounds(double Di) => (left: 1.2 * Di, right: 5 * Di);
        public static string Get_bкBounds(double bП2)  => $"[{bП2} : {5 * bП2}]";
        public static string Get_hpBounds(double Dpст, double ΔГ2) => 
            $"[{Math.Round(0.125 * Get_Dp(Dpст, ΔГ2), 2)} : {Math.Round(0.375 * Get_Dp(Dpст, ΔГ2), 2)}]";
        public static string Get_dвBounds(double Dpст, double ΔГ2, double hp) => $"[0 : {Math.Round(0.5 * (0.5 * Get_Dp(Dpст, ΔГ2) - hp), 2)}]";
        public static string Get_dвBounds(double Da) => $"[0,19 : {Math.Round(0.25 * Da, 2)}]";
        public static string Get_aкBounds(double hp) => $"[{Math.Round(1.05 * hp, 2)} : {Math.Round(1.5 * hp, 2)}]"; 
        public static double Get_Dp(double Dpст, double ΔГ2) => Dpст + ΔГ2;
        public static (double left, double right) Get_dкпBounds(double Dpст, double Z2) => (left: 5, right: 0.5 * Math.PI * Dpст / Z2);
        public static string dкпBoundsString((double left, double right) bounds) => $"[{bounds.left} : {Math.Round(bounds.right, 2)}]";
        public static (double left, double right) bounds_bZH(double Dpст, double hp, double Z2) => (left: 0.25 * Math.PI * (Dpст - 2 * hp) / Z2,
            right: 0.8 * Math.PI * (Dpст - 2 * hp) / Z2);
        public static string Get_bZHBounds((double left, double right) bounds) => $"[{Math.Round(bounds.left, 2)} : {Math.Round(bounds.right, 2)}]";
        public static double dпн(double Dpст, double hp, double Z2, double bZH) => (Math.PI * (Dpст - 2 * hp) / Z2 - bZH) / (1 - Math.PI / Z2);
        //public static double dпн(double Dpст, double hp, double Z2, double bZH) => Math.PI * (Dpст - 2 * hp) / (Z2 - Math.PI) - bZH;
        //public static double hp1(double hp, double hш, double dкп, double hp2, double dпн, double Z2) => (hp - hш - dкп - hp2 - dпн) / (1 + Math.Sin(2 * Math.PI / Z2));
        //public static double hp1(double hp, double hш, double dпн, double Z2) => 0.25 * (hp - hш - 2 * dпн) / Math.Tan(Math.PI / Z2);
        public static double hp1(double dпв, double dпн, double Z2) => 0.5 * (dпв - dпн) / Math.Tan(Math.PI / Z2);
        //public static double dпв(double dпн, double hp1, double Z2) => dпн + 2 * hp1 * Math.Sin(2 * Math.PI / Z2);
        //public static double dпв(double dпн, double hp1, double Z2) => dпн + 2 * hp1 * Math.Tan(Math.PI / Z2);
        public static double dпв(double Z2, double Dpст, double hш, double bZH) => (Math.PI * (Dpст - 2 * hш) / Z2 - bZH) / (1 + Math.PI / Z2);
        public static (double left, double right) bounds_aкн(double dпн, double dпв, double hp1) =>
            (left: Math.Round(0.5 * (dпн + dпв) + hp1, 2), right: Math.Round(1.2 * (0.5 * (dпн + dпв) + hp1), 2));
        public static string Get_aкнBounds((double left, double right) bounds) => $"[{Math.Round(bounds.left, 2)} : {Math.Round(bounds.right, 2)}]";
        public static (double left, double right) bounds_aк(double dкп, double hш, double hp2) => (left: dкп, right: dкп + hш + hp2);
        public static string Get_aкBounds((double left, double right) bounds) => $"[{Math.Round(bounds.left, 2)} : {Math.Round(bounds.right, 2)}]";
    }
}
