namespace Codefire.Tools.RemapDatabaseId.Models.Requests
{
    public class UpdateColumnRequest
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}