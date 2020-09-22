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
        }

        public void ConvertToSRT(string inputPath, string outputPath)
        {
            var finalResult = string.Empty;

            foreach (var sf in _supportedFormats)
            {
                Console.WriteLine(sf.Key);
                var result = sf.Value.ToSRT(inputPath);

                if (!string.IsNullOrEmpty(result))
                {
                    finalResult = result;
                    break;
                }
            }

            if (string.IsNullOrEmpty(finalResult))
            {
                throw new FormatException("This file is an unsupported format.");
            }

            File.WriteAllText(outputPath, finalResult, Encoding.UTF8);
        }
    }
}
