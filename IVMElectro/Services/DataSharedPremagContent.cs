using System;
using System.Collections.Generic;
using System.Text;

namespace IVMElectro.Services {
    static class DataSharedPremagContent {
        #region string error
        public const string errorBδ = "Значение параметра Bδ должно быть > 0.";
        public const string errorρx = "Значение параметра ρx должно быть > 0.";
        public const string errorρГ = "Значение параметра ρГ должно быть > 0.";
        public const string errorR0 = "Значение параметра R0 должно быть ≥ 0.";
        public const string errorR10 = "Значение параметра R'0 должно быть ≥ 0.";
        public const string errorR110 = "Значение параметра R\"0 должно быть ≥ 0.";
        public const string errorR1110 = "Значение параметра R\"'0 должно быть ≥ 0.";
        public const string errordпз2 = "Значение параметра dпз2 должно быть > 0.";
        public const string errordвст = "Значение параметра dвст должно быть ≥ 0.";
        public const string errorΔk1 = "Значение параметра Δk1 должно быть > 0.";
        public const string errorhяр = "Значение параметра hяр должно быть > 0.";
        public const string errorhяк = "Значение параметра hяк должно быть > 0.";
        public const string errordм = "Значение параметра dм должно быть ≥ 0.";
        public const string errordиз = "Значение параметра dм должно быть ≥ 0.";
        public const string errorhфл = "Значение параметра hфл должно быть > 0.";
        public const string errorl1 = "Значение параметра l1 должно быть > 0.";
        public const string errorl2 = "Значение параметра l2 должно быть > 0.";
        public const string errordпз1 = "Значение параметра dпз1 должно быть ≥ 0.";
        #region StringOfVarParameters, StringOfVarParametersPlunger errors
        public const string errorU = "Значение параметра U должно быть > 0.";
        public const string errorδ = "Значение параметра δ должно быть > 0.";
        public const string errorq = "Значение параметра q должно быть > 0.";
        public const string errorh = "Значение параметра h должно быть > 0.";
        public const string errorR1 = "Значение параметра R1 должно быть > 0.";
        public const string errorR1R0 = "Значение параметра R1 должно быть > R0.";
        public const string errorR1R10 = "Значение параметра R1 должно быть > R'0.";
        public const string errorR1R110 = "Значение параметра R1 должно быть > R''0.";
        public const string errorR1R1110 = "Значение параметра R1 должно быть > R'''0.";
        public const string errorR2 = "Значение параметра R2 должно быть > 0.";
        public const string errorR2R1 = "Значение параметра R2 должно быть > R1.";
        public const string errorR2dпз1 = "Значение параметра R2 должно быть > dпз1.";
        public const string errorR2dвст = "Значение параметра R2 должно быть > dвст.";
        public const string errorR3 = "Значение параметра R3 должно быть > 0.";
        public const string errorR3R2 = "Значение параметра R3 должно быть > R2.";
        public const string errorqm = "Значение параметра qm должно быть > 0.";
        public const string errorWs = "Значение параметра Ws должно быть > 0.";
        public const string errorα = "Значение параметра α должно быть ≥ 0.";
        public const string errorhhфлδαl1l2 = "Некорректные значения параметров: h, hфл, δ, α, l1, l2.";
        public const string errordпз1_plngr = "Значение параметра dпз1 должно быть > 0.";
        #endregion
        #endregion
        public static List<string> MarksOfSteel => new List<string> { "09Х17Н", "ст. 3, ст. 10", "ст. 10880 (Э10)" };
    }
}
