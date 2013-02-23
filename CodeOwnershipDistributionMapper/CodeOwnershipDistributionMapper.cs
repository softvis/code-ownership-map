using System;
using Lib;
using System.Drawing;

namespace CodeOwnershipDistributionMapper
{
    internal class CodeOwnershipDistributionMapper
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Requires SVN Log file as input.");
                Console.WriteLine("Two-colored developer usage: codemap [svn_dump.log]");
                Console.WriteLine("Multi-colored developer usage: codemap [svn_dump.log] multi");
                return;
            }
            var useRedGreenGraph = true;
            if (args.Length == 2)
            {
                useRedGreenGraph = false;
            }
            
            var logFileName = args[0];
            var parser = new SvnParser();
            Console.WriteLine("Parsing...");
            parser.ParseFile(logFileName);

            Console.WriteLine("Calculating...");
            var calculator = new OwnershipCalculator();
            calculator.Calculate(parser.authorCommitCount, parser.totalCommitCount);

            if (!useRedGreenGraph)
            {
                Console.WriteLine("Legend for Developers:");
                foreach (var author in parser.authorCommitCount)
                {
                    Console.WriteLine(author.Key + ": " +
                        ((SolidBrush)GdiGrapher.BrushesForAuthors[calculator._authorIndex.IndexForAuthor(author.Key)]).Color.Name);
                }
            }

            var outputName = logFileName.Remove(logFileName.Length - 4);
            var grapher = new GdiGrapher(useRedGreenGraph);
            Console.WriteLine("Graphing...");
            var outputFileName = grapher.Graph(calculator.OwnershipDistribution, outputName);
            
            Console.WriteLine("Map created: " + outputFileName);
        }
    }
}