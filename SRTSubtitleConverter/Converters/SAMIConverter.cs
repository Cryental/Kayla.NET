using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Converters
{
    public class SAMIConverter : ISubtitleConverter
    {
        public string Convert(List<SubtitleItem> data)
        {
            var ConvertedItems = new List<string>();

            foreach (var f in data)
            {
                string finalResult;

                if (string.IsNullOrEmpty(f.Text))
                {
                    finalResult = "&nbsp;";
                }
                else
                {
                    finalResult = f.Text.Replace(Environment.NewLine, "<br>")
                        .Replace("\r", "<br>")
                        .Replace("\n", "<br>");
                }

                ConvertedItems.Add(GenerateItem(f.StartTime, finalResult));
            }

            var listedItems = string.Join(Environment.NewLine, ConvertedItems.ToArray());
            var finalOutput = GenerateTemplate(listedItems);

            return finalOutput;
        }

        private string GenerateTemplate(string bodyContent)
        {
            var template = $@"<SAMI>
<HEAD>
<TITLE></TITLE>
</HEAD>
<BODY>
{bodyContent}
</BODY>
</SAMI>";
            return template;
        }

        private string GenerateItem(long startTime, string text)
        {
            var template = $"<SYNC Start={startTime}><P>{text}";
            return template;
        }
    }
}