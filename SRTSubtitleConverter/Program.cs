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

            var path = "E:\\[Cerberus] Steins;Gate + Steins;Gate 0 +  Special + Movie [BD 1080p HEVC 10-bit AAC]\\Steins;Gate 0\\Season 1\\슈타인즈 게이트 제로 - 1x23 - 무한원점의 아크라이트.smi";
            var result = CharsetDetector.DetectFromFile(path);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var encoding = Encoding.GetEncoding(result.Detected.EncodingName);

            var getSomething = new SAMIParser().ToSRT(path);
            File.WriteAllText("sub.srt", getSomething);
            Console.WriteLine("DONE");

            Console.ReadKey();
        }
    }
}