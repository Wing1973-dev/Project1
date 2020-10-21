using IVMElectro.Models.Premag;

namespace IVMElectro.Models {
    /// <summary>
    /// Builder manager
    /// </summary>
    class Director {
        /// <summary>
        /// Construction management of EM models
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public AsdnCompositeModel MakeModelEMotors(BuilderEMotors builder) {
            builder.CreateModel();
            builder.SetAsdnCommonModel();
            builder.SetAsdnSingleModel();
            builder.SetAsdnRedSingleModel();
            return builder.AsdnCompositeModel;
        }
        public PremagCompositeModel MakeModelEMagnet(BuilderEMagnet builder) {
            builder.CreateModel();
            builder.SetPremagCommonModel();
            builder.SetPremagFlatArmModel();
            builder.SetPremagPlungerModel();
            return builder.PremagCompositeModel;
        }
    }
}
