using System;
using System.IO;
using log4net;
using log4net.Config;

namespace CsvInsert
{
    class Program
    {
        private static readonly ILog loger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            string sourceFileName = args[0];

            loger.Info("Utility work started");
            string instructionPath = "./instruction.json";
            string csvPath = $"./{sourceFileName}";

            ScriptGenerator generator = new ScriptGenerator(instructionPath, csvPath);
            generator.GenerateScript();
            generator.Save();
            loger.Info("Utility work finished");
        }
    }
}
