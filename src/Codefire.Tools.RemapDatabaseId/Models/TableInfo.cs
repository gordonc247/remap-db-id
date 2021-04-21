namespace Codefire.Tools.RemapDatabaseId.Models
{
    public class TableInfo
    {
        public int Id { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public int PrimaryKeyId { get; set; }
        public string PrimaryKeyName { get; set; }
        public string PrimaryKeyType { get; set; }
    }
}
