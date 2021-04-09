using System;
using System.Collections.Generic;
using static LibraryAlgorithms.Services.ServicePREMAG;

namespace LibraryAlgorithms {
    public class AlgorithmPremagFlatEM {
        // input data
        double U, δ, Bδ, q, ρx, ρГ, h, R1, R2, R3, hяр, hяк, R0, R10, dпз1, dвст, Δk1, qm, Ws;
        //output data
        double Sзаз, Sзаз1, Sзаз2, Sяр, Sяк, lяр, lяк, lпол, ν, lср, ls, r20, rГ, I, Fм, Qм, Kм, Фδ,
            Fδ, Фяр, Bяр, Fяр, Фяк, Bяк, Fяк, Фp, Bp1, Bp2, Fp1, Fp2, F, Wp, Fтм, P, Δt, Kt;
        
        public AlgorithmPremagFlatEM(Dictionary<string, double> inputData) {
            Bδ = inputData["Bδ"] * 1e4; //Тл -> Гс
            U = inputData["U"]; δ = inputData["δ"];  q = inputData["q"]; ρx = inputData["ρx"]; ρГ = inputData["ρГ"];
            h = inputData["h"]; R1 = inputData["R1"]; R2 = inputData["R2"]; R3 = inputData["R3"]; hяр = inputData["hяр"]; hяк = inputData["hяк"];
            R0 = inputData["R0"]; R10 = inputData["R10"]; dпз1 = inputData["dпз1"]; dвст = inputData["dвст"]; Δk1 = inputData["Δk1"]; qm = inputData["qm"]; 
            Ws = inputData["Ws"];
            if (inputData["markSteel"] == 0) mrkStl = MarkSteel.st09Х17Н;
            else if (inputData["markSteel"] == 1) mrkStl = MarkSteel.st3st10;
            else if (inputData["markSteel"] == 2) mrkStl = MarkSteel.st10880_Э10;

            SolutionIsDone = false;
        }
        //the calculation is performed in Gauss (B - magnetic induction)
        public void Run() {
            
            SolutionIsDone = true;
        }
        //result of the calculation
        //B: Гс -> Тл
        //Fтм: кг -> Н
        //Ф: Мкс -> Вб
        public Dictionary<string, double> GetResult => new Dictionary<string, double> { { "Sзаз", Sзаз }, { "Sзаз1", Sзаз1 }, { "Sзаз2", Sзаз2 }, { "Sяр", Sяр },
            { "Sяк", Sяк }, { "lяр", lяр }, { "lяк", lяк }, { "lпол", lпол }, { "ν", ν }, { "lср", lср }, { "ls", ls }, { "r20", r20 }, { "rГ", rГ }, { "I", I },
            { "Fм", Fм }, { "Qм", Qм }, { "Kм", Kм }, { "Фδ", Фδ * 1e-8 }, { "Bδ", Bδ * 1e-4 }, { "Fδ", Fδ }, { "Фяр", Фяр * 1e-8 }, { "Bяр", Bяр * 1e-4 }, { "Fяр", Fяр }, 
            { "Фяк", Фяк * 1e-8 }, { "Bяк", Bяк * 1e-4 }, { "Fяк", Fяк }, { "Фp", Фp * 1e-8 }, { "Bp1", Bp1 * 1e-4 }, { "Bp2", Bp2 * 1e-4 }, { "Fp1", Fp1 }, { "Fp2", Fp2 }, 
            { "F", F }, { "Wp", Wp }, { "Fтм", Fтм / 9.81 }, { "P", P }, { "Δt", Δt }, { "Kt", Kt } };
        //readiness of data for output to the interface
        public bool SolutionIsDone { get; private set; }
    }
}
