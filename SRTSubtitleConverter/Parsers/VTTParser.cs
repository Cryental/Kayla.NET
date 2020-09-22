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
    public class VTTParser : ISubtitleParser
    {
        private readonly string[] _delimiters = {"-->", "- >", "->"};
        public string FileExtension { get; set; } = ".vtt";

        public bool ParseFormat(string path, Encoding encoding, out List<SubtitleItem> result)
        {
            var vttStream = new StreamReader(path, encoding).BaseStream;
            if (!vttStream.CanRead || !vttStream.CanSeek)
            {
                result = null;
                return false;
            }

            vttStream.Position = 0;

            var reader = new StreamReader(vttStream, encoding, true);

            var items = new List<SubtitleItem>();
            var vttSubParts = GetVttSubTitleParts(reader).ToList();
            if (vttSubParts.Any())
            {
                foreach (var vttSubPart in vttSubParts)
                {
                    var lines =
                        vttSubPart.Split(new[] {Environment.NewLine}, StringSplitOptions.None)
                            .Select(s => s.Trim())
                            .Where(l => !string.IsNullOrEmpty(l))
                            .ToList();

                    var item = new SubtitleItem();
                    foreach (var line in lines)
                    {
                        if (item.StartTime == 0 && item.EndTime == 0)
                        {
                            int startTc;
                            int endTc;
                            var success = TryParseTimecodeLine(line, out startTc, out endTc);
                            if (success)
                            {
                                item.StartTime = startTc;
                                item.EndTime = endTc;
                            }
                        }
                        else
                        {
                            item.Text = ConvertString(line);

                        }

                        item.Text = string.IsNullOrEmpty(item.Text) ? "" : item.Text;
                    }

                    if ((item.StartTime != 0 || item.EndTime != 0) && item.Text.Any()) 
                        items.Add(item);
                }

                result = items;
                return true;
            }

            result = null;
            return false;
        }

        private string ConvertString(string str)
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

        private IEnumerable<string> GetVttSubTitleParts(TextReader reader)
        {
            string line;
            var sb = new StringBuilder();

            while ((line = reader.ReadLine()) != null)
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    var res = sb.ToString().TrimEnd();
                    if (!string.IsNullOrEmpty(res)) yield return res;
                    sb = new StringBuilder();
                }
                else
                {
                    sb.AppendLine(line);
                }

            if (sb.Length > 0) yield return sb.ToString();
        }

        private bool TryParseTimecodeLine(string line, out int startTc, out int endTc)
        {
            var parts = line.Split(_delimiters, StringSplitOptions.None);
            if (parts.Length != 2)
            {
                startTc = -1;
                endTc = -1;
                return false;
            }

            startTc = ParseVttTimecode(parts[0]);
            endTc = ParseVttTimecode(parts[1]);
            return true;
        }

        private int ParseVttTimecode(string s)
        {
            var timeString = string.Empty;
            var match = Regex.Match(s, "[0-9]+:[0-9]+:[0-9]+[,\\.][0-9]+");
            if (match.Success)
            {
                timeString = match.Value;
            }
            else
            {
                match = Regex.Match(s, "[0-9]+:[0-9]+[,\\.][0-9]+");
                if (match.Success) timeString = "00:" + match.Value;
            }

            if (!string.IsNullOrEmpty(timeString))
            {
                timeString = timeString.Replace(',', '.');
                TimeSpan result;
                if (TimeSpan.TryParse(timeString, out result))
                {
                    var nbOfMs = (int) result.TotalMilliseconds;
                    return nbOfMs;
                }
            }

            return -1;
        }
    }
}