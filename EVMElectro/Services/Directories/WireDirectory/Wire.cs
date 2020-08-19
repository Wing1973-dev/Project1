using System.Data;

namespace IVMElectro.Services.Directories.WireDirectory {
    abstract class Wire {
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal DataTable Table { get; private set; } 
        public Wire() {
            Table = new DataTable();
            DataColumn dиз = new DataColumn("dиз, мм", typeof(string)), qГ = new DataColumn("qГ, мм²", typeof(string));
            Table.Columns.AddRange(new DataColumn[] { dиз, qГ });
        }
        public abstract void CreateTable();
        protected void AddRow(object[] content) {
            DataRow row = Table.NewRow();
            row.ItemArray = content;
            Table.Rows.Add(row);
        }
    }
}
