namespace IVMElectro.Models {
    public class SingleValueEventArgs {
        internal SingleValueEventArgs(double value) => Value = value;
        internal double Value { get; }
    }
}
