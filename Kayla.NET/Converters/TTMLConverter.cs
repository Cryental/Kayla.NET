using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Kayla.NET.Models;

namespace Kayla.NET.Converters
{
    public class TTMLConverter : ISubtitleConverter
    {
        public string FileExtension { get; set; } = ".xml";
        public string Convert(List<SubtitleItem> data)
        {
            var filteredItems = new List<string>();

            for (var index = 0; index < data.Count; index++)
            {
                var f = data[index];

                if (string.IsNullOrEmpty(f.Text))
                {
                    continue;
                }

                var result = Regex.Replace(f.Text, "(\r\n|\r|\n)", @"<br />");

                result = result.Replace(Environment.NewLine, @"<br />");

                var startTime = new TimeSpan(f.StartTime * 10000).TotalSeconds;
                var endTime = new TimeSpan(f.EndTime * 10000).TotalSeconds;
                
                filteredItems.Add($"		<p begin=\"{startTime}s\" xml:id=\"p{index}\" end=\"{endTime}s\">{result}</p>");
            }

            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<tt xmlns=""http://www.w3.org/ns/ttml"" xmlns:ttp=""http://www.w3.org/ns/ttml#parameter"" ttp:timeBase=""media"" xmlns:tts=""http://www.w3.org/ns/ttml#styling"" xml:lang=""en"" xmlns:ttm=""http://www.w3.org/ns/ttml#metadata"">
  <body style=""s0"">
    <div>
{string.Join(Environment.NewLine, filteredItems)}
    </div>
  </body>
</tt>";
        }
    }
}
