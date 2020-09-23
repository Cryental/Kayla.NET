using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kayla.NET.Models;
using UtfUnknown;

namespace Kayla.NET.Parsers
{
    public class SSAParser : ISubtitleParser
    {
        private const string EventLine = "[Events]";
        private const char Separator = ',';

        private const string StartColumn = "Start";
        private const string EndColumn = "End";
        private const string TextColumn = "Text";
        public string FileExtension { get; set; } = ".ass|.ssa";

        public bool ParseFormat(string path, out List<SubtitleItem> result)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var detect = CharsetDetector.DetectFromFile(path);
            var encoding = Encoding.GetEncoding(detect.Detected.EncodingName);

            var ssaStream = new StreamReader(path, encoding).BaseStream;
            if (!ssaStream.CanRead || !ssaStream.CanSeek)
            {
                result = null;
                return false;
            }

            ssaStream.Position = 0;

            var reader = new StreamReader(ssaStream, encoding, true);

            var line = reader.ReadLine();
            var lineNumber = 1;
            while (line != null && line != EventLine)
            {
                line = reader.ReadLine();
                lineNumber++;
            }

            if (line != null)
            {
                var headerLine = reader.ReadLine();
                if (!string.IsNullOrEmpty(headerLine))
                {
                    var columnHeaders = headerLine.Split(Separator).Select(head => head.Trim()).ToList();

                    var startIndexColumn = columnHeaders.IndexOf(StartColumn);
                    var endIndexColumn = columnHeaders.IndexOf(EndColumn);
                    var textIndexColumn = columnHeaders.IndexOf(TextColumn);

                    if (startIndexColumn > 0 && endIndexColumn > 0 && textIndexColumn > 0)
                    {
                        var items = new List<SubtitleItem>();

                        line = reader.ReadLine();
                        while (line != null)
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                var columns = line.Split(Separator);
                                var startText = columns[startIndexColumn];
                                var endText = columns[endIndexColumn];

                                var textLine = string.Join(",", columns.Skip(textIndexColumn));

                                var start = ParseSsaTimecode(startText);
                                var end = ParseSsaTimecode(endText);

                                if (start > 0 && end > 0 && !string.IsNullOrEmpty(textLine))
                                {
                                    var item = new SubtitleItem
                                    {
                                        StartTime = start, EndTime = end, Text = ConvertString(textLine)
                                    };
                                    items.Add(item);
                                }
                            }

                            line = reader.ReadLine();
                        }

                        if (items.Any())
                        {
                            result = Filters.RemoveDuplicateItems(items);
                            return true;
                        }

                        result = null;
                        return false;
                    }

                    result = null;
                    return false;
                }

                result = null;
                return false;
            }

            result = null;
            return false;
        }

        private string ConvertString(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("<BR>", "\n");
            str = str.Replace("\\N", "\n");
            try
            {
                while (str.IndexOf("{", StringComparison.Ordinal) != -1)
                {
                    var i = str.IndexOf("{", StringComparison.Ordinal);
                    var j = str.IndexOf("}", StringComparison.Ordinal);
                    str = str.Remove(i, j - i + 1);
                }

                return str;
            }
            catch
            {
                return str;
            }
        }

        private int ParseSsaTimecode(string s)
        {
            TimeSpan result;

            if (TimeSpan.TryParse(s, out result))
            {
                var nbOfMs = (int) result.TotalMilliseconds;
                return nbOfMs;
            }

            return -1;
        }
    }
}