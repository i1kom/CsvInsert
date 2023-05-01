using System;
using CsvInsert.Model;

namespace CsvInsert.Generator
{
	public interface IGeneratorConfig
	{
        public string TableName { get; set; }
        public bool NeedCreateTable { get; set; }
        public List<GeneratorConfigColumn> Columns { get; set; }
    }
}

