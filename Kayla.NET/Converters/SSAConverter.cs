using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kayla.NET.Models;

namespace Kayla.NET.Converters
{
    public class SSAConverter : ISubtitleConverter
    {
        public string FileExtension { get; set; } = ".ass";

        public string Convert(List<SubtitleItem> data)
        {
            var filteredItems = new List<string>();

            foreach (var d in data)
            {
                if (string.IsNullOrEmpty(d.Text))
                {
                    continue;
                }

                var result = Regex.Replace(d.Text, "(\r\n|\r|\n)", @"\N");

                result = result.Replace(Environment.NewLine, @"\N");

                var startTime = new TimeSpan(d.StartTime * 10000).ToString(@"h\:mm\:ss\.ff");
                var endTime = new TimeSpan(d.EndTime * 10000).ToString(@"h\:mm\:ss\.ff");

                var text = $"Dialogue: Marked=0,{startTime},{endTime},*Default,NTP,0000,0000,0000,,{result}";

                filteredItems.Add(text);
            }

            return $@"[Script Info]
; This is a Sub Station Alpha v4 script.
Title: sub
ScriptType: v4.00
Collisions: Normal
PlayDepth: 0

[V4 Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, TertiaryColour, BackColour, Bold, Italic, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding
Style: Default,Arial,20,16777215,65535,65535,-2147483640,0,0,1,2,1,2,10,10,10,0,1

[Events]
Format: Marked, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
{string.Join(Environment.NewLine, filteredItems)}";
        }
    }
}
