using System;
using System.Collections.Generic;
using System.Text;
using Kayla.NET.Models;

namespace Kayla.NET.Converters
{
    public class VTTConverter : ISubtitleConverter
    {
        public string FileExtension { get; set; } = ".vtt";

        public string Convert(List<SubtitleItem> data)
        {
            var finalString = "WEBVTT\r\n\r\n";

            for (var i = 0; i < data.Count; i++)
            {
                if (string.IsNullOrEmpty(data[i].Text))
                {
                    continue;
                }

                var startTime = new TimeSpan(data[i].StartTime * 10000).ToString(@"hh\:mm\:ss\.fff");
                var endTime = new TimeSpan(data[i].EndTime * 10000).ToString(@"hh\:mm\:ss\.fff");
                var text = data[i].Text;
                var format = $"{startTime} --> {endTime}\r\n{text}";

                if (i != data.Count - 1)
                {
                    format += "\r\n\r\n";
                }

                finalString += format;
            }

            return finalString;
        }
    }
}
