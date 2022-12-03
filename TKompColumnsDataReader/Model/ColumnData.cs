namespace TKompColumnsDataReader.Model
{
    public class ColumnData
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }

        public ColumnData(string columnName, string tableName)
        {
            ColumnName = columnName;
			TableName = tableName;
        }
    }
}
