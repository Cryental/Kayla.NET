using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter.Parsers
{
    public class SAMIParser : ISubtitleParser
    {
        public bool ParseFormat(string path, Encoding encoding, out List<Common> result)
        {
            var li = new List<Common>();
            StreamReader sr = new StreamReader(path, encoding);

            var line = sr.ReadLine();
            if (line == null || !line.Equals("<SAMI>"))
            {
                sr.Close();
                result = null;
                return false;
            }

            while ((line = sr.ReadLine()) != null)
                if (line.Equals("<BODY>"))
                    break;

            if (string.IsNullOrEmpty(line))
            {
                sr.Close();
                result = null;
                return false;
            }
            
            var check = false;
            var miClassString = new string[2];
            var sb = new StringBuilder();
            var sbComment = false;

            while (string.IsNullOrEmpty(line) != true)
            {
                if (check == false)
                {
                    line = sr.ReadLine();
                    
                    while (true)
                        if (string.IsNullOrEmpty(line))
                            line = sr.ReadLine();
                        else
                            break;
                }
                else
                {
                    check = false;
                }

                if (line.Contains("<--") && line.Contains("-->"))
                {
                    continue;
                }

                if (line.Contains("<!--"))
                {
                    sbComment = true;
                }

                if (line.Contains("-->"))
                {
                    sbComment = false;
                }

                if (sbComment)
                {
                    continue;
                }

                if (line.Contains("</BODY>")) break;
                if (line.Contains("</SAMI>")) break;

                if (line[0].Equals('<'))
                {
                    
                    var length = line.IndexOf('>');
                    miClassString[0] = line.Substring(1, length - 1);
                    miClassString[1] = line.Substring(length + 2);
                    var splitIndex = miClassString[1].IndexOf('>');
                    miClassString[1] = miClassString[1].Remove(splitIndex);
                    var miSync = miClassString[0].Split('=');

                    sb.Append(line);

                    while ((line = sr.ReadLine())?.ToUpper().Contains("<SYNC", StringComparison.OrdinalIgnoreCase) == false)
                    {
                        sb.Append(line);
                    }

                    
                    li.Add(new Common(int.Parse(miSync[1]), ConvertString(sb.ToString())));

                    sb = new StringBuilder();

                    check = true;
                }
            }

            sr.Close();
            result = li;
            return true;
        }


        private static string ConvertString(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("<BR>", "\n");
            str = str.Replace("&nbsp;", "");
            try
            {
                while (str.IndexOf("<", StringComparison.Ordinal) != -1)
                {
                    var i = str.IndexOf("<", StringComparison.Ordinal);
                    var j = str.IndexOf(">", StringComparison.Ordinal);
                    str = str.Remove(i, j - i + 1);
                }

                return str;
            }
            catch
            {
                return str;
            }
        }
    }
}