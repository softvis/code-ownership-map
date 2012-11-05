using System;
using Lib;

namespace CodeOwnershipDistributionMapper
{
    internal class CodeOwnershipDistributionMapper
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Requires SVN Log file as input");
                return;
            }
            
            var logFileName = args[0];
            var parser = new SvnParser();
            Console.WriteLine("Parsing...");
            parser.ParseFile(logFileName);

            Console.WriteLine("Calculating...");
            var calculator = new OwnershipCalculator();
            calculator.Calculate(parser.authorCommitCount, parser.totalCommitCount);

            var outputName = logFileName.Remove(logFileName.Length - 4);
            var grapher = new GdiGrapher();
            Console.WriteLine("Graphing...");
            var outputFileName = grapher.Graph(calculator.OwnershipDistribution, outputName);
            
            Console.WriteLine("Map created: " + outputFileName);
        }
    }
}