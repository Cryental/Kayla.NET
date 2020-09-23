using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Kayla.NET.Converters;
using Kayla.NET.Parsers;

namespace Kayla.NET
{
    public class ProcessingHandler
    {
        private readonly Dictionary<string, ISubtitleParser> _supportedParsers =
            new Dictionary<string, ISubtitleParser>();

        private readonly Dictionary<string, ISubtitleConverter> _supportedConverters =
            new Dictionary<string, ISubtitleConverter>();

        public ProcessingHandler()
        {
            _supportedParsers.Add("MicroDVD", new MicroDVDParser());
            _supportedParsers.Add("SAMI", new SAMIParser());
            _supportedParsers.Add("SubStationAlpha", new SSAParser());
            _supportedParsers.Add("SubViewer", new SubViewerParser());
            _supportedParsers.Add("TimedText", new TTMLParser());
            _supportedParsers.Add("WebVTT", new VTTParser());
            _supportedParsers.Add("YtXml", new YtXmlParser());
            _supportedParsers.Add("SubRip", new SRTParser());

            _supportedConverters.Add("MicroDVD", new MicroDVDConverter());
            _supportedConverters.Add("SAMI", new SAMIConverter());
            _supportedConverters.Add("SubStationAlpha", new SSAConverter());
            _supportedConverters.Add("SubViewer", new SubViewerConverter());
            _supportedConverters.Add("SubRip", new SRTConverter());
        }

        public bool ConvertToSRT(string inputPath, string outputPath)
        {
            var finalResult = string.Empty;

            var outputFilePath = outputPath;

            foreach (var sf in _supportedParsers)
            {
                var extensions = sf.Value.FileExtension.Split('|');

                foreach (var ext in extensions)
                {
                    if (Path.GetExtension(inputPath) == ext)
                    {
                        var parsingStatus = sf.Value.ParseFormat(inputPath, out var parsedData);

                        if (!parsingStatus)
                        {
                            continue;
                        }

                        var srtConverter = new SubViewerConverter();
                        var result = srtConverter.Convert(parsedData);

                        if (!string.IsNullOrEmpty(result))
                        {
                            finalResult = result;
                            break;
                        }
                    }
                }
            }


            if (string.IsNullOrEmpty(finalResult))
            {
                return false;
            }

            File.WriteAllText(outputFilePath, finalResult, Encoding.UTF8);
            Console.WriteLine($"[+] Converted File: {Path.GetFileName(inputPath)}");
            Console.WriteLine("[*] The operation is completed.");
            return true;
        }

        public bool ConvertBathToSRT(string inputPath, string outputPath)
        {
            var files = new DirectoryInfo(inputPath);

            var convertedFiles = new List<string>();
            var unconvertedFiles = new List<string>();

            foreach (var f in files.GetFiles())
            {
                var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(f.Name) + ".srt");
                var finalResult = string.Empty;

                foreach (var sf in _supportedParsers)
                {
                    var extensions = sf.Value.FileExtension.Split('|');

                    foreach (var ext in extensions)
                    {
                        if (Path.GetExtension(f.Name) != ext)
                        {
                            continue;
                        }

                        var parsingStatus = sf.Value.ParseFormat(inputPath, out var parsedData);

                        if (!parsingStatus)
                        {
                            continue;
                        }

                        var srtConverter = new SRTConverter();
                        var result = srtConverter.Convert(parsedData);

                        if (string.IsNullOrEmpty(result))
                        {
                            continue;
                        }

                        finalResult = result;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(finalResult))
                {
                    File.WriteAllText(outputFilePath, finalResult, Encoding.UTF8);
                    convertedFiles.Add(f.Name);
                }
                else
                {
                    unconvertedFiles.Add(f.Name);
                }
            }

            Console.WriteLine("[+] Converted Files ---");
            Console.WriteLine();
            foreach (var f in convertedFiles)
            {
                Console.WriteLine($"[-] {f}");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("[+] Not Converted Files ---");
            Console.WriteLine();
            foreach (var f in unconvertedFiles)
            {
                Console.WriteLine($"[-] {f}");
            }

            Console.WriteLine();

            Console.WriteLine("[*] The operation is completed.");
            return true;
        }
    }
}