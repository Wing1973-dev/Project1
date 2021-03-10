using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using static IVMElectro.Services.DataSharedASDNContent;

namespace IVMElectro.Models {
    public class AsdnCommonModel : DatasetFromModels, IValidatableObject {
        #region fields
        double bгрс => bП1 + 2 * (h5 + h4) * Math.Sin(Math.PI / Convert.ToDouble(Z1));
        #region machine parameters 
        /// <summary>
        /// Pʹ2
        /// </summary>
        public double P12 { get; set; }
        public double U1 { get; set; }
        public double f1 { get; set; }
        /// <summary>
        /// число пар полюсов
        /// </summary>
        public int p { get; set; } //is defined in the interface (not checked)
        public double Pмех { get; set; }
        #endregion
        #region stator parameters
        public double Di { get; set; }
        public double ΔГ1 { get; set; }
        public int Z1 { get; set; }
        public double q1 => Z1 / (6.0 * p);
        public double Da { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public double Δкр { get; set; }
        public double dиз { get; set; }
        public double qГ { get; set; }
        public double bz1 { get; set; }
        public double h8 { get; set; }
        public double h7 { get; set; }
        public double h6 { get; set; }
        public double bП1_calc => bП1Calc(Di, h8, h7, h6, bz1, Z1); //interface output only
        public double bП1 { get; set; }
        public double h5 { get; set; }
        public double h3 { get; set; }
        public double h4 { get; set; }
        public double ac { get; set; }
        public double bПН => Math.Round(ac * 2 * h8 * Math.Tan(Math.PI / 12), 3);
        public double h1 { get; set; }  //dep 51
        public double h1_calc => h8 + h7 + h6 + h5 + h4 + 2 * h3 + h2;  //interface output only 
        public double h2 { get; set; }  //dep 51
        public double h2_calc => Get_h2(Math.Tan(Math.PI / Convert.ToDouble(Z1)), bП1 + 2 * (h5 + h4 + h3) * Math.Tan(Math.PI / Convert.ToDouble(Z1)), Sсл);  
        public double li { get; set; }
        public double cз { get; set; }
        public double bП { get; set; }
        public double bП_calc => 2 * (h2 + 2 * h3 + h4 + h5) * Math.Tan(Math.PI / Z1);  //interface output only 
        public int W1 { get; set; }
        public int Wc { get; set; }
        public double Wc_calc => 0.5 * W1 * a1 / Convert.ToDouble(p * q1);
        //public double Kзап { get; set; }
        public double Kзап => KзапCalculation();
        public double y1 { get; set; }
        public double β => Z1 == 0 ? double.NaN : 2 * p * y1 / Z1; //interface output only 
        public double K2 { get; set; } 
        public double d1 { get; set; }
        public double Kfe1 { get; set; }
        public double ρ1x { get; set; }
        public double ρРУБ { get; set; } 
        public double ρ1Г { get; set; }
        public double B { get; set; }
        public double p10_50 { get; set; }
        public double Sсл => (bП1 + h5 * Math.Tan(Math.PI / Convert.ToDouble(Z1)) + (h5 + h4) * Math.Tan(Math.PI / Convert.ToDouble(Z1))) * h4;
        public string PR { get; set; } //признак формы паза статора
        #endregion
        #region rotor parameters
        public double ΔГ2 { get; set; }
        public double Dpст { get; set; }
        public string bСК { get; set; } //input in the interface (not checked) (прямые, скошенные)
        public int Z2 { get; set; }
        public double ρ2Г { get; set; }
        public double Kfe2 { get; set; }
        public double hp { get; set; }
        #endregion
        #endregion
        public AsdnCommonModel() {
            ΔГ1 = ΔГ2 = Di = Da = Pмех = P12 = U1 = h4 = h6 = li = ac = Kfe2 = Δкр = d1 = h8 = y1 = bz1 = Dpст = hp = ρРУБ = K2 = qГ = dиз = p10_50 = bП =
            h1 = h2 = bП1 = 0;
            a2 = a1 = Z2 = Z1 =  W1 = Wc = 0; p = 1;
            f1 = 50; h7 = 1; h5 = 1.9; h3 = 0.7; cз = 100;  Kfe1 = 0.93; ρ1x = 0.0175; ρ1Г = 0.0247; B = 10; ρ2Г = 0.0224; 
            
            bСК = "прямые"; p = 1; PR = "трапецеидальный";
        }
        public override void CreationDataset() => Dataset = new Dictionary<string, double> {
            { "P12", P12 }, { "U1", U1 }, { "f1", f1 }, { "p", p }, { "Pмех", Pмех }, 
            { "Di", Di }, { "ΔГ1", ΔГ1 }, { "Da", Da }, { "a2", a2 }, { "a1", a1 }, { "Δкр", Δкр }, { "bz1", bz1 }, { "h8", h8 }, { "h7", h7 }, { "h6", h6 },
            { "bП1", bП1 }, { "h1", h1 }, { "h2", h2 }, { "bП", bП }, { "β", β }, { "Sсл", Sсл },
            { "h5", h5 }, { "h3", h3 }, { "h4", h4 }, { "ac", ac }, { "bПН", bПН }, { "li", li }, { "cз", cз }, { "W1", W1}, { "Wc", Wc}, { "y1", y1 }, { "d1", d1 }, 
            { "Kfe1", Kfe1 }, { "ρ1x", ρ1x }, { "ρ1Г", ρ1Г }, { "B", B }, 
            { "ΔГ2", ΔГ2 },  { "Dpст", Dpст},  { "ρ2Г", ρ2Г }, { "Kfe2", Kfe2 }, { "hp", hp }, { "Z2", Z2 }, { "Z1", Z1 }, 
            { "ρРУБ", ρРУБ }, { "K2", K2 }, { "qГ", qГ }, { "dиз", dиз }, { "p10_50", p10_50 } };
        double Get_h2(double A, double B, double Ssl) => (-B + Math.Sqrt(B * B + 4 * A * Ssl)) / (2 * A);
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            List<ValidationResult> errors = new List<ValidationResult>();

            #region machine parameters 
            if (!((50 <= P12) && (P12 <= 2e5))) errors.Add(new ValidationResult(errorP12));
            if (!((48 <= U1) && (U1 <= 660))) errors.Add(new ValidationResult(errorU1));
            if (!((10 <= f1) && (f1 <= 400))) errors.Add(new ValidationResult(errorf1));
            //if (p < 0) errors.Add(new ValidationResult("Error p")); //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            if (!((0 <= Pмех) && (Pмех <= PмехBoundRight(P12))))
                errors.Add(new ValidationResult($"Значение параметра Pмех должно принадлежать [0 : {PмехBoundRight(P12)}]."));
            #endregion
            #region stator parameters
            if (!((50 < Di) && (Di <= 800))) errors.Add(new ValidationResult(errorDi));
            if ((!(0 <= ΔГ1) && (ΔГ1 <= 0))) errors.Add(new ValidationResult(errorΔГ1));
            if ((Z1 < 0) || !int.TryParse(Z1.ToString(), out _)) errors.Add(new ValidationResult(errorZ1));
            if (!((Get_DaBounds(Di).left <= Da) && (Da < Get_DaBounds(Di).right)))
                errors.Add(new ValidationResult($"Значение параметра Da должно принадлежать [{Math.Round(Get_DaBounds(Di).left, 2)} : {Math.Round(Get_DaBounds(Di).right, 2)})."));
            if ((a1 < 0) || !int.TryParse(a1.ToString(), out _)) errors.Add(new ValidationResult(errora1));
            if (!int.TryParse(a2.ToString(), out _)) errors.Add(new ValidationResult(errora2));
            if (!((0 <= a2) && (a2 <= 30))) errors.Add(new ValidationResult(errora2));
            if ((W1 < 0) || !int.TryParse(W1.ToString(), out _)) errors.Add(new ValidationResult(errorW1));
            if(W1%2!=0) errors.Add(new ValidationResult(errorW1parity));
            if ((Wc < 0) || !int.TryParse(Wc.ToString(), out _)) errors.Add(new ValidationResult(errorWc));
            if (!((3 <= Δкр) && (Δкр <= 40))) errors.Add(new ValidationResult(errorΔкр));
            if ((dиз < 0) || double.IsNaN(dиз)) errors.Add(new ValidationResult(errordиз));
            if ((qГ < 0) || double.IsNaN(qГ)) errors.Add(new ValidationResult(errorqГ));
            if (!((3.5 <= bz1) && (bz1 <= 15))) errors.Add(new ValidationResult(errorbz1));
            if (!((0 <= h8) && (h8 <= 20))) errors.Add(new ValidationResult(errorh8));
            if (!((0 <= h7) && (h7 <= 2))) errors.Add(new ValidationResult(errorh7));
            if (!((0 <= h6) && (h6 <= 20))) errors.Add(new ValidationResult(errorh6));
            if (!((0.1 <= h5) && (h5 <= 5))) errors.Add(new ValidationResult(errorh5));
            if (!((0 <= h3) && (h3 <= 5))) errors.Add(new ValidationResult(errorh3));
            if (!((5 <= h4) && (h4 <= 50))) errors.Add(new ValidationResult(errorh4));
            //if (!((dиз < ac) && (ac < 2 * dиз))) errors.Add(new ValidationResult($"Значение параметра расчета ac должно принадлежать ({dиз} : {2 * dиз})."));
            if (!(dиз < ac)) errors.Add(new ValidationResult($"Значение параметра ac должно быть > {dиз}."));
            if (!((0 <= bПН) && (bПН <= bП1Calc(Di, h8, h7, h6, bz1, Z1)))) 
                errors.Add(new ValidationResult($"Значение параметра bПН должно принадлежать [0 : {Math.Round(bП1Calc(Di, h8, h7, h6, bz1, Z1), 2)}]."));
            if (h1 < 0 || double.IsNaN(h1)) errors.Add(new ValidationResult(errorh1));
            if (h2 < 0 || double.IsNaN(h2)) errors.Add(new ValidationResult(errorh2));
            if (!((0.5 <= li) && (li >= 3))) 
                errors.Add(new ValidationResult($"Значение параметра li должно принадлежать [{Get_liBounds(U1, I1(P12, U1), Di).left} : " +
                                $"{Get_liBounds(U1, I1(P12, U1), Di).right}]."));
            if (!((0 <= cз) && (cз <= 200))) errors.Add(new ValidationResult(errorcз));
            if (bП < 0 || double.IsNaN(bП)) errors.Add(new ValidationResult(errorbП));
            if (bП1 < 0 || double.IsNaN(bП1)) errors.Add(new ValidationResult(errorbП1));
            //if (!((0.36 <= Kзап) && (Kзап <= 0.76))) errors.Add(new ValidationResult(errorKзап));
            if (!((1 <= y1) && (y1 <= 0.5 * Z1 / Convert.ToDouble(p))))
                errors.Add(new ValidationResult($"Значение параметра расчета y1 должно принадлежать [1 : {0.5 * Z1 / Convert.ToDouble(p)}]."));
            if (!((0.5 <= β) && (β <= 0.95))) errors.Add(new ValidationResult(errorβ));
            if ((K2 < 0) || double.IsNaN(K2)) errors.Add(new ValidationResult(errorK2));
            //if (string.IsNullOrEmpty(PR)) errors.Add(new ValidationResult("Error PR.")); //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            if (!((0 <= d1) && (d1 <= bП1Calc(Di, h8, h7, h6, bz1, Z1))))
                errors.Add(
                    new ValidationResult($"Значение параметра расчета d1 должно принадлежать [0 : {Math.Round(bП1Calc(Di, h8, h7, h6, bz1, Z1), 2)}].")); 
            if (!((0.9 <= Kfe1) && (Kfe1 <= 1))) errors.Add(new ValidationResult(errorKfe1));
            if (!((0.002 <= ρ1x) && (ρ1x <= 0.05))) errors.Add(new ValidationResult(errorρ1x));
            if ((ρРУБ < 0) || double.IsNaN(ρРУБ)) errors.Add(new ValidationResult(errorρРУБ));
            if (!((ρ1x <= ρ1Г) && (ρ1Г <= 0.1235)))
                errors.Add(new ValidationResult($"Значение параметра расчета ρ1Г должно принадлежать [{ρ1x} : 0.1235]."));
            if (!((0 <= B) && (B <= 20))) errors.Add(new ValidationResult(errorB));
            if ((p10_50 < 0) || double.IsNaN(p10_50)) errors.Add(new ValidationResult(errorp10_50));
            #endregion
            #region rotor parameters
            if (!((0 <= ΔГ2) && (ΔГ2 <= 5))) errors.Add(new ValidationResult(errorΔГ2));
            //if (!((DpстBoundCalculation - 5 <= Dpст) && (Dpст < DpстBoundCalculation - 0.1))) 
            //    errors.Add(new ValidationResult($"Значение параметра Dp.ст должно принадлежать [{ Math.Round(DpстBoundCalculation - 5, 2) } : { Math.Round(DpстBoundCalculation - 0.1, 2)})."));
            if (double.IsNaN(Dpст) || Dpст < 0)
                errors.Add(new ValidationResult(errorDpст));
            //if (string.IsNullOrEmpty(bСК)) errors.Add(new ValidationResult("Error bСК.")); //such a state is unattainable. This is used for  Validator.TryValidateObject(..., true)
            if ((Z2 < 0) || !int.TryParse(Z2.ToString(), out _)) errors.Add(new ValidationResult(errorZ2));
            
            if (!((0.01 <= ρ2Г) && (ρ2Г <= 0.2))) errors.Add(new ValidationResult(errorρ2Г));
            if (!((0.9 <= Kfe2) && (Kfe2 <= 1))) errors.Add(new ValidationResult(errorKfe2));
            //if (!((0.125 * Get_Dp(Dpст, ΔГ2) <= hp) && (hp <= 0.375 * Get_Dp(Dpст, ΔГ2))))
            //    errors.Add(new ValidationResult($"Значение параметра hp должно принадлежать {Get_hpBounds(Dpст, ΔГ2)}."));
            if (hp < 0 || double.IsNaN(hp))
                errors.Add(new ValidationResult(errorhp));
            #endregion

            return errors;
        }
        internal double DpстBoundCalculation => Di - 2 * (ΔГ1 - ΔГ2);
        double KзапCalculation() {
            double nэл = W1 * a1 * a2 / p / q1;
            double qИЗ = nэл * dиз * dиз;
            double Sп = d1 == 0 ? 0.5 * (bП + bП1) * (h2 + 2 * h3 + h4 + h5) : 0.5 * (bП + d1) * (h2 + 2 * h3 + h4 + h5);
            double Sʹп = d1 == 0 ? Sп - (2 * h3 * (bП + h2 + h3 + h4) + h5 * bП1) : Sп - (2 * h3 * (bП + h2 + h3 + h4) + h5 * d1);
            return qИЗ / Sʹп;
        }
    }
}
