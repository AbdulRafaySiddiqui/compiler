using System;
using System.IO;
using System.Reflection;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string testFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"test_code.txt");
            string outputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output.txt");
            string outputFileCsv = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output.csv");

            var sourceCode = File.ReadAllText(testFile);

            var lexicalAnalyzer = new LexicalAnalyzer();
            var tokens = lexicalAnalyzer.Analyze(sourceCode);

            var syntaxAnalyzer = new SyntaxAnalyzer();
            var token = syntaxAnalyzer.Analyze(tokens);

            if (token == null)
            {
                Console.WriteLine("Source code parsed Successfully!");
            }
            else
            {
                Console.WriteLine($"Error! Invalid character \"{token.Value}\" found on Line number \"{token.LineNumber}\"");
            }

            //Write to file
            //var fileText = "LINE NUMBER,CLASS NAME,VALUE\r\n";
            //foreach (var token in tokens)
            //{
            //    fileText += token.ToCSVString();
            //}
            //File.WriteAllText(outputFileCsv, fileText);
            //fileText = "";
            //foreach (var token in tokens)
            //{
            //    fileText += token.ToString();
            //}
            //File.WriteAllText(outputFile, fileText);

            //Print to console
            foreach (var t in tokens)
            {
                Console.WriteLine(t.ToString());
            }

            Console.ReadLine();
        }
    }
}
