using System;
using System.IO;
using System.Text;
using CommandLine;

namespace Kayla.NET
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "SRTSubtitleConverter";

            var input = "";
            var output = "";
            var batchProcess = false;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                input = o.Input;
                output = o.Output;
                batchProcess = o.BatchProcess;
            });

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(output))
            {
                return;
            }

            var processingHandler = new ProcessingHandler();

            if (batchProcess)
            {
                if (!Directory.Exists(input))
                {
                    Console.WriteLine("[!] The input path is not a directory or does not exist.");
                    return;
                }

                if (!Directory.Exists(output))
                {
                    Console.WriteLine("[!] The output path is not a directory or does not exist.");
                    return;
                }

                processingHandler.ConvertBathToSRT(input, output);
            }
            else
            {
                if (!File.Exists(input))
                {
                    Console.WriteLine("[!] The input file does not exist.");
                    return;
                }

                if (Directory.Exists(output))
                {
                    var fileName = Path.GetFileNameWithoutExtension(input) + ".srt";
                    var finalPath = Path.Combine(output, fileName);

                    processingHandler.ConvertToSRT(input, finalPath);
                    return;
                }

                processingHandler.ConvertToSRT(input, output);
            }
        }
    }
}