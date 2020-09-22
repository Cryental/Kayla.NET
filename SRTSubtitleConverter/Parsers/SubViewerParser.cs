using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SRTSubtitleConverter.Models;
using UtfUnknown;

namespace SRTSubtitleConverter.Parsers
{
    public class SubViewerParser : ISubtitleParser
    {
        private const string FirstLine = "[INFORMATION]";
        private const short MaxLineNumberForItems = 20;
        private const char TimecodeSeparator = ',';

        private readonly Regex _timestampRegex =
            new Regex(@"\d{2}:\d{2}:\d{2}\.\d{2},\d{2}:\d{2}:\d{2}\.\d{2}", RegexOptions.Compiled);

        public string FileExtension { get; set; } = ".sub";


        public bool ParseFormat(string path, Encoding encoding, out List<SubtitleItem> result)
        {
            var subStream = new StreamReader(path, encoding).BaseStream;
            subStream.Position = 0;
            var reader = new StreamReader(subStream, encoding, true);

            var firstLine = reader.ReadLine();
            if (firstLine == FirstLine)
            {
                var line = reader.ReadLine();
                var lineNumber = 2;
                while (line != null && lineNumber <= MaxLineNumberForItems && !IsTimestampLine(line))
                {
                    line = reader.ReadLine();
                    lineNumber++;
                }

                if (line != null && lineNumber <= MaxLineNumberForItems && IsTimestampLine(line))
                {
                    var items = new List<SubtitleItem>();

                    var timeCodeLine = line;
                    var textLines = new List<string>();

                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (IsTimestampLine(line))
                        {
                            var timeCodes = ParseTimecodeLine(timeCodeLine);
                            var start = timeCodes.Item1;
                            var end = timeCodes.Item2;

                            if (start > 0 && end > 0 && textLines.Any())
                            {
                                items.Add(new SubtitleItem
                                {
                                    StartTime = start,
                                    EndTime = end,
                                    Text = ConvertString(string.Join("\r\n", textLines.ToArray()))
                                });
                            }

                            timeCodeLine = line;
                            textLines = new List<string>();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                textLines.Add(line);
                            }
                        }
                    }

                    var lastTimeCodes = ParseTimecodeLine(timeCodeLine);
                    var lastStart = lastTimeCodes.Item1;
                    var lastEnd = lastTimeCodes.Item2;
                    if (lastStart > 0 && lastEnd > 0 && textLines.Any())
                    {
                        items.Add(new SubtitleItem
                        {
                            StartTime = lastStart,
                            EndTime = lastEnd,
                            Text = ConvertString(string.Join("\r\n", textLines.ToArray()))
                        });
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

        public string ToSRT(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var result = CharsetDetector.DetectFromFile(path);
            var encoding = Encoding.GetEncoding(result.Detected.EncodingName);

            var resFormat = ParseFormat(path, encoding, out var data);

            if (!resFormat)
            {
                return string.Empty;
            }

            var finalString = "";

            for (var i = 0; i < data.Count; i++)
            {
                var number = i;
                var startTime = Converters.ConvertMilliSecondsToString(data[i].StartTime);

                var endTime = Converters.ConvertMilliSecondsToString(data[i].EndTime);

                var text = data[i].Text;

                var format = $"{number}\r\n{startTime} --> {endTime}\r\n{text}";

                if (i != data.Count - 1)
                {
                    format += "\r\n\r\n";
                }

                finalString += format;
            }

            return finalString;
        }

        private string ConvertString(string str)
        {
            str = str.Replace("[br]", "\r\n");
            str = str.Replace("[BR]", "\r\n");

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

        private Tuple<int, int> ParseTimecodeLine(string line)
        {
            var parts = line.Split(TimecodeSeparator);
            if (parts.Length == 2)
            {
                var start = ParseTimecode(parts[0]);
                var end = ParseTimecode(parts[1]);
                return new Tuple<int, int>(start, end);
            }

            var message = string.Format("Couldn't parse the timecodes in line '{0}'", line);
            throw new ArgumentException(message);
        }

        private int ParseTimecode(string s)
        {
            TimeSpan result;

            if (TimeSpan.TryParse(s, out result))
            {
                var nbOfMs = (int) result.TotalMilliseconds;
                return nbOfMs;
            }

            return -1;
        }

        private bool IsTimestampLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            var isMatch = _timestampRegex.IsMatch(line);
            return isMatch;
        }
    }
}