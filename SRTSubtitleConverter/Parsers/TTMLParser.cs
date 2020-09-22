using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SRTSubtitleConverter.Models;
using UtfUnknown;

namespace SRTSubtitleConverter.Parsers
{
    public class TTMLParser : ISubtitleParser
    {
        public string FileExtension { get; set; } = ".xml|.ttml";

        public bool ParseFormat(string path, Encoding encoding, out List<SubtitleItem> result)
        {
            var xmlStream = new StreamReader(path, encoding).BaseStream;
            xmlStream.Position = 0;
            var items = new List<SubtitleItem>();

            var xElement = XElement.Load(xmlStream);
            var tt = xElement.GetNamespaceOfPrefix("tt") ?? xElement.GetDefaultNamespace();

            var nodeList = xElement.Descendants(tt + "p").ToList();
            foreach (var node in nodeList)
            {
                try
                {
                    var reader = node.CreateReader();
                    reader.MoveToContent();
                    var beginString = node.Attribute("begin").Value.Replace("t", "");
                    var startTicks = ParseTimecode(beginString);
                    var endString = node.Attribute("end").Value.Replace("t", "");
                    var endTicks = ParseTimecode(endString);
                    var text = reader.ReadInnerXml()
                        .Replace("<tt:", "<")
                        .Replace("</tt:", "</")
                        .Replace(string.Format(@" xmlns:tt=""{0}""", tt), "")
                        .Replace(string.Format(@" xmlns=""{0}""", tt), "");

                    items.Add(new SubtitleItem()
                    {
                        StartTime = (int)(startTicks),
                        EndTime = (int)(endTicks),
                        Text = ConvertString(text)
                    });
                }
                catch
                {
                    result = null;
                    return false;
                }
            }

            if (items.Any())
            {
                result = items;
                return true;
            }

            result = null;
            return false;
        }

        public string ToSRT(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var result = CharsetDetector.DetectFromFile(path);
            var encoding = Encoding.GetEncoding(result.Detected.EncodingName);

            var resFormat = ParseFormat(path, encoding, out var data);

            if (!resFormat) return string.Empty;

            var finalString = "";

            for (var i = 0; i < data.Count; i++)
            {
                var number = i;
                var startTime = Converters.ConvertMilliSecondsToString(data[i].StartTime);

                var endTime = Converters.ConvertMilliSecondsToString(data[i].EndTime);

                var text = data[i].Text;

                var format = $"{number}\r\n{startTime} --> {endTime}\r\n{text}";

                if (i != data.Count - 1) format += "\r\n\r\n";

                finalString += format;
            }

            return finalString;

        }

        private static long ParseTimecode(string s)
        {
            TimeSpan result;
            if (TimeSpan.TryParse(s, out result))
            {
                return (long)result.TotalMilliseconds;
            }
            long ticks;
            if (long.TryParse(s.TrimEnd('t'), out ticks))
            {
                return ticks / 10000;
            }
            return -1;
        }

        private string ConvertString(string str)
        {
            str = str.Replace("<br />", "\r\n");
            str = str.Replace("<BR />", "\r\n");
            str = str.Replace("<br>", "\r\n");
            str = str.Replace("<BR>", "\r\n");

            try
            {
                while (str.IndexOf("<", StringComparison.Ordinal) != -1)
                {
                    var i = str.IndexOf("<", StringComparison.Ordinal);
                    var j = str.IndexOf(">", StringComparison.Ordinal);
                    str = str.Remove(i, j - i + 1);
                }

                return str;
            }
            catch
            {
                return str;
            }
        }
    }
}
