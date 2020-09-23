using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Kayla.NET.Models;

namespace Kayla.NET.Converters
{
    public class MicroDVDConverter : ISubtitleConverter
    {
        public readonly float DefaultFrameRate = 23.976f;
        public string FileExtension { get; set; } = ".sub";

        public string Convert(List<SubtitleItem> data)
        {
            var filteredItems = new List<string>();

            foreach (var d in data)
            {
                if (string.IsNullOrEmpty(d.Text))
                {
                    continue;
                }

                var startTime = Math.Round(d.StartTime * DefaultFrameRate / 1000);
                var endTime = Math.Round(d.EndTime * DefaultFrameRate / 1000);

                var result = Regex.Replace(d.Text, "(\r\n|\r|\n)", @"|");
                result = result.Replace(Environment.NewLine, "|");

                filteredItems.Add($"{{{startTime}}}{{{endTime}}}{result}");
            }

            return string.Join(Environment.NewLine, filteredItems);
        }
    }
}