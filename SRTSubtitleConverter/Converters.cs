using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SRTSubtitleConverter.Models;
using UtfUnknown;

namespace SRTSubtitleConverter
{
    public static class Converters
    {
        private static string ConvertMilliSecondsToString(long ms)
        {
            return new TimeSpan(ms * 10000).ToString();
        }

        public static string ToSRT(List<Common> data)
        {
            var finalString = "";

            for (var i = 0; i < data.Count; i++)
            {
                var number = i;
                var startTime = ConvertMilliSecondsToString(data[i].StartTime);

                var endTime = i == data.Count - 1
                    ? ConvertMilliSecondsToString(data[i].StartTime + 1000)
                    : ConvertMilliSecondsToString(data[i + 1].StartTime);

                var text = data[i].Text;

                var format = $"{number}\r\n{startTime} --> {endTime}\r\n{text}";

                if (i != data.Count - 1) format += "\r\n\r\n";

                finalString += format;
            }

            return finalString;
        }
    }
}