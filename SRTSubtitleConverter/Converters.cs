using System;

namespace SRTSubtitleConverter
{
    public static class Converters
    {
        public static string ConvertMilliSecondsToString(long ms)
        {
            return new TimeSpan(ms * 10000).ToString(@"hh\:mm\:ss\.fff");
        }
    }
}