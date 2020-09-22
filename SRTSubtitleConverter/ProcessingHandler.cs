using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SRTSubtitleConverter.Parsers;

namespace SRTSubtitleConverter
{
    public class ProcessingHandler
    {
        private Dictionary<string, ISubtitleParser> _supportedFormats = new Dictionary<string, ISubtitleParser>();

        public ProcessingHandler()
        {
            _supportedFormats.Add("MicroDVD", new MicroDVDParser());
            _supportedFormats.Add("SAMI", new SAMIParser());
            _supportedFormats.Add("SSA", new SSAParser());
            _supportedFormats.Add("SubViewer", new SubViewerParser());
            _supportedFormats.Add("TTML", new TTMLParser());
            _supportedFormats.Add("VTT", new VTTParser());
            _supportedFormats.Add("YoutubeXML", new YTXMLParser());
        }

        public bool ConvertToSRT(string inputPath, string outputPath)
        {
            var finalResult = string.Empty;

            foreach (var sf in _supportedFormats)
            {
                var extensions = sf.Value.FileExtension.Split('|');

                foreach (var ext in extensions)
                {
                    if (Path.GetExtension(inputPath) == ext)
                    {
                        var result = sf.Value.ToSRT(inputPath);
                        
                        if (!string.IsNullOrEmpty(result))
                        {
                            Console.WriteLine(sf.Key);
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

            File.WriteAllText(outputPath, finalResult, Encoding.UTF8);

            return true;
        }
    }
}
