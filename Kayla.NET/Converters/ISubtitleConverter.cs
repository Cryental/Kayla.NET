using System.Collections.Generic;
using Kayla.NET.Models;

namespace Kayla.NET.Converters
{
    public interface ISubtitleConverter
    {
        string FileExtension { get; set; }
        string Convert(List<SubtitleItem> data);
    }
}