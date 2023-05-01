using System;
using CsvInsert.Generator;
using CsvInsert.Reader;

namespace CsvInsert.Model
{
	public class GeneratorConfig : IGeneratorConfig, IReaderConfig
	{
		public string TableName { get; set; }
		public bool NeedCreateTable { get; set; }
		public List<GeneratorConfigColumn> Columns { get; set; }
	}
}

