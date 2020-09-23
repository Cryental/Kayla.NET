using System;
using System.Text;
using CommandLine;

namespace Kayla.NET
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Kayla.NET";

            var input = "";
            var output = "";
            var format = "";
            var sync = 0;
            var batchProcess = false;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                input = o.Input;
                output = o.Output;
                format = o.Format;
                sync = o.AdjustSync;
                batchProcess = o.BatchProcess;
            });


            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(output))
            {
                return;
            }

            var processingHandler = new ProcessingHandler();

            if (batchProcess)
            {
                processingHandler.ConvertBath(input, output, format, sync);
            }
            else
            {
                processingHandler.Convert(input, output, format, sync);
            }
        }
    }
}