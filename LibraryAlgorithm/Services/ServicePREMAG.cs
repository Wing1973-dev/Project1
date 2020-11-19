using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAlgorithms.Services {
    class ServicePREMAG {
        //материал статора
        public enum MarkSteel {
            st09Х17Н,
            st3st10,
            st10880_Э10
        }
        public static MarkSteel mrkStl;
        /// <summary>
        /// Напряженность магнитного поля стальных участков магнитной цепи ЭМ
        /// </summary>
        /// <param name="mrkstl">сталь</param>
        /// <param name="B">магнитная индукция</param>
        /// <returns></returns>
        public static double Get_AWH(MarkSteel mrkstl, double B) {
            return mrkstl switch
            {
                MarkSteel.st09Х17Н => Steel09X17H(B, 0.0001 * B),
                MarkSteel.st3st10 => Steel3(B, 0.0001 * B),
                MarkSteel.st10880_Э10 => Steel10880(B, 0.0001 * B),
                _ => double.NaN,
            };
        }
        /// <summary>
        /// Kривая намагничивания стали 09X17H
        /// </summary>
        /// <param name="B">магнитная индукция</param>
        /// <param name="H_dash">редуцированная магнитная индукция</param>
        /// <returns>напряженность магнитного поля</returns>
        static double Steel09X17H(double B, double H_dash) {
            if (B <= 8000)
                return (((375.00006 * H_dash - 841.66683) * H_dash + 706.25015) * H_dash - 246.58339) * H_dash + 33.300009;
            else if (B > 8000 && B <= 14000)
                return (((2832.2887 - 632.81182 * H_dash) * H_dash - 4456.5576) * H_dash + 3036.8298) * H_dash - 749.49908;
            else if (B > 14000 && B <= 16000)
                return (((7751.6083 * H_dash - 53486.287) * H_dash + 139364.06) * H_dash - 160356.69) * H_dash + 68427.602;
            else if (B > 16000 && B <= 18100)
                return ((114551.19 * H_dash - 769070.14) * H_dash + 1941650.7) * H_dash + 920100.89;
            return (B - 16800) / 1.256;
        }
        /// <summary>
        /// Kривая намагничивания стали 3
        /// </summary>
        /// <param name="B">магнитная индукция</param>
        /// <param name="H_dash">редуцированная магнитная индукция</param>
        /// <returns>напряженность магнитного поля</returns>
        static double Steel3(double B, double H_dash) {
            if (B <= 8000)
                return ((((605.28273 - 259.672615 * H_dash) * H_dash - 499.999993) * H_dash + 164.181545) * H_dash - 6.76309502) * H_dash;
            else if (B > 8000 && B <= 12000)
                return ((((1.07869299 * H_dash + 59.342635) * H_dash - 156.544738) * H_dash + 134.55467) * H_dash - 30.1553654) * H_dash;
            else if (B > 12000 && B <= 16000)
                return ((((41.8236051 * H_dash - 116.767556) * H_dash + 81.1313861) * H_dash + 37.755009) * H_dash - 36.41963) * H_dash;
            else if (B > 16000 && B <= 18100)
                return ((((7925.13615 * H_dash - 53369.2427) * H_dash + 135112.58) * H_dash - 152253.377) * H_dash + 64406.9423) * H_dash;
            return 700 * H_dash - 1150;
        }
        /// <summary>
        /// Kривая намагничивания стали 10880
        /// </summary>
        /// <param name="B">магнитная индукция</param>
        /// <param name="H_dash">редуцированная магнитная индукция</param>
        /// <returns>напряженность магнитного поля</returns>
        static double Steel10880(double B, double H_dash) {
            if (B <= 9000)
                return ((((54.9281493 * H_dash - 148.29009) * H_dash + 149.506667) * H_dash - 68.9346993) * H_dash + 15.7837241) * H_dash;
            else if ((B > 9000) && (B <= 12000))
                return ((((1463.92314 * H_dash - 6233.67889) * H_dash + 9920.74071) * H_dash - 6991.70086) * H_dash + 1843.38052) * H_dash;
            else if ((B > 12000) && (B <= 14000))
                return ((((11189.256 - 2152.42409 * H_dash) * H_dash - 21779.4553) * H_dash + 18817.5919) * H_dash - 6087.46256) * H_dash;
            else if ((B > 14000) && (B <= 16000))
                return ((((39455.7061 - 6558.02269 * H_dash) * H_dash - 88824.7095) * H_dash + 88719.7173) * H_dash - 33180.3314) * H_dash;
            else if ((B > 16000) && (B <= 18000))
                return ((((132824.459 - 19741.5133 * H_dash) * H_dash - 334439.525) * H_dash + 373665.793) * H_dash - 156359.211) * H_dash;
            return 430 * H_dash - 687;
        }
        public static double Get_TEPLO(double Kt1) {
            if (Kt1 <= 0.02)
                return 0.0008 + 0.006 * Kt1;
            else if ((Kt1 > 0.02) && (Kt1 <= 0.04))
                return 0.00092 + (Kt1 - 0.02) * 0.0055;
            else if ((Kt1 > 0.04) && (Kt1 <= 0.06))
                return 0.00103 + (Kt1 - 0.04) * 0.0045;
            else if ((Kt1 > 0.06) && (Kt1 <= 0.08))
                return 0.00112 + (Kt1 - 0.06) * 0.003;
            else if ((Kt1 > 0.08) && (Kt1 <= 0.1))
                return 0.00118 + (Kt1 - 0.08) * 0.0025;
            return 0.00123 + (Kt1 - 0.1) * 0.002;
        }

    }
}
