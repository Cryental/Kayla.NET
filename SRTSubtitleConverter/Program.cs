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

            var path = "";
            var result = CharsetDetector.DetectFromFile(path);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var encoding = Encoding.GetEncoding(result.Detected.EncodingName);

            var getSomething = new MicroDVDParser().ToSRT(path);
            File.WriteAllText("sub.srt", getSomething);
            Console.WriteLine("DONE");

            Console.ReadKey();
        }
    }
}