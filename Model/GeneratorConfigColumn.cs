using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CsvInsert.Model
{
	public class GeneratorConfigColumn
	{
        public string Name { get; set; }
        public string OriginName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GeneratorConfigColumnType Type { get; set; }

		public GeneratorConfigColumn()
		{

		}
	}
}

