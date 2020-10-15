using System;
using System.Collections.Generic;
using System.Text;

namespace IVMElectro.Models {
    /// <summary>
    /// Electric motors
    /// </summary>
    abstract class BuilderEM {
        public AsdnCompositeModel AsdnCompositeModel { get; private set; }
        public void CreateModel() => AsdnCompositeModel = new AsdnCompositeModel();
        public abstract void SetAsdnCommonModel();
        public abstract void SetAsdnSingleModel();
        public abstract void SetAsdnRedSingleModel();
    }
    class AsdnSingleBuilder : BuilderEM {
        public override void SetAsdnCommonModel() => AsdnCompositeModel.Common = new AsdnCommonModel();
        public override void SetAsdnRedSingleModel() => AsdnCompositeModel.AsdnRedSingle = null;
        public override void SetAsdnSingleModel() => AsdnCompositeModel.AsdnSingle = new AsdnSingleModel();
    }
    class AsdnRedSingleBuilder : BuilderEM {
        public override void SetAsdnCommonModel() => AsdnCompositeModel.Common = new AsdnCommonModel();
        public override void SetAsdnRedSingleModel() => AsdnCompositeModel.AsdnRedSingle = new AsdnRedSingleModel();
        public override void SetAsdnSingleModel() => AsdnCompositeModel.AsdnSingle = null;
    }

    class Director {
        public AsdnCompositeModel MakeModelEM(BuilderEM builder) {
            builder.CreateModel();
            builder.SetAsdnCommonModel();
            builder.SetAsdnSingleModel();
            builder.SetAsdnRedSingleModel();
            return builder.AsdnCompositeModel;
        }
    }
}
