using System;
using CsvInsert.Model;

namespace CsvInsert.Generator
{
	public class PostgreScriptGenerator : SqlGenerator, IScriptGenerator
	{
        public IGeneratorConfig Config { get; }
		public PostgreScriptGenerator(IGeneratorConfig config)
		{
            Config = config;
		}

        public string GenerateScript(IEnumerable<Dictionary<string, string>> data)
        {
            string script = string.Empty;
            if (Config.NeedCreateTable) script = GetCreateTableStatement(Config.Columns, Config.TableName) + DoubleLineFeed;
            script += GetInsertStatement(Config.Columns, Config.TableName, data);
            return script;
        }

        protected override string GetInsertStatement(IEnumerable<GeneratorConfigColumn> columns,
            string tableName, IEnumerable<Dictionary<string, string>> data)
        {
            string insertTemplate = $"{InsertKeyword} {IntoKeyword} {GetTableSql(tableName)} {GetColumnsToInsertSql(columns)} {ValuesKeyword}";
            string rows = string.Join(CloseTransactionChar + LineFeed, data.Select(row => GetRowToInsertSql(row, columns, insertTemplate)));
            return rows + CloseTransactionChar;
        }

        protected override string GetRowToInsertSql(Dictionary<string, string> data, IEnumerable<GeneratorConfigColumn> columns, string template)
        {
            List<string> values = new List<string>(); 
            foreach (GeneratorConfigColumn column in columns)
            {
                values.Add(FormatValueToInsert(data[column.Name], column.Type));
            }
            return template + GetParenthesizedList(values, ArgumentSeparator);
        }

        private string FormatValueToInsert(string value, GeneratorConfigColumnType type)
        {
            return type switch
            {
                GeneratorConfigColumnType.Varchar50 => $"\'{value}\'",
                GeneratorConfigColumnType.Integer => value,
                GeneratorConfigColumnType.Float => value.Replace(",", ".")
            };
        }


        protected override string GetTableSql(string tableName)
        {
            return $"\"{tableName}\"";
        }


        protected override string GetColumnSql(string columnName)
        {
            return $"\"{columnName}\"";
        }
    }
}

