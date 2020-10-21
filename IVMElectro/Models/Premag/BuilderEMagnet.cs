namespace IVMElectro.Models.Premag {
    /// <summary>
    /// Electromagnet
    /// </summary>
    abstract class BuilderEMagnet {
        public PremagCompositeModel PremagCompositeModel { get; private set; }
        public void CreateModel() => PremagCompositeModel = new PremagCompositeModel();
        public abstract void SetPremagCommonModel();
        public abstract void SetPremagFlatArmModel();
        public abstract void SetPremagPlungerModel();
    }
    class PremagFlatArmBuilder : BuilderEMagnet {
        public override void SetPremagCommonModel() => PremagCompositeModel.Common = new PremagCommonModel();
        public override void SetPremagFlatArmModel() => PremagCompositeModel.FlatArm = new PremagFlatArmModel();
        public override void SetPremagPlungerModel() => PremagCompositeModel.Plunger = null;
    }
    class PremagPlungerBuilder : BuilderEMagnet {
        public override void SetPremagCommonModel() => PremagCompositeModel.Common = new PremagCommonModel();
        public override void SetPremagFlatArmModel() => PremagCompositeModel.FlatArm = null;
        public override void SetPremagPlungerModel() => PremagCompositeModel.Plunger = new PremagPlungerModel();
    }
}
