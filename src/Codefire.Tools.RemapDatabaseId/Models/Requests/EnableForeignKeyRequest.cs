namespace Codefire.Tools.RemapDatabaseId.Models.Requests
{
    public class EnableForeignKeyRequest
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ForeignKeyName { get; set; }
    }
}