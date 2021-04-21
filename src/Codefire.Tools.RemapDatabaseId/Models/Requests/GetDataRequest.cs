namespace Codefire.Tools.RemapDatabaseId.Models.Requests
{
    public class GetDataRequest
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}