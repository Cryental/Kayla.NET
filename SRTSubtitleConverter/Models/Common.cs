using System;

namespace SRTSubtitleConverter.Models
{
    public class Common
    {
        public Common()
        {
        }

        public Common(long startTime, string text)
        {
            StartTime = startTime;
            Text = text;
        }

        public long StartTime { get; set; }
        public string Text { get; set; }
    }
}