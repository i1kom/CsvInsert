using System;
using System.IO;
using CsvInsert.Generator;
using CsvInsert.Model;
using System.Text.Json;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CsvInsert.Reader;


namespace CsvInsert
{
    class Program
    {
        private const string InstructionPath = "./instruction.json"; //need to move to app config
        private const string DebugDefaultPath = "./test.csv";

        static void Main(string[] args)//decompose to separate methods
        {
            var provider = ConfigureProvider(args);
            var logger = provider.GetService<ILogger<Program>>();
            logger.LogInformation("Utility work started");

            logger.LogInformation("Reading data from file");
            var reader = provider.GetService<IReader>();
            var rows = reader.Read();

            logger.LogInformation("Generating SQL script");
            var generator = provider.GetService<IScriptGenerator>();
            string script = generator.GenerateScript(rows);

            logger.LogInformation("Saving script to file");
            var config = provider.GetService<IGeneratorConfig>();
            string newPath = $"./{config.TableName}Insert.sql";
            File.WriteAllText(newPath, script);

            logger.LogInformation("Utility work finished");
        }

        private static IServiceCollection ConfigureCollection(string[] args)
        {
            var csvPath = args.FirstOrDefault();
            if (csvPath == null && System.Diagnostics.Debugger.IsAttached) csvPath = DebugDefaultPath;
            else throw new ArgumentNullException(nameof(csvPath));

            var collection = new ServiceCollection();
            var config = GetGeneratorConfig();

            collection.AddLogging(builder =>
            {
                builder.AddConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.TimestampFormat = "HH:mm:ss ";
                    options.DisableColors = true;
                });
                builder.SetMinimumLevel(LogLevel.Information);
            });

            collection.AddSingleton<IReaderConfig>(config);
            collection.AddSingleton<IGeneratorConfig>(config);
            collection.AddTransient<IScriptGenerator, PostgreScriptGenerator>(
                provider => new PostgreScriptGenerator(provider.GetService<IGeneratorConfig>())
                );
            collection.AddTransient<IReader, CsvReader>(
                provider => new CsvReader(provider.GetService<IReaderConfig>(), csvPath)
                );
            return collection;
        }

        private static IServiceProvider ConfigureProvider(string[] args)
        {
            var collection = ConfigureCollection(args);
            return collection.BuildServiceProvider();
        }

        private static GeneratorConfig GetGeneratorConfig()
        {
            var fileContent = File.ReadAllText(InstructionPath);
            return JsonConvert.DeserializeObject<GeneratorConfig>(fileContent);
        }
    }
}
