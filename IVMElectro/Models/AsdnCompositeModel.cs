namespace IVMElectro.Models {
    public class AsdnCompositeModel {
        public AsdnCommonModel Common { get; set; }
        public AsdnRedSingleModel AsdnRedSingle { get; set; }
        public AsdnSingleModel AsdnSingle { get; set; }
    }
}
