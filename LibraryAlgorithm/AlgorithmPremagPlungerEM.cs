using System;
using System.Collections.Generic;
using static LibraryAlgorithms.Services.ServicePREMAG;

namespace LibraryAlgorithms {
    public class AlgorithmPremagPlungerEM {
        // input data
        double U, δ, Bδ, q, ρx, ρГ, h, R1, R2, R3, hфл, R0, R10, R110, R1110, dпз1, dпз2, dвст, Δk1, qm, Ws, l1, l2, α;
        //output data
        double δэ, S1, S11, S12, Sстоп, Sкор, Sпз, lплст, lк, lфл, ν1, ν2, lср, ls, r20, rГ, I, Fм, Qм, Kм, Фδ,
            Fδ, Фp, Bp1, Bp11, Bp12, Fp, Фк, Bк, Fк, Fфл, Фпз, Bпз, Fпз, F, Wp, Fтм, P, Kt, Δt;
        public AlgorithmPremagPlungerEM(Dictionary<string, double> inputData) {
            Bδ = inputData["Bδ"] * 1e4; //Тл -> Гс
            U = inputData["U"]; δ = inputData["δ"]; q = inputData["q"]; ρx = inputData["ρx"]; ρГ = inputData["ρГ"];
            h = inputData["h"]; R1 = inputData["R1"]; R2 = inputData["R2"]; R3 = inputData["R3"]; hфл = inputData["hфл"];
            R0 = inputData["R0"]; R10 = inputData["R10"]; R110 = inputData["R110"]; R1110 = inputData["R1110"];
            dпз1 = inputData["dпз1"]; dпз2 = inputData["dпз2"]; dвст = inputData["dвст"]; Δk1 = inputData["Δk1"]; qm = inputData["qm"];
            Ws = inputData["Ws"]; l1 = inputData["l1"]; l2 = inputData["l2"]; α = inputData["α"];
            if (inputData["markSteel"] == 0) mrkStl = MarkSteel.st09Х17Н;
            else if (inputData["markSteel"] == 1) mrkStl = MarkSteel.st3st10;
            else if (inputData["markSteel"] == 2) mrkStl = MarkSteel.st10880_Э10;

            SolutionIsDone = false;
        }
        public void Run() {
            SolutionIsDone = false;
            double Bdelta = Bδ;
            S1 = Math.PI * (R1 - R10) * (R1 + R10);
            S11 = Math.PI * (R1 - R110) * (R1 + R110);
            S12 = Math.PI * (R1 - R1110) * (R1 + R1110);
            Sстоп = Math.PI * (R1 - R0) * (R1 + R0);
            Sкор = Math.PI * (R3 - R2) * (R3 + R2);
            Sпз = 2 * Math.PI * R1 * hфл;
            δэ = δ * Math.Cos(α * Math.PI / 180) * Math.Cos(α * Math.PI / 180);
            lплст = h + Math.PI * hфл / 2 - δэ - l1 - l2;
            lк = h + Math.PI * (R3 - R2) / 2;
            lфл = 2 * (R2 - R1);
            double p_delta = R0 > R10 ? Math.PI * 0.04 * Sстоп / δэ : Math.PI * 0.04 * S1 / δэ;
            double p7 = 0.204728 * (R1 + 0.25 * δэ);
            double p8v = 0.2512 * Math.Log(4 * (R2 - R1) / Math.PI / δэ) * R1;
            double p8v_dash = 0;
            if (R10 > R2 - R1)
                p8v_dash = p8v * R10 / R1;
            else if (R10 > 0)
                p8v_dash = 0.2512 * Math.Log(4 * R10 / Math.PI / δэ) * R10;
            double p11 = 0.204728 * (R10 + 0.25 * δэ);
            double p15a = 0.04 * Math.PI * (0.125 * Math.PI * h * (R2 + R1) / (R2 - R1) - R2 - R1 + 2 * (R2 + R1) * (R2 - R1) / Math.PI / h);
            double p16_dash = 0.41448 * (R1 + 0.5 * dпз1);
            double p16_ddash = 0.41448 * (R1 + 0.5 * dпз2);
            double p18a_dash = 0.5024 * (R1 + Math.Sqrt(dпз1 * (dвст + dпз1)) * Math.Log((dвст + dпз1) / dпз1));
            double p18a_ddash = 0.5024 * (R1 + Math.Sqrt(dпз2 * (dвст + dпз2)) * Math.Log((dвст + dпз2) / dпз2));
            double ppz1 = 0.78917 * R1 * hфл / dпз1;
            double ppz2 = 0.78917 * R1 * hфл / dпз2;
            double ppz_dash = ppz1 + p16_dash + p18a_dash;
            double ppz_ddash = ppz2 + p16_ddash + p18a_ddash;
            double p_delta_dash = p_delta + p7 + p11 + p8v_dash;
            double pa = p_delta_dash * ppz_dash * ppz_ddash / (p_delta_dash * ppz_dash + ppz_dash * ppz_ddash + p_delta_dash * ppz_ddash);
            ν1 = (pa + p8v) * ppz_dash * ppz_ddash / (ppz_dash * ppz_ddash - pa * (ppz_ddash + ppz_dash)) / p_delta;
            ν2 = (pa + p8v + p15a) * ppz_dash * ppz_ddash / (ppz_dash * ppz_ddash - pa * (ppz_ddash + ppz_dash)) / p_delta;
            double Rsr = 0.5 * (R2 + R1 + Δk1 + dпз1 + dвст) - 0.25;
            lср = 2 * Math.PI * Rsr;
            ls = 2 * Math.PI * Rsr * Ws;
            r20 = ρx * 0.001 * ls / qm;
            rГ = ρГ * r20 / ρx;
            I = U / rГ;
            Fм = I * Ws;
            Qм = qm * Ws;
            double Swindow = h * (R2 - R1 - dпз1 - dвст);
            Kм = Qм / Swindow;
            do {
                Фδ = R0 <= R10 ? 0.01 * Bdelta * S1 : 0.01 * Bdelta * Sстоп;
                Fδ = 0.08 * Bdelta * δэ;
                Фp = ν1 * Фδ;
                Bp1 = 100 * Фp / S1;
                Bp11 = 100 * Фp / S11;
                Bp12 = 100 * Фp / S12;
                Fp = 0.1 * (Get_AWH(mrkStl, Bp1) * l1 + Get_AWH(mrkStl, Bp11) * l2 + Get_AWH(mrkStl, Bp12) * lплст);
                Фк = ν2 * Фδ;
                Bк = 100 * Фк / Sкор;
                Fк = 0.1 * Get_AWH(mrkStl, Bк) * lк;
                Fфл = 0.1 * Get_AWH(mrkStl, Bк) * lфл;
                Фпз = ν2 * Фδ;
                Bпз = 100 * Фпз / Sпз;
                Fпз = 0.08 * Bпз * (dпз1 + dпз2);
                F = Fδ + Fp + Fк + Fфл + Fпз;
                if (Math.Abs(F - Fм) / Fм > 0.01)
                    Bdelta = F < Fм ? Bdelta * 1.002 : Bdelta * 0.999;
            } while (Math.Abs(F - Fм) / Fм > 0.01);
            Bδ = Bdelta;
            Wp = 5.1E-8 * Фδ * Fδ;
            Fтм = 10 * Wp / δ;
            P = U * I;
            double Scool = 2 * Math.PI * h * (R2 + R1 + dпз1 + dвст);
            Scool += 2 * Math.PI * (R2 * R2 - (R1 + dпз1 + dвст) * (R1 + dпз1 + dвст));
            double kt1 = P * q * 100 / Scool;
            Kt = Get_TEPLO(kt1);
            Δt = kt1 / Kt;
            SolutionIsDone = true;
        }
        //result of the calculation
        //B: Гс -> Тл
        //Fтм: кг -> Н
        //Ф: Мкс -> Вб
        public Dictionary<string, double> GetResult => new Dictionary<string, double> { { "δэ", δэ }, { "S1", S1 }, { "S11", S11 }, { "S12", S12 },
        { "Sстоп", Sстоп }, { "Sкор", Sкор },  { "Sпз", Sпз }, { "lпл.ст", lплст }, { "lк", lк }, { "lфл", lфл }, { "ν1", ν1 }, { "ν2", ν2 },
        { "lср", lср }, { "ls", ls }, { "r20", r20 }, { "rГ", rГ }, { "I", I }, { "Fм", Fм }, { "Qм", Qм }, { "Kм", Kм }, { "Фδ", Фδ * 1e-8 },
        { "Bδ", Bδ * 1e-4 }, { "Fδ", Fδ }, { "Фp", Фp * 1e-8 }, { "Bp1", Bp1 * 1e-4 }, { "Bp11", Bp11 * 1e-4 }, { "Bp12", Bp12 * 1e-4 }, { "Fp", Fp },
        { "Фк", Фк * 1e-8 }, { "Bк", Bк * 1e-4 }, { "Fк", Fк }, { "Fфл", Fфл }, { "Фпз", Фпз * 1e-8 }, { "Bпз", Bпз * 1e-4 }, { "Fпз", Fпз }, { "F", F },
        { "Wp", Wp }, { "Fтм", Fтм }, { "P", P }, { "Kt", Kt }, { "Δt", Δt } };
        //readiness of data for output to the interface
        public bool SolutionIsDone { get; private set; }
    }
}
