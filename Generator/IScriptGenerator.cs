using System;
namespace CsvInsert.Generator
{
	public interface IScriptGenerator
	{
		public string GenerateScript(IEnumerable<Dictionary<string, string>> data);
	}
}
