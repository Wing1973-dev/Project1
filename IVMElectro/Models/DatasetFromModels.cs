using System.Collections.Generic;

namespace IVMElectro.Models {
    public abstract class DatasetFromModels {
        //this collections are intented for keyboard input element 
        public Dictionary<string, double> Dataset { get; set; }
        public abstract void CreationDataset();
        public Dictionary<string, double> GetDataset => Dataset;
    }
}
