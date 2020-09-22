using System;
using System.Collections.Generic;

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
        /// <summary>
        /// End time in milliseconds.
        /// </summary>
        public long EndTime { get; set; }
        public string Text { get; set; }
    }
}