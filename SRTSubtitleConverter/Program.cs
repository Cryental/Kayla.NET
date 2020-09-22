using System;
using System.IO;
using System.Text;
using SRTSubtitleConverter.Parsers;
using UtfUnknown;

namespace SRTSubtitleConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var path = "C:\\Users\\conta\\Videos\\sub.vtt";

            var handler = new ProcessingHandler();
            handler.ConvertToSRT(path, "sub.srt");

            Console.WriteLine("DONE");

            Console.ReadKey();
        }
    }
}