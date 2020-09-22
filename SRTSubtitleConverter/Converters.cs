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
        public static string ConvertMilliSecondsToString(long ms)
        {
            return new TimeSpan(ms * 10000).ToString(@"hh\:mm\:ss\.fff");
        }
    }
}