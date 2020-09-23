using System.Collections.Generic;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Converters
{
    public interface ISubtitleConverter
    {
        string Command { get; set; }
        string FileExtension { get; set; }
        string Convert(List<SubtitleItem> data);
    }
}