using System.Globalization;
using System.Text.Json;
using CsvHelper;

public class ScriptGenerator
{
    public Instruction Instruction;
    private List<dynamic> Rows;
    private string Script;

    public ScriptGenerator(string instructionPath, string csvPath)
    {
        string jsonString = File.ReadAllText(instructionPath);
        Instruction = JsonSerializer.Deserialize<Instruction>(jsonString);
        GetRowsFromCsv(csvPath);
    }

    public void GenerateScript()
    {
        if (Instruction.NeedCreateTable) Script = GenerateTableStatement();
        Script += GenerateInsertStatement();
    }

    public void Save()
    {
        string path = $"./{Instruction.TableName}Insert.sql";
        File.WriteAllText(path, Script);
    }

    private string GenerateTableStatement()
    {
        string script = $"CREATE TABLE \"{Instruction.TableName}\" (\n";

        foreach (Column column in Instruction.Columns)
        {
            bool isLastColumn = GetIsLastColumn(column);
            string delimeter = !isLastColumn ?  "," : "";
            script += $"\t \"{column.Name}\" {column.Type}{delimeter}\n";
        }

        return script += ");\n";
    }

    private string GenerateInsertStatement()
    {
        string script = "";
        string scriptTemplate = $"INSERT INTO \"{Instruction.TableName}\" (";

        foreach(Column column in Instruction.Columns)
        {
            bool isLastColumn = GetIsLastColumn(column);
            string delimeter = !isLastColumn ? "," : "";
            scriptTemplate += $"\"{column.Name}\"{delimeter}";
        }

        scriptTemplate += ") VALUES";

        foreach(dynamic row in Rows)
        {
            script += scriptTemplate + " (";
            foreach (Column column in Instruction.Columns)
            {
                script += StringifyRowColumnValue(row, column);
                if (GetIsLastColumn(column)) script += ");\n";
                else script += ",";
            }
        }

        return script;
    }

    private void GetRowsFromCsv(string csvPath)
    {
        using (var reader = new StreamReader(csvPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            Rows = csv.GetRecords<dynamic>().ToList();
        }
    }

    private string StringifyRowColumnValue(dynamic row, Column column)
    {
        string value = "";
        var rowProperties = (IDictionary<string, object>)row;
        string rawValue = (string)rowProperties[column.OriginName];

        if (column.Type == ColumnType.Varchar) value += $"\'{rawValue}\'";
        else if (column.Type == ColumnType.Integer || column.Type == ColumnType.Float) value = rawValue.Replace(',', '.');

        return value;
    }

    private bool GetIsLastColumn(Column column)
    {
        bool isLastColumn = column == Instruction.Columns.Last();
        return isLastColumn;
    }
}