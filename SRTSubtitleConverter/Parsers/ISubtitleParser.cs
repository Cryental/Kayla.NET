using System.Collections.Generic;
using System.Text;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Parsers
{
    public interface ISubtitleParser
    {
        string FileExtension { get; set; }

        bool ParseFormat(string path, Encoding encoding, out List<SubtitleItem> result);
        string ToSRT(string path);
    }
}