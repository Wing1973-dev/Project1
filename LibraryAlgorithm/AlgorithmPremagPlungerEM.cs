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
