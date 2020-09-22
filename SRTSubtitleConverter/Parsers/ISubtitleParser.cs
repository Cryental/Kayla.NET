using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Parsers
{
    public interface ISubtitleParser
    {
        bool ParseFormat(string path, Encoding encoding, out List<Common> result);
        string ToSRT(string path);
    }
}
