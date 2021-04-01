
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

            //prepare for W1
            //double Wk = 4 * input["Sсл"] * input["Kзап"] / Math.PI / dиз / dиз / a2;
            //W1 = Z1 * Wk / 3.0 / a1;

            SolutionIsDone = false; Logging = new List<string>();
        }
        
        //Расчетные алгоритмы: 
        //расчет параметров мащины и магнитной цепи, холостой ход, номинальный режим
        public void Run() {
            double τi, τy, bz2ср, Kp1, Ky1, ls, lʹs, Dк, γ2, rк, rc, ν, Kʹβ, Kβ, λn1, λq1, λq2, λлоб1, λлоб2, Σλ1, ΔKƨ, Δƨ, rпр, I2c, Ф10;
            double y;
            double Gz1, Gz2, Gj1, t1, nc, t2, δ, rруб, rs; //для холостого хода
            double f2, ξn, hx, hr, qr, r2cп, r2ξ, E1П_new; //пусковой режим
            double aСК, fСК; //скос

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
            bZ1MAX = Math.PI * (Di + 2 * h1) / Z1 - bП;
            bZ1MIN = d1 == 0 ? Math.PI * (Di + 2 * (h8 + h7 + h6)) / Z1 - bП1 : Math.PI * (Di + 2 * (h8 + h7 + h6)) / Z1 - d1;
            bZ1СР = (bZ1MAX + 2 * bZ1MIN) / 3;
            y = 0.5 * Z1 / p; 
            Kp1 = P3 ? 0.5 * Math.Sin(Math.PI * q1 / y) / q1 / Math.Sin(0.5 * Math.PI / y) : Math.Sin(0.5 * Math.PI * q1 / y) / q1 / Math.Sin(0.5 * Math.PI / y);
            Ky1 = Math.Sin(0.5 * Math.PI * y1 / y);
            k1 = Kp1 * Ky1;
            bZ2MIN = Math.PI * (Dp - 2 * (hp + ΔГ2)) / Z2 - bП2;
            bZ2MAX = Math.PI * Dp / Z2 - bП2;
            bz2ср = (bZ2MAX + 2 * bZ2MIN) / 3;
            δ = 0.5 * (Di - Dp);
            Kδ1 = (t1 + 5 * δ * t1 / ac) / (t1 - ac + 5 * δ * t1 / ac);
            Kδ2 = ΔГ2 == 0 ? (t2 + 5 * δ * t2 / bП2) / (t2 - bП2 + 5 * δ * t2 / bП2) : 1;
            Kδ = Kδ1 * Kδ2;
            lp = li + 5;
            aСК = bСК == 0 ? 0 : 2 * Math.PI * p * bСК / Z2 / t2; //скос
            fСК = aСК == 0 ? 1 : 2 * Math.Sin(0.5 * aСК) / aСК; //скос
            hj1 = 0.5 * (Da - (Di + 2 * h1));
            lc = 0.5 * Math.PI * (Da - hj1) / p;
            τy = 0.5 * Math.PI * (Di + 2 * (h8 + h7 + h6 + h5) + h4 + h3 + h2) * β / p;
            ls = K2 * τy + 2 * B; //могут быть отличия из-за K2
            lB = 0.5 * Math.Sqrt(ls * ls - τy * τy) * 1.03;
            L = 2 * (li + 2 * Δкр + ls) * 1.05;
            lʹs = 0.5 * (L - 2 * li);
            r1x = ρ1x * L * W1 * 0.001 / a1 / a2 / qГ;
            r1Г = r1x * ρ1Г / ρ1x;
            qк = aк * bк;
            Dк = Dp - aк - 2 * ΔГ2;
            γ2 = 2 * Math.Sin(Math.PI * p / Z2);
            rк = ρ2Г * 2 * Math.PI * Dк * 1e-3 / Z2 / qк / γ2 / γ2;
            //qс = hp * bc; //bc (bст) - ширина стержня к.з. клетки ротора
            qс = hp * bП2; //bП2 - ширина стержня к.з. клетки ротора
            rc = ρ2Г * lp * 1e-3 / qс;
            ν = 4 * m1 * (W1 * k1) * (W1 * k1) / Z2 / fСК / fСК; //скос FCK присутствует
            rʹ2 = (rc + rк) * ν;
            qИЗ = nэл * dиз * dиз;
            Sп = d1 == 0 ? 0.5 * (bП + bП1) * (h2 + 2 * h3 + h4 + h5) : 0.5 * (bП + d1) * (h2 + 2 * h3 + h4 + h5);
            Sʹп = d1 == 0 ? Sп - (2 * h3 * (bП + h2 + h3 + h4) + h5 * bП1) : Sп - (2 * h3 * (bП + h2 + h3 + h4) + h5 * d1);
            Kзап = qИЗ / Sʹп;
            Kʹβ = β <= 2 / 3 ? 0.25 * (6 * β - 1) : 0.25 * (1 + 3 * β);
            if ( P3 ) {
                β = β > 1 ? 2 - β : β; //β меняется!!!!!!!!!!!!
                Kʹβ = β <= 2 / 3 ? 1.125 : 0.75;
            }
            Kβ = 0.25 + 0.75 * Kʹβ;
            λn1 = d1 == 0 ? Kβ * (h2 + h4) / 3 / bП1 + (h5 / bП1 + 3 * h6 / (bП1 + 2 * ac) + h7 / ac + 3 * h8 / (bПН + 2 * ac)) * Kʹβ + 0.25 * h3 / bП1 :
                Kβ * (h2 + h4) / 3 / d1 + (0.785 - 0.5 * ac / d1 + h5 / d1 + h7 / ac + h8 / (bПН + 2 * ac)) * Kʹβ + 0.25 * h3 / d1;
            λq1 = t1 * k1 * k1 / 11.9 / δ / Kδ;
            λлоб1 = P3 ? 0.26 * q1 * (lʹs - 0.64 * β * τi) / li : 0.34 * q1 * (lʹs - 0.64 * β * τi) / li;
            Σλ1 = λn1 + λq1 + λлоб1; //bСК не используется
            x1 = 158e-6 * f1 * W1 * W1 * li * Σλ1 / 1e4 / p / q1;
            λq2 = t2 / 11.9 / δ / Kδ;
            λлоб2 = Dк * Math.Log(4.7 * Dк / (aк + bк)) / Z2 / lp / γ2 / γ2;
            ΔKƨ = functionAreshyan(cз / τi) * τi / li;
            rs = 3.82 * ρРУБ * li * W1 * W1 * k1 * k1 * 1e-3 / ΔГ1 / Di;
            Δƨ = ΔKƨ * rs; rпр = rs * cз / li; rруб = rпр * Δƨ / (rпр + Δƨ);
            Gj1 = 15.2 * hj1 * li * p * lc * Kfe1 * 1e-6;
            Gz1 = 7.6 * Z1 * bZ1СР * h1 * li * Kfe1 * 1e-6;
            Gz2 = 7.6 * Z2 * bz2ср * hp * Kfe2 * lp * 1e-6;
            E1 = 0.95 * U1; //начальное приближение ЭДС хх
            //предварительные параметры
            I2c = Ki * 2 * m1 * I1 * W1 * k1 / Z2;
            Ф10 = U1 * 1e8 / 4.44 / W1 / k1 / f1;
            SН = RatingLoop(S, Ф10, τi, t1, t2, bz2ср, δ, I2c, λлоб2, λq2, ν, Gz1, Gz2, Gj1, nc, rs, rруб, 
                out double PFe, out double PFe_round, out double I2Н, out double ZM, out double RM, out double ρ1, out double xʹk, out double Σλ2, out _, out double Sp, out double λСК);
            if ( double.IsNaN(SН) ) {
                Logging.Add($"Type: {GetType()}  message: Ошибка расчета номинального режима -> double.IsNaN(SН)");
                return;
            }
            while ( Math.Abs(S - SН) >= S * 5e-3 ) {
                S = 0.5 * (S + SН);
                SН = RatingLoop(S, Ф10, τi, t1, t2, bz2ср, δ, I2c, λлоб2, λq2, ν, Gz1, Gz2, Gj1, nc, rs, rруб, 
                    out PFe, out PFe_round, out I2Н, out ZM, out RM, out ρ1, out xʹk, out Σλ2, out _, out Sp, out λСК);
                if ( double.IsNaN(SН) ) {
                    Logging.Add($"Type: {GetType()}  message: Ошибка расчета номинального режима -> double.IsNaN(SН)");
                    return;
                }
            }
            SН = S; //требуется использовать величины после подроста = 0.5 * (S + SН). S не выводится
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
            Δi2 = I2Н / qс;
            ΔiК = I2Н / qк / γ2;
            rʹʹ2ЭН = rʹʹ2Э; xʹʹ2ЭН = xʹʹ2Э; //сбор данных для номинального режима
            //перегрузочная способность
            S = SН * 3; //начальное приближение скольжения
            SM = OverloadRegimLoop(S, ρ1, λq2, λлоб2, Σλ2, τi, Ф10, t2, bz2ср, λСК, ref xʹk, ref RM, ref ZM);
            while( Math.Abs(S - SM) >= S * 5e-3 ) {
                S = 0.5 * (S + SM);
                SM = OverloadRegimLoop(S, ρ1, λq2, λлоб2, Σλ2, τi, Ф10, t2, bz2ср, λСК, ref xʹk, ref RM, ref ZM);
            }
            SM = S; //требуется использовать величины после подроста = 0.5 * (S + SM). S не выводится
            n2 = nc - nc * SM;
            PM = 0.5 * m1 * U1 * U1 / RM;
            MM = 0.975 * PM / nc;
            KM = MM / MН;
            //пусковой режим
            f2 = f1;
            ξn = 0.2 * Math.PI * hp * Math.Sqrt(0.9 * f2 / ρ2Г * 1e-5);
            hx = 1.5 * hp * (Math.Sinh(2 * ξn) - Math.Sin(2 * ξn)) / (Math.Cosh(2 * ξn) - Math.Cos(2 * ξn)) / ξn;
            hr = hp * (Math.Cosh(2 * ξn) - Math.Cos(2 * ξn)) / ξn / (Math.Sinh(2 * ξn) + Math.Sin(2 * ξn));
            qr = hr * bП2; //bП2 - ширина стержня к.з. клетки ротора
            r2cп = hr < hp ? ρ2Г * lp * 1e-3 / qr : ρ2Г * lp * 1e-3 / qс;
            r2ξ = (r2cп + rк) * ν;
            E1П = 0.25 * U1; //предварительное ЭДС пускового режима
            Iʹʹ2П = 1.5* Iʹʹ2M; //предварительный ток ротора
            E1П_new = StartingRegimLoop_E1П(E1П, Ф10, f2, hx, λq2, λлоб2, Σλ2, τi, r2ξ, t2, Sp, ρ1, λСК, out xʹk, out _, out _, out double PFe2П, ref I2c);
            while ( Math.Abs(E1П_new - E1П) >= E1П * 5e-3 ) {
                E1П = 0.5 * (E1П + E1П_new);
                E1П_new = StartingRegimLoop_E1П(E1П, Ф10, f2, hx, λq2, λлоб2, Σλ2, τi, r2ξ, t2, Sp, ρ1, λСК, out xʹk, out _, out _, out PFe2П, ref I2c);
            };
            MП = 0.975 * (m1 * Iʹʹ2П * Iʹʹ2П * rʹʹ2Э + PFe2П) / nc;
            KП = MП / MН;
            KI = I1П / I1Н;
            SolutionIsDone = true;
        }
        //расчет SН номинального режима (используется значение S хх) Здесь же вычисляется E1нр
        double RatingLoop( double S, double Ф10, double τi, double t1, double t2, double bz2ср, double δ, double I2c, double λлоб2, double λq2,
            double ν, double Gz1, double Gz2, double Gj1, double nc, double rs, double rруб, 
            out double PFe, out double PFe_round, out double I2Н, out double ZM, out double RM,
            out double ρ1, out double xʹk, out double Σλ2, out double Zʹoo, out double Sp, out double λСК ) {
            double f2, ФМН, μzН, E1_new, BδMН, Bz2Н;
            f2 = f1 * S;
            E1_new = IdleLoop(E1, Ф10, f2, τi, t1, t2, bz2ср, δ, I2c, λлоб2, λq2, ν, Gz1, Gz2, Gj1, nc, rs, rруб, 
                out double μe, out ρ1, out PFe, out PFe_round, out Σλ2, out Sp, out λСК);
            if ( double.IsNaN(E1_new) ) { I2Н = ZM = RM = xʹk = Zʹoo = double.NaN; return double.NaN; }
            while ( Math.Abs(E1_new - E1) >= E1 * 5e-3 ) {
                E1 = 0.5 * (E1_new + E1);
                E1_new = IdleLoop(E1, Ф10, f2, τi, t1, t2, bz2ср, δ, I2c, λлоб2, λq2, ν, Gz1, Gz2, Gj1, nc, rs, rруб, 
                    out μe, out ρ1, out PFe, out PFe_round, out Σλ2, out Sp, out λСК);
                if ( double.IsNaN(E1_new) ) { I2Н = ZM = RM = xʹk = Zʹoo = double.NaN; return double.NaN; }
            }
            RatingCore(μe, f2, τi, ρ1, S, out double sinφН, out _, out _, out _, out _, out _);
            //RatingCore(μe, f2, τi, ρ1, S, out double sinφН, out double RH, out ZM, out RM, out xʹk, out Zʹoo); //old
            E1нр = Math.Sqrt((U1 * cosφН - I1Н * r1Г) * (U1 * cosφН - I1Н * r1Г) + (U1 * sinφН - I1Н * x1) * (U1 * sinφН - I1Н * x1));
            ФМН = E1нр * Ф10 / U1;
            //новый поток, требуется пересчитать BδM, Bz2, Hz2
            BδMН = get_BδM(ФМН, αδ, τi, li);
            Bz2Н = get_Bz2(BδMН, t2, li, bz2ср, lp, Kfe2);
            μzН = Bz2Н / functionMagnetization09X17H(Bz2Н);
            RatingCore(μzН, f2, τi, ρ1, S, out _, out double RH, out ZM, out RM, out xʹk, out Zʹoo);// пересчет при μe = μz
            //RatingCore(μzН, f2, τi, ρ1, S, out sinφН, out RH, out ZM, out RM, out xʹk, out Zʹoo);// old
            I2Н = Iʹʹ2Н * c1 * 2 * m1 * W1 * k1 / Z2;
            return 1.4 / (1 + RH / rʹʹ2Э);
        }
        //часть расчета номинального режима с определяющим параметром μ (4.3.22)
        void RatingCore( double μ, double f2, double τi, double ρ1, double S, out double sinφН, out double RH, out double ZM, out double RM, 
            out double xʹk, out double Zʹoo ) {
            double Zʹk, RʹH, ZH;
            //для номинального режима собираем r2ст, x2ст
            relation_μ(μ, f2, τi, S, xʹ2, rʹ2, out xʹk, out double rʹk, out RM, out ZM, out Zʹoo, out r2ст, out x2ст);
            Zʹk = Math.Sqrt(rʹk * rʹk + xʹk * xʹk);
            RH = 1.5 * U1 * U1 / Pʹ2 - rʹk + Math.Sqrt((1.5 * U1 * U1 / Pʹ2 - rʹk) * (1.5 * U1 * U1 / Pʹ2 - rʹk) - Zʹk * Zʹk);
            RʹH = RH + rʹk;
            ZH = Math.Sqrt(RʹH * RʹH + xʹk * xʹk);
            Iʹʹ2Н = U1 / ZH;
            I1A = I0А + Iʹʹ2Н * (RʹH + 2 * ρ1 * xʹk) / ZH;
            I1R = Iμ + Iʹʹ2Н * (xʹk - 2 * ρ1 * RʹH) / ZH;
            I1Н = Math.Sqrt(I1A * I1A + I1R * I1R);
            cosφН = I1A / I1Н;
            sinφН = I1R / I1Н;
        }
        //расчет E1 холостого хода
        double IdleLoop(double E1xx, double Ф10, double f2, double τi, double t1, double t2, double bz2ср, double δ, double I2c, double λлоб2, double λq2,
            double ν, double Gz1, double Gz2, double Gj1, double nc, double rs, double rруб, out double μe, out double ρ1, out double PFe, 
            out double PFe_round, out double Σλ2, out double Sp, out double λСК ) {
            double Фm, λn2, Pудz1, Pудj1, Pпов, Pпул, rэр, sinφ0, αδ_new;
            Фm = E1xx * Ф10 / U1;
            //αδ - начальное приближение (должно меняться)
            αδ_new = function_αδнLoop(αδ, Фm, τi, t1, E1xx, bZ1СР, hj1, f2, lp, t2, bz2ср, δ, Kδ, out double Hj2, out μe, out λСК);
            if ( double.IsNaN(αδ_new) ) {
                Logging.Add($"Type: {GetType()}  message: Ошибка расчета параметров машины и магнитной цепи -> double.IsNaN(αδ_new)");
                SolutionIsDone = false; ρ1 = PFe = PFe_round = Σλ2 = Sp = double.NaN;
                return double.NaN;
            }
            while ( Math.Abs(αδ_new - αδ) >= αδ * 5e-3 ) {
                αδ = 0.5 * (αδ + αδ_new);
                αδ_new = function_αδнLoop(αδ, Фm, τi, t1, E1xx, bZ1СР, hj1, f2, lp, t2, bz2ср, δ, Kδ, out Hj2, out μe, out λСК);
                if ( double.IsNaN(αδ_new) ) {
                    Logging.Add($"Type: {GetType()}  message: Ошибка расчета параметров машины и магнитной цепи -> double.IsNaN(αδ_new)");
                    SolutionIsDone = false; ρ1 = PFe = PFe_round = Σλ2 = Sp = double.NaN;
                    return double.NaN;
                }
            }
            Iμ = p * ΣF / 2.7 / W1 / k1;
            λn2 = hp / 3 / bП2 + ΔГ2 / bП2 + 950 * ΔГ2 / I2c;
            Σλ2 = λn2 + λлоб2 + λq2 + λСК; 
            xʹ2 = 7.9e-9 * lp * Σλ2 * ν * f1;
            xμ = E1xx / Iμ;
            c1 = 1 + x1 / xμ; ρ1 = r1Г / (x1 + xμ);
            //расчет E1 холостого хода
            Pудz1 = f1 < 150 ? p10_50 * Bz1 * Bz1 * Math.Pow(f1 / 50, 1.5) * 1e-8 : p10_50 * Bz1 * Bz1 * Math.Pow(f1 / 400, 1.5) * 1e-8;
            Pz1 = 1.8 * Pудz1 * Gz1; Pz1_round = Pz1 * 1.4;
            Pудj1 = f1 < 150 ? p10_50 * Bj1 * Bj1 * Math.Pow(f1 / 50, 1.5) * 1e-8 : p10_50 * Bj1 * Bj1 * Math.Pow(f1 / 400, 1.5) * 1e-8;
            Pj1 = 1.6 * Pудj1 * Gj1; Pj1_round = Pj1 * 1.4;
            Pпов1 = ΔГ2 == 0 ? 2.67 * Di * li * (t1 - ac) * Math.Pow(Z2 * nc * 1e-4, 1.5) * Math.Pow(function_β0(bП2 / δ) * Kδ * BδM * t2 * 1e-3, 2) * 1e-8 / t1 : 0;
            Pпов1_round = Pпов1 * 1.4;
            Pпов2 = 36.58 * Di * li * (t2 - bП2) * Math.Pow(Z1 * nc * 1e-4, 1.5) * Math.Pow(function_β0(ac / δ) * Kδ * BδM * t1 * 1e-3, 2) * 1e-8 / t2;
            Pпов2_round = Pпов2 * 1.4;
            Pпов = Pпов1 + Pпов2;
            Sp = Math.PI * Dp * lp * 1e-6;
            PFe2 = 1.2 * Math.Sqrt(f2 * 1e-4 * Bj2 * Hj2 * Hj2 * Hj2) * Sp; PFe2_round = PFe2 * 1.4;
            Pпул1 = ΔГ2 == 0 ?
                0.0225 * (Z2 * nc * δ * Bz1 * bП2 / δ * bП2 / δ * 1e-7 / t1 / (5 + bП2 / δ)) * (Z2 * nc * δ * Bz1 * bП2 / δ * bП2 / δ * 1e-7 / t1 / (5 + bП2 / δ)) * Gz1 : 0;
            Pпул1_round = Pпул1 * 1.4;
            Pпул2 = 0.0275 * (Z1 * nc * δ * Bz2 * ac / δ * ac / δ * 1e-7 / t2 / (5 + ac / δ)) * (Z1 * nc * δ * Bz2 * ac / δ * ac / δ * 1e-7 / t2 / (5 + ac / δ)) * Gz2;
            Pпул2_round = Pпул2 * 1.4;
            Pпул = Pпул1 + Pпул2;
            rэр = rs + rруб;
            PГ = m1 * E1 * E1 / rэр; PГ_round = PГ * 1.3;
            PFe = Pz1 + Pj1 + Pпов + Pпул + PFe2;
            PFe_round = Pz1_round + Pj1_round + Pпов * 1.4 + Pпул * 1.4 + PFe2_round;
            P0 = m1 * Iμ * Iμ * r1Г + PFe + PГ;
            W0 = P0 + Pмех;
            I0А = Iμ * ρ1 + (PГ + PFe) / 3 / U1;
            IХХА = I0А * W0 / P0;
            IXX = Math.Sqrt(IХХА * IХХА + Iμ * Iμ);
            cosφ0 = IХХА / IXX; sinφ0 = Iμ / IXX;
            return Math.Sqrt((U1 * cosφ0 - IXX * r1Г) * (U1 * cosφ0 - IXX * r1Г) + (U1 * sinφ0 - IXX * x1) * (U1 * sinφ0 - IXX * x1));
        }
        //расчет SM максимального режима
        double OverloadRegimLoop( double S, double ρ1, double λq2, double λлоб2, double Σλ2, double τi, double Ф10, double t2, double bz2ср, 
            double λСК, ref double xʹk,  ref double RM, ref double ZM ) {
            double f2, I1MA, I1MR, sinφM, I2cM, λn2M, Σλ2M, xʹ2M, μz, ФММ, BδM, Bz2M, Hz2M;
            Iʹʹ2M = U1 / ZM;
            f2 = f1 * S;
            I1MA = I0А + Iʹʹ2M * (RM + 2 * ρ1 * xʹk) / ZM;
            I1MR = Iμ + Iʹʹ2M * (xʹk - 2 * ρ1 * RM) / ZM;
            I1M = Math.Sqrt(I1MA * I1MA + I1MR * I1MR);
            cosφM = I1MA / I1M; sinφM = I1MR / I1M;
            I2cM = Iʹʹ2M * c1 * 2 * m1 * W1 * k1 / Z2;
            λn2M = hp / 3 / bП2 + ΔГ2 / bП2 + 950 * ΔГ2 / I2cM;
            Σλ2M = λn2M + λq2 + λлоб2 + λСК;
            xʹ2M = xʹ2 * Σλ2M / Σλ2;
            E1M = Math.Sqrt((U1 * cosφM - I1M * r1Г) * (U1 * cosφM - I1M * r1Г) + (U1 * sinφM - I1M * x1) * (U1 * sinφM - I1M * x1));
            //новый поток (HZ2) считается при параметрах номинального режима: пересчитанные BδM, Bz2 -> BδM = get_BδM(ФМН, αδ, τi, li);
            μz = Bz2 / functionMagnetization09X17H(Bz2);
            //расчет при S = SM 
            relation_μ(μz, f2, τi, S, xʹ2M, rʹ2, out xʹk, out _, out RM, out ZM, out double Zʹoo, out _, out _);
            //relation_μ(μz, f2, τi, S, xʹ2M, rʹ2, out xʹk, out double rʹk, out RM, out ZM, out double Zʹoo, out double r2ст, out double x2ст); //old
            ФММ = E1M * Ф10 / U1;
            BδM = get_BδM(ФММ, αδ, τi, li);
            Bz2M = get_Bz2(BδM, t2, li, bz2ср, lp, Kfe2);
            //Hz2M = functionMagnetization09X17H(Bz2M); //old
            _ = functionMagnetization09X17H(Bz2M);
            return rʹʹ2Э / Zʹoo;
        }
        //расчет тока ротора пускового режима
        //неявно зависит от: bП2, ΔГ2, xʹ2, c1, m1, W1
        double StartingRegimLoop_Iʹʹ2П( double Iʹʹ2П, double hx, double λq2, double λлоб2, double Σλ2, double μ, double f2, double τi, 
            double rʹ2, double λСК, out double xʹk, out double rʹk, out double Zʹk, ref double I2c ) {
            double λп2П, Σλ2П, xʹ2П;
            λп2П = hx / 3 / bП2 + ΔГ2 / bП2 + 950 * ΔГ2 / I2c;
            Σλ2П = λп2П + λq2 + λлоб2 + λСК;
            xʹ2П = xʹ2 * Σλ2П / Σλ2;
            relation_μ(μ, f2, τi, 1, xʹ2П, rʹ2, out xʹk, out rʹk, out _, out _, out _, out _, out _);
            //relation_μ(μ, f2, τi, 1, xʹ2П, rʹ2, out xʹk, out rʹk, out double RM, out double ZM, out double Zʹoo, out double r2ст, out double x2ст); //old
            Zʹk = Math.Sqrt(rʹk * rʹk + xʹk * xʹk);
            I2c = Iʹʹ2П * c1 * 2 * m1 * W1 * k1 / Z2;
            return U1 / Zʹk;
        }
        //расчет E1П пускового режима
        double StartingRegimLoop_E1П( double E1П, double Ф10, double f2, double hx, double λq2, double λлоб2, double Σλ2, double τi,
            double r2ξ, double t2, double Sp, double ρ1, double λСК, out double xʹk, out double rʹk, out double ZʹkП, out double PFe2П, ref double I2c ) {
            double ФМП, Bj2П_new, Bj2П, Iʹʹ2П_new, I1AП, I1RП, cosφП, sinφП;
            ФМП = E1П * Ф10 / U1; //предварительный поток
            Bj2П = ФМП * 50 / hʹj2 / lp; //предварительная индукция
            Bj2П_new = functionBj2(Bj2П, f2, ФМП, lp, out double μeП, out double hʹj2П);
            while (Math.Abs(Bj2П_new - Bj2П) >= Bj2П * 5e-3) {
                Bj2П += 0.1 * (Bj2П_new - Bj2П);
                Bj2П_new = functionBj2(Bj2П, f2, ФМП, lp, out μeП, out hʹj2П);
            };
            Iʹʹ2П_new = StartingRegimLoop_Iʹʹ2П(Iʹʹ2П, hx, λq2, λлоб2, Σλ2, μeП, f2, τi, r2ξ, λСК, out xʹk, out rʹk, out ZʹkП, ref I2c);
            while ( Math.Abs(Iʹʹ2П_new - Iʹʹ2П) >= Iʹʹ2П * 5e-3 ) {
                Iʹʹ2П = 0.5 * (Iʹʹ2П + Iʹʹ2П_new);
                Iʹʹ2П_new = StartingRegimLoop_Iʹʹ2П(Iʹʹ2П, hx, λq2, λлоб2, Σλ2, μeП, f2, τi, r2ξ, λСК, out xʹk, out rʹk, out ZʹkП, ref I2c);
            };
            Sp = hʹj2П <= 0.5 * bZ2MIN ? (2 * hp + t2) * lp * Z2 * 1e-6 : Sp;
            PFe2П = 1.2 * Sp * Math.Sqrt(f2 * Bj2П * functionMagnetization09X17H(Bj2П) * functionMagnetization09X17H(Bj2П) * functionMagnetization09X17H(Bj2П) * 1e-4);
            I1AП = I0А + PFe2П / m1 / U1 + Iʹʹ2П * (rʹk + 2 * ρ1 * xʹk) / ZʹkП;
            I1RП = Iμ + Iʹʹ2П * (xʹk - 2 * ρ1 * rʹk) / ZʹkП;
            I1П = Math.Sqrt(I1AП * I1AП + I1RП * I1RП); I1П_round = I1П * 1.2;
            cosφП = I1AП / I1П; sinφП = I1RП / I1П;
            return Math.Sqrt((U1 * cosφП - I1П * r1Г) * (U1 * cosφП - I1П * r1Г) + (U1 * sinφП - I1П * x1) * (U1 * sinφП - I1П * x1));
        }
        #region вспомогательные функции
        //неявно зависит от: bП2, m1, hp, Z2, p, f1, r2ст, x2ст, k1, c1, rʹʹ2Э, xʹʹ2Э, r1Г, hp, xʹ1, x1, li, Z2
        void relation_μ( double μ, double f2, double τi, double S, double xʹ2, double rʹ2, out double xʹk, out double rʹk, out double RM, out double ZM,
            out double Zʹoo, out double r2ст, out double x2ст ) {
            double W, Y, Z1н, qн, bн, γʹн, γн, q, y2, Kн, Kʹʹн, Z2ст, Z22ст, Z22кл, g12, b12, y212;
            W = 9.92e5 * Math.Sqrt(1 / μ);
            Y = 1.633 * W;
            Z1н = (W * W + Y * Y) * 1e-5;
            qн = Math.Sqrt(25.12 * W * Math.Sqrt(f2) * 1e-8 / bП2);
            bн = Math.Sqrt(25.12 * Y * Math.Sqrt(f2) * 1e-8 / bП2);
            γʹн = Math.Pow(qн * qн * qн * qн + bн * bн * bн * bн, 0.25);
            γн = 0.1 * γʹн * hp;
            q = Math.Exp(γʹн);
            y2 = Math.Exp(-γʹн);
            Kн = (q - y2) / (q + y2) / γн;
            Kʹʹн = 0.25 * τi * (1 + Z2 * hp * Kн / p / τi) / li;
            Z2ст = Math.PI * Math.Sqrt(f1) * m1 * W1 * W1 * k1 * k1 * 1e-4 / Math.Sqrt(S * 100) / Kʹʹн / p;
            r2ст = Y * Z2ст / Z1н;
            x2ст = W * Z2ст / Z1н;
            Z22ст = r2ст * r2ст + x2ст * x2ст;
            Z22кл = rʹ2 / S * rʹ2 / S + xʹ2 * xʹ2;
            g12 = r2ст / Z22ст + rʹ2 / S / Z22кл;
            b12 = x2ст / Z22ст + xʹ2 / Z22кл;
            y212 = g12 * g12 + b12 * b12;
            rʹʹ2Э = g12 * S * c1 * c1 / y212;
            xʹʹ2Э = b12 * c1 * c1 / y212;
            rʹ1 = r1Г * c1;
            rʹk = rʹ1 + rʹʹ2Э;
            xʹ1 = x1 * c1;
            xʹk = xʹ1 + xʹʹ2Э;
            Zʹoo = Math.Sqrt(rʹ1 * rʹ1 + xʹk * xʹk);
            RM = Zʹoo + rʹ1;
            ZM = Math.Sqrt(2 * RM * Zʹoo);
        }
        //определяется λСК
        double function_αδнLoop( double αδ, double Фm, double τi, double t1, double E1xx, double bZ1СР, double hj1, double f2, double lp, double t2, 
            double bz2ср, double δ, double Kδ, out double Hj2, out double μe, out double λСК ) {
            double HZ1, Hj1, Lp, ξ, Kz, Bj2_new; 
            Bj2 = 14e3; //сходимость по Bj2 всегда начинается с данного приближения
            BδM = get_BδM(Фm, αδ, τi, li);
            Bz1 = U1 * BδM * t1 / E1xx / bZ1СР / Kfe1;
            HZ1 = functionMagnetizationStatorToothing(mrkStlStr, Bz1);
            if ( double.IsNaN(HZ1) ) {
                Logging.Add($"Type: {GetType()}  message: Ошибка -> double.IsNaN(HZ1)");
                μe = Hj2 = λСК = double.NaN;
                return double.NaN;
            }
            Bj1 = Фm * U1 / E1xx / 2e-2 / hj1 / li / Kfe1;
            Hj1 = functionMagnetizationStatorBack(mrkStlStr, Bj1);
            if( double.IsNaN(Hj1) ) {
                Logging.Add($"Type: {GetType()}  message: Ошибка -> double.IsNaN(Hj1)");
                μe = Hj2 = λСК = double.NaN;
                return double.NaN;
            }
            Bj2_new = functionBj2(Bj2, f2, Фm, lp, out μe);
            while ( Math.Abs(Bj2_new - Bj2) >= Bj2 * 5e-3 ) {
                Bj2 += 0.1 * (Bj2_new - Bj2);
                Bj2_new = functionBj2(Bj2, f2, Фm, lp, out μe);
            };
            Bz2 = get_Bz2(BδM, t2, li, bz2ср, lp, Kfe2);
            Fδ = 0.16 * BδM * δ * Kδ;
            Fz1 = 0.2 * HZ1 * h1;
            Fj1 = 0.1 * Hj1 * lc;
            Fz2 = 0.2 * functionMagnetization09X17H(Bz2) * (hp + ΔГ2);
            Lp = 0.5 * Math.PI * (Dp - 2 * (hp + ΔГ2) - hʹj2) / p;
            Hj2 = functionMagnetization09X17H(Bj2);
            ξ = FunctionVariabilityInduction(Bj2);
            Fj2 = 0.1 * Hj2 * ξ * Lp;
            ΣF = Fδ + Fz1 + Fz2 + Fj1 + Fj2;
            λСК = bСК * bСК * Fδ / t2 / 11.9 / δ / Kδ / ΣF; //скос
            Kz = (Fδ + Fz1 + Fz2) / Fδ;

            return ((((0.26038319 - 0.027650412 * Kz) * Kz - 0.90870916) * Kz + 1.38167423) * Kz - 0.68171898) * Kz + 0.608124486;
        }
        //только для магнитной цепи и параметров машины, т.к. присутствует hj2
        double functionBj2( double Bj2, double f2, double Ф, double lp, out double μ  ) {
            double hj2 = 0.5 * (Dp - 2 * (hp + ΔГ2) - dв);
            μ = Bj2 / functionMagnetization09X17H(Bj2);
            hʹj2 = 5.65 / Math.Sqrt(μ * 1e-8 * f2 * γ);
            hʹj2 = hʹj2 > hj2 ? hj2 : hʹj2;
            return Ф / 2e-2 / hʹj2 / lp / Kfe2;
        }
        //только для пускового режима
        double functionBj2( double Bj2, double f2, double Ф, double lp, out double μ, out double hʹj2П ) {
            μ = Bj2 / functionMagnetization09X17H(Bj2);
            hʹj2П = 5.65 / Math.Sqrt(μ * 1e-8 * f2 * γ);
            return Ф / 2e-2 / hʹj2П / lp;
        }
        //кривая Арешяна (приложение 7)
        double functionAreshyan( double K ) {
            if ( K <= 0.25 ) return ((29.7619032 - 2.97618736 * K) * K - 16.22648878) * K + 3.22800594;
            if ( K <= 0.7 ) return ((10.28 - 5.6 * K) * K - 6.592) * K + 2.078;
            if ( K <= 1.4 ) return ((0.157142859 - 0.0119047625 * K) * K - 0.403452382) * K + 0.7895;
            return 0.5;
        }
        //кривая намагничивания зубцов статора для сталей 1412, 2411, 2412, 1521 (приложение 2)
        double functionMagnetizationStatorToothing(MarkSteelStator markSteel, double B ) {
            double H = double.NaN, K = B * 1e-4;
            switch ( markSteel ) {
                case MarkSteelStator.Сталь1412:
                {
                    if ( B <= 12e3 ) H = (((2.3958332 - 0.31249987 * K) * K - 1.9499997) * K + 1.6966665) * K + 0.22;
                    else if ( B > 12e3 && B <= 16e3 ) H = (((162.50112 * K - 815.83961) * K + 1547.8881) * K - 1308.8037) * K + 417.35415;
                    else if ( B > 16e3 && B <= 2e4 ) H = (((245.00986 - 31.771664 * K) * K - 291.78204) * K - 395.36079) * K + 593.34809;
                    else H = (((3744.3257 * K - 31929.962) * K + 103689.38) * K - 151173.19) * K + 83206.565;
                }
                break;
                case MarkSteelStator.Сталь2411:
                case MarkSteelStator.Сталь2412:
                {
                    if ( B <= 12e3 ) H = (((8.3854161 * K - 22.541665) * K + 22.339581) * K - 8.7533323) * K + 1.6919998;
                    else if ( B > 12e3 && B <= 16e3 ) H = (((59.582511 * K - 298.91207) * K + 574.14457) * K - 493.62696) * K + 160.34693;
                    else if ( B > 16e3 && B <= 2e4 ) H = (((147.68687 * K - 1165.9466) * K + 3868.3712) * K - 5836.9957) * K + 3250.4895;
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
                case MarkSteelStator.Сталь1412:
                {
                    if ( B <= 11e3 ) H = (((1.3690474 * K - 2.8749994) * K + 3.2130945) * K - 0.80499967) * K + 0.45385709;
                    else if ( B > 11e3 && B <= 15e3 ) H = (((285.00144 * K - 1337.0076) * K + 2364.1148) * K - 1862.6978) * K + 552.30914;
                    else if ( B > 15e3 && B <= 21.7e3 ) H = (((500.83246 * K - 2802.1611) * K + 5912.0784) * K - 5518.8745) * K + 1905.9046;
                    else H = (((16582.576 * K - 155326.54) * K + 546441.37) * K - 853272.75) * K + 498183.62;
                }
                break;
                case MarkSteelStator.Сталь2411:
                case MarkSteelStator.Сталь2412:
                {
                    if ( B <= 11e3 ) H = (((4.6488085 * K - 11.120831) * K + 9.9276157) * K - 3.3231651) * K + 0.69657117;
                    else if ( B > 11e3 && B <= 15e3 ) H = (((645.00313 * K - 3170.3494) * K + 5842.9806) * K - 4780.4825) * K + 1464.9681;
                    else if ( B > 15e3 && B <= 22e3 ) H = (((2074.9974 * K - 13796.652) * K + 34499.221) * K - 38340.31) * K + 15952.594;
                    else H = (((24508.498 * K - 227179.88) * K + 790171.32) * K - 1219851.5) * K + 704416.34;
                }
                break;
                case MarkSteelStator.Сталь1521: H = functionMagnetization1521(B); break;
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
        //кривая намагничивания стали 09X17H (ротор) (приложение 4)
        double functionMagnetization09X17H( double B ) {
            double K = B * 1e-4;
            if ( B <= 8e3 ) return (((375.00006 * K - 841.66683) * K + 706.25015) * K - 246.58339) * K + 33.30001;
            if ( B <= 14e3 ) return (((2832.2887 - 632.81182 * K) * K - 4456.5576) * K + 3026.8298) * K - 749.49908;
            if ( B <= 16e3 ) return (((7751.6083 * K - 53486.287) * K + 139364.06) * K - 160356.69) * K + 68427.602;
            if ( B <= 18.1e3 ) return (((114551.19 * K - 769070.14) * K + 1941650.7) * K - 2181867.6) * K + 920100.89;
            return (B - 16800) / 1.256;
        }
        //коэффициент, учитывающий непостоянство индукции вдоль магнитной силовой линии (приложение 5)
        double FunctionVariabilityInduction( double B ) {
            double Kʹ = B * 1e-4;
            if ( B <= 5e3 ) return 0.65;
            else if ( B <= 14e3 ) return (((2.1164609 - 0.600823044 * Kʹ) * Kʹ - 2.97938271) * Kʹ + 1.72478189) * Kʹ + 0.305448559;
            return ((((0.37798886 * Kʹ - 3.1372993) * Kʹ + 9.538626) * Kʹ - 12.324376) * Kʹ + 4.930866) * Kʹ + 1.4779086;
        }
        //расчетный коэффициент полосного перекрытия (приложение 6)
        //double function_αδ( double Kz ) => ((((0.26038319 - 0.027650412 * Kz) * Kz - 0.90870916) * Kz + 1.38167423) * Kz - 0.68171898) * Kz + 0.608124486;
        //функция 4.2.6
        double function_β0( double arg ) {
            if ( arg <= 2 ) return 0.0555 * arg;
            if ( arg <= 4 ) return 0.0695 * arg - 0.028;
            return (((0.0032291664 - 0.0000937499 * arg) * arg - 0.041874997) * arg + 0.26008332) * arg - 0.30299998;
        }
        double get_BδM( double Ф, double αδ, double τi, double li ) => 100 * Ф / αδ / τi / li;
        double get_Bz2( double BδM, double t2, double li, double bz2ср, double lp, double Kfe2 ) => BδM * t2 * li / bz2ср / lp / Kfe2;
        #endregion
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
