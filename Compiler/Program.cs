using System;
using System.IO;
using System.Reflection;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("ENTER SOURCE CODE PATH: ");
            //var path = @"C:\Users\Dell\source\repos\Compiler\test_code.txt";

            string testFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"test_code.txt");
            string outputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output.txt");
            string outputFileCsv = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output.csv");

            var sourceCode = File.ReadAllText(testFile);

            var lexicalAnalyzer = new LexicalAnalyzer();
            var tokens = lexicalAnalyzer.Analyze(sourceCode);

            var fileText = "LINE NUMBER,CLASS NAME,VALUE\r\n";
            foreach (var token in tokens)
            {
                fileText += token.ToCSVString();
            }
            File.WriteAllText(outputFileCsv, fileText);
            fileText = "";
            foreach (var token in tokens)
            {
                fileText += token.ToString();
            }
            File.WriteAllText(outputFile, fileText);

        }
    }
}
