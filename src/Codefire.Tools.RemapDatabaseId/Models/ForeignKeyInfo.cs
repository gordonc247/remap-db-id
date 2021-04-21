namespace Codefire.Tools.RemapDatabaseId.Models
{
    public class ForeignKeyInfo
    {
        public string Name { get; set; }
        public int PrimaryTableId { get; set; }
        public int PrimaryColumnId { get; set; }
        public int ForeignTableId { get; set; }
        public string ForeignSchemaName { get; set; }
        public string ForeignTableName { get; set; }
        public int ForeignColumnId { get; set; }
        public string ForeignColumnName { get; set; }
    }
}
