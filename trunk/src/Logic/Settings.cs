﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

namespace Nikse.SubtitleEdit.Logic
{
    // The settings classes are build for easy xml-serilization (makes save/load code simple)
    // ...but the built-in serialization is too slow - so a custom (de-)serialization has been used!

    public class RecentFileEntry
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string VideoFileName { get; set; }
        public int FirstVisibleIndex { get; set; }
        public int FirstSelectedIndex { get; set; }
    }

    public class RecentFilesSettings
    {
        private const int MaxRecentFiles = 25;

        [XmlArrayItem("FileName")]
        public List<RecentFileEntry> Files { get; set; }

        public RecentFilesSettings()
        {
            Files = new List<RecentFileEntry>();
        }

        public void Add(string fileName, int firstVisibleIndex, int firstSelectedIndex, string videoFileName, string originalFileName)
        {
            var newList = new List<RecentFileEntry> { new RecentFileEntry() { FileName = fileName, FirstVisibleIndex = firstVisibleIndex, FirstSelectedIndex = firstSelectedIndex, VideoFileName = videoFileName, OriginalFileName = originalFileName } };
            int index = 0;
            foreach (var oldRecentFile in Files)
            {
                if (string.Compare(fileName, oldRecentFile.FileName, true) != 0 && index < MaxRecentFiles)
                    newList.Add(new RecentFileEntry() { FileName = oldRecentFile.FileName, FirstVisibleIndex = oldRecentFile.FirstVisibleIndex, FirstSelectedIndex = oldRecentFile.FirstSelectedIndex, VideoFileName = oldRecentFile.VideoFileName, OriginalFileName = oldRecentFile.OriginalFileName });
                index++;
            }
            Files = newList;
        }

        public void Add(string fileName, string videoFileName, string originalFileName)
        {
            var newList = new List<RecentFileEntry>();
            foreach (var oldRecentFile in Files)
            {
                if (string.Compare(fileName, oldRecentFile.FileName, true) == 0)
                    newList.Add(new RecentFileEntry() { FileName = oldRecentFile.FileName, FirstVisibleIndex = oldRecentFile.FirstVisibleIndex, FirstSelectedIndex = oldRecentFile.FirstSelectedIndex, VideoFileName = oldRecentFile.VideoFileName, OriginalFileName = oldRecentFile.OriginalFileName });
            }
            if (newList.Count == 0)
                newList.Add(new RecentFileEntry() { FileName = fileName, FirstVisibleIndex = -1, FirstSelectedIndex = -1, VideoFileName = videoFileName, OriginalFileName = originalFileName });

            int index = 0;
            foreach (var oldRecentFile in Files)
            {
                if (string.Compare(fileName, oldRecentFile.FileName, true) != 0 && index < MaxRecentFiles)
                    newList.Add(new RecentFileEntry() { FileName = oldRecentFile.FileName, FirstVisibleIndex = oldRecentFile.FirstVisibleIndex, FirstSelectedIndex = oldRecentFile.FirstSelectedIndex, VideoFileName = oldRecentFile.VideoFileName, OriginalFileName = oldRecentFile.OriginalFileName });
                index++;
            }
            Files = newList;
        }

    }

    public class ToolsSettings
    {
        public int StartSceneIndex { get; set; }
        public int EndSceneIndex { get; set; }
        public int VerifyPlaySeconds { get; set; }
        public int MergeLinesShorterThan { get; set; }
        public string MusicSymbol { get; set; }
        public string MusicSymbolToReplace { get; set; }
        public bool SpellCheckAutoChangeNames { get; set; }
        public string Interjections { get; set; }

        public ToolsSettings()
        {
            StartSceneIndex = 1;
            EndSceneIndex = 1;
            VerifyPlaySeconds = 2;
            MergeLinesShorterThan = 33;
            MusicSymbol = "♪";
            MusicSymbolToReplace = "âª â¶ â™ª âTª ã¢â™âª ?t×3 ?t¤3";
            SpellCheckAutoChangeNames = true;
            Interjections = "Ugh;Ughh;Hm;Hmm;Hmmm;Ahh;Whew;Phew;Gah;Oh;Ohh;Uh;Uhh;";
        }
    }

    public class WordListSettings
    {
        public string LastLanguage { get; set; }
        public string NamesEtcUrl { get; set; }
        public bool UseOnlineNamesEtc { get; set; }

        public WordListSettings()
        {
            LastLanguage = "en-US";
            NamesEtcUrl = "http://www.nikse.dk/se/Names_Etc.xml";
        }
    }

    public class SsaStyleSettings
    {
        public string FontName { get; set; }
        public double FontSize { get; set; }
        public int FontColorArgb { get; set; }

        public SsaStyleSettings()
        {
            FontName = "Tahoma";
            FontSize = 18;
            FontColorArgb = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
        }
    }

    public class ProxySettings
    {
        public string ProxyAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public string DecodePassword()
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(Password));
        }
        public void EncodePassword(string unencryptedPassword)
        {
            Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(unencryptedPassword));
        }
    }

    public class FixCommonErrorsSettings
    {
        public bool EmptyLinesTicked { get; set; }
        public bool OverlappingDisplayTimeTicked { get; set; }
        public bool TooShortDisplayTimeTicked { get; set; }
        public bool TooLongDisplayTimeTicked { get; set; }
        public bool InvalidItalicTagsTicked { get; set; }
        public bool BreakLongLinesTicked { get; set; }
        public bool MergeShortLinesTicked { get; set; }
        public bool MergeShortLinesAllTicked { get; set; }
        public bool UnneededSpacesTicked { get; set; }
        public bool UnneededPeriodsTicked { get; set; }
        public bool MissingSpacesTicked { get; set; }
        public bool AddMissingQuotesTicked { get; set; }
        public bool Fix3PlusLinesTicked { get; set; }
        public bool FixHyphensTicked { get; set; }
        public bool UppercaseIInsideLowercaseWordTicked { get; set; }
        public bool DoubleApostropheToQuoteTicked { get; set; }
        public bool AddPeriodAfterParagraphTicked { get; set; }
        public bool StartWithUppercaseLetterAfterParagraphTicked { get; set; }
        public bool StartWithUppercaseLetterAfterPeriodInsideParagraphTicked { get; set; }
        public bool AloneLowercaseIToUppercaseIEnglishTicked { get; set; }
        public bool FixOcrErrorsViaReplaceListTicked { get; set; }
        public bool DanishLetterITicked { get; set; }
        public bool SpanishInvertedQuestionAndExclamationMarksTicked { get; set; }
        public bool FixDoubleDashTicked { get; set; }
        public bool FixDoubleGreaterThanTicked { get; set; }
        public bool FixEllipsesStartTicked { get; set; }
        public bool FixMissingOpenBracketTicked { get; set; }
        public bool FixMusicNotationTicked { get; set; }

        public FixCommonErrorsSettings()
        {
            EmptyLinesTicked = true;
            OverlappingDisplayTimeTicked = true;
            TooShortDisplayTimeTicked  = true;
            TooLongDisplayTimeTicked = true;
            InvalidItalicTagsTicked = true; 
            BreakLongLinesTicked = true;
            MergeShortLinesTicked = true;
            UnneededPeriodsTicked = true;
            UnneededSpacesTicked = true;
            MissingSpacesTicked = true;
            UppercaseIInsideLowercaseWordTicked = true;
            DoubleApostropheToQuoteTicked = true;
            AddPeriodAfterParagraphTicked = false;
            StartWithUppercaseLetterAfterParagraphTicked = true;
            StartWithUppercaseLetterAfterPeriodInsideParagraphTicked = false;
            AloneLowercaseIToUppercaseIEnglishTicked = false;
            DanishLetterITicked = false;
            FixDoubleDashTicked = true;
            FixDoubleGreaterThanTicked = true;
            FixEllipsesStartTicked = true;
            FixMissingOpenBracketTicked = true;
            FixMusicNotationTicked = true;
        }
    }

    public class GeneralSettings
    {
        public bool ShowToolbarNew { get; set; }
        public bool ShowToolbarOpen { get; set; }
        public bool ShowToolbarSave { get; set; }
        public bool ShowToolbarSaveAs { get; set; }
        public bool ShowToolbarFind { get; set; }
        public bool ShowToolbarReplace { get; set; }
        public bool ShowToolbarVisualSync { get; set; }
        public bool ShowToolbarSpellCheck { get; set; }
        public bool ShowToolbarSettings { get; set; }
        public bool ShowToolbarHelp { get; set; }

        public bool ShowVideoPlayer { get; set; }
        public bool ShowWaveForm { get; set; }
        public bool ShowFrameRate { get; set; }
        public double DefaultFrameRate { get; set; }
        public string DefaultEncoding { get; set; }
        public bool AutoGuessAnsiEncoding { get; set; }
        public string SubtitleFontName { get; set; }
        public int SubtitleFontSize { get; set; }
        public bool SubtitleFontBold { get; set; }
        public Color SubtitleFontColor { get; set; }
        public Color SubtitleBackgroundColor { get; set; }
        public bool ShowRecentFiles { get; set; }
        public bool RememberSelectedLine { get; set; }
        public bool StartLoadLastFile { get; set; }
        public bool StartRememberPositionAndSize { get; set; }
        public string StartPosition { get; set; }
        public string StartSize { get; set; }
        public int StartListViewWidth { get; set; }
        public bool StartInSourceView { get; set; }
        public bool RemoveBlankLinesWhenOpening { get; set; }
        public int SubtitleLineMaximumLength { get; set; }
        public int SubtitleMaximumCharactersPerSeconds { get; set; }
        public string SpellCheckLanguage { get; set; }
        public string VideoPlayer { get; set; }
        public int VideoPlayerDefaultVolume { get; set; }
        public bool VideoPlayerShowStopButton { get; set; }
        public string Language { get; set; }
        public string ListViewLineSeparatorString { get; set; }
        public int ListViewDoubleClickAction { get; set; }
        public string UppercaseLetters { get; set; }
        public int DefaultAdjustMilliseconds { get; set; }
        public bool AutoRepeatOn { get; set; }
        public bool AutoContinueOn { get; set; }
        public bool SyncListViewWithVideoWhilePlaying { get; set; }
        public int AutoBackupSeconds { get; set; }
        public string SpellChecker { get; set; }
        public bool AllowEditOfOriginalSubtitle { get; set; }

        public GeneralSettings()
        {
            ShowToolbarNew = true;
            ShowToolbarOpen = true;
            ShowToolbarSave = true;
            ShowToolbarSaveAs = false;
            ShowToolbarFind = true;
            ShowToolbarReplace = true;
            ShowToolbarVisualSync = true;
            ShowToolbarSpellCheck = true;
            ShowToolbarSettings = false;
            ShowToolbarHelp = true;

            ShowVideoPlayer = false;
            ShowWaveForm = false;
            ShowFrameRate = false;
            DefaultFrameRate = 23.976;
            SubtitleFontName = "Tahoma";
            if (Environment.OSVersion.Version.Major < 6) // 6 == Vista/Win2008Server/Win7
                SubtitleFontName = "Courier New";

            SubtitleFontSize = 8;
            SubtitleFontBold = false;
            SubtitleFontColor = System.Drawing.Color.Black;
            SubtitleBackgroundColor = System.Drawing.Color.White;
            DefaultEncoding = "UTF-8";
            AutoGuessAnsiEncoding = false;
            ShowRecentFiles = true;
            RememberSelectedLine = false;
            StartLoadLastFile = true;
            StartRememberPositionAndSize = true;
            SubtitleLineMaximumLength = 65;
            SubtitleMaximumCharactersPerSeconds = 25;
            SpellCheckLanguage = null;
            VideoPlayer = string.Empty;
            VideoPlayerDefaultVolume = 50;
            VideoPlayerShowStopButton = true;
            ListViewLineSeparatorString = "<br />";
            ListViewDoubleClickAction = 1;
            UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWZYXÆØÃÅÄÖÉÈÁÂÀÇÉÊÍÓÔÕÚŁ";
            DefaultAdjustMilliseconds = 1000;
            AutoRepeatOn = true;
            AutoContinueOn = false;
            SyncListViewWithVideoWhilePlaying = false;
            AutoBackupSeconds = 0;
            SpellChecker = "hunspell";
            AllowEditOfOriginalSubtitle = false;
        }
    }


    public class VideoControlsSettings
    {
        public string CustomSearchText { get; set; }
        public string CustomSearchUrl { get; set; }
        public string LastActiveTab { get; set; }
        public bool WaveFormDrawGrid { get; set; }
        public Color WaveFormGridColor { get; set; }
        public Color WaveFormColor { get; set; }
        public Color WaveFormSelectedColor { get; set; }
        public Color WaveFormBackgroundColor { get; set; }
        public Color WaveFormTextColor { get; set; }

        public VideoControlsSettings()
        {
            CustomSearchText = "MS Encarta Thesaurus";
            CustomSearchUrl = "http://encarta.msn.com/encnet/features/dictionary/DictionaryResults.aspx?lextype=2&search={0}";
            LastActiveTab = "Translate";
            WaveFormDrawGrid = true;
            WaveFormGridColor = Color.FromArgb(255, 20, 20, 18);
            WaveFormColor = Color.GreenYellow;
            WaveFormSelectedColor = Color.Red;
            WaveFormBackgroundColor = Color.Black;
            WaveFormTextColor = Color.Gray;
        }
    }

    public class VobSubOcrSettings
    {
        public int XOrMorePixelsMakesSpace { get; set; }
        public double AllowDifferenceInPercent { get; set; }
        public string LastImageCompareFolder { get; set; }
        public int LastModiLanguageId { get; set; }
        public string LastOcrMethod { get; set; }
        public string TesseractLastLanguage { get; set; }
        public bool RightToLeft { get; set; }

        public VobSubOcrSettings()
        {
            XOrMorePixelsMakesSpace = 5;
            AllowDifferenceInPercent = 1.0;
            LastImageCompareFolder = "English";
            LastModiLanguageId = 9;
            LastOcrMethod = "Tesseract";
            RightToLeft = false;
        }
    }

    public class MultipleSearchAndReplaceSetting
    {
        public bool Enabled { get; set; }
        public string FindWhat { get; set; }
        public string ReplaceWith { get; set; }
        public string SearchType { get; set; }
    }

    public class NetworkSettings
    {
        public string UserName { get; set; }
        public string WebServiceUrl { get; set; }
        public string SessionKey { get; set; }

        public NetworkSettings()
        {
            UserName = string.Empty;
            SessionKey = "DemoSession"; // TODO - leave blank or use guid
            WebServiceUrl = "http://www.nikse.dk/se/SeService.asmx";
        }
    }

    public class Settings
    {
        public RecentFilesSettings RecentFiles { get; set; }
        public GeneralSettings General { get; set; }
        public ToolsSettings Tools { get; set; }
        public SsaStyleSettings SsaStyle { get; set; }
        public ProxySettings Proxy { get; set; }
        public WordListSettings WordLists { get; set; }
        public FixCommonErrorsSettings CommonErrors { get; set; }
        public VobSubOcrSettings VobSubOcr { get; set; }
        public VideoControlsSettings VideoControls { get; set; }
        public NetworkSettings NetworkSettings { get; set; }

        [XmlArrayItem("MultipleSearchAndReplaceItem")]
        public List<MultipleSearchAndReplaceSetting> MultipleSearchAndReplaceList { get; set; }

        [XmlIgnore]
        public Language Language { get; set; }

        private Settings()
        {
            RecentFiles = new RecentFilesSettings();
            General = new GeneralSettings();
            Tools = new ToolsSettings();
            WordLists = new WordListSettings();
            SsaStyle = new SsaStyleSettings();
            Proxy = new ProxySettings();
            CommonErrors = new FixCommonErrorsSettings();
            VobSubOcr = new VobSubOcrSettings();
            VideoControls = new VideoControlsSettings();
            NetworkSettings = new Logic.NetworkSettings();
            MultipleSearchAndReplaceList = new List<MultipleSearchAndReplaceSetting>();
            Language = new Language();
        }

        public void Save()
        {
            //slow
            //Serialize(Configuration.BaseDirectory + "Settings.xml", this);

            //Fast - TODO: Fix in release
            CustomSerialize(Configuration.SettingsFileName, this);
        }

        private static void Serialize(string fileName, Settings settings)
        {
            var s = new XmlSerializer(typeof(Settings));
            TextWriter w = new StreamWriter(fileName);
            s.Serialize(w, settings);
            w.Close();
        }

        public static Settings GetSettings()
        {
            Settings settings = new Settings();
            string settingsFileName = Configuration.SettingsFileName;
            if (File.Exists(settingsFileName))
            {
                try
                {
                    //TODO: Fix in release
                    settings = CustomDeserialize(settingsFileName); //  15 msecs

                    //too slow... :(
                    //settings = Deserialize(Configuration.BaseDirectory + "Settings.xml"); // 688 msecs
                }
                catch
                {
                    settings = new Settings();
                }

                if (string.IsNullOrEmpty(settings.General.ListViewLineSeparatorString))
                    settings.General.ListViewLineSeparatorString = Environment.NewLine;
            }

            return settings;
        }

        private static Settings Deserialize(string fileName)
        {
            var r = new StreamReader(fileName);
            var s = new XmlSerializer(typeof(Settings));
            var settings = (Settings)s.Deserialize(r);
            r.Close();

            if (settings.RecentFiles == null)
                settings.RecentFiles = new RecentFilesSettings();
            if (settings.General == null)
                settings.General = new GeneralSettings();
            if (settings.SsaStyle == null)
                settings.SsaStyle = new SsaStyleSettings();
            if (settings.CommonErrors == null)
                settings.CommonErrors = new FixCommonErrorsSettings();
            if (settings.VideoControls == null)
                settings.VideoControls = new VideoControlsSettings();
            if (settings.VobSubOcr == null)
                settings.VobSubOcr = new VobSubOcrSettings();
            if (settings.MultipleSearchAndReplaceList == null)
                settings.MultipleSearchAndReplaceList = new List<MultipleSearchAndReplaceSetting>();
            if (settings.NetworkSettings == null)
                settings.NetworkSettings = new NetworkSettings();

            return settings;
        }

        /// <summary>
        /// A faster serializer than xml serializer... which is insanely slow (first time)!!!!
        /// This method is auto-generated with XmlSerializerGenerator
        /// </summary>
        /// <param name="fileName">File name of xml settings file to load</param>
        /// <returns>Newly loaded settings</returns>
        private static Settings CustomDeserialize(string fileName)
        {
            var doc = new XmlDocument();
            doc.Load(fileName);

            Settings settings = new Settings();

            settings.RecentFiles = new Nikse.SubtitleEdit.Logic.RecentFilesSettings();
            XmlNode node = doc.DocumentElement.SelectSingleNode("RecentFiles");
            foreach (XmlNode listNode in node.SelectNodes("FileNames/FileName"))
            {
                string firstVisibleIndex = "-1";
                if (listNode.Attributes["FirstVisibleIndex"] != null)
                    firstVisibleIndex = listNode.Attributes["FirstVisibleIndex"].Value;

                string firstSelectedIndex = "-1";
                if (listNode.Attributes["FirstSelectedIndex"] != null)
                    firstSelectedIndex = listNode.Attributes["FirstSelectedIndex"].Value;

                string videoFileName = null;
                if (listNode.Attributes["VideoFileName"] != null)
                    videoFileName = listNode.Attributes["VideoFileName"].Value;

                string originalFileName = null;
                if (listNode.Attributes["OriginalFileName"] != null)
                    originalFileName = listNode.Attributes["OriginalFileName"].Value;

                settings.RecentFiles.Files.Add(new RecentFileEntry() { FileName = listNode.InnerText, FirstVisibleIndex = int.Parse(firstVisibleIndex), FirstSelectedIndex = int.Parse(firstSelectedIndex), VideoFileName = videoFileName, OriginalFileName = originalFileName });
            }


            settings.General = new Nikse.SubtitleEdit.Logic.GeneralSettings();
            node = doc.DocumentElement.SelectSingleNode("General");
            XmlNode subNode = node.SelectSingleNode("ShowToolbarNew");
            if (subNode != null)
                settings.General.ShowToolbarNew = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarOpen");
            if (subNode != null)
                settings.General.ShowToolbarOpen = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarSave");
            if (subNode != null)
                settings.General.ShowToolbarSave = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarSaveAs");
            if (subNode != null)
                settings.General.ShowToolbarSaveAs = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarFind");
            if (subNode != null)
                settings.General.ShowToolbarFind = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarReplace");
            if (subNode != null)
                settings.General.ShowToolbarReplace = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarVisualSync");
            if (subNode != null)
                settings.General.ShowToolbarVisualSync = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarSpellCheck");
            if (subNode != null)
                settings.General.ShowToolbarSpellCheck = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarSettings");
            if (subNode != null)
                settings.General.ShowToolbarSettings = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowToolbarHelp");
            if (subNode != null)
                settings.General.ShowToolbarHelp = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowFrameRate");
            if (subNode != null)
                settings.General.ShowFrameRate = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowVideoPlayer");
            if (subNode != null)
                settings.General.ShowVideoPlayer = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("ShowWaveForm");
            if (subNode != null)
                settings.General.ShowWaveForm = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("DefaultFrameRate");
            if (subNode != null)
                settings.General.DefaultFrameRate = Convert.ToDouble(subNode.InnerText);
            subNode = node.SelectSingleNode("DefaultEncoding");
            if (subNode != null)
                settings.General.DefaultEncoding = subNode.InnerText;
            subNode = node.SelectSingleNode("AutoGuessAnsiEncoding");
            if (subNode != null)
                settings.General.AutoGuessAnsiEncoding = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("SubtitleFontName");
            if (subNode != null)
                settings.General.SubtitleFontName = subNode.InnerText;
            subNode = node.SelectSingleNode("SubtitleFontSize");
            if (subNode != null)
                settings.General.SubtitleFontSize = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("SubtitleFontBold");
            if (subNode != null)
                settings.General.SubtitleFontBold = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("SubtitleFontColor");
            if (subNode != null)
                settings.General.SubtitleFontColor = Color.FromArgb(Convert.ToInt32(subNode.InnerText));
            subNode = node.SelectSingleNode("SubtitleBackgroundColor");
            if (subNode != null)
                settings.General.SubtitleBackgroundColor = Color.FromArgb(Convert.ToInt32(subNode.InnerText));
            subNode = node.SelectSingleNode("ShowRecentFiles");
            if (subNode != null)
                settings.General.ShowRecentFiles = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("RememberSelectedLine");
            if (subNode != null)
                settings.General.RememberSelectedLine = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("StartLoadLastFile");
            if (subNode != null)
                settings.General.StartLoadLastFile = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("StartRememberPositionAndSize");
            if (subNode != null)
                settings.General.StartRememberPositionAndSize = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("StartPosition");
            if (subNode != null)
                settings.General.StartPosition = subNode.InnerText;
            subNode = node.SelectSingleNode("StartSize");
            if (subNode != null)
                settings.General.StartSize = subNode.InnerText;
            subNode = node.SelectSingleNode("StartListViewWidth");
            if (subNode != null)
                settings.General.StartListViewWidth = Convert.ToInt32(subNode.InnerText);            
            subNode = node.SelectSingleNode("StartInSourceView");
            if (subNode != null)
                settings.General.StartInSourceView = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("RemoveBlankLinesWhenOpening");
            if (subNode != null)
                settings.General.RemoveBlankLinesWhenOpening = Convert.ToBoolean(subNode.InnerText);            
            subNode = node.SelectSingleNode("SubtitleLineMaximumLength");
            if (subNode != null)
                settings.General.SubtitleLineMaximumLength = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("SubtitleMaximumCharactersPerSeconds");
            if (subNode != null)
                settings.General.SubtitleMaximumCharactersPerSeconds = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("SpellCheckLanguage");
            if (subNode != null)
                settings.General.SpellCheckLanguage = subNode.InnerText;
            subNode = node.SelectSingleNode("VideoPlayer");
            if (subNode != null)
                settings.General.VideoPlayer = subNode.InnerText;
            subNode = node.SelectSingleNode("VideoPlayerDefaultVolume");
            if (subNode != null)
                settings.General.VideoPlayerDefaultVolume = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("VideoPlayerShowStopButton");
            if (subNode != null)
                settings.General.VideoPlayerShowStopButton = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("Language");
            if (subNode != null)
                settings.General.Language = subNode.InnerText;
            subNode = node.SelectSingleNode("ListViewLineSeparatorString");
            if (subNode != null)
                settings.General.ListViewLineSeparatorString = subNode.InnerText;
            subNode = node.SelectSingleNode("ListViewDoubleClickAction");
            if (subNode != null)
                settings.General.ListViewDoubleClickAction = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("UppercaseLetters");
            if (subNode != null)
                settings.General.UppercaseLetters = subNode.InnerText;
            subNode = node.SelectSingleNode("DefaultAdjustMilliseconds");
            if (subNode != null)
                settings.General.DefaultAdjustMilliseconds = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("AutoRepeatOn");
            if (subNode != null)
                settings.General.AutoRepeatOn = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("SyncListViewWithVideoWhilePlaying");
            if (subNode != null)
                settings.General.SyncListViewWithVideoWhilePlaying = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("AutoContinueOn");
            if (subNode != null)
                settings.General.AutoContinueOn = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("AutoBackupSeconds");
            if (subNode != null)
                settings.General.AutoBackupSeconds = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("SpellChecker");
            if (subNode != null)
                settings.General.SpellChecker = subNode.InnerText;
            subNode = node.SelectSingleNode("AllowEditOfOriginalSubtitle");
            if (subNode != null)
                settings.General.AllowEditOfOriginalSubtitle = Convert.ToBoolean(subNode.InnerText);
            
            settings.Tools = new Nikse.SubtitleEdit.Logic.ToolsSettings();
            node = doc.DocumentElement.SelectSingleNode("Tools");
            subNode = node.SelectSingleNode("StartSceneIndex");
            if (subNode != null)
                settings.Tools.StartSceneIndex = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("EndSceneIndex");
            if (subNode != null)
                settings.Tools.EndSceneIndex = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("VerifyPlaySeconds");
            if (subNode != null)
                settings.Tools.VerifyPlaySeconds = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("MergeLinesShorterThan");
            if (subNode != null)
                settings.Tools.MergeLinesShorterThan = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("MusicSymbol");
            if (subNode != null)
                settings.Tools.MusicSymbol = subNode.InnerText;
            subNode = node.SelectSingleNode("MusicSymbolToReplace");
            if (subNode != null)
                settings.Tools.MusicSymbolToReplace = subNode.InnerText;
            subNode = node.SelectSingleNode("SpellCheckAutoChangeNames");
            if (subNode != null)
                settings.Tools.SpellCheckAutoChangeNames = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("Interjections");
            if (subNode != null)
                settings.Tools.Interjections = subNode.InnerText;           

            settings.SsaStyle = new Nikse.SubtitleEdit.Logic.SsaStyleSettings();
            node = doc.DocumentElement.SelectSingleNode("SsaStyle");
            subNode = node.SelectSingleNode("FontName");
            if (subNode != null)
                settings.SsaStyle.FontName = subNode.InnerText;
            subNode = node.SelectSingleNode("FontSize");
            if (subNode != null)
                settings.SsaStyle.FontSize = Convert.ToDouble(subNode.InnerText);
            subNode = node.SelectSingleNode("FontColorArgb");
            if (subNode != null)
                settings.SsaStyle.FontColorArgb = Convert.ToInt32(subNode.InnerText);

            settings.Proxy = new Nikse.SubtitleEdit.Logic.ProxySettings();
            node = doc.DocumentElement.SelectSingleNode("Proxy");
            subNode = node.SelectSingleNode("ProxyAddress");
            if (subNode != null)
                settings.Proxy.ProxyAddress = subNode.InnerText;
            subNode = node.SelectSingleNode("UserName");
            if (subNode != null)
                settings.Proxy.UserName = subNode.InnerText;
            subNode = node.SelectSingleNode("Password");
            if (subNode != null)
                settings.Proxy.Password = subNode.InnerText;
            subNode = node.SelectSingleNode("Domain");
            if (subNode != null)
                settings.Proxy.Domain = subNode.InnerText;

            settings.WordLists = new Nikse.SubtitleEdit.Logic.WordListSettings();
            node = doc.DocumentElement.SelectSingleNode("WordLists");
            subNode = node.SelectSingleNode("LastLanguage");
            if (subNode != null)
                settings.WordLists.LastLanguage = subNode.InnerText;
            subNode = node.SelectSingleNode("NamesEtcUrl");
            if (subNode != null)
                settings.WordLists.NamesEtcUrl = subNode.InnerText;
            subNode = node.SelectSingleNode("UseOnlineNamesEtc");
            if (subNode != null)
                settings.WordLists.UseOnlineNamesEtc = Convert.ToBoolean(subNode.InnerText);

            settings.CommonErrors = new Nikse.SubtitleEdit.Logic.FixCommonErrorsSettings();
            node = doc.DocumentElement.SelectSingleNode("CommonErrors");
            subNode = node.SelectSingleNode("EmptyLinesTicked");
            if (subNode != null)
                settings.CommonErrors.EmptyLinesTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("OverlappingDisplayTimeTicked");
            if (subNode != null)
                settings.CommonErrors.OverlappingDisplayTimeTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("TooShortDisplayTimeTicked");
            if (subNode != null)
                settings.CommonErrors.TooShortDisplayTimeTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("TooLongDisplayTimeTicked");
            if (subNode != null)
                settings.CommonErrors.TooLongDisplayTimeTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("InvalidItalicTagsTicked");
            if (subNode != null)
                settings.CommonErrors.InvalidItalicTagsTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("BreakLongLinesTicked");
            if (subNode != null)
                settings.CommonErrors.BreakLongLinesTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("MergeShortLinesTicked");
            if (subNode != null)
                settings.CommonErrors.MergeShortLinesTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("UnneededSpacesTicked");
            if (subNode != null)
                settings.CommonErrors.UnneededSpacesTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("UnneededPeriodsTicked");
            if (subNode != null)
                settings.CommonErrors.UnneededPeriodsTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("MissingSpacesTicked");
            if (subNode != null)
                settings.CommonErrors.MissingSpacesTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("AddMissingQuotesTicked");
            if (subNode != null)
                settings.CommonErrors.AddMissingQuotesTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("Fix3PlusLinesTicked");
            if (subNode != null)
                settings.CommonErrors.Fix3PlusLinesTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("FixHyphensTicked");
            if (subNode != null)
                settings.CommonErrors.FixHyphensTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("UppercaseIInsideLowercaseWordTicked");
            if (subNode != null)
                settings.CommonErrors.UppercaseIInsideLowercaseWordTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("DoubleApostropheToQuoteTicked");
            if (subNode != null)
                settings.CommonErrors.DoubleApostropheToQuoteTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("AddPeriodAfterParagraphTicked");
            if (subNode != null)
                settings.CommonErrors.AddPeriodAfterParagraphTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("StartWithUppercaseLetterAfterParagraphTicked");
            if (subNode != null)
                settings.CommonErrors.StartWithUppercaseLetterAfterParagraphTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("StartWithUppercaseLetterAfterPeriodInsideParagraphTicked");
            if (subNode != null)
                settings.CommonErrors.StartWithUppercaseLetterAfterPeriodInsideParagraphTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("AloneLowercaseIToUppercaseIEnglishTicked");
            if (subNode != null)
                settings.CommonErrors.AloneLowercaseIToUppercaseIEnglishTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("FixOcrErrorsViaReplaceListTicked");
            if (subNode != null)
                settings.CommonErrors.FixOcrErrorsViaReplaceListTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("DanishLetterITicked");
            if (subNode != null)
                settings.CommonErrors.DanishLetterITicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("SpanishInvertedQuestionAndExclamationMarksTicked");
            if (subNode != null)
                settings.CommonErrors.SpanishInvertedQuestionAndExclamationMarksTicked = Convert.ToBoolean(subNode.InnerText);           
            subNode = node.SelectSingleNode("FixDoubleDashTicked");
            if (subNode != null)
                settings.CommonErrors.FixDoubleDashTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("FixDoubleGreaterThanTicked");
            if (subNode != null)
                settings.CommonErrors.FixDoubleGreaterThanTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("FixEllipsesStartTicked");
            if (subNode != null)
                settings.CommonErrors.FixEllipsesStartTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("FixMissingOpenBracketTicked");
            if (subNode != null)
                settings.CommonErrors.FixMissingOpenBracketTicked = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("FixMusicNotationTicked");
            if (subNode != null)
                settings.CommonErrors.FixMusicNotationTicked = Convert.ToBoolean(subNode.InnerText);

            settings.VideoControls = new VideoControlsSettings();
            node = doc.DocumentElement.SelectSingleNode("VideoControls");
            subNode = node.SelectSingleNode("CustomSearchText");
            if (subNode != null)
                settings.VideoControls.CustomSearchText = subNode.InnerText;
            subNode = node.SelectSingleNode("CustomSearchUrl");
            if (subNode != null)
                settings.VideoControls.CustomSearchUrl = subNode.InnerText;
            subNode = node.SelectSingleNode("LastActiveTab");
            if (subNode != null)
                settings.VideoControls.LastActiveTab = subNode.InnerText;
            subNode = node.SelectSingleNode("WaveFormDrawGrid");
            if (subNode != null)
                settings.VideoControls.WaveFormDrawGrid = Convert.ToBoolean(subNode.InnerText);
            subNode = node.SelectSingleNode("WaveFormGridColor");
            if (subNode != null)
                settings.VideoControls.WaveFormGridColor = Color.FromArgb(int.Parse(subNode.InnerText));
            subNode = node.SelectSingleNode("WaveFormColor");
            if (subNode != null)
                settings.VideoControls.WaveFormColor = Color.FromArgb(int.Parse(subNode.InnerText));
            subNode = node.SelectSingleNode("WaveFormSelectedColor");
            if (subNode != null)
                settings.VideoControls.WaveFormSelectedColor = Color.FromArgb(int.Parse(subNode.InnerText));
            subNode = node.SelectSingleNode("WaveFormBackgroundColor");
            if (subNode != null)
                settings.VideoControls.WaveFormBackgroundColor = Color.FromArgb(int.Parse(subNode.InnerText));
            subNode = node.SelectSingleNode("WaveFormTextColor");
            if (subNode != null)
                settings.VideoControls.WaveFormTextColor = Color.FromArgb(int.Parse(subNode.InnerText));

            settings.NetworkSettings = new NetworkSettings();
            node = doc.DocumentElement.SelectSingleNode("NetworkSettings");
            if (node != null)
            {
                subNode = node.SelectSingleNode("SessionKey");
                if (subNode != null)
                    settings.NetworkSettings.SessionKey = subNode.InnerText;
                subNode = node.SelectSingleNode("UserName");
                if (subNode != null)
                    settings.NetworkSettings.UserName = subNode.InnerText;
                subNode = node.SelectSingleNode("WebServiceUrl");
                if (subNode != null)
                    settings.NetworkSettings.WebServiceUrl = subNode.InnerText;
            }

            settings.VobSubOcr = new Nikse.SubtitleEdit.Logic.VobSubOcrSettings();
            node = doc.DocumentElement.SelectSingleNode("VobSubOcr");
            subNode = node.SelectSingleNode("XOrMorePixelsMakesSpace");
            if (subNode != null)
                settings.VobSubOcr.XOrMorePixelsMakesSpace = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("AllowDifferenceInPercent");
            if (subNode != null)
                settings.VobSubOcr.AllowDifferenceInPercent = Convert.ToDouble(subNode.InnerText);
            subNode = node.SelectSingleNode("LastImageCompareFolder");
            if (subNode != null)
                settings.VobSubOcr.LastImageCompareFolder = subNode.InnerText;
            subNode = node.SelectSingleNode("LastModiLanguageId");
            if (subNode != null)
                settings.VobSubOcr.LastModiLanguageId = Convert.ToInt32(subNode.InnerText);
            subNode = node.SelectSingleNode("LastOcrMethod");
            if (subNode != null)
                settings.VobSubOcr.LastOcrMethod = subNode.InnerText;
            subNode = node.SelectSingleNode("TesseractLastLanguage");
            if (subNode != null)
                settings.VobSubOcr.TesseractLastLanguage = subNode.InnerText;
            subNode = node.SelectSingleNode("RightToLeft");
            if (subNode != null)
                settings.VobSubOcr.RightToLeft = Convert.ToBoolean(subNode.InnerText);

            foreach (XmlNode listNode in doc.DocumentElement.SelectNodes("MultipleSearchAndReplaceList/MultipleSearchAndReplaceItem"))
            {
                MultipleSearchAndReplaceSetting item = new MultipleSearchAndReplaceSetting();
                subNode = listNode.SelectSingleNode("Enabled");
                if (subNode != null)
                    item.Enabled = Convert.ToBoolean(subNode.InnerText);
                subNode = listNode.SelectSingleNode("FindWhat");
                if (subNode != null)
                    item.FindWhat = subNode.InnerText;
                subNode = listNode.SelectSingleNode("ReplaceWith");
                if (subNode != null)
                    item.ReplaceWith = subNode.InnerText;
                subNode = listNode.SelectSingleNode("SearchType");
                if (subNode != null)
                    item.SearchType = subNode.InnerText;
                settings.MultipleSearchAndReplaceList.Add(item);
            }

            return settings;
        }

        private static void CustomSerialize(string fileName, Settings settings)
        {
            var textWriter = new XmlTextWriter(fileName, null) {Formatting = Formatting.Indented};
            textWriter.WriteStartDocument();

            textWriter.WriteStartElement("Settings", "");

            textWriter.WriteStartElement("RecentFiles", "");
            textWriter.WriteStartElement("FileNames", "");
            foreach (var item in settings.RecentFiles.Files)
            {
                textWriter.WriteStartElement("FileName");
                if (item.OriginalFileName != null)
                    textWriter.WriteAttributeString("OriginalFileName", item.OriginalFileName);
                if (item.VideoFileName != null)
                    textWriter.WriteAttributeString("VideoFileName", item.VideoFileName);
                textWriter.WriteAttributeString("FirstVisibleIndex", item.FirstVisibleIndex.ToString());
                textWriter.WriteAttributeString("FirstSelectedIndex", item.FirstSelectedIndex.ToString());
                textWriter.WriteString(item.FileName);
                textWriter.WriteEndElement();
            }
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("General", "");
            textWriter.WriteElementString("ShowToolbarNew", settings.General.ShowToolbarNew.ToString());
            textWriter.WriteElementString("ShowToolbarOpen", settings.General.ShowToolbarOpen.ToString());
            textWriter.WriteElementString("ShowToolbarSave", settings.General.ShowToolbarSave.ToString());
            textWriter.WriteElementString("ShowToolbarSaveAs", settings.General.ShowToolbarSaveAs.ToString());
            textWriter.WriteElementString("ShowToolbarFind", settings.General.ShowToolbarFind.ToString());
            textWriter.WriteElementString("ShowToolbarReplace", settings.General.ShowToolbarReplace.ToString());
            textWriter.WriteElementString("ShowToolbarVisualSync", settings.General.ShowToolbarVisualSync.ToString());
            textWriter.WriteElementString("ShowToolbarSpellCheck", settings.General.ShowToolbarSpellCheck.ToString());
            textWriter.WriteElementString("ShowToolbarSettings", settings.General.ShowToolbarSettings.ToString());
            textWriter.WriteElementString("ShowToolbarHelp", settings.General.ShowToolbarHelp.ToString());
            textWriter.WriteElementString("ShowFrameRate", settings.General.ShowFrameRate.ToString());
            textWriter.WriteElementString("ShowVideoPlayer", settings.General.ShowVideoPlayer.ToString());
            textWriter.WriteElementString("ShowWaveForm", settings.General.ShowWaveForm.ToString());
            textWriter.WriteElementString("DefaultFrameRate", settings.General.DefaultFrameRate.ToString());
            textWriter.WriteElementString("DefaultEncoding", settings.General.DefaultEncoding);
            textWriter.WriteElementString("AutoGuessAnsiEncoding", settings.General.AutoGuessAnsiEncoding.ToString());
            textWriter.WriteElementString("SubtitleFontName", settings.General.SubtitleFontName);
            textWriter.WriteElementString("SubtitleFontSize", settings.General.SubtitleFontSize.ToString());
            textWriter.WriteElementString("SubtitleFontBold", settings.General.SubtitleFontBold.ToString());
            textWriter.WriteElementString("SubtitleFontColor", settings.General.SubtitleFontColor.ToArgb().ToString());
            textWriter.WriteElementString("SubtitleBackgroundColor", settings.General.SubtitleBackgroundColor.ToArgb().ToString());
            textWriter.WriteElementString("ShowRecentFiles", settings.General.ShowRecentFiles.ToString());
            textWriter.WriteElementString("RememberSelectedLine", settings.General.RememberSelectedLine.ToString());            
            textWriter.WriteElementString("StartLoadLastFile", settings.General.StartLoadLastFile.ToString());
            textWriter.WriteElementString("StartRememberPositionAndSize", settings.General.StartRememberPositionAndSize.ToString());
            textWriter.WriteElementString("StartPosition", settings.General.StartPosition);
            textWriter.WriteElementString("StartSize", settings.General.StartSize);
            textWriter.WriteElementString("StartListViewWidth", settings.General.StartListViewWidth.ToString());
            textWriter.WriteElementString("StartInSourceView", settings.General.StartInSourceView.ToString());
            textWriter.WriteElementString("RemoveBlankLinesWhenOpening", settings.General.RemoveBlankLinesWhenOpening.ToString());            
            textWriter.WriteElementString("SubtitleLineMaximumLength", settings.General.SubtitleLineMaximumLength.ToString());
            textWriter.WriteElementString("SubtitleMaximumCharactersPerSeconds", settings.General.SubtitleMaximumCharactersPerSeconds.ToString());            
            textWriter.WriteElementString("SpellCheckLanguage", settings.General.SpellCheckLanguage);
            textWriter.WriteElementString("VideoPlayer", settings.General.VideoPlayer);
            textWriter.WriteElementString("VideoPlayerDefaultVolume", settings.General.VideoPlayerDefaultVolume.ToString());
            textWriter.WriteElementString("VideoPlayerShowStopButton", settings.General.VideoPlayerShowStopButton.ToString());
            textWriter.WriteElementString("Language", settings.General.Language);
            textWriter.WriteElementString("ListViewLineSeparatorString", settings.General.ListViewLineSeparatorString);
            textWriter.WriteElementString("ListViewDoubleClickAction", settings.General.ListViewDoubleClickAction.ToString());
            textWriter.WriteElementString("UppercaseLetters", settings.General.UppercaseLetters);
            textWriter.WriteElementString("DefaultAdjustMilliseconds", settings.General.DefaultAdjustMilliseconds.ToString());
            textWriter.WriteElementString("AutoRepeatOn", settings.General.AutoRepeatOn.ToString());
            textWriter.WriteElementString("AutoContinueOn", settings.General.AutoContinueOn.ToString());
            textWriter.WriteElementString("SyncListViewWithVideoWhilePlaying", settings.General.SyncListViewWithVideoWhilePlaying.ToString());
            textWriter.WriteElementString("AutoBackupSeconds", settings.General.AutoBackupSeconds.ToString());
            textWriter.WriteElementString("SpellChecker", settings.General.SpellChecker);
            textWriter.WriteElementString("AllowEditOfOriginalSubtitle", settings.General.AllowEditOfOriginalSubtitle.ToString());
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("Tools", "");
            textWriter.WriteElementString("StartSceneIndex", settings.Tools.StartSceneIndex.ToString());
            textWriter.WriteElementString("EndSceneIndex", settings.Tools.EndSceneIndex.ToString());
            textWriter.WriteElementString("VerifyPlaySeconds", settings.Tools.VerifyPlaySeconds.ToString());
            textWriter.WriteElementString("MergeLinesShorterThan", settings.Tools.MergeLinesShorterThan.ToString());
            textWriter.WriteElementString("MusicSymbol", settings.Tools.MusicSymbol);
            textWriter.WriteElementString("MusicSymbolToReplace", settings.Tools.MusicSymbolToReplace);
            textWriter.WriteElementString("SpellCheckAutoChangeNames", settings.Tools.SpellCheckAutoChangeNames.ToString());
            textWriter.WriteElementString("Interjections", settings.Tools.Interjections);            
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("SsaStyle", "");
            textWriter.WriteElementString("FontName", settings.SsaStyle.FontName);
            textWriter.WriteElementString("FontSize", settings.SsaStyle.FontSize.ToString());
            textWriter.WriteElementString("FontColorArgb", settings.SsaStyle.FontColorArgb.ToString());
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("Proxy", "");
            textWriter.WriteElementString("ProxyAddress", settings.Proxy.ProxyAddress);
            textWriter.WriteElementString("UserName", settings.Proxy.UserName);
            textWriter.WriteElementString("Password", settings.Proxy.Password);
            textWriter.WriteElementString("Domain", settings.Proxy.Domain);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("WordLists", "");
            textWriter.WriteElementString("LastLanguage", settings.WordLists.LastLanguage);
            textWriter.WriteElementString("NamesEtcUrl", settings.WordLists.NamesEtcUrl);
            textWriter.WriteElementString("UseOnlineNamesEtc", settings.WordLists.UseOnlineNamesEtc.ToString());
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("CommonErrors", "");
            textWriter.WriteElementString("EmptyLinesTicked", settings.CommonErrors.EmptyLinesTicked.ToString());
            textWriter.WriteElementString("OverlappingDisplayTimeTicked", settings.CommonErrors.OverlappingDisplayTimeTicked.ToString());
            textWriter.WriteElementString("TooShortDisplayTimeTicked", settings.CommonErrors.TooShortDisplayTimeTicked.ToString());
            textWriter.WriteElementString("TooLongDisplayTimeTicked", settings.CommonErrors.TooLongDisplayTimeTicked.ToString());
            textWriter.WriteElementString("InvalidItalicTagsTicked", settings.CommonErrors.InvalidItalicTagsTicked.ToString());
            textWriter.WriteElementString("BreakLongLinesTicked", settings.CommonErrors.BreakLongLinesTicked.ToString());
            textWriter.WriteElementString("MergeShortLinesTicked", settings.CommonErrors.MergeShortLinesTicked.ToString());
            textWriter.WriteElementString("UnneededSpacesTicked", settings.CommonErrors.UnneededSpacesTicked.ToString());
            textWriter.WriteElementString("UnneededPeriodsTicked", settings.CommonErrors.UnneededPeriodsTicked.ToString());
            textWriter.WriteElementString("MissingSpacesTicked", settings.CommonErrors.MissingSpacesTicked.ToString());
            textWriter.WriteElementString("AddMissingQuotesTicked", settings.CommonErrors.AddMissingQuotesTicked.ToString());
            textWriter.WriteElementString("Fix3PlusLinesTicked", settings.CommonErrors.Fix3PlusLinesTicked.ToString());
            textWriter.WriteElementString("FixHyphensTicked", settings.CommonErrors.FixHyphensTicked.ToString());
            textWriter.WriteElementString("UppercaseIInsideLowercaseWordTicked", settings.CommonErrors.UppercaseIInsideLowercaseWordTicked.ToString());
            textWriter.WriteElementString("DoubleApostropheToQuoteTicked", settings.CommonErrors.DoubleApostropheToQuoteTicked.ToString());
            textWriter.WriteElementString("AddPeriodAfterParagraphTicked", settings.CommonErrors.AddPeriodAfterParagraphTicked.ToString());
            textWriter.WriteElementString("StartWithUppercaseLetterAfterParagraphTicked", settings.CommonErrors.StartWithUppercaseLetterAfterParagraphTicked.ToString());
            textWriter.WriteElementString("StartWithUppercaseLetterAfterPeriodInsideParagraphTicked", settings.CommonErrors.StartWithUppercaseLetterAfterPeriodInsideParagraphTicked.ToString());
            textWriter.WriteElementString("AloneLowercaseIToUppercaseIEnglishTicked", settings.CommonErrors.AloneLowercaseIToUppercaseIEnglishTicked.ToString());
            textWriter.WriteElementString("FixOcrErrorsViaReplaceListTicked", settings.CommonErrors.FixOcrErrorsViaReplaceListTicked.ToString());
            textWriter.WriteElementString("DanishLetterITicked", settings.CommonErrors.DanishLetterITicked.ToString());
            textWriter.WriteElementString("SpanishInvertedQuestionAndExclamationMarksTicked", settings.CommonErrors.SpanishInvertedQuestionAndExclamationMarksTicked.ToString());            
            textWriter.WriteElementString("FixDoubleDashTicked", settings.CommonErrors.FixDoubleDashTicked.ToString());
            textWriter.WriteElementString("FixDoubleGreaterThanTicked", settings.CommonErrors.FixDoubleGreaterThanTicked.ToString());
            textWriter.WriteElementString("FixEllipsesStartTicked", settings.CommonErrors.FixEllipsesStartTicked.ToString());
            textWriter.WriteElementString("FixMissingOpenBracketTicked", settings.CommonErrors.FixMissingOpenBracketTicked.ToString());
            textWriter.WriteElementString("FixMusicNotationTicked", settings.CommonErrors.FixMusicNotationTicked.ToString());
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("VideoControls", "");
            textWriter.WriteElementString("CustomSearchText", settings.VideoControls.CustomSearchText);
            textWriter.WriteElementString("CustomSearchUrl", settings.VideoControls.CustomSearchUrl);
            textWriter.WriteElementString("LastActiveTab", settings.VideoControls.LastActiveTab);
            textWriter.WriteElementString("WaveFormDrawGrid", settings.VideoControls.WaveFormDrawGrid.ToString());
            textWriter.WriteElementString("WaveFormGridColor", settings.VideoControls.WaveFormGridColor.ToArgb().ToString());
            textWriter.WriteElementString("WaveFormColor", settings.VideoControls.WaveFormColor.ToArgb().ToString());
            textWriter.WriteElementString("WaveFormSelectedColor", settings.VideoControls.WaveFormSelectedColor.ToArgb().ToString());
            textWriter.WriteElementString("WaveFormBackgroundColor", settings.VideoControls.WaveFormBackgroundColor.ToArgb().ToString());
            textWriter.WriteElementString("WaveFormTextColor", settings.VideoControls.WaveFormTextColor.ToArgb().ToString());
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("NetworkSettings", "");
            textWriter.WriteElementString("SessionKey", settings.NetworkSettings.SessionKey);
            textWriter.WriteElementString("UserName", settings.NetworkSettings.UserName);
            textWriter.WriteElementString("WebServiceUrl", settings.NetworkSettings.WebServiceUrl);
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("VobSubOcr", "");
            textWriter.WriteElementString("XOrMorePixelsMakesSpace", settings.VobSubOcr.XOrMorePixelsMakesSpace.ToString());
            textWriter.WriteElementString("AllowDifferenceInPercent", settings.VobSubOcr.AllowDifferenceInPercent.ToString());
            textWriter.WriteElementString("LastImageCompareFolder", settings.VobSubOcr.LastImageCompareFolder);
            textWriter.WriteElementString("LastModiLanguageId", settings.VobSubOcr.LastModiLanguageId.ToString());
            textWriter.WriteElementString("LastOcrMethod", settings.VobSubOcr.LastOcrMethod);
            textWriter.WriteElementString("TesseractLastLanguage", settings.VobSubOcr.TesseractLastLanguage);
            textWriter.WriteElementString("RightToLeft", settings.VobSubOcr.RightToLeft.ToString());
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("MultipleSearchAndReplaceList", "");
            foreach (var item in settings.MultipleSearchAndReplaceList)
            {
                textWriter.WriteStartElement("MultipleSearchAndReplaceItem", "");
                textWriter.WriteElementString("Enabled", item.Enabled.ToString());
                textWriter.WriteElementString("FindWhat", item.FindWhat);
                textWriter.WriteElementString("ReplaceWith", item.ReplaceWith);
                textWriter.WriteElementString("SearchType", item.SearchType);
                textWriter.WriteEndElement();
            }
            textWriter.WriteEndElement();

            textWriter.WriteEndElement();

            textWriter.WriteEndDocument();
            textWriter.Close();
        }

    }
}
