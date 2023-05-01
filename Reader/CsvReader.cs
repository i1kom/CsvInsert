using System;
using CsvInsert.Generator;
using System.Globalization;

namespace CsvInsert.Reader
{
	public class CsvReader : IReader
	{
		IReaderConfig Config { get; set; }
		string FilePath { get; set; }
		public CsvReader(IReaderConfig config, string filePath)
		{
			Config = config;
			FilePath = filePath;
		}

		public IEnumerable<Dictionary<string, string>> Read()
		{
			using var reader = new StreamReader(FilePath);
			using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
			{
                return ReadRows(csv).ToList();
            };
		}

		private IEnumerable<Dictionary<string, string>> ReadRows(CsvHelper.IReader reader)
		{
			return reader.GetRecords<dynamic>().Select(ReadRow);
		}


		private Dictionary<string, string> ReadRow(dynamic record)
		{
			Dictionary<string, string> dic = new Dictionary<string, string>();

			foreach (var property in record)
			{
				var column = Config.Columns.Find(column => column.OriginName == property.Key);
                if (column != null)
				{
					dic.Add(column.Name, property.Value);
				}
			}

			return dic;
		}
    }
}

