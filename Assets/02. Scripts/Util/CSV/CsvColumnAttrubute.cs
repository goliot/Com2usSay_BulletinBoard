using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CsvColumnAttribute : Attribute
{
    public string ColumnName { get; }

    public CsvColumnAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}