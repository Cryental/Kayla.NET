using System.Collections.Generic;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Parsers
{
    public interface ISubtitleParser
    {
        string FileExtension { get; set; }

        bool ParseFormat(string path, out List<SubtitleItem> result);
    }
}