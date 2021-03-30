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
            SolutionIsDone = false;
            double Bdelta = Bδ;
            Sзаз = R0 >= R10 ? Math.PI * (R1 - R0) * (R1 + R0) : Math.PI * (R1 - R10) * (R1 + R10);
            Sзаз1 = Math.PI * (R1 - R0) * (R1 + R0);
            Sзаз2 = Math.PI * (R3 - R2) * (R3 + R2);
            Sяр = 2 * Math.PI * hяр * (2 * R1 + R2) / 3;
            Sяк = 2 * Math.PI * hяк * (2 * R1 + R2) / 3;
            lяр = R2 - R1;
            lяк = R2 - R1 + Math.PI * hяк / 2;
            lпол = h + Math.PI * hяр / 4;
            double P1 = 0.1256 * Math.PI * (R3 - R2) * (R3 + R2) / δ;
            double P8v = 0.2512 * R3 * Math.Log(1 + R1 / δ);
            double P12v = 0.5024 * R2 * (Math.Log(R2 - R1) - Math.Log(Math.PI * δ));
            double Pfa = 0.204728 * R3 + 0.409456 * R2 + P8v + P12v;
            double P1_dash = 0.1256 * Math.PI * (R1 - R10) * (R1 + R10) / δ;
            double P8v_dash = 0.2512 * R0 * Math.Log(1 + R1 / δ);
            double P12v_dash = 0.5024 * R1 * (Math.Log(R2 - R1) - Math.Log(Math.PI * δ));
            double Pfi = 0.204728 * R0 + 0.409456 * R1 + P8v_dash + P12v_dash;
            double Pl = 0.1256 * (R2 + R1) * (1.57 * h * 1 / (R2 - R1) - 0.5 * (1 - Math.PI * δ / (R2 - R1)));
            double Pa = (P1 + Pfa) * (P1_dash + Pfi) / (P1_dash + Pfi + P1 + Pfa);
            double Pu = P1 * P1_dash / (P1 + P1_dash);
            ν = (Pa + Pl) / Pu;
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
                Фδ = Bdelta * Sзаз * 0.01;
                Fδ = 0.16 * Bdelta * δ;
                Фяр = ν * Фδ;
                Bяр = 100 * Фяр / Sяр;
                Fяр = 0.1 * lяр * Get_AWH(mrkStl, Bяр);
                Фяк = Pa * Фδ / Pu;
                Bяк = 100 * Фяк / Sяк;
                Fяк = 0.1 * Get_AWH(mrkStl, Bяк) * lяк;
                Фp = Фяк + 2 * Pl * Фδ / (3 * Pu);
                Bp1 = 100 * Фp / Sзаз1;
                Fp1 = 0.1 * lпол * Get_AWH(mrkStl, Bp1);
                Bp2 = 100 * Фp / Sзаз2;
                Fp2 = 0.1 * lпол * Get_AWH(mrkStl, Bp2);
                F = Fδ + Fяр + Fяк + Fp1 + Fp2;

                if (Math.Abs(F - Fм) / Fм > 0.01)
                    Bdelta = F < Fм ? Bdelta * 1.002 : Bdelta * 0.999;
            }
            while (Math.Abs(F - Fм) / Fм > 0.01);
            //сохранить значение Bδ на вывод
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
        public Dictionary<string, double> GetResult => new Dictionary<string, double> { { "Sзаз", Sзаз }, { "Sзаз1", Sзаз1 }, { "Sзаз2", Sзаз2 }, { "Sяр", Sяр },
            { "Sяк", Sяк }, { "lяр", lяр }, { "lяк", lяк }, { "lпол", lпол }, { "ν", ν }, { "lср", lср }, { "ls", ls }, { "r20", r20 }, { "rГ", rГ }, { "I", I },
            { "Fм", Fм }, { "Qм", Qм }, { "Kм", Kм }, { "Фδ", Фδ * 1e-8 }, { "Bδ", Bδ * 1e-4 }, { "Fδ", Fδ }, { "Фяр", Фяр * 1e-8 }, { "Bяр", Bяр * 1e-4 }, { "Fяр", Fяр }, 
            { "Фяк", Фяк * 1e-8 }, { "Bяк", Bяк * 1e-4 }, { "Fяк", Fяк }, { "Фp", Фp * 1e-8 }, { "Bp1", Bp1 * 1e-4 }, { "Bp2", Bp2 * 1e-4 }, { "Fp1", Fp1 }, { "Fp2", Fp2 }, 
            { "F", F }, { "Wp", Wp }, { "Fтм", Fтм / 9.81 }, { "P", P }, { "Δt", Δt }, { "Kt", Kt } };
        //readiness of data for output to the interface
        public bool SolutionIsDone { get; private set; }
    }
}
