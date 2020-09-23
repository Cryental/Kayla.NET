using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Kayla.NET.Models;

namespace Kayla.NET.Converters
{
    public class SAMIConverter : ISubtitleConverter
    {
        public string FileExtension { get; set; } = ".smi";
        public string Convert(List<SubtitleItem> data)
        {
            return $@"<SAMI>
<HEAD>
<TITLE></TITLE>
</HEAD>
<BODY>
{string.Join(Environment.NewLine, data.Select(ConvertItem))}
</BODY>
</SAMI>";
        }

        private string ConvertItem(SubtitleItem item)
        {
            var result = string.IsNullOrEmpty(item.Text)
                ? "&nbsp;"
                : Regex.Replace(item.Text, "(\r\n|\r|\n)", "<br>");

            return $"<SYNC Start={item.StartTime}><P>{result}";
        }
    }
}