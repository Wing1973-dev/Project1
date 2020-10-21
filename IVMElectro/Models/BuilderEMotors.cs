namespace IVMElectro.Models {
    /// <summary>
    /// Electric motors
    /// </summary>
    abstract class BuilderEMotors {
        public AsdnCompositeModel AsdnCompositeModel { get; private set; }
        public void CreateModel() => AsdnCompositeModel = new AsdnCompositeModel();
        public abstract void SetAsdnCommonModel();
        public abstract void SetAsdnSingleModel();
        public abstract void SetAsdnRedSingleModel();
    }
    class AsdnSingleBuilder : BuilderEMotors {
        public override void SetAsdnCommonModel() => AsdnCompositeModel.Common = new AsdnCommonModel();
        public override void SetAsdnSingleModel() => AsdnCompositeModel.AsdnSingle = new AsdnSingleModel();
        public override void SetAsdnRedSingleModel() => AsdnCompositeModel.AsdnRedSingle = null;
    }
    class AsdnRedSingleBuilder : BuilderEMotors {
        public override void SetAsdnCommonModel() => AsdnCompositeModel.Common = new AsdnCommonModel();
        public override void SetAsdnSingleModel() => AsdnCompositeModel.AsdnSingle = null;
        public override void SetAsdnRedSingleModel() => AsdnCompositeModel.AsdnRedSingle = new AsdnRedSingleModel();
    }
}
