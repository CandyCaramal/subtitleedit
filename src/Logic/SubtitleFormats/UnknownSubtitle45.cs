﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Nikse.SubtitleEdit.Logic.SubtitleFormats
{
    public class UnknownSubtitle45 : SubtitleFormat
    {
        //*         00001.00-00003.00 02.01 00.0 1 0001 00 16-090-090
        //*         00138.10-00143.05 00.00 00.0 1 0003 00 16-090-090
        static Regex regexTimeCodes = new Regex(@"^\*\s+\d\d\d\d\d\.\d\d-\d\d\d\d\d\.\d\d \d\d.\d\d \d\d.\d\ \d \d\d\d\d \d\d \d\d-\d\d\d-\d\d\d$", RegexOptions.Compiled);

        public override string Extension
        {
            get { return ".rtf"; }
        }

        public override string Name
        {
            get { return "Unknown 45"; }
        }

        public override bool IsTimeBased
        {
            get { return true; }
        }

        public override bool IsMine(List<string> lines, string fileName)
        {
            if (fileName != null && !fileName.ToLower().EndsWith(Extension))
                return false;

            var subtitle = new Subtitle();
            LoadSubtitle(subtitle, lines, fileName);
            return subtitle.Paragraphs.Count > _errorCount;
        }

        public override string ToText(Subtitle subtitle, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"0 2 1.0 1.0 3.0 048 0400 0040 0500 100 100 0 100 0 6600 6600 01
CRULIC R1
ST 0 EB 3.10
@");


            int index = 0;
            foreach (Paragraph p in subtitle.Paragraphs)
            {
                //1 00:50:34:22 00:50:39:13
                //Ich muss dafür sorgen,
                //dass die Epsteins weiterleben
                index++;
                sb.AppendLine(string.Format("*         {0}-{1} 00.00 00.0 1 {2} 00 16-090-090{3}{4}{3}@", EncodeTimeCode(p.StartTime), EncodeTimeCode(p.EndTime), index.ToString().PadLeft(4, '0'), Environment.NewLine, Utilities.RemoveHtmlTags(p.Text)));
            }
            System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
            rtBox.Text = sb.ToString();
            return rtBox.Rtf;
        }

        private string EncodeTimeCode(TimeCode time)
        {
            return string.Format("{0:00000}.{1:00}", time.TotalSeconds, MillisecondsToFramesMaxFrameRate(time.Milliseconds));
        }

        public override void LoadSubtitle(Subtitle subtitle, List<string> lines, string fileName)
        {
//*         00001.00-00003.00 02.01 00.0 1 0001 00 16-090-090
//CRULIC R1
//pour Bobi
//@
            _errorCount = 0;
            var sb = new StringBuilder();
            foreach (string line in lines)
                sb.AppendLine(line);

            string rtf = sb.ToString().Trim();
            if (!rtf.StartsWith("{\\rtf"))
                return;

            var rtBox = new System.Windows.Forms.RichTextBox();
            try
            {
                rtBox.Rtf = rtf;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
                return;
            }

            Paragraph p = null;
            subtitle.Paragraphs.Clear();
            foreach (string line in rtBox.Text.Replace("\r\n", "\n").Split('\n'))
            {
                if (regexTimeCodes.IsMatch(line.Trim()))
                {
                    string[] temp = line.Substring(1).Trim().Substring(0, 17).Split('-');
                    if (temp.Length == 2)
                    {
                        string start = temp[0];
                        string end = temp[1];

                        string[] startParts = start.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] endParts = end.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (startParts.Length == 2 && endParts.Length == 2)
                        {
                            p = new Paragraph(DecodeTimeCode(startParts), DecodeTimeCode(endParts), string.Empty);
                            subtitle.Paragraphs.Add(p);
                        }
                    }
                }
                else if (line.Trim().Length == 0 || line.Trim() == "@")
                {
                    // skip these lines
                }
                else if (line.Trim().Length > 0 && p != null)
                {
                    if (p.Text.Length > 2000)
                        return; // wrong format
                    else if (string.IsNullOrEmpty(p.Text))
                        p.Text = line;
                    else
                        p.Text = p.Text + Environment.NewLine + line;
                }
            }
            subtitle.Renumber(1);
        }

        private TimeCode DecodeTimeCode(string[] parts)
        {
            //00119.12
            string seconds = parts[0];
            string frames = parts[1];
            TimeCode tc = new TimeCode(0, 0, int.Parse(seconds), FramesToMillisecondsMax999(int.Parse(frames)));
            return tc;
        }

    }
}