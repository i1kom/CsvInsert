public class Column{
    public string Name { get; set; }
    public string OriginName { get; set; }
    public string Type { get; set; }

    public Column() { }

    public Column(string name, string originName, string type)
    {
        Name = name;
        OriginName = originName;
        Type = type;
    }
    public Column(string name, string type)
    {
        Name = name;
        OriginName = name;
        Type = type;
    }
}

public static class ColumnType
{
    public const string Varchar = "varchar";
    public const string Integer = "integer";
    public const string Float = "float";
}
