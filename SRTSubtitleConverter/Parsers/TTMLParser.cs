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

        public bool ParseFormat(string path, out List<SubtitleItem> result)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var detect = CharsetDetector.DetectFromFile(path);
            var encoding = Encoding.GetEncoding(detect.Detected.EncodingName);

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

                    items.Add(new SubtitleItem
                    {
                        StartTime = (int) startTicks, EndTime = (int) endTicks, Text = ConvertString(text)
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
                result = Filters.RemoveDuplicateItems(items);
                return true;
            }

            result = null;
            return false;
        }

        private static long ParseTimecode(string s)
        {
            TimeSpan result;
            if (TimeSpan.TryParse(s, out result))
            {
                return (long) result.TotalMilliseconds;
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