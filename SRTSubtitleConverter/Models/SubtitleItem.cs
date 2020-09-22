namespace SRTSubtitleConverter.Models
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

        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string Text { get; set; }
    }
}
