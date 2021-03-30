using System;
using System.Collections.Generic;

namespace LibraryAlgorithms {
    //расчет параметров мащины, магнитной цепи, холостой ход, номинальный режим
    //abstract: все переменные глобальной видимости типа передаются в функции неявно (экономия параметров функций)
    //требуется следить за использованием таких параметров при вызове функций
    public class AlgorithmASDNRED {
        #region input data
        private readonly double Z1;
        private readonly double Z2;
        private readonly double p;
        private readonly double W1;
        private readonly double a1;
        private readonly double a2;
        private readonly double y1;
        private readonly double Di;
        private readonly double Dp;
        private readonly double Da;
        private readonly double f1;
        private readonly double h1;
        private readonly double h2;
        private readonly double h3;
        private readonly double h4;
        private readonly double h5;
        private readonly double h6;
        private readonly double h7;
        private readonly double h8;
        private readonly double bп;
        private readonly double bп1;
        private readonly double bп2;
        private readonly double ΔГ1;
        private readonly double ΔГ2;
        private readonly double li;
        private readonly double B;
        private readonly double Δкр;
        private readonly double ρ1x;
        private readonly double ρ1Г;
        private readonly double ρ2Г;
        private readonly double ρРУБ;
        private readonly double qГ;
        private readonly double bк;
        private readonly double aк;
        private readonly double ac;
        private readonly double dиз;
        private readonly double d1;
        private readonly double bпн;
        private readonly double cз;
        private readonly double Kfe1;
        private readonly double Kfe2;
        private readonly double U1;
        private readonly double S = 0.1;
       // private readonly double S = 0.02; //DEBUG ONLY
        private double αδ = 0.65;
        private readonly double dв;
        private readonly double p10_50;
        private readonly double Pмех;
        private readonly double Pʹ2;
        private readonly double bСК;

        //new
        private readonly double K2;
        private readonly double dкп;
        private readonly double dпв;
        private readonly double dпн;
        private readonly double hp1;
        private readonly double hр2;
        private readonly double hш;
        private readonly double bш;
        private readonly double aкн;
        private readonly double bкн;

        //материал статора
        private enum MarkSteelStator {
            Сталь1412,
            Сталь2411,
            Сталь2412,
            Сталь1521
        }
        private MarkSteelStator mrkStlStr;
        #endregion
        #region output data
        #region геометрические размеры и параметры машины
        double lp, hj2, bZ2MIN, bZ2MAX, qст, qк, rʹ2хх, xʹ2хх; //параметры ротора
        double hj1, Kз, bZ1MAX, bZ1MIN, bZ1СР, m1, L, lB, r1x, lc; //параметры статора
        private double qИЗ;
        private double Sп;
        private double Sʹп;
        private double q1;
        private double Wc;
        private double np;
        private double nэл;
        private readonly double β;
        private double k1;
        #endregion
        #region расчет магнитной цепи
        double Bz1, Bj1, Kδ1, Fz1, Fj1, r1Г, x1; //статор
        double Bz2, Bj2, Kδ2, Fz2, Fj2; //ротор
        double Bδm, Fδ; //зазор
        double Iμ, xμ, Kδ, ΣF; //прочее
        #endregion
        #region холостой ход
        double Pz1, Pz1_round, Pj1, Pj1_round, PГ, PГ_round, Pпов1, Pпов1_round, Pпул1, Pпул1_round; //статор
        double Pпов2, Pпов2_round, Pпул2, Pпул2_round; //ротор
        double E1, P0, W0, I0А, IХХА, IXX, cosφ0; //прочее
        #endregion
        #region номинальный режим
        double I1A, I1R, I1Н, I1Н_round, PЭ1, PЭ1_round, Δi1; //статор
        //ротор
        double rʹʹ2н, rʹ2н, xʹʹ2н, xʹ2н, Iʹʹ2Н, nН, PЭ2, PЭ2_round, Δi2, ΔiК; 
            //r2ст, x2ст; //отсутствуют сопротивления массивного ротора
        //прочее
        double MН, SН, cosφН, cosφН_round, ηЭЛ, ηЭЛ_round, P1, P1_round, SK, SK_round, c1, rʹ1, xʹ1н; 
            //E1нр; //отсутствует
        #endregion
        //перегрузочная способность
        double I1M, Iʹʹ2M, PM, MM, KM, SM, cosφM;
            //n2, E1M; //отсутствуют
        //пусковой режим
        double Iʹʹ2П, I1П, I1П_round, MП, KП, KI;
            //E1П; //отсутствует
        #endregion
        double rʹ2, xʹ2, xʹ1; //для разных режимов  - разное значение
        //тип паза ротора (1 - круглый, 2 - прямоугольный, 3 - грушевидный, 4 - двойная клетка)
        enum TypeOfRotorSlot { Round, Rectangular, Pyriform, DoubleDeck }
        TypeOfRotorSlot PAS;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="in_markSteelStator"></param>
        /// <param name="in_markSteelRotor"></param>
        /// <param name="in_P3"></param>
        /// <param name="in_bСК">true - использовать скос, false - не использовать скос</param>
        public AlgorithmASDNRED( Dictionary<string, double> input ) {
            Pʹ2 = input["P12"]; U1 = input["U1"]; f1 = input["f1"]; p = input["p"]; Pмех = input["Pмех"];

            Di = input["Di"]; ΔГ1 = input["ΔГ1"]; Z1 = input["Z1"]; Da = input["Da"]; a1 = input["a1"]; a2 = input["a2"]; Δкр = input["Δкр"];
            dиз = input["dиз"]; qГ = input["qГ"]; h8 = input["h8"]; h7 = input["h7"]; h6 = input["h6"]; h5 = input["h5"]; h3 = input["h3"];
            h4 = input["h4"]; ac = input["ac"]; bпн = input["bПН"]; li = input["li"]; cз = input["cз"]; y1 = input["y1"]; K2 = input["K2"];
            d1 = input["d1"]; Kfe1 = input["Kfe1"]; ρ1x = input["ρ1x"]; ρРУБ = input["ρРУБ"]; ρ1Г = input["ρ1Г"]; B = input["B"]; 
            p10_50 = input["p10_50"]; β = input["β"];
            W1 = input["W1"]; Wc = input["Wc"];

            ΔГ2 = input["ΔГ2"]; bСК = input["bСК"] == 1 ? Math.PI * Di / Z1 : 0; bп2 = input["bП2"]; Z2 = input["Z2"]; dв = input["dв"];
            bк = input["bк"]; aк = input["aк"]; ρ2Г = input["ρ2Г"]; Kfe2 = input["Kfe2"];
            bп1 = input["bП1"]; Dp = input["Dpст"] + ΔГ2; h1 = input["h1"]; h2 = input["h2"]; bп = input["bП"];

            if (p10_50 == 1.94) mrkStlStr = MarkSteelStator.Сталь1412;
            else if (p10_50 == 1.38) mrkStlStr = MarkSteelStator.Сталь2412;
            else if (p10_50 == 1.7) mrkStlStr = MarkSteelStator.Сталь2411;
            else if (p10_50 == 19.6) mrkStlStr = MarkSteelStator.Сталь1521;
            //prepare for W1
            //double Wk = 4 * input["Sсл"] * input["Kзап"] / Math.PI / dиз / dиз / a2;
            //W1 = Z1 * Wk / 3.0 / a1;

            //red
            hш = input["hш"]; bш = input["bш"]; dкп = input["dкп"]; hр2 = input["hр2"]; bкн = input["bкн"]; aкн = input["aкн"];
            dпн = input["dпн"]; hp1 = input["hp1"]; dпв = input["dпв"];

            switch (input["PAS"]) {
                case 1: PAS = TypeOfRotorSlot.Round; break;
                case 2: PAS = TypeOfRotorSlot.Rectangular; break;
                case 3: PAS = TypeOfRotorSlot.Pyriform; break;
                case 4: PAS = TypeOfRotorSlot.DoubleDeck; break;
            }

            SolutionIsDone = false; Logging = new List<string>();
        }
        //расчетные алгоритмы: расчет параметров мащины и магнитной цепи, холостой ход, номинальный режим, перегрузочная способность и пусковой режим
        public void Run() {
            double t1, t2, nc, τi, δ, τy, ls, lʹs, γ2, Kʹβ, Kβ, λn1, λq1, y, Kp1, Ky1, λлоб1, Σλ1, λq2, ΔKг, rs, Δг, rпр, rруб;
            double Gj1, Gz1, Ф10, E1_new;
            double aСК, fСК; //скос
            double rʹʹ2 = 0, xʹʹ2 = 0,  rʹk = 0, xʹk = 0, Zʹoo = 0, RM = 0, ZM = 0, Zʹk, RH, RʹH, ZH, I2Н;
            double Kp, λлоб2 = 0, λСК = 0, qств = 0, qстн = 0,  qкв = 0, qкн = 0;
            double I1MA, I1MR, SM_new;
            double Iʹʹ2П_new, I1AП, I1RП, rʹʹ2П = 0;
            SolutionIsDone = false;
            m1 = 3;
            q1 = 0.5 * Z1 / p / m1;
            nэл = W1 * a1 * a2 / p / q1;
            np = W1 * a1 / p / q1;
            //Wc = 0.5 * np;
            t1 = Math.PI * Di / Z1;
            t2 = Math.PI * Dp / Z2;
            nc = 60 * f1 / p;
            τi = Math.PI * 0.5 * Di / p;
            bZ1MAX = Math.PI * (Di + 2 * h1) / Z1 - bп;
            bZ1MIN = d1 == 0 ? Math.PI * (Di + 2 * (h8 + h7 + h6)) / Z1 - bп1 : Math.PI * (Di + 2 * (h8 + h7 + h6)) / Z1 - d1;
            bZ1СР = (bZ1MAX + 2 * bZ1MIN) / 3;
            y = 0.5 * Z1 / p;
            Kp1 = Math.Sin(0.5 * Math.PI * q1 / y) / q1 / Math.Sin(0.5 * Math.PI / y);
            Ky1 = Math.Sin(0.5 * Math.PI * y1 / y);
            k1 = Kp1 * Ky1;
            δ = 0.5 * (Di - Dp);
            Kδ1 = (t1 + 5 * δ * t1 / ac) / (t1 - ac + 5 * δ * t1 / ac);
            Kδ2 = ΔГ2 == 0 ? (t2 + 5 * δ * t2 / bш) / (t2 - bш + 5 * δ * t2 / bш) : 1;
            Kδ = Kδ1 * Kδ2;
            lp = li + 5;
            //надо скос
            aСК = bСК == 0 ? 0 : 2 * Math.PI * p * bСК / Z2 / t2; //скос
            fСК = aСК == 0 ? 1 : 2 * Math.Sin(0.5 * aСК) / aСК; //скос
            hj1 = 0.5 * (Da - (Di + 2 * h1));
            lc = 0.5 * Math.PI * (Da - hj1) / p;
            τy = 0.5 * Math.PI * (Di + 2 * (h8 + h7 + h6 + h5) + h4 + h3 + h2) * β / p;
            ls = K2 * τy + 2 * B;
            lB = 0.5 * Math.Sqrt(ls * ls - τy * τy) * 1.03;
            L = 2 * (li + 2 * Δкр + ls) * 1.05;
            lʹs = 0.5 * (L - 2 * li);
            r1x = ρ1x * L * W1 * 0.001 / a1 / a2 / qГ;
            r1Г = r1x * ρ1Г / ρ1x;
            //red here
            γ2 = 2 * Math.Sin(Math.PI * p / Z2);
            qИЗ = nэл * dиз * dиз;
            Sп = d1 == 0 ? 0.5 * (bп + bп1) * (h2 + 2 * h3 + h4 + h5) : 0.5 * (bп + d1) * (h2 + 2 * h3 + h4 + h5);
            Sʹп = d1 == 0 ? Sп - (2 * h3 * (bп + h2 + h3 + h4) + h5 * bп1) : Sп - (2 * h3 * (bп + h2 + h3 + h4) + h5 * d1);
            Kз = qИЗ / Sʹп;
            Kʹβ = β <= 2 / 3 ? 0.25 * (6 * β - 1) : 0.25 * (1 + 3 * β);
            Kβ = 0.25 + 0.75 * Kʹβ;
            λn1 = d1 == 0 ? Kβ * (h2 + h4) / 3 / bп1 + (h5 / bп1 + 3 * h6 / (bп1 + 2 * ac) + h7 / ac + 3 * h8 / (bпн + 2 * ac)) * Kʹβ + 0.25 * h3 / bп1 :
                Kβ * (h2 + h4) / 3 / d1 + (0.785 - 0.5 * ac / d1 + h5 / d1 + h7 / ac + h8 / (bпн + 2 * ac)) * Kʹβ + 0.25 * h3 / d1;
            λq1 = t1 * k1 * k1 / 11.9 / δ / Kδ;
            λлоб1 = 0.34 * q1 * (lʹs - 0.64 * β * τi) / li;
            Σλ1 = λn1 + λq1 + λлоб1; //bСК не используется
            x1 = 158e-6 * f1 * W1 * W1 * li * Σλ1 / 1e4 / p / q1;
            λq2 = t2 / 11.9 / δ / Kδ;
            #region гильза
            ΔKг = functionAreshyan(cз / τi) * τi / li;
            rs = 3.82 * ρРУБ * li * W1 * W1 * k1 * k1 * 1e-3 / ΔГ1 / Di;
            Δг = ΔKг * rs; rпр = rs * cз / li;
            rруб = rпр * Δг / (rпр + Δг); 
            #endregion
            Gj1 = 15.2 * hj1 * li * p * lc * Kfe1 * 1e-6;
            Gz1 = 7.6 * Z1 * bZ1СР * h1 * li * Kfe1 * 1e-6;
            E1 = 0.95 * U1; //начальное приближение ЭДС хх
            Ф10 = U1 * 1e8 / 4.44 / W1 / k1 / f1;
            //расчет ЭДС хх
            E1_new = IdleRegim_E1(E1, Ф10, τi, t1, t2, δ, λq2, Gz1, Gj1, nc, rs, rруб, Σλ1, γ2, fСК,  out double ρ1, out double PFe, out double PFe_round);
            if (double.IsNaN(E1_new)) {
                Logging.Add($"Type: {GetType()}  message: Ошибка расчета холостого хода -> double.IsNaN(E1_new)");
                return; 
            }
            while (Math.Abs(E1_new - E1) >= E1 * 5e-3) {
                E1 = 0.5 * (E1_new + E1);
                E1_new = IdleRegim_E1(E1, Ф10, τi, t1, t2, δ, λq2, Gz1, Gj1, nc, rs, rруб, Σλ1, γ2, fСК, out ρ1, out PFe, out PFe_round);
                if (double.IsNaN(E1_new)) {
                    Logging.Add($"Type: {GetType()}  message: Ошибка расчета холостого хода -> double.IsNaN(E1_new)");
                    return; 
                }
            }
            rʹ2хх = rʹ2; xʹ2хх = xʹ2; //сбор данных для холостого хода
            //номинальный режим
            //пересчет rʹ2, xʹ2 
            switch (PAS) {
                case TypeOfRotorSlot.Round:
                    Rotor_roundSlot(get_Bδm(E1 * Ф10 / U1, αδ, τi, li), t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, true, E1 * Ф10 / U1, δ, λq2, Σλ1,
                                     out _, out λлоб2, out _, out λСК);
                    break;
                case TypeOfRotorSlot.Rectangular:
                    Rotor_rectangularSlot(get_Bδm(E1 * Ф10 / U1, αδ, τi, li), t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, true, E1 * Ф10 / U1, δ, λq2, Σλ1,
                                         out _, out λлоб2, out _, out λСК);
                    break;
                case TypeOfRotorSlot.Pyriform:
                    Rotor_pyriformSlot(get_Bδm(E1 * Ф10 / U1, αδ, τi, li), t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, true, E1 * Ф10 / U1, δ, λq2, Σλ1,
                                         out _, out λлоб2, out _, out λСК);
                    break;
                case TypeOfRotorSlot.DoubleDeck:
                    Rotor_doubleDeck(get_Bδm(E1 * Ф10 / U1, αδ, τi, li), t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, true, E1 * Ф10 / U1, δ, λq2, Σλ1,
                        out _, out _, out λСК, out qств, out qстн, out qкв, out qкн);
                    break;
            }
            CalculationResistance(x1, xʹ2, ref rʹʹ2, ref xʹʹ2, ref rʹk, ref xʹk, ref Zʹoo, ref RM, ref ZM);
            rʹʹ2н = rʹʹ2; xʹʹ2н = xʹʹ2; rʹ2н = rʹ2; xʹ2н = xʹ2; xʹ1н = xʹ1; //сбор данных для номинального режима
            Zʹk = Math.Sqrt(rʹk * rʹk + xʹk * xʹk);
            RH = 1.5 * U1 * U1 / Pʹ2 - rʹk + Math.Sqrt((1.5 * U1 * U1 / Pʹ2 - rʹk) * (1.5 * U1 * U1 / Pʹ2 - rʹk) - Zʹk * Zʹk); //error
            if (double.IsNaN(RH)) {
                Logging.Add($"Type: {GetType()}  message: Ошибка расчета сопротивления схемы замещения -> double.IsNaN(RH)");
                return;
            }
            RʹH = RH + rʹk;
            ZH = Math.Sqrt(RʹH * RʹH + xʹk * xʹk);
            Iʹʹ2Н = U1 / ZH;
            I1A = I0А + Iʹʹ2Н * (RʹH + 2 * ρ1 * xʹk) / ZH;
            I1R = Iμ + Iʹʹ2Н * (xʹk - 2 * ρ1 * RʹH) / ZH;
            I1Н = Math.Sqrt(I1A * I1A + I1R * I1R);
            cosφН = I1A / I1Н;
            I2Н = Iʹʹ2Н * c1 * 2 * m1 * W1 * k1 / Z2;
            SН = 1.4 / (1 + RH / rʹʹ2);
            PЭ1 = m1 * I1Н * I1Н * r1Г; PЭ1_round = PЭ1 * 1.21;
            PЭ2 = SН * Pʹ2; PЭ2_round = PЭ2 * 1.07;
            P1 = PЭ1 + PЭ2 + PFe + PГ + Pʹ2;
            P1_round = PЭ1_round + PЭ2_round + PFe_round + PГ_round + Pʹ2;
            I1Н_round = I1Н * 1.1;
            SK = P1 / cosφН / 1000; SK_round = 3 * U1 * I1Н_round / 1000;
            cosφН_round = P1_round / SK_round / 1000;
            ηЭЛ = Pʹ2 / P1; ηЭЛ_round = Pʹ2 / P1_round;
            nН = nc - nc * SН;
            MН = 0.975 * Pʹ2 / nН;
            Δi1 = I1Н / a1 / a2 / qГ;
            Δi2 = PAS != TypeOfRotorSlot.DoubleDeck ? I2Н / qст : I2Н / (qств + qстн);
            ΔiК = PAS != TypeOfRotorSlot.DoubleDeck ? I2Н / qк / γ2 : I2Н / (qкв + qкн) / γ2;
            //перегрузочная способность
            Kp = 1.8 * Math.Sqrt(Z1 / Z2);
            if (Da <= 300 && p == 1) Kp = 0.5 * Math.Sqrt(Z1 / Z2);
            else if (Da <= 300 && p == 2) Kp = Math.Sqrt(Z1 / Z2);
            else if (Da > 300 && p <= 2) Kp = 0.5;
            else if (Da > 300 && p > 2) Kp = 1;
            SM = rʹʹ2 / Zʹoo; //начальное приближение скольжения (с номинсльного режима)
            Iʹʹ2M = 1.2 * U1 / ZM; //начальное приближение тока ротора 
            SM_new = MaximumRegim_SM(SM, γ2, fСК, Kp, Kʹβ, λn1, δ, λq1, λлоб1, Σλ1, λq2, λлоб2, λСК, ref xʹk, ref RM, ref ZM);
            while (Math.Abs(SM_new - SM) >= SM * 5e-3) {
                SM = 0.5 * (SM_new + SM);
                SM_new = MaximumRegim_SM(SM, γ2, fСК, Kp, Kʹβ, λn1, δ, λq1, λлоб1, Σλ1, λq2, λлоб2, λСК, ref xʹk, ref RM, ref ZM);
            }
            I1MA = I0А + Iʹʹ2M * (RM + 2 * ρ1 * xʹk) / ZM;
            I1MR = Iμ + Iʹʹ2M * (xʹk - 2 * ρ1 * RM) / ZM;
            I1M = Math.Sqrt(I1MA * I1MA + I1MR * I1MR);
            cosφM = I1MA / I1M;
            PM = 0.5 * m1 * U1 * U1 / RM;
            MM = 0.975 * PM / nc;
            KM = MM / MН;
            //пусковой режим
            Iʹʹ2П = 1.4 * U1 / Zʹk; //предварительное значение (с номинсльного режима)
            Iʹʹ2П_new = StartingRegim_Iʹʹ2П(Iʹʹ2П, γ2, fСК, Kp, Kʹβ, λn1, δ, λq1, λq2, λлоб1, λлоб2, λСК, Σλ1, ref xʹk, ref rʹk, ref Zʹk, ref rʹʹ2П);
            while (Math.Abs(Iʹʹ2П_new - Iʹʹ2П) >= Iʹʹ2П * 5e-3) {
                Iʹʹ2П = 0.5 * (Iʹʹ2П + Iʹʹ2П_new);
                Iʹʹ2П_new = StartingRegim_Iʹʹ2П(Iʹʹ2П, γ2, fСК, Kp, Kʹβ, λn1, δ, λq1, λq2, λлоб1, λлоб2, λСК, Σλ1, ref xʹk, ref rʹk, ref Zʹk, ref rʹʹ2П);
            };
            I1AП = I0А + Iʹʹ2П * (rʹk + 2 * ρ1 * xʹk) / Zʹk;
            I1RП = Iμ + Iʹʹ2П * (xʹk - 2 * ρ1 * rʹk) / Zʹk;
            I1П = Math.Sqrt(I1AП * I1AП + I1RП * I1RП); I1П_round = I1П * 1.2;
            _ = I1AП / I1П;
            MП = 0.975 * m1 * Iʹʹ2П * Iʹʹ2П * rʹʹ2П / nc;
            KП = MП / MН;
            KI = I1П / I1Н;

            SolutionIsDone = true;
        }
        //расчет ЕДС холостого хода
        double IdleRegim_E1(double E1xx, double Ф10, double τi, double t1, double t2, double δ, double λq2, double Gz1, double Gj1,
            double nc, double rs, double rруб, double Σλ1, double γ2, double fСК, out double ρ1, out double PFe, out double PFe_round) {
            double Фm, Pудz1, Pудj1, Pпов, Pпул, rэр, sinφ0, αδ_new, Gz2;
            Фm = E1xx * Ф10 / U1;
            //αδ - начальное приближение (должно меняться)
            αδ_new = function_αδн(αδ, Фm, τi, t1, E1xx, bZ1СР, hj1, lp, t2, δ, Kδ, Σλ1, λq2, γ2, fСК, out double hz2, out double bz2ср);
            if ( double.IsNaN(αδ_new) ) {
                Logging.Add($"Type: {GetType()}  message: Ошибка расчета параметров машины и магнитной цепи -> double.IsNaN(αδ_new)");
                SolutionIsDone = false; ρ1 = PFe = PFe_round = double.NaN;
                return double.NaN;
            }
            while ( Math.Abs(αδ_new - αδ) >= αδ * 5e-3 ) {
                αδ = 0.5 * (αδ + αδ_new);
                αδ_new = function_αδн(αδ, Фm, τi, t1, E1xx, bZ1СР, hj1, lp, t2, δ, Kδ, Σλ1, λq2, γ2, fСК, out hz2, out bz2ср);
                if ( double.IsNaN(αδ_new) ) {
                    Logging.Add($"Type: {GetType()}  message: Ошибка расчета параметров машины и магнитной цепи -> double.IsNaN(αδ_new)");
                    SolutionIsDone = false; ρ1 = PFe = PFe_round = double.NaN;
                    return double.NaN;
                }
            }
            Iμ = p * ΣF / 2.7 / W1 / k1;
            Gz2 = 7.6 * Z2 * bz2ср * hz2 * Kfe2 * lp * 1e-6;
            xμ = E1xx / Iμ;
            c1 = 1 + x1 / xμ; ρ1 = r1Г / (x1 + xμ);
            //расчет E1 холостого хода
            Pудz1 = f1 < 150 ? p10_50 * Bz1 * Bz1 * Math.Pow(f1 / 50, 1.5) * 1e-8 : p10_50 * Bz1 * Bz1 * Math.Pow(f1 / 400, 1.5) * 1e-8;
            Pz1 = 1.8 * Pудz1 * Gz1; Pz1_round = Pz1 * 1.4;
            Pудj1 = f1 < 150 ? p10_50 * Bj1 * Bj1 * Math.Pow(f1 / 50, 1.5) * 1e-8 : p10_50 * Bj1 * Bj1 * Math.Pow(f1 / 400, 1.5) * 1e-8;
            Pj1 = 1.6 * Pудj1 * Gj1; Pj1_round = Pj1 * 1.4;
            Pпов1 = ΔГ2 == 0 ? 2.67 * Di * li * (t1 - ac) * Math.Pow(Z2 * nc * 1e-4, 1.5) * Math.Pow(function_β0(bп2 / δ) * Kδ * Bδm * t2 * 1e-3, 2) * 1e-8 / t1 : 0;
            Pпов1_round = Pпов1 * 1.4;
            Pпов2 = 2.67 * Di * li * (t2 - bш) * Math.Pow(Z1 * nc * 1e-4, 1.5) * Math.Pow(function_β0(ac / δ) * Kδ * Bδm * t1 * 1e-3, 2) * 1e-8 / t2;
            Pпов2_round = Pпов2 * 1.4;
            Pпов = Pпов1 + Pпов2;
            Pпул1 = ΔГ2 == 0 ?
                0.0225 * (Z2 * nc * δ * Bz1 * bш / δ * bш / δ * 1e-7 / t1 / (5 + bш / δ)) * (Z2 * nc * δ * Bz1 * bш / δ * bш / δ * 1e-7 / t1 / (5 + bш / δ)) * Gz1 : 0;
            Pпул1_round = Pпул1 * 1.4;
            Pпул2 = 0.0225 * (Z1 * nc * δ * Bz2 * ac / δ * ac / δ * 1e-7 / t2 / (5 + ac / δ)) * (Z1 * nc * δ * Bz2 * ac / δ * ac / δ * 1e-7 / t2 / (5 + ac / δ)) * Gz2;
            Pпул2_round = Pпул2 * 1.4;
            Pпул = Pпул1 + Pпул2;
            #region гильза
            rэр = rs + rруб;
            PГ = m1 * E1 * E1 / rэр; PГ_round = PГ * 1.3; 
            #endregion
            PFe = Pz1 + Pj1 + Pпов + Pпул;
            PFe_round = Pz1_round + Pj1_round + Pпов * 1.4 + Pпул * 1.4;
            P0 = m1 * Iμ * Iμ * r1Г + PFe + PГ;
            W0 = P0 + Pмех;
            I0А = Iμ * ρ1 + (PГ + PFe) / 3 / U1;
            IХХА = I0А * W0 / P0;
            IXX = Math.Sqrt(IХХА * IХХА + Iμ * Iμ);
            cosφ0 = IХХА / IXX; sinφ0 = Iμ / IXX;
            return Math.Sqrt((U1 * cosφ0 - IXX * r1Г) * (U1 * cosφ0 - IXX * r1Г) + (U1 * sinφ0 - IXX * x1) * (U1 * sinφ0 - IXX * x1));
        }
        //расчет SM максимального режима
        double MaximumRegim_SM( double in_S, double γ2, double fСК, double Kp, double Kʹβ, double λn1, double δ, double λq1, double λлоб1, double Σλ1,
            double λq2, double λлоб2, double λСК, ref double xʹk,  ref double RM, ref double ZM ) {
            double rʹʹ2 = 0, rʹk = 0, Zʹoo = 0;
            MaximumRegim_core(in_S, Iʹʹ2M, γ2, fСК, Kp, Kʹβ, λn1, δ, λq1, λq2, λлоб1, λлоб2, λСК, Σλ1, ref rʹʹ2, ref rʹk, ref xʹk, ref Zʹoo, ref RM, ref ZM);
            Iʹʹ2M = U1 / ZM;
            return rʹʹ2 / Zʹoo;
        }
        //расчет тока ротора пускового режима
        double StartingRegim_Iʹʹ2П( double in_Iʹʹ2П, double γ2, double fСК, double Kp, double Kʹβ, double λn1, double δ, double λq1, double λq2, 
            double λлоб1, double λлоб2, double λСК, double Σλ1,
            ref double xʹk, ref double rʹk, ref double Zʹk, ref double rʹʹ2П) {
            double Zʹoo = 0, RM = 0, ZM = 0;
            MaximumRegim_core(1, in_Iʹʹ2П, γ2, fСК, Kp, Kʹβ, λn1, δ, λq1, λq2, λлоб1, λлоб2, λСК, Σλ1, ref rʹʹ2П, ref rʹk, ref xʹk, ref Zʹoo, ref RM, ref ZM);
            Zʹk = Math.Sqrt(rʹk * rʹk + xʹk * xʹk);
            return U1 / Zʹk;
        }
        #region вспомогательные функции
        double function_αδн( double αδ, double Фm, double τi, double t1, double E1xx, double bZ1СР, double hj1, 
            double lp, double t2, double δ, double Kδ, double Σλ1, double λq2, double γ2, double fСК, out double hz2, out double bz2ср) {
            double HZ1, Hj1, Kz;
            Bδm = get_Bδm(Фm, αδ, τi, li);
            Fδ = 0.16 * Bδm * δ * Kδ;
            Bz1 = U1 * Bδm * t1 / E1xx / bZ1СР / Kfe1;
            HZ1 = functionMagnetizationToothing(mrkStlStr, Bz1);
            if ( double.IsNaN(HZ1) ) {
                Logging.Add($"Type: {GetType()}  message: Кривая намагничивания стали зубцов статора не определена -> double.IsNaN(HZ1)");
                hz2 = bz2ср = double.NaN;
                return double.NaN;
            }
            Bj1 = Фm * U1 * 50 / E1xx / hj1 / li / Kfe1; //???
            Hj1 = functionMagnetizationStatorBack(mrkStlStr, Bj1);
            if( double.IsNaN(Hj1) ) {
                Logging.Add($"Type: {GetType()}  message: Кривая намагничивания стали спинки статора не определена -> double.IsNaN(Hj1)");
                hz2 = bz2ср = double.NaN;
                return double.NaN;
            }
            Fz1 = 0.2 * HZ1 * h1;
            Fj1 = 0.1 * Hj1 * lc;
            switch (PAS) {
                case TypeOfRotorSlot.Round: {
                        Rotor_roundSlot(Bδm, t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, false, Фm, δ, λq2, Σλ1,
                            out hz2, out _, out bz2ср, out _);
                    } break;
                case TypeOfRotorSlot.Rectangular: {
                        Rotor_rectangularSlot(Bδm, t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, false, Фm, δ, λq2, Σλ1,
                            out hz2, out _, out bz2ср, out _);
                    } break;
                case TypeOfRotorSlot.Pyriform: {
                        Rotor_pyriformSlot(Bδm, t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, false, Фm, δ, λq2, Σλ1,
                            out hz2, out _, out bz2ср, out _);
                    } break;
                case TypeOfRotorSlot.DoubleDeck: {
                        Rotor_doubleDeck(Bδm, t2, li, lp, Kfe2, S, γ2, fСК, mrkStlStr, false, Фm, δ, λq2, Σλ1,
                            out hz2, out bz2ср, out _, out _, out _, out _, out _);
                    } break;
                default: { hz2 = bz2ср = double.NaN; } break;
            }
            Kz = (Fδ + Fz1 + Fz2) / Fδ;

            return ((((0.26038319 - 0.027650412 * Kz) * Kz - 0.90870916) * Kz + 1.38167423) * Kz - 0.68171898) * Kz + 0.608124486;
        }
        //кривая Арешяна (приложение 7)
        double functionAreshyan( double K ) {
            if ( K <= 0.25 ) return ((29.7619032 - 2.97618736 * K) * K - 16.22648878) * K + 3.22800594;
            if ( K <= 0.7 ) return ((10.28 - 5.6 * K) * K - 6.592) * K + 2.078;
            if ( K <= 1.4 ) return ((0.157142859 - 0.0119047625 * K) * K - 0.403452382) * K + 0.7895;
            return 0.5;
        }
        //кривая намагничивания зубцов для сталей 1412, 2411, 2412, 1521 (приложение 2)
        double functionMagnetizationToothing(MarkSteelStator markSteel, double B ) {
            double H = double.NaN, K = B * 1e-4;
            switch ( markSteel ) {
                case MarkSteelStator.Сталь1412: {
                        if (B <= 12e3) H = (((2.3958332 - 0.31249987 * K) * K - 1.9499997) * K + 1.6966665) * K + 0.22;
                        else if (B > 12e3 && B <= 16e3) H = (((162.50112 * K - 815.83961) * K + 1547.8881) * K - 1308.8037) * K + 417.35415;
                        else if (B > 16e3 && B <= 2e4) H = (((245.00986 - 31.771664 * K) * K - 291.78204) * K - 395.36079) * K + 593.34809;
                        else H = (((3744.3257 * K - 31929.962) * K + 103689.38) * K - 151173.19) * K + 83206.565;
                    }
                break;
                case MarkSteelStator.Сталь2411:
                case MarkSteelStator.Сталь2412: {
                        if (B <= 12e3) H = (((8.3854161 * K - 22.541665) * K + 22.339581) * K - 8.7533323) * K + 1.6919998;
                        else if (B > 12e3 && B <= 16e3) H = (((59.582511 * K - 298.91207) * K + 574.14457) * K - 493.62696) * K + 160.34693;
                        else if (B > 16e3 && B <= 2e4) H = (((147.68687 * K - 1165.9466) * K + 3868.3712) * K - 5836.9957) * K + 3250.4895;
                        else H = (((5766.0038 * K - 48425.854) * K + 153421.77) * K - 216835.82) * K + 115220.73;
                    }
                break;
                case MarkSteelStator.Сталь1521:
                    H = functionMagnetization1521(B);
                break;
            }
            return H;
        }
        //кривая намагничивания спинки статора для сталей 1412, 2411, 2412, 1521 (приложение 1)
        double functionMagnetizationStatorBack(MarkSteelStator markSteel, double B ) {
            double H = double.NaN, K = B * 1e-4;
            switch ( markSteel ) {
                case MarkSteelStator.Сталь1412: {
                        if (B <= 11e3) H = (((1.3690474 * K - 2.8749994) * K + 3.2130945) * K - 0.80499967) * K + 0.45385709;
                        else if (B > 11e3 && B <= 15e3) H = (((285.00144 * K - 1337.0076) * K + 2364.1148) * K - 1862.6978) * K + 552.30914;
                        else if (B > 15e3 && B <= 21.7e3) H = (((500.83246 * K - 2802.1611) * K + 5912.0784) * K - 5518.8745) * K + 1905.9046;
                        else H = (((16582.576 * K - 155326.54) * K + 546441.37) * K - 853272.75) * K + 498183.62;
                    }
                break;
                case MarkSteelStator.Сталь2411:
                case MarkSteelStator.Сталь2412: {
                        if (B <= 11e3) H = (((4.6488085 * K - 11.120831) * K + 9.9276157) * K - 3.3231651) * K + 0.69657117;
                        else if (B > 11e3 && B <= 15e3) H = (((645.00313 * K - 3170.3494) * K + 5842.9806) * K - 4780.4825) * K + 1464.9681;
                        else if (B > 15e3 && B <= 22e3) H = (((2074.9974 * K - 13796.652) * K + 34499.221) * K - 38340.31) * K + 15952.594;
                        else H = (((24508.498 * K - 227179.88) * K + 790171.32) * K - 1219851.5) * K + 704416.34;
                    }
                break;
                case MarkSteelStator.Сталь1521: H = functionMagnetization1521(B);
                    break;
            }
            return H;
        }
        //кривая намагничивания стали 1521 (приложение 3)
        double functionMagnetization1521( double B ) {
            double K = B * 1e-4;
            if ( B <= 6.3e3 ) return B / 0.9;
            if ( B <= 10.8e3 ) return (((113.87206 * K - 366.50114) * K + 442.98829) * K - 234.68379) * K + 46.521154;
            if ( B <= 12.8e3 ) return (((290.66977 * K - 334.06096) * K - 1152.3847) * K + 2219.2271) * K - 1024.114;
            return (K - 1.27) * 102.3529 + 7.4;
        }
        //функция 4.2.6
        double function_β0( double arg ) {
            if ( arg <= 2 ) return 0.0555 * arg;
            if ( arg <= 4 ) return 0.0695 * arg - 0.028;
            return (((0.0032291664 - 0.0000937499 * arg) * arg - 0.041874997) * arg + 0.26008332) * arg - 0.30299998;
        }
        double get_Bδm( double Ф, double αδ, double τi, double li ) => 100 * Ф / αδ / τi / li;
        #region функции red
        /// <summary>
        /// Параметры ротора с круглым пазом. Расчитывается xʹ2, (λлоб2, bz2ср, Bz2, λСК - не зависит от режима)
        /// </summary>
        void Rotor_roundSlot(double BδM, double t2, double li, double lp, double Kfe2, double S, double γ2, double fСК,
            MarkSteelStator markSteel, bool is_nominal, double Фm, double δ, double λq2, double Σλ1,
            out double hz2, out double λлоб2, out double bz2ср, out double λСК) {
            double Σλ2, Σλʹ2, Lp, Hj2, HZ2;
            double Dср = Dp - (dкп + 2 * hш), tср = Math.PI * (Dср + dкп / 3) / Z2;
            bz2ср = tср - 0.94 * dкп;
            Bz2 = BδM * t2 * li / bz2ср / lp / Kfe2;
            HZ2 = functionMagnetizationToothing(markSteel, Bz2);
            hz2 = 0.9 * dкп + hш;
            Fz2 = 0.2 * (hz2 + ΔГ2) * HZ2; //добавил ΔГ2
            Rotor_roundSlot_part(is_nominal, S, γ2, fСК, out _ , out double Dк, out double λn2); //is_nominal
            λлоб2 = Dк * Math.Log(2.35 * Dк / (aк + bк)) / Z2 / lp / γ2 / γ2; //не зависит от is_nominal

            hj2 = 0.5 * (Dp - 2 * (hz2 + ΔГ2) - dв);
            Bj2 = Фm * 50 / hj2 / lp / Kfe2;
            Hj2 = functionMagnetizationStatorBack(markSteel, Bj2);
            Lp = 0.5 * Math.PI * (Dp - 2 * (hz2 + ΔГ2) - hj2) / p;
            Fj2 = 0.1 * Hj2 * Lp;
            ΣF = Fδ + Fz1 + Fz2 + Fj1 + Fj2;

            λСК = bСК * bСК * Fδ / t2 / 11.9 / δ / Kδ / ΣF; //скос не зависит от is_nominal
            Σλ2 = λn2 + λлоб2 + λq2 + λСК; //is_nominal
            Σλʹ2 = Σλ2 * lp * k1 * k1 * Z1 / li / Z2; //is_nominal
            xʹ2 = x1 * Σλʹ2 / Σλ1; //is_nominal
        }
        /// <summary>
        /// Параметры ротора с прямоугольным пазом. Расчитывается xʹ2, (λлоб2, bz2ср, Bz2, λСК - не зависит от режима)
        /// </summary>
        void Rotor_rectangularSlot(double BδM, double t2, double li, double lp, double Kfe2, double S, double γ2, double fСК,
            MarkSteelStator markSteel, bool is_nominal, double Фm, double δ, double λq2, double Σλ1,
            out double hz2, out double λлоб2, out double bz2ср, out double λСК) {
            double Σλ2, Σλʹ2, Lp, Hj2, HZ2;
            bZ2MAX = Math.PI * (Dp - 2 * hш) / Z2 - bп2;
            bZ2MIN = Math.PI * (Dp - 2 * (hш + hp1 + ΔГ2)) / Z2 - bп2; //добавил ΔГ2
            bz2ср = (bZ2MAX + 2 * bZ2MIN) / 3;
            Bz2 = BδM * t2 * li / bz2ср / lp / Kfe2;
            HZ2 = functionMagnetizationToothing(markSteel, Bz2);
            hz2 = hp1 + hш;
            Fz2 = 0.2 * (hz2 + ΔГ2) * HZ2; //добавил ΔГ2
            Rotor_rectangularSlot_part(is_nominal, S, γ2, fСК, out _, out double Dк, out double λn2);
            λлоб2 = Dк * Math.Log(2.35 * Dк / (aк + bк)) / Z2 / lp / γ2 / γ2; //не зависит от is_nominal

            hj2 = 0.5 * (Dp - 2 * (hz2 + ΔГ2) - dв);
            Bj2 = Фm * 50 / hj2 / lp / Kfe2;
            Hj2 = functionMagnetizationStatorBack(markSteel, Bj2);
            Lp = 0.5 * Math.PI * (Dp - 2 * (hz2 + ΔГ2) - hj2) / p;
            Fj2 = 0.1 * Hj2 * Lp;
            ΣF = Fδ + Fz1 + Fz2 + Fj1 + Fj2;

            λСК = bСК * bСК * Fδ / t2 / 11.9 / δ / Kδ / ΣF; //скос не зависит от is_nominal
            Σλ2 = λn2 + λлоб2 + λq2 + λСК; //is_nominal
            Σλʹ2 = Σλ2 * lp * k1 * k1 * Z1 / li / Z2; //is_nominal
            xʹ2 = x1 * Σλʹ2 / Σλ1; //is_nominal
        }
        /// <summary>
        /// Параметры ротора с грушевидным пазом. Расчитывается xʹ2, (λлоб2, bz2ср, Bz2, λСК - не зависит от режима)
        /// </summary>
        void Rotor_pyriformSlot(double BδM, double t2, double li, double lp, double Kfe2, double S, double γ2, double fСК,
            MarkSteelStator markSteel, bool is_nominal, double Фm, double δ, double λq2, double Σλ1,
            out double hz2, out double λлоб2, out double bz2ср, out double λСК) {
            double HZ2, Hj2, Lp, Σλ2, Σλʹ2;
            bZ2MIN = Math.PI * (Dp - 2 * hш - dпв) / Z2 - dпв;
            bZ2MAX = Math.PI * (Dp - dпв - 2 * (hш + hp1 + ΔГ2)) / Z2 - dпн; //добавил ΔГ2
            bz2ср = (bZ2MIN + 2 * bZ2MAX) / 3;
            Bz2 = BδM * t2 * li / bz2ср / lp / Kfe2;
            HZ2 = functionMagnetizationToothing(markSteel, Bz2);
            hz2 = hp1 + hш + 0.4 * dпн + 0.5 * dпв;
            Fz2 = 0.2 * (hz2 + ΔГ2) * HZ2; //добавил ΔГ2
            Rotor_pyriformSlot_part(is_nominal, S, γ2, fСК, out _, out double Dк, out double λn2);
            λлоб2 = Dк * Math.Log(2.35 * Dк / (aк + bк)) / Z2 / lp / γ2 / γ2; //не зависит от is_nominal

            hj2 = 0.5 * (Dp - 2 * (hz2 + ΔГ2) - dв);
            Bj2 = Фm * 50 / hj2 / lp / Kfe2;
            Hj2 = functionMagnetizationStatorBack(markSteel, Bj2);
            Lp = 0.5 * Math.PI * (Dp - 2 * (hz2 + ΔГ2) - hj2) / p;
            Fj2 = 0.1 * Hj2 * Lp;
            ΣF = Fδ + Fz1 + Fz2 + Fj1 + Fj2;

            λСК = bСК * bСК * Fδ / t2 / 11.9 / δ / Kδ / ΣF; //скос не зависит от is_nominal
            Σλ2 = λn2 + λлоб2 + λq2 + λСК; //is_nominal
            Σλʹ2 = Σλ2 * lp * k1 * k1 * Z1 / li / Z2; //is_nominal
            xʹ2 = x1 * Σλʹ2 / Σλ1; //is_nominal
        }
        /// <summary>
        /// Параметры ротора двойная клетка. Расчитывается xʹ2, rʹ2 (λлоб2 не расчитывается, bz2ср, Bz2, λСК - не зависит от режима)
        /// </summary>
        void Rotor_doubleDeck(double BδM, double t2, double li, double lp, double Kfe2, double S, double γ2, double fСК,
            MarkSteelStator markSteel, bool is_nominal, double Фm, double δ, double λq2, double Σλ1,
            out double hz2, out double bz2ср, out double λСК, out double qств, out double qстн, out double qкв, out double qкн) {
            double HZ2, HZ2н, Hj2, Lp, Fz2в, bZ2MAXн, bZ2MINн, bz2срн, Bz2н;
            double Dср = Dp - (dкп + 2 * hш), tср = Math.PI * (Dср + dкп / 3) / Z2;
            bz2ср = tср - 0.94 * dкп;
            Bz2 = BδM * t2 * li / bz2ср / lp / Kfe2;
            HZ2 = functionMagnetizationToothing(markSteel, Bz2);
            Fz2в = 0.2 * (0.9 * dкп + hш) * HZ2;
            bZ2MAX = bZ2MAXн = Math.PI * (Dp - dпв - 2 * (hш + hp1 + hр2 + dкп + ΔГ2)) / Z2 - dпн; //добавил ΔГ2
            bZ2MIN = bZ2MINн = Math.PI * (Dp - 2 * (hш + hр2 + dкп) - dпв) / Z2 - dпв;
            bz2срн = (bZ2MAXн + 2 * bZ2MINн) / 3;
            Bz2н= BδM * t2 * li / bz2срн / lp / Kfe2;
            HZ2н= functionMagnetizationToothing(markSteel, Bz2н);
            Fz2 = 0.2 * (hp1 + 0.4 * (dпв + dпн) + ΔГ2) * HZ2н + Fz2в; //добавил ΔГ2
            hz2 = hр2 + hp1 + hш + dкп + 0.4 * dпн + 0.5 * dпв;

            Rotor_doubleDeck_part1(is_nominal, S, γ2, fСК, out qстн, out qств, out double Kgн, out double Kgв, out double Dкн, out double Dкв,
                out double rʹво, out double rʹн, out double rʹв, out qкв, out qкн);
            Rotor_doubleDeck_part2(qстн, Kgн, Kgв, Dкн, Dкв, γ2, Σλ1, λq2, rʹво, rʹн, rʹв, is_nominal);

            hj2 = 0.5 * (Dp - 2 * (hz2 + ΔГ2) - dв);
            Bj2 = Фm * 50 / hj2 / lp / Kfe2;
            Hj2 = functionMagnetizationStatorBack(markSteel, Bj2);
            Lp = 0.5 * Math.PI * (Dp - 2 * (hz2 + ΔГ2) - hj2) / p;
            Fj2 = 0.1 * Hj2 * Lp;
            ΣF = Fδ + Fz1 + Fz2 + Fj1 + Fj2;
            λСК = bСК * bСК * Fδ / t2 / 11.9 / δ / Kδ / ΣF; //скос не зависит от is_nominal
        }
        /// <summary>
        /// Круглый паз. Определяет rʹ2, Kg, λn2, (Dк - не зависит от режима)
        /// </summary>
        void Rotor_roundSlot_part(bool is_nominal, double S, double γ2, double fСК, out double Kg, out double Dк, out double λn2) {
            double Kr, r2cт, r2к, ν, ξ;
            qст = 0.636 * dкп * dкп;
            ξ = 0.565 * dкп * Math.Sqrt(S * f1 * 9e-6 / ρ2Г);
            Kr = is_nominal ? 1 : 1 + get_φ(ξ); //is_nominal
            r2cт = ρ2Г * lp * 1e-3 * Kr / qст; //is_nominal (rcт asdn)

            Dк = Dp - aк - 2 * ΔГ2; //добавил ΔГ2
            qк = aк * bк;
            r2к = ρ2Г * 2 * Math.PI * Dк * 1e-3 / Z2 / qк / γ2 / γ2; //rк asdn
            ν = 4 * m1 * (W1 * k1) * (W1 * k1) / Z2 / fСК / fСК; //скос FCK присутствует
            rʹ2 = (r2cт + r2к) * ν; //is_nominal
            Kg = is_nominal ? 1 : 1.5 * (Math.Sinh(2 * ξ) - Math.Sin(2 * ξ)) / (Math.Cosh(2 * ξ) - Math.Cos(2 * ξ)) / ξ; //is_nominal

            λn2 = (0.785 - 0.5 * bш / dкп) * Kg + hш / bш; //is_nominal
        }
        /// <summary>
        /// Прямоугольный паз. Определяет rʹ2, Kg, λn2, (Dк - не зависит от режима)
        /// </summary>
        void Rotor_rectangularSlot_part(bool is_nominal, double S, double γ2, double fСК, out double Kg, out double Dк, out double λn2) {
            double Kr, r2cт, r2к, ν, ξ;
            qст = 0.9 * hp1 * bп2;
            ξ = 0.628 * hp1 * Math.Sqrt(S * f1 * 9e-6 / ρ2Г);
            Kr = is_nominal ? 1 : ξ * (Math.Sinh(2 * ξ) + Math.Sin(2 * ξ)) / (Math.Cosh(2 * ξ) - Math.Cos(2 * ξ)); //is_nominal
            r2cт = ρ2Г * lp * 1e-3 * Kr / qст; //is_nominal (rcт asdn)

            Dк = Dp - aк - 2 * ΔГ2; //добавил ΔГ2
            qк = aк * bк;
            r2к = ρ2Г * 2 * Math.PI * Dк * 1e-3 / Z2 / qк / γ2 / γ2; //rк asdn
            ν = 4 * m1 * (W1 * k1) * (W1 * k1) / Z2 / fСК / fСК; //скос FCK присутствует
            rʹ2 = (r2cт + r2к) * ν; //is_nominal
            Kg = is_nominal ? 1 : 1.5 * (Math.Sinh(2 * ξ) - Math.Sin(2 * ξ)) / (Math.Cosh(2 * ξ) - Math.Cos(2 * ξ)) / ξ; //is_nominal

            λn2 = hp1 * Kg / 3 / bп2 + hш / bш; //is_nominal
        }
        /// <summary>
        /// Грушевидный паз. Определяет rʹ2, Kg, λn2, (Dк - не зависит от режима)
        /// </summary>
        void Rotor_pyriformSlot_part(bool is_nominal, double S, double γ2, double fСК, out double Kg, out double Dк, out double λn2) {
            double hст, ξ, hr, φ, br, qr, Kr, r2cт, r2к, ν;
            qст = 0.318 * (dпв * dпв + dпн * dпн) + 0.9 * (dпн + dпв) * hp1 / 2;
            hст = 0.45 * (dпн + dпв) + hp1;
            ξ = 0.628 * hст * Math.Sqrt(S * f1 * 9e-6 / ρ2Г);
            φ = ξ * (Math.Sinh(2 * ξ) + Math.Sin(2 * ξ)) / (Math.Cosh(2 * ξ) - Math.Cos(2 * ξ)) - 1;
            hr = hст / (1 + φ);
            br = 0.9 * dпв - 0.9 * (dпв - dпн) * (hr - 0.45 * dпв) / hp1;
            qr = 0.318 * dпв * dпв + 0.5 * (0.9 * dпв + br) * (hr - 0.45 * dпв);
            Kr = is_nominal || hr > hp1 + 0.45 * dпв ? 1 : qст / qr; //is_nominal
            r2cт = ρ2Г * lp * 1e-3 * Kr / qст; //is_nominal (rcт asdn)

            Dк = Dp - aк - 2 * ΔГ2; //добавил ΔГ2
            qк = aк * bк;
            r2к = ρ2Г * 2 * Math.PI * Dк * 1e-3 / Z2 / qк / γ2 / γ2; //rк asdn
            ν = 4 * m1 * (W1 * k1) * (W1 * k1) / Z2 / fСК / fСК; //скос FCK присутствует
            rʹ2 = (r2cт + r2к) * ν; //is_nominal
            Kg = is_nominal ? 1 : 1.5 * (Math.Sinh(2 * ξ) - Math.Sin(2 * ξ)) / (Math.Cosh(2 * ξ) - Math.Cos(2 * ξ)) / ξ; //is_nominal

            λn2 = ((hp1 + 0.4 * dпн) * (1 - 0.393 * dпв * dпв / qст) * (1 - 0.393 * dпв * dпв / qст) / 3 / dпв + 0.66 - 0.5 * bш / dпв) * Kg + hш / bш; //is_nominal
        }
        /// <summary>
        /// Параметры ротора двойная клетка. Расчет по 4.1.112 - 4.1.129 
        /// </summary>
        void Rotor_doubleDeck_part1(bool is_nominal, double S, double γ2, double fСК,
            out double qстн, out double qств, out double Kgн, out double Kgв, out double Dкн, out double Dкв, out double rʹво, out double rʹн, out double rʹв,
            out double qкв, out double qкн) {
            double ξв, Krв, r2cтв, Ψ, rвн, hст, ξн, φ, hr, br, qr, Krн, r2cтн, rcтн, rcтв, r2кв, r2кн, ν;
            #region для верхней к.з. клетки ротора
            qств = 0.636 * dкп * dкп;
            ξв = 0.565 * dкп * Math.Sqrt(S * f1 * 9e-6 / ρ2Г);
            Krв = is_nominal ? 1 : 1 + get_φ(ξв); //is_nominal
            r2cтв = ρ2Г * lp * 1e-3 * Krв / qств; //is_nominal (rcт asdn)
            Ψ = is_nominal ? 0 : ξв * (Math.Sinh(ξв) - Math.Sin(ξв)) / (Math.Cosh(ξв) + Math.Cos(ξв)); //is_nominal
            rвн = r2cтв * Ψ / Krв;
            Kgв = is_nominal ? 1 : (Math.Sinh(ξв) + Math.Sin(ξв)) / (Math.Cosh(ξв) + Math.Cos(ξв)) / ξв; //is_nominal 
            #endregion
            #region для нижнего паза к.з. клетки ротора
            qстн = 0.318 * (dпв * dпв + dпн * dпн) + 0.9 * (dпн + dпв) * hp1 / 2;
            hст = 0.45 * (dпн + dпв) + hp1;
            ξн = 0.628 * hст * Math.Sqrt(S * f1 * 9e-6 / ρ2Г);
            φ = ξн * (Math.Sinh(2 * ξн) + Math.Sin(2 * ξн)) / (Math.Cosh(2 * ξн) - Math.Cos(2 * ξн)) - 1;
            hr = hст / (1 + φ);
            br = 0.9 * dпв - 0.9 * (dпв - dпн) * (hr - 0.45 * dпв) / hp1;
            qr = 0.318 * dпв * dпв + 0.5 * (0.9 * dпв + br) * (hr - 0.45 * dпв);
            Krн = is_nominal || hr > hp1 + 0.45 * dпв ? 1 : qст / qr; //is_nominal
            r2cтн = ρ2Г * lp * 1e-3 * Krн / qст; //is_nominal (rcт asdn) 
            #endregion
            rcтн = r2cтн + rвн; rcтв = r2cтв - rвн;
            Dкв = Dp - aк - 2 * ΔГ2; //добавил ΔГ2
            qкв = aк * bк;
            r2кв = ρ2Г * 2 * Math.PI * Dкв * 1e-3 / Z2 / qкв / γ2 / γ2; //rк asdn
            Dкн = Dp - 2 * (hш + dкп + hр2 + ΔГ2) - aкн; //добавил ΔГ2
            qкн = aкн * bкн;
            r2кн = ρ2Г * 2 * Math.PI * Dкн * 1e-3 / Z2 / qкн / γ2 / γ2; //rк asdn
            ν = 4 * m1 * (W1 * k1) * (W1 * k1) / Z2 / fСК / fСК; //скос FCK присутствует
            rʹв = (rcтв + r2кв) * ν; //is_nominal
            rʹн = (rcтн + r2кн) * ν; //is_nominal
            rʹво = rвн * ν;
            Kgн = is_nominal ? 1 : 1.5 * (Math.Sinh(2 * ξн) - Math.Sin(2 * ξн)) / (Math.Cosh(2 * ξн) - Math.Cos(2 * ξн)) / ξн; //is_nominal
        }
        /// <summary>
        /// Параметры ротора двойная клетка. Расчет по 4.1.136 - 4.1.140, rʹ2, xʹ2
        /// </summary>
        void Rotor_doubleDeck_part2(double qстн, double Kgн, double Kgв, double Dкн, double Dкв, double γ2, double Σλ1, double λq2,
            double rʹво, double rʹн, double rʹв, bool is_nominal) {
            double λn2н = ((hp1 + 0.4 * dпн) * (1 - 0.393 * dпв * dпв / qстн) * (1 - 0.393 * dпв * dпв / qстн) / 3 / dпв + 0.66 - 0.5 * bп2 / dпв) * Kgн +
                (0.785 - 0.5 * bп2 / dкп) * Kgв + hр2 / bп2;
            double λнв = (0.785 - 0.5 * bш / dкп) * Kgв + hш / bш;
            double λsн = Dкн / Z2 / lp / γ2 / γ2 * Math.Log(4.7 * Dкн / 2 / (aкн + bкн));
            double λsв = Dкв / Z2 / lp / γ2 / γ2 * Math.Log(4.7 * Dкв / 2 / (aк + bк));
            double λ2н = (λn2н + λsн * li / lp) * lp * Z1 * k1 * k1 / li / Z2;
            double Xн = x1 * λ2н / Σλ1;
            //Расчет по 4.1.136 - 4.1.140
            double λ2о = λнв + λq2 + λsв * li / lp;
            double λʹ2о = λ2о * lp * Z1 * k1 * k1 / li / Z2;
            double Xо = x1 * λʹ2о / Σλ1;
            rʹ2 = is_nominal ? rʹво + (rʹн * rʹв * rʹв + rʹв * rʹн * rʹн) / (rʹн + rʹв) / (rʹн + rʹв) :
                rʹво + (rʹн * rʹв * rʹв + rʹв * (rʹн * rʹн + S * Xн * S * Xн)) / ((rʹн + rʹв) * (rʹн + rʹв) + S * Xн * S * Xн); //is_nominal
            xʹ2 = is_nominal ? Xо + Xн * rʹв * rʹв / (rʹн + rʹв) / (rʹн + rʹв) :
                 Xо + Xн * rʹв * rʹв / ((rʹн + rʹв) * (rʹн + rʹв) + S * Xн * S * Xн);
        }
        /// <summary>
        /// Коэффициент вытеснения тока (приложение 3)
        /// </summary>
        /// <param name="ξ"></param>
        /// <returns></returns>
        double get_φ(double ξ) {
            if (ξ <= 1) return 0.045 * Math.Pow(ξ, 6);
            else if (ξ <= 1.8) return ((((1.82877552 * ξ - 10.2368727) * ξ + 21.3409097) * ξ - 19.2803795) * ξ + 6.38604606) * ξ;
            else if (ξ <= 2.4) return ((((3.45008453 * ξ - 28.8691783) * ξ + 90.0073613) * ξ - 123.415916) * ξ - 62.994462) * ξ;
            else if (ξ <= 3) return ((((29.0400339 - 2.68472346 * ξ) * ξ - 117.647563) * ξ + 211.888673) * ξ - 142.642932) * ξ;
            else if (ξ <= 4) return ((((6.41375234 - 0.460707453 * ξ) * ξ - 33.4027525) * ξ + 77.3584017) * ξ - 66.491109) * ξ;
            return ξ;
        }
        //расчет сопротивлений схемы замещения
        void CalculationResistance(double x1, double xʹ2, ref double rʹʹ2, ref double xʹʹ2, ref double rʹk, ref double xʹk, 
            ref double Zʹoo, ref double RM, ref double ZM) {
            rʹʹ2 = rʹ2 * c1 * c1;
            xʹʹ2 = xʹ2 * c1 * c1;
            rʹ1 = r1Г * c1;
            rʹk = rʹ1 + rʹʹ2; //пр
            xʹ1 = x1 * c1;
            xʹk = xʹ1 + xʹʹ2; //пр
            Zʹoo = Math.Sqrt(rʹ1 * rʹ1 + xʹk * xʹk);
            RM = Zʹoo + rʹ1;
            ZM = Math.Sqrt(2 * RM * Zʹoo);
        }
        void MaximumRegim_core(double in_S, double Iʹʹ2,  double γ2, double fСК, double Kp, double Kʹβ, double λn1, double δ, double λq1, 
            double λq2, double λлоб1, double λлоб2, double λСК, double Σλ1, 
            ref double rʹʹ2, ref double rʹk, ref double xʹk, ref double Zʹoo, ref double RM, ref double ZM) {
            double AWм, A, A1, A2, Δac, Δλn1, Δbш, Δλn2 = 0, λn1M, Kλz, λq1M, λq2M, Σλ1M, x1M, xʹʹ2 = 0, Σλ2, λn2 = 0, λn2M, Σλ2M, xʹ2M = 0, Kg = 0;
            double qстн = 0, Kgн = 0, Kgв = 0, Dкн = 0, Dкв = 0, rʹво = 0, rʹн = 0, rʹв = 0;
            switch (PAS) {
                case TypeOfRotorSlot.Round:
                    Rotor_roundSlot_part(false, in_S, γ2, fСК, out Kg, out _, out λn2);
                    break;
                case TypeOfRotorSlot.Rectangular:
                    Rotor_rectangularSlot_part(false, in_S, γ2, fСК, out Kg, out _, out λn2);
                    break;
                case TypeOfRotorSlot.Pyriform:
                    Rotor_pyriformSlot_part(false, in_S, γ2, fСК, out Kg, out _, out λn2);
                    break;
                case TypeOfRotorSlot.DoubleDeck:
                    Rotor_doubleDeck_part1(false, in_S, γ2, fСК,
                        out qстн, out _, out Kgн, out Kgв, out Dкн, out Dкв, out rʹво, out rʹн, out rʹв, out _, out _);
                    break;
                default:  λn2 = 0; break;
            }
            AWм = Kp * W1 * k1 * Iʹʹ2 / 0.37 * p;
            A1 = 1 / Kfe1 - 1600 * ac / AWм;
            A = d1 == 0 ? bп1 : d1;
            Δac = A1 <= 0 ? 0 : (A - ac) * A1 / (1 + 1600 * (A - ac) / AWм);
            Δλn1 = Δac == 0 ? 0 : Kʹβ * (3 * h6 / (A + Δac) * Δac / (Δac + ac + 0.5 * A) + 3 * h8 / (A + Δac) * Δac / (Δac + ac + 0.5 * bпн) + h7 / ac * Δac / (Δac + ac));
            A2 = 1 / Kfe2 - 1600 * bш / AWм;
            switch (PAS) { //1 - круглый, 2 - прямоугольный, 3 - грушевидный, 4 - двойная клетка
                case TypeOfRotorSlot.Round:
                case TypeOfRotorSlot.DoubleDeck:
                    A = dкп;
                    break;
                case TypeOfRotorSlot.Rectangular:
                    A = bп2;
                    break;
                case TypeOfRotorSlot.Pyriform:
                    A = dпв;
                    break;
            }
            Δbш = A2 <= 0 ? 0 : (A - bш) * A2 / (1 + 1600 * (A - bш) / AWм);
            switch (PAS) { //1 - круглый, 2 - прямоугольный, 3 - грушевидный, 4 - двойная клетка
                case TypeOfRotorSlot.Round:
                    Δλn2 = hш / bш * Δbш / (Δbш + bш) + 0.5 * Δbш / dкп * Kg;
                    break;
                case TypeOfRotorSlot.Rectangular:
                    Δλn2 = hш / bш * Δbш / (Δbш + bш);
                    break;
                case TypeOfRotorSlot.Pyriform:
                    Δλn2 = hш / bш * Δbш / (Δbш + bш) + 0.5 * Δbш / dпв * Kg;
                    break;
                case TypeOfRotorSlot.DoubleDeck:
                    Δλn2 = hш / bш * Δbш / (Δbш + bш) + 0.5 * Δbш / dкп * Kgв;
                    break;
            }
            λn1M = λn1 - Δλn1;
            Kλz = (1 + Δac / (ac + 5 * δ)) * (1 + Δbш / (bш + 5 * δ));
            λq1M = λq1 / Kλz;
            λq2M = λq2 / Kλz;
            Σλ1M = λn1M + λq1M + λлоб1;
            x1M = x1 * Σλ1M / Σλ1;
            if (PAS == TypeOfRotorSlot.DoubleDeck) 
                Rotor_doubleDeck_part2(qстн, Kgн, Kgв, Dкн, Dкв, γ2, Σλ1, λq2, rʹво, rʹн, rʹв, false);
            else {
                Σλ2 = λn2 + λлоб2 + λq2 + λСК; //is_nominal
                λn2M = λn2 - Δλn2;
                Σλ2M = λn2M + λq2M + λлоб2 + λСК;
                xʹ2M = xʹ2 * Σλ2M / Σλ2;
            }
            CalculationResistance(x1M, xʹ2M, ref rʹʹ2, ref xʹʹ2, ref rʹk, ref xʹk, ref Zʹoo, ref RM, ref ZM);
        }
        #endregion
        #endregion
        #region результаты работы
        //B: Гс -> Тл
        //F: кг -> Н
        //геометрические размеры и параметры машины
        public Dictionary<string, Dictionary<string, double>> Get_DataMachine => SolutionIsDone ?
                    new Dictionary<string, Dictionary<string, double>>() {
                        { "ротор", new Dictionary<string, double>() {
                                { "lp", Math.Round(lp, 5) },
                                { "hj2", Math.Round(hj2, 5) },
                                { "bZ2MIN", Math.Round(bZ2MIN, 5) },
                                { "bZ2MAX", Math.Round(bZ2MAX, 5) },
                                { "qс", Math.Round(qст, 5) },
                                { "qк", Math.Round(qк, 5) },
                                { "rʹ2", Math.Round(rʹ2хх, 5) },
                                { "xʹ2", Math.Round(xʹ2хх, 5) }
                            }
                        },
                        { "статор", new Dictionary<string, double>() {
                                { "hj1", Math.Round(hj1, 5) },
                                { "hZ1", Math.Round(h2 + 2 * h3 + h4 + h5 + h6 + h7 + h8, 5) },
                                { "Kз", Math.Round(Kз, 5) },
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
                                { "BδM", Math.Round(Bδm * 1e-4 * 1e-4, 5) },
                                { "Fδ", Math.Round(Fδ, 5) }
                            }
                        }
                    } : null;
        //холостой ход PFe2, PFe2 окр отсутствуют [0], [1]
        public Dictionary<string, Dictionary<string, double>> Get_Idle => SolutionIsDone ?
                    new Dictionary<string, Dictionary<string, double>>() {
                        { "ротор", new Dictionary<string, double>() {
                                { "Pпов2", Math.Round(Pпов2, 5) },
                                { "Pпов2 окр", Math.Round(Pпов2_round, 1) },
                                { "Pпул2", Math.Round(Pпул2, 5) },
                                { "Pпул2 окр", Math.Round(Pпул2_round, 1) }
                            }
                        },
                        { "статор", new Dictionary<string, double>() {
                                { "Pz1", Math.Round(Pz1, 5) },
                                { "Pz1 окр", Math.Round(Pz1_round, 1) },
                                { "Pj1", Math.Round(Pj1, 5) },
                                { "Pj1 окр", Math.Round(Pj1_round, 1) },
                                { "PГ", Math.Round(PГ, 5) },
                                { "PГ окр", Math.Round(PГ_round, 1) },
                                { "Pпов1", Math.Round(Pпов1, 5) },
                                { "Pпов1 окр", Math.Round(Pпов1_round, 1) },
                                { "Pпул1", Math.Round(Pпул1, 5) },
                                { "Pпул1 окр", Math.Round(Pпул1_round, 1) }
                            }
                        },
                        { "прочее", new Dictionary<string, double>() {
                                { "E1", Math.Round(E1, 5) },
                                { "P0", Math.Round(P0, 5) },
                                { "W0", Math.Round(W0, 5) },
                                { "I0А", Math.Round(I0А, 5) },
                                { "IХХА", Math.Round(IХХА, 5) },
                                { "IXX", Math.Round(IXX, 5) },
                                { "cosφ0", Math.Round(cosφ0, 5) },
                                { "Iμ", Math.Round(Iμ, 5) },
                                { "xμ", Math.Round(xμ, 5) },
                                { "Kδ", Math.Round(Kδ, 5) },
                                { "ΣF", Math.Round(ΣF, 5) }
                            }
                        }
                    } : null;
        //номинальный режим (SK, SK окр - кВ*А, MН * 9.8)
        public Dictionary<string, Dictionary<string, double>> Get_NominalRating => SolutionIsDone ?
                    new Dictionary<string, Dictionary<string, double>>() {
                        { "ротор", new Dictionary<string, double>() {
                                { "rʹʹ2", Math.Round(rʹʹ2н, 5) },
                                { "xʹʹ2", Math.Round(xʹʹ2н, 5) },
                                { "rʹ2", Math.Round(rʹ2н, 5) },
                                { "xʹ2", Math.Round(xʹ2н, 5) },
                                { "Iʹʹ2Н", Math.Round(Iʹʹ2Н, 5) },
                                { "nН", Math.Round(nН, 5) },
                                { "PЭ2", Math.Round(PЭ2, 5) },
                                { "PЭ2 окр", Math.Round(PЭ2_round, 1) },
                                { "Δi2", Math.Round(Δi2, 5) },
                                { "ΔiК", Math.Round(ΔiК, 5) }
                            }
                        },
                        { "статор", new Dictionary<string, double>() {
                                { "I1A", Math.Round(I1A, 5) },
                                { "I1R", Math.Round(I1R, 5) },
                                { "I1Н", Math.Round(I1Н, 5) },
                                { "I1Н окр", Math.Round(I1Н_round, 1) },
                                { "PЭ1", Math.Round(PЭ1, 5) },
                                { "PЭ1 окр", Math.Round(PЭ1_round, 1) },
                                { "Δi1", Math.Round(Δi1, 5) }
                            }
                        },
                        { "прочее", new Dictionary<string, double>() {
                                { "MН", Math.Round(MН * 9.8, 5) },
                                { "SН", Math.Round(SН, 5) },
                                { "cosφН", Math.Round(cosφН, 5) },
                                { "cosφН окр", Math.Round(cosφН_round, 1) },
                                { "ηЭЛ", Math.Round(ηЭЛ, 5) },
                                { "ηЭЛ окр", Math.Round(ηЭЛ_round, 1) },
                                { "P1", Math.Round(P1, 5) },
                                { "P1 окр", Math.Round(P1_round, 1) },
                                { "Pʹ2", Math.Round(Pʹ2, 5) },
                                { "SK", Math.Round(SK, 5) },
                                { "SK окр", Math.Round(SK_round, 1) },
                                { "A", Math.Round(60 * W1 * I1Н / Math.PI / Di, 5) },
                                { "c1", Math.Round(c1, 5) },
                                { "rʹ1", Math.Round(rʹ1, 5) },
                                { "xʹ1", Math.Round(xʹ1н, 5) }
                            }
                        }
                    } : null;
        //перегрузочная способность (MM * 9.8)
        public Dictionary<string, double> Get_OverloadCapacity => SolutionIsDone ?
                    new Dictionary<string, double>() {
                        {"I1M", Math.Round(I1M, 5) },
                        {"Iʹʹ2M", Math.Round(Iʹʹ2M, 5) },
                        {"PM", Math.Round(PM, 5) },
                        {"MM", Math.Round(MM * 9.8, 5) },
                        {"KM", Math.Round(KM, 5) },
                        {"SM", Math.Round(SM, 5) },
                        {"cosφM", Math.Round(cosφM, 5) }
                    } : null;
        //пусковой режим (MП * 9.8)
        public Dictionary<string, double> Get_StartingConditions => SolutionIsDone ?
                    new Dictionary<string, double>() {
                        { "Iʹʹ2П", Math.Round(Iʹʹ2П, 5) },
                        { "I1П", Math.Round(I1П, 5) },
                        { "I1П окр", Math.Round(I1П_round, 1) },
                        { "MП", Math.Round(MП * 9.8, 5) },
                        { "KП", Math.Round(KП, 5) },
                        { "KI", Math.Round(KI, 5) }
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
                { "Ротор", Math.Round(PЭ2_round + Pпов2_round + Pпул2_round * 0.001, 5) },
                { "Суммарные электрические потери",
                Math.Round(PЭ1_round*0.001, 5) + Math.Round(Pj1_round*0.001, 5) + Math.Round(Pz1_round + Pпов1_round + Pпул1_round * 0.001, 5) +
                    Math.Round(PГ_round * 0.001, 5) + Math.Round(PЭ2_round + Pпов2_round + Pпул2_round * 0.001, 5) },
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
                { "F", lB  },
                { "B", B * 1e-4  },
                { "ΔГ1", ΔГ1  },
                { "ΔГ2", ΔГ2  },
                { "aк", aк  },
                { "bк", bк  },
                { "dв", dв  },
                { "Z1", Z1  },
                { "Z2", Z2  },
                { "hZ1", Math.Round(h2 + 2 * h3 + h4 + h5 + h6 + h7 + h8, 5)  },
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
        //готовность данных к выводу в интерфейс
        public bool SolutionIsDone { get; private set; }
        public List<string> Logging { get; private set; }
    }
}
