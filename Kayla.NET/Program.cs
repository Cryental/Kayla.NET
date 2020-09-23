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
            var batchProcess = false;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                input = o.Input;
                output = o.Output;
                format = o.Format;
                batchProcess = o.BatchProcess;
            });


            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(output))
            {
                return;
            }

            var processingHandler = new ProcessingHandler();

            if (batchProcess)
            {
                processingHandler.ConvertBath(input, output, format);
            }
            else
            {
                processingHandler.Convert(input, output, format);
            }
        }
    }
}