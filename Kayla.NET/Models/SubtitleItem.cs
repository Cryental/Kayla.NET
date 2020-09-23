namespace Kayla.NET.Models
{
    public class SubtitleItem
    {
        public SubtitleItem()
        {
        }

        public SubtitleItem(long startTime, string text)
        {
            StartTime = startTime;
            Text = text;
        }

        public SubtitleItem(long startTime, long endTime, string text)
        {
            StartTime = startTime;
            EndTime = endTime;
            Text = text;
        }

        public long StartTime { get; set; } // Frame Count
        public long EndTime { get; set; } // Frame Count
        public string Text { get; set; }
    }
}