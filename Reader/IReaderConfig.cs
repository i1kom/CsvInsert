using System;
using CsvInsert.Model;

namespace CsvInsert.Reader
{
	public interface IReaderConfig
	{
		List<GeneratorConfigColumn> Columns { get; set; }
	}
}

