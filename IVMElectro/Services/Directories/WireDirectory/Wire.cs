using System.Data;

namespace IVMElectro.Services.Directories.WireDirectory {
    public abstract class Wire {
        public int Id { get; set; }
        public string NameWire { get; set; }
        public DataTable Table { get; private set; } 
        public Wire() {
            Table = new DataTable();
            DataColumn dиз = new DataColumn("dиз, мм", typeof(string)), qГ = new DataColumn("qГ, мм²", typeof(string)),
                ID = new DataColumn("№пп", typeof(int)) {
                    Unique = true,
                    AllowDBNull = false,
                    AutoIncrement = true,
                    AutoIncrementSeed = 1,
                    AutoIncrementStep = 1
                };
            Table.Columns.AddRange(new DataColumn[] { ID, dиз, qГ }); Table.PrimaryKey = new DataColumn[] { Table.Columns["ID_culc"] };
        }
        public abstract void CreateTable();
        protected void AddRow(object[] content) {
            DataRow row = Table.NewRow();
            row.ItemArray = content;
            Table.Rows.Add(row);
        }
    }
}
