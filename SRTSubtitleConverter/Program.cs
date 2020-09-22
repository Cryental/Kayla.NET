using System;
using System.IO;
using System.Text;
using CommandLine;

namespace SRTSubtitleConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "SRTSubtitleConverter";

            var input = "";
            var output = "";
            var folderFlag = false;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                input = o.Input;
                output = o.Output;
                folderFlag = o.FolderFlag;
            });

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(output)) return;

            if (folderFlag)
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

                var processingHandler = new ProcessingHandler();
                processingHandler.ConvertBathToSRT(input, output);
            }
            else
            {
                if (!File.Exists(input))
                {
                    Console.WriteLine("[!] The input file is not a file or does not exist.");
                    return;
                }

                var processingHandler = new ProcessingHandler();

                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(input)))
                    {
                        Console.WriteLine("[!] The output path does not exist.");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("[!] The output path does not exist.");
                    return;
                }

                if (Directory.Exists(output))
                {
                    var result = processingHandler.ConvertToSRT(input, output, true);

                    if (!result) Console.WriteLine("[!] This file is an unsupported format.");
                }
                else
                {
                    var result = processingHandler.ConvertToSRT(input, output);

                    if (!result) Console.WriteLine("[!] This file is an unsupported format.");
                }
            }
        }
    }
}