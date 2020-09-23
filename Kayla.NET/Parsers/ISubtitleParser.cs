using System.Collections.Generic;
using Kayla.NET.Models;

namespace Kayla.NET.Parsers
{
    public interface ISubtitleParser
    {
        string FileExtension { get; set; }

        bool ParseFormat(string path, out List<SubtitleItem> result);
    }
}