using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SRTSubtitleConverter.Models;
using UtfUnknown;

namespace SRTSubtitleConverter.Parsers
{
    public class MicroDVDParser : ISubtitleParser
    {
        private const string LineRegex = @"^[{\[](-?\d+)[}\]][{\[](-?\d+)[}\]](.*)";

        private readonly char[] _lineSeparators = {'|'};
        private readonly float defaultFrameRate = 23.976f;

        public MicroDVDParser()
        {
        }

        public MicroDVDParser(float defaultFrameRate)
        {
            this.defaultFrameRate = defaultFrameRate;
        }

        public string FileExtension { get; set; } = ".sub";

        public bool ParseFormat(string path, Encoding encoding, out List<Common> result)
        {
            var subStream = new StreamReader(path, encoding).BaseStream;

            if (!subStream.CanRead || !subStream.CanSeek)
            {
                result = null;
                return false;
            }

            subStream.Position = 0;
            var reader = new StreamReader(subStream, encoding, true);

            var items = new List<Common>();
            var line = reader.ReadLine();
            while (line != null && !IsMicroDvdLine(line)) line = reader.ReadLine();

            if (line != null)
            {
                float frameRate;
                var firstItem = ParseLine(line, defaultFrameRate);
                if (firstItem.Text != null && firstItem.Text.Any())
                {
                    var success = TryExtractFrameRate(firstItem.Text, out frameRate);
                    if (!success)
                    {
                        frameRate = defaultFrameRate;

                        items.Add(firstItem);
                    }

                    if (success) Console.WriteLine(frameRate);
                }
                else
                {
                    frameRate = defaultFrameRate;
                }

                line = reader.ReadLine();
                while (line != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        var item = ParseLine(line, frameRate);
                        items.Add(item);
                    }

                    line = reader.ReadLine();
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

        private static string ConvertString(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("<BR>", "\n");
            str = str.Replace("&nbsp;", "");
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

        private bool IsMicroDvdLine(string line)
        {
            return Regex.IsMatch(line, LineRegex);
        }

        private Common ParseLine(string line, float frameRate)
        {
            var match = Regex.Match(line, LineRegex);
            if (!match.Success || match.Groups.Count <= 2) return null;
            var startFrame = match.Groups[1].Value;
            var start = (int) (1000 * double.Parse(startFrame) / frameRate);
            var endTime = match.Groups[2].Value;
            var end = (int) (1000 * double.Parse(endTime) / frameRate);
            var text = match.Groups[^1].Value;
            var lines = text.Split(_lineSeparators);
            var nonEmptyLines = lines.Where(l => !string.IsNullOrEmpty(l)).ToList();
            var item = new Common
            {
                StartTime = start,
                EndTime = end,
                Text = ConvertString(string.Join("\r\n", nonEmptyLines.ToArray()))
            };

            return item;
        }

        private bool TryExtractFrameRate(string text, out float frameRate)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var success = float.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                    out frameRate);
                return success;
            }

            frameRate = defaultFrameRate;
            return false;
        }
    }
}