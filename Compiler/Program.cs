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

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"test_code.txt");

            var sourceCode = File.ReadAllText(path);

            var lexicalAnalyzer = new LexicalAnalyzer();
            var tokens = lexicalAnalyzer.Analyze(sourceCode);

            foreach (var token in tokens)
            {
                Console.WriteLine(token.ToString());
            }

            Console.ReadLine();
        }
    }
}
