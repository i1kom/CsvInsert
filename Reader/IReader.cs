using System;
namespace CsvInsert.Reader
{
	public interface IReader
	{
		public IEnumerable<Dictionary<string, string>> Read();
	}
}

