using System;
using System.Collections.Generic;
using Kayla.NET.Models;

namespace Kayla.NET.Converters
{
    public class SubViewerConverter : ISubtitleConverter
    {
        public string FileExtension { get; set; } = ".sub";

        public string Convert(List<SubtitleItem> data)
        {
            var finalString = "";

            for (var i = 0; i < data.Count; i++)
            {
                if (string.IsNullOrEmpty(data[i].Text))
                {
                    continue;
                }

                var startTime = new TimeSpan(data[i].StartTime * 10000).ToString(@"hh\:mm\:ss\.ff");
                var endTime = new TimeSpan(data[i].EndTime * 10000).ToString(@"hh\:mm\:ss\.ff");
                var text = data[i].Text;
                var format = $"{startTime},{endTime}\r\n{text}";

                if (i != data.Count - 1)
                {
                    format += "\r\n\r\n";
                }

                finalString += format;
            }

            return $@"[INFORMATION]
[TITLE]
[AUTHOR]
[SOURCE]
[PRG]
[FILEPATH]
[DELAY]0
[CD TRACK]0
[COMMENT]
[END INFORMATION]
[SUBTITLE]
{finalString}";
        }
    }
}