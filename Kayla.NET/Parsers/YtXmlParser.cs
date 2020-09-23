using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Kayla.NET.Models;
using UtfUnknown;

namespace Kayla.NET.Parsers
{
    public class YtXmlParser : ISubtitleParser
    {
        public string FileExtension { get; set; } = ".xml";

        public bool ParseFormat(string path, out List<SubtitleItem> result)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var detect = CharsetDetector.DetectFromFile(path);
            var encoding = Encoding.GetEncoding(detect.Detected.EncodingName);

            var xmlStream = new StreamReader(path, encoding).BaseStream;
            // rewind the stream
            xmlStream.Position = 0;
            var items = new List<SubtitleItem>();

            // parse xml stream
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlStream);

            if (xmlDoc.DocumentElement != null)
            {
                var nodeList = xmlDoc.DocumentElement.SelectNodes("//text");

                if (nodeList != null)
                {
                    for (var i = 0; i < nodeList.Count; i++)
                    {
                        var node = nodeList[i];
                        try
                        {
                            var startString = node.Attributes["start"].Value;
                            var start = float.Parse(startString, CultureInfo.InvariantCulture);
                            var durString = node.Attributes["dur"].Value;
                            var duration = float.Parse(durString, CultureInfo.InvariantCulture);
                            var text = node.InnerText;

                            items.Add(new SubtitleItem
                            {
                                StartTime = (int) (start * 1000),
                                EndTime = (int) ((start + duration) * 1000),
                                Text = ConvertString(text)
                            });
                        }
                        catch
                        {
                            result = null;
                            return false;
                        }
                    }
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