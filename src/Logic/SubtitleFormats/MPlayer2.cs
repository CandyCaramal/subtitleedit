﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Nikse.SubtitleEdit.Logic.SubtitleFormats
{
    public class MPlayer2 : SubtitleFormat
    {
        readonly Regex _regexMPlayer2Line = new Regex(@"^\[-?\d+]\[-?\d+].*$", RegexOptions.Compiled);

        public override string Extension
        {
            get { return ".mpl"; }
        }

        public override string Name
        {
            get { return "MPlayer2"; }
        }

        public override bool HasLineNumber
        {
            get { return false; }
        }

        public override bool IsTimeBased
        {
            get { return true; }
        }

        public override bool IsMine(List<string> lines, string fileName)
        {
            int errors = 0;
            List<string> trimmedLines = new List<string>();
            foreach (string line in lines)
            {
                if (line.Trim().Length > 0 && line.Contains("["))
                {
                    string s = RemoveIllegalSpacesAndFixEmptyCodes(line);
                    if (_regexMPlayer2Line.IsMatch(s))
                        trimmedLines.Add(line);
                    else 
                        errors++;
                }
                else
                {
                    errors++;
                }
            }

            return trimmedLines.Count > errors;
        }

        private string RemoveIllegalSpacesAndFixEmptyCodes(string line)
        {
            int index = line.IndexOf("]");
            if (index >= 0 && index < line.Length)
            {
                index = line.IndexOf("]", index + 1);
                if (index >= 0 && index + 1 < line.Length)
                {
                    if (line.IndexOf("[]") >= 0 && line.IndexOf("[]") < index)
                    {
                        line = line.Insert(line.IndexOf("[]") + 1, "0"); // set empty time codes to zero
                        index++;
                    }

                    while (line.IndexOf(" ") >= 0 && line.IndexOf(" ") < index)
                    {
                        line = line.Remove(line.IndexOf(" "), 1);
                        index--;
                    }
                }
            }
            return line;
        }


        public override string ToText(Subtitle subtitle, string title)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Paragraph p in subtitle.Paragraphs)
            {
                sb.Append("[");
                sb.Append(((int)(p.StartTime.TotalMilliseconds / 100)).ToString());
                sb.Append("][");
                sb.Append(((int)(p.EndTime.TotalMilliseconds / 100)).ToString());
                sb.Append("]");

                string text = p.Text.Replace(Environment.NewLine, "|");
                text = text.Replace("<b>", "{Y:b}");
                text = text.Replace("</b>", string.Empty);
                text = text.Replace("<i>", "{Y:i}");
                text = text.Replace("</i>", string.Empty);
                text = text.Replace("<u>", "{Y:u}");
                text = text.Replace("</u>", string.Empty);

                sb.AppendLine(text);
            }
            return sb.ToString().Trim();
        }

        public override void LoadSubtitle(Subtitle subtitle, List<string> lines, string fileName)
        {
            _errorCount = 0;

            foreach (string line in lines)
            {
                string s = RemoveIllegalSpacesAndFixEmptyCodes(line);
                if (_regexMPlayer2Line.IsMatch(s))
                {
                    try
                    {
                        
                        int textIndex = s.LastIndexOf("]") + 1;
                        if (textIndex < s.Length)
                        {
                            string text = s.Substring(textIndex);
                            string temp = s.Substring(0, textIndex - 1);
                            string[] frames = temp.Replace("][", ":").Replace("[", string.Empty).Replace("]", string.Empty).Split(':');

                            double startSeconds = double.Parse(frames[0]) / 10;
                            double endSeconds = double.Parse(frames[1]) / 10;
                            
                            if (startSeconds == 0 && subtitle.Paragraphs.Count > 0)
                            {
                                startSeconds = (subtitle.Paragraphs[subtitle.Paragraphs.Count-1].EndTime.TotalMilliseconds / 1000) + 0.1;
                            }
                            if (endSeconds == 0)
                            {
                                endSeconds = startSeconds;
                            }

                            subtitle.Paragraphs.Add(new Paragraph(text, startSeconds * 1000, endSeconds * 1000));
                        }
                    }
                    catch
                    {
                        _errorCount++;
                    }
                }
                else
                {
                    _errorCount++;
                }
            }
            subtitle.Renumber(1);
        }
    }
}
