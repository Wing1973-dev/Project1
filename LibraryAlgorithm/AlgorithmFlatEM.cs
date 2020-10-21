using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LibraryAlgorithms {
    public class AlgorithmFlatEM {
        // input data
        double U, δ, Bδ, q, ρx, ρГ, h, R1, R2, R3, hяр, hяк, R0, R10, dпз1, dвст, Δk1, qm, Ws;
        //output data
        double Sзаз, Sзаз1, Sзаз2, Sяр, Sяк, lяр, lяк, lпол, ν, lср, ls, r20, rГ, I, Fм, Qм, Kм;
        //материал статора
        private enum MarkSteel {
            st09Х17Н,
            st3st10,
            st10880_Э10
        }
        private MarkSteel mrkStl;
        public AlgorithmFlatEM(Dictionary<string, double> inputData, int markSteel) {
            if (markSteel == 0) mrkStl = MarkSteel.st09Х17Н;
            else if (markSteel == 1) mrkStl = MarkSteel.st3st10;
            else if (markSteel == 2) mrkStl = MarkSteel.st10880_Э10;
            U = inputData["U"]; δ = inputData["δ"]; Bδ = inputData["Bδ"]; q = inputData["q"]; ρx = inputData["ρx"]; ρГ = inputData["ρГ"];
            h = inputData["h"]; R1 = inputData["R1"]; R2 = inputData["R2"]; R3 = inputData["R3"]; hяр = inputData["hяр"]; hяк = inputData["hяк"];
            R0 = inputData["R0"]; R10 = inputData["R10"]; dпз1 = inputData["dпз1"]; dвст = inputData["dвст"]; Δk1 = inputData["Δk1"]; qm = inputData["qm"]; 
            Ws = inputData["Ws"];
        }
        public void Run() {
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
        }
    }
}
