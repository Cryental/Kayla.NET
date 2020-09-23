using System;
using System.Collections.Generic;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Converters
{
    public class SRTConverter : ISubtitleConverter
    {
        public string Command { get; set; } = "srt";
        public string FileExtension { get; set; } = ".srt";

        public string Convert(List<SubtitleItem> data)
        {
            var finalString = "";

            for (var i = 0; i < data.Count; i++)
            {
                var number = i;
                var startTime = new TimeSpan(data[i].StartTime * 10000).ToString(@"hh\:mm\:ss\.fff");
                var endTime = new TimeSpan(data[i].EndTime * 10000).ToString(@"hh\:mm\:ss\.fff");
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
    }
}