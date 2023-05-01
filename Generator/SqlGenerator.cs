using System;
using System.Collections.Generic;
using CsvInsert.Model;

namespace CsvInsert.Generator
{
	public abstract class SqlGenerator
	{
        public const string CreateKeyword = "CREATE";
        public const string TableKeyword = "TABLE";
        public const string InsertKeyword = "INSERT";
        public const string IntoKeyword = "INTO";
        public const string ValuesKeyword = "VALUES";
        public const string ArgumentSeparator = ", ";
		public const string LineFeed = "\n";
        public const string DoubleLineFeed = "\n\n";
        public const string CloseTransactionChar = ";";
        public const string Tabulation = "\t";


        public virtual string GetCreateTableStatement(IEnumerable<GeneratorConfigColumn> columns, string tableName)
		{
			return $"{CreateKeyword} {TableKeyword} {GetTableSql(tableName)} {GetColumnsToCreateTable(columns)}{CloseTransactionChar}";
		}

        protected abstract string GetInsertStatement(IEnumerable<GeneratorConfigColumn> columns, string tableName, IEnumerable<Dictionary<string, string>> data);


        protected virtual string GetColumnsToCreateTable(IEnumerable<GeneratorConfigColumn> columns) 
        {
			IEnumerable<string> formatedColumns = columns.Select(column => GetColumnTypePair(column.Name, column.Type));
            string separator = ArgumentSeparator + LineFeed;
            return GetParenthesizedList(formatedColumns, separator, true);
        }


        protected virtual string GetColumnsToInsertSql(IEnumerable<GeneratorConfigColumn> columns)
        {
            IEnumerable<string> formatedColumns = columns.Select(column => GetColumnSql(column.Name));
            return GetParenthesizedList(formatedColumns, ArgumentSeparator);
        }


		protected abstract string GetRowToInsertSql(Dictionary<string, string> row, IEnumerable<GeneratorConfigColumn> columns, string template);


		protected virtual string GetParenthesizedList(IEnumerable<string> args, string separator, bool addParenthesesLineBreak = false)
		{
			string lineFeed = addParenthesesLineBreak ? LineFeed : "";
			return $"({lineFeed}{string.Join(separator, args)}{lineFeed})";
		}


        protected virtual string GetColumnTypeSql(GeneratorConfigColumnType type)
        {
            return type switch
            {
                GeneratorConfigColumnType.Varchar50 => "VARCHAR(50)",
                GeneratorConfigColumnType.Integer => type.ToString().ToUpper(),
                GeneratorConfigColumnType.Float => type.ToString().ToUpper()
            };
        }


        protected virtual string GetColumnTypePair(string column, GeneratorConfigColumnType type)
		{
			return $"{Tabulation}{GetColumnSql(column)} {GetColumnTypeSql(type)}";
		}


        protected virtual string GetTableSql(string tableName)
        {
            return tableName;
        }


        protected virtual string GetColumnSql(string columnName)
		{
			return columnName;
		}
	}
}

