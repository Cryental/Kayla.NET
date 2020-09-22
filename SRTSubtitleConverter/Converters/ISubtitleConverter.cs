using System.Collections.Generic;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Converters
{
    public interface ISubtitleConverter
    {
        string Convert(List<SubtitleItem> data);
    }
}