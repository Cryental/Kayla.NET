using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SRTSubtitleConverter.Models;
using UtfUnknown;

namespace SRTSubtitleConverter.Parsers
{
    public class SSAParser : ISubtitleParser
    {
        public string FileExtension { get; set; } = ".ass|.ssa";

        private const string EventLine = "[Events]";
        private const char Separator = ',';

        private const string StartColumn = "Start";
        private const string EndColumn = "End";
        private const string TextColumn = "Text";

        public bool ParseFormat(string path, Encoding encoding, out List<SubtitleItem> result)
        {
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
                                        StartTime = start,
                                        EndTime = end,
                                        Text = ConvertString(textLine)
                                    };
                                    items.Add(item);
                                }
                            }

                            line = reader.ReadLine();
                        }

                        if (items.Any())
                        {
                            result = items;
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