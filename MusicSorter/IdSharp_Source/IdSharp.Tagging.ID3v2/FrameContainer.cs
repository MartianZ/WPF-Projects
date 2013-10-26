namespace IdSharp.Tagging.ID3v2
{
    using IdSharp.ComInterop;
    using IdSharp.Tagging.ID3v2.Frames;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using IdSharp.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class FrameContainer : IFrameContainer, INotifyPropertyChanged, INotifyInvalidData
    {
        public event InvalidDataEventHandler InvalidData;
        public event PropertyChangedEventHandler PropertyChanged;

        public FrameContainer()
        {
            this.m_FrameBinder = new FrameBinder(this);
            this.m_UnknownFrames = new List<UnknownFrame>();
            this.m_ID3v24SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            this.m_ID3v24MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();
            this.m_ID3v23SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            this.m_ID3v23MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();
            this.m_ID3v22SingleOccurrenceFrames = new Dictionary<string, IFrame>();
            this.m_ID3v22MultipleOccurrenceFrames = new Dictionary<string, IBindingList>();
            this.m_ID3v24FrameAliases = new Dictionary<string, string>();
            this.m_ID3v23FrameAliases = new Dictionary<string, string>();
            this.m_AttachedPictureList = new AttachedPictureBindingList();
            this.m_UserDefinedUrlList = new UserDefinedUrlBindingList();
            this.m_CommentsList = new CommentsBindingList();
            this.m_CommercialInfoUrlList = new UrlBindingList("WCOM", "WCOM", "WCM");
            this.m_ArtistUrlList = new UrlBindingList("WOAR", "WOAR", "WAR");
            this.m_UserDefinedTextList = new UserDefinedTextBindingList();
            this.m_RelativeVolumeAdjustmentList = new RelativeVolumeAdjustmentBindingList();
            this.m_UnsynchronizedLyricsList = new UnsynchronizedLyricsBindingList();
            this.m_GeneralEncapsulatedObjectList = new GeneralEncapsulatedObjectBindingList();
            this.m_UniqueFileIdentifierList = new UniqueFileIdentifierBindingList();
            this.m_PrivateFrameList = new PrivateFrameBindingList();
            this.m_PopularimeterList = new PopularimeterBindingList();
            this.m_TermsOfUseList = new TermsOfUseBindingList();
            this.m_LinkedInformationList = new LinkedInformationBindingList();
            this.m_CommercialInfoList = new CommercialBindingList();
            this.m_EncryptionMethodList = new EncryptionMethodBindingList();
            this.m_GroupIdentificationList = new GroupIdentificationBindingList();
            this.m_SignatureList = new SignatureBindingList();
            this.m_AudioEncryptionList = new AudioEncryptionBindingList();
            this.m_EncryptedMetaFrameList = new EncryptedMetaFrameBindingList();
            this.m_SynchronizedLyricsList = new SynchronizedTextBindingList();
            this.m_EqualizationList = new EqualizationListBindingList();
            this.m_AudioTextList = new AudioTextBindingList();
            this.AddMultipleOccurrenceFrame("APIC", "APIC", "PIC", this.m_AttachedPictureList);
            this.AddMultipleOccurrenceFrame("WXXX", "WXXX", "WXX", this.m_UserDefinedUrlList);
            this.AddMultipleOccurrenceFrame("COMM", "COMM", "COM", this.m_CommentsList);
            this.AddMultipleOccurrenceFrame("WCOM", "WCOM", "WCM", this.m_CommercialInfoUrlList);
            this.AddMultipleOccurrenceFrame("WOAR", "WOAR", "WAR", this.m_ArtistUrlList);
            this.AddMultipleOccurrenceFrame("TXXX", "TXXX", "TXX", this.m_UserDefinedTextList);
            this.AddMultipleOccurrenceFrame("RVA2", "RVAD", "RVA", this.m_RelativeVolumeAdjustmentList);
            this.AddMultipleOccurrenceFrame("USLT", "USLT", "ULT", this.m_UnsynchronizedLyricsList);
            this.AddMultipleOccurrenceFrame("GEOB", "GEOB", "GEO", this.m_GeneralEncapsulatedObjectList);
            this.AddMultipleOccurrenceFrame("UFID", "UFID", "UFI", this.m_UniqueFileIdentifierList);
            this.AddMultipleOccurrenceFrame("PRIV", "PRIV", null, this.m_PrivateFrameList);
            this.AddMultipleOccurrenceFrame("POPM", "POPM", "POP", this.m_PopularimeterList);
            this.AddMultipleOccurrenceFrame("USER", "USER", null, this.m_TermsOfUseList);
            this.AddMultipleOccurrenceFrame("LINK", "LINK", "LNK", this.m_LinkedInformationList);
            this.AddMultipleOccurrenceFrame("AENC", "AENC", "CRA", this.m_AudioEncryptionList);
            this.AddMultipleOccurrenceFrame(null, null, "CRM", this.m_EncryptedMetaFrameList);
            this.AddMultipleOccurrenceFrame("SYLT", "SYLT", "SLT", this.m_SynchronizedLyricsList);
            this.AddMultipleOccurrenceFrame("EQU2", "EQUA", "EQU", this.m_EqualizationList);
            this.AddMultipleOccurrenceFrame("COMR", "COMR", null, this.m_CommercialInfoList);
            this.AddMultipleOccurrenceFrame("ENCR", "ENCR", null, this.m_EncryptionMethodList);
            this.AddMultipleOccurrenceFrame("GRID", "GRID", null, this.m_GroupIdentificationList);
            this.AddMultipleOccurrenceFrame("SIGN", "SIGN", null, this.m_SignatureList);
            this.AddMultipleOccurrenceFrame("ATXT", "ATXT", null, this.m_AudioTextList);
            this.m_Title = this.CreateTextFrame("TIT2", "TIT2", "TT2", "Title", null);
            this.m_Album = this.CreateTextFrame("TALB", "TALB", "TAL", "Album", null);
            this.m_EncodedByWho = this.CreateTextFrame("TENC", "TENC", "TEN", "EncodedByWho", null);
            this.m_Artist = this.CreateTextFrame("TPE1", "TPE1", "TP1", "Artist", null);
            this.m_Year = this.CreateTextFrame("TYER", "TYER", "TYE", "Year", new MethodInvoker(this.ValidateYear));
            this.m_DateRecorded = this.CreateTextFrame("TDAT", "TDAT", "TDA", "DateRecorded", new MethodInvoker(this.ValidateDateRecorded));
            this.m_TimeRecorded = this.CreateTextFrame("TIME", "TIME", "TIM", "TimeRecorded", new MethodInvoker(this.ValidateTimeRecorded));
            this.m_Genre = this.CreateTextFrame("TCON", "TCON", "TCO", "Genre", null);
            this.m_Composer = this.CreateTextFrame("TCOM", "TCOM", "TCM", "Composer", null);
            this.m_OriginalArtist = this.CreateTextFrame("TOPE", "TOPE", "TOA", "OriginalArtist", null);
            this.m_Copyright = this.CreateTextFrame("TCOP", "TCOP", "TCR", "Copyright", new MethodInvoker(this.ValidateCopyright));
            this.m_RemixedBy = this.CreateTextFrame("TPE4", "TPE4", "TP4", "RemixedBy", null);
            this.m_Publisher = this.CreateTextFrame("TPUB", "TPUB", "TPB", "Publisher", null);
            this.m_InternetRadioStationName = this.CreateTextFrame("TRSN", "TRSN", null, "InternetRadioStationName", null);
            this.m_InternetRadioStationOwner = this.CreateTextFrame("TRSO", "TRSO", null, "InternetRadioStationOwner", null);
            this.m_Accompaniment = this.CreateTextFrame("TPE2", "TPE2", "TP2", "Accompaniment", null);
            this.m_Conductor = this.CreateTextFrame("TPE3", "TPE3", "TP3", "Conductor", null);
            this.m_Lyricist = this.CreateTextFrame("TEXT", "TEXT", "TXT", "Lyricist", null);
            this.m_OriginalLyricist = this.CreateTextFrame("TOLY", "TOLY", "TOL", "OriginalLyricist", null);
            this.m_TrackNumber = this.CreateTextFrame("TRCK", "TRCK", "TRK", "TrackNumber", new MethodInvoker(this.ValidateTrackNumber));
            this.m_BPM = this.CreateTextFrame("TBPM", "TBPM", "TBP", "BPM", new MethodInvoker(this.ValidateBPM));
            this.m_FileType = this.CreateTextFrame("TFLT", "TFLT", "TFT", "FileType", null);
            this.m_DiscNumber = this.CreateTextFrame("TPOS", "TPOS", "TPA", "DiscNumber", new MethodInvoker(this.ValidateDiscNumber));
            this.m_EncoderSettings = this.CreateTextFrame("TSSE", "TSSE", "TSS", "EncoderSettings", null);
            this.m_ISRC = this.CreateTextFrame("TSRC", "TSRC", "TRC", "ISRC", new MethodInvoker(this.ValidateISRC));
            this.m_IsPartOfCompilation = this.CreateTextFrame("TCMP", "TCMP", "TCP", "IsPartOfCompilation", null);
            this.m_ReleaseTimestamp = this.CreateTextFrame("TDRL", "TDRL", null, "ReleaseTimestamp", new MethodInvoker(this.ValidateReleaseTimestamp));
            this.m_RecordingTimestamp = this.CreateTextFrame("TDRC", "TDRC", null, "RecordingTimestamp", new MethodInvoker(this.ValidateRecordingTimestamp));
            this.m_OriginalReleaseTimestamp = this.CreateTextFrame("TDOR", "TDOR", null, "OriginalReleaseTimestamp", null);
            this.m_PlaylistDelayMilliseconds = this.CreateTextFrame("TDLY", "TDLY", "TDY", "PlaylistDelayMilliseconds", null);
            this.m_InitialKey = this.CreateTextFrame("TKEY", "TKEY", "TKE", "InitialKey", null);
            this.m_EncodingTimestamp = this.CreateTextFrame("TDEN", "TDEN", null, "EncodingTimestamp", null);
            this.m_TaggingTimestamp = this.CreateTextFrame("TDTG", "TDTG", null, "TaggingTimestamp", null);
            this.m_ContentGroup = this.CreateTextFrame("TIT1", "TIT1", "TT1", "ContentGroup", null);
            this.m_Mood = this.CreateTextFrame("TMOO", "TMOO", null, "Mood", null);
            this.m_LengthMilliseconds = this.CreateTextFrame("TLEN", "TLEN", "TLE", "LengthMilliseconds", null);
            this.m_MediaType = this.CreateTextFrame("TMED", "TMED", "TMT", "MediaType", null);
            this.m_FileSizeExcludingTag = this.CreateTextFrame(null, "TSIZ", "TSI", "FileSizeExcludingTag", null);
            this.m_OriginalReleaseYear = this.CreateTextFrame("TORY", "TORY", "TOR", "OriginalReleaseYear", null);
            this.m_OriginalSourceTitle = this.CreateTextFrame("TOAL", "TOAL", "TOT", "OriginalSourceTitle", null);
            this.m_OriginalFileName = this.CreateTextFrame("TOFN", "TOFN", "TOF", "OriginalFileName", null);
            this.m_FileOwnerName = this.CreateTextFrame("TOWN", "TOWN", null, "FileOwnerName", null);
            this.m_RecordingDates = this.CreateTextFrame("TRDA", "TRDA", "TRD", "RecordingDates", null);
            this.m_Subtitle = this.CreateTextFrame("TIT3", "TIT3", "TT3", "Subtitle", null);
            this.m_AlbumSortOrder = this.CreateTextFrame("TSOA", "TSOA", null, "AlbumSortOrder", null);
            this.m_ArtistSortOrder = this.CreateTextFrame("TSOP", "TSOP", null, "ArtistSortOrder", null);
            this.m_TitleSortOrder = this.CreateTextFrame("TSOT", "TSOT", null, "TitleSortOrder", null);
            this.m_ProducedNotice = this.CreateTextFrame("TPRO", "TPRO", null, "ProducedNotice", null);
            this.m_SetSubtitle = this.CreateTextFrame("TSST", "TSST", null, "SetSubtitle", null);
            this.m_PositionSynchronization = this.CreatePositionSynchronizationFrame("POSS", "POSS", null, "PositionSynchronization", null);
            this.m_Ownership = this.CreateOwnershipFrame("OWNE", "OWNE", null, "Ownership", null);
            this.m_RecommendedBufferSize = this.CreateRecommendedBufferSizeFrame("RBUF", "RBUF", "BUF", "RecommendedBufferSize", null);
            this.m_InvolvedPersonList = this.CreateInvolvedPersonListFrame("TIPL", "IPLS", "IPL", "InvolvedPersonList", null);
            this.m_Languages = this.CreateLanguageFrame("TLAN", "TLAN", "TLA", "Languages", null);
            this.m_MusicCDIdentifier = this.CreateMusicCDIdentifierFrame("MCDI", "MCDI", "MCI", "MusicCDIdentifier", null);
            this.m_EventTiming = this.CreateEventTimingFrame("ETCO", "ETCO", "ETC", "EventTiming", null);
            this.m_MpegLookupTable = this.CreateMpegLookupTableFrame("MLLT", "MLLT", "MLL", "MpegLookupTable", null);
            this.m_Reverb = this.CreateReverbFrame("RVRB", "RVRB", "REV", "Reverb", null);
            this.m_SynchronizedTempoCodes = this.CreateSynchronizedTempoCodesFrame("SYTC", "SYTC", "STC", "SynchronizedTempoCodeList", null);
            this.m_SeekNextTag = this.CreateSeekFrame("SEEK", "SEEK", null, "SeekNextTag", null);
            this.m_MusicianCreditsList = this.CreateMusicianCreditsListFrame("TMCL", "TMCL", null, "MusicianCreditsList", null);
            this.m_AudioSeekPointIndex = this.CreateAudioSeekPointIndexFrame("ASPI", "ASPI", null, "AudioSeekPointIndex", null);
            this.m_PlayCount = this.CreateFrame<IdSharp.Tagging.ID3v2.Frames.PlayCount>("PCNT", "PCNT", "CNT", "PlayCount");
            this.m_CopyrightUrl = this.CreateUrlFrame("WCOP", "WCOP", "WCP", "CopyrightUrl", new MethodInvoker(this.ValidateCopyrightUrl));
            this.m_AudioFileUrl = this.CreateUrlFrame("WOAF", "WOAF", "WAF", "AudioFileUrl", new MethodInvoker(this.ValidateAudioFileUrl));
            this.m_AudioSourceUrl = this.CreateUrlFrame("WOAS", "WOAS", "WAS", "AudioSourceUrl", new MethodInvoker(this.ValidateAudioSourceUrl));
            this.m_InternetRadioStationUrl = this.CreateUrlFrame("WORS", "WORS", null, "InternetRadioStationUrl", new MethodInvoker(this.ValidateInternetRadioStationUrl));
            this.m_PaymentUrl = this.CreateUrlFrame("WPAY", "WPAY", null, "PaymentUrl", new MethodInvoker(this.ValidatePaymentUrl));
            this.m_PublisherUrl = this.CreateUrlFrame("WPUB", "WPUB", "WPB", "PublisherUrl", new MethodInvoker(this.ValidatePublisherUrl));
            this.m_ID3v24FrameAliases.Add("RVAD", "RVA2");
            this.m_ID3v24FrameAliases.Add("IPLS", "TIPL");
            this.m_ID3v24FrameAliases.Add("EQUA", "EQU2");
            this.m_ID3v23FrameAliases.Add("RVA2", "RVAD");
            this.m_ID3v23FrameAliases.Add("TIPL", "IPLS");
            this.m_ID3v23FrameAliases.Add("EQU2", "EQUA");
        }

        private void AddMultipleOccurrenceFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, IBindingList bindingList)
        {
            if (id3v24FrameID != null)
            {
                this.m_ID3v24MultipleOccurrenceFrames.Add(id3v24FrameID, bindingList);
            }
            if (id3v23FrameID != null)
            {
                this.m_ID3v23MultipleOccurrenceFrames.Add(id3v23FrameID, bindingList);
            }
            if (id3v22FrameID != null)
            {
                this.m_ID3v22MultipleOccurrenceFrames.Add(id3v22FrameID, bindingList);
            }
        }

        private void Bind(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, IFrame frame, string frameProperty, string property, MethodInvoker validator)
        {
            this.m_FrameBinder.Bind(frame, frameProperty, property, validator);
            if (id3v24FrameID != null)
            {
                this.m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, frame);
            }
            if (id3v23FrameID != null)
            {
                this.m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, frame);
            }
            if (id3v22FrameID != null)
            {
                this.m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, frame);
            }
        }

        private IdSharp.Tagging.ID3v2.Frames.AudioSeekPointIndex CreateAudioSeekPointIndexFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.AudioSeekPointIndex index1 = new IdSharp.Tagging.ID3v2.Frames.AudioSeekPointIndex();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, index1, "TODO", property, validator);
            return index1;
        }

        private IdSharp.Tagging.ID3v2.Frames.EventTiming CreateEventTimingFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.EventTiming timing1 = new IdSharp.Tagging.ID3v2.Frames.EventTiming();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, timing1, "TODO", property, validator);
            return timing1;
        }

        private T CreateFrame<T>(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property) where T: IFrame, new()
        {
            T local1 = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, local1, "TODO", property, null);
            return local1;
        }

        private IdSharp.Tagging.ID3v2.Frames.InvolvedPersonList CreateInvolvedPersonListFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.InvolvedPersonList list1 = new IdSharp.Tagging.ID3v2.Frames.InvolvedPersonList();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, list1, "TODO", property, validator);
            return list1;
        }

        private LanguageFrame CreateLanguageFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            LanguageFrame frame1 = new LanguageFrame();
            this.m_FrameBinder.Bind(frame1, "Items", property, validator);
            if (id3v24FrameID != null)
            {
                this.m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, frame1);
            }
            if (id3v23FrameID != null)
            {
                this.m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, frame1);
            }
            if (id3v22FrameID != null)
            {
                this.m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, frame1);
            }
            return frame1;
        }

        private IdSharp.Tagging.ID3v2.Frames.MpegLookupTable CreateMpegLookupTableFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.MpegLookupTable table1 = new IdSharp.Tagging.ID3v2.Frames.MpegLookupTable();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, table1, "TODO", property, validator);
            return table1;
        }

        private IdSharp.Tagging.ID3v2.Frames.MusicCDIdentifier CreateMusicCDIdentifierFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.MusicCDIdentifier identifier1 = new IdSharp.Tagging.ID3v2.Frames.MusicCDIdentifier();
            this.m_FrameBinder.Bind(identifier1, "TOC", property, validator);
            if (id3v24FrameID != null)
            {
                this.m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, identifier1);
            }
            if (id3v23FrameID != null)
            {
                this.m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, identifier1);
            }
            if (id3v22FrameID != null)
            {
                this.m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, identifier1);
            }
            return identifier1;
        }

        private IdSharp.Tagging.ID3v2.Frames.MusicianCreditsList CreateMusicianCreditsListFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.MusicianCreditsList list1 = new IdSharp.Tagging.ID3v2.Frames.MusicianCreditsList();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, list1, "TODO", property, validator);
            return list1;
        }

        private IdSharp.Tagging.ID3v2.Frames.Ownership CreateOwnershipFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.Ownership ownership1 = new IdSharp.Tagging.ID3v2.Frames.Ownership();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, ownership1, "TODO", property, validator);
            return ownership1;
        }

        private IdSharp.Tagging.ID3v2.Frames.PositionSynchronization CreatePositionSynchronizationFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.PositionSynchronization synchronization1 = new IdSharp.Tagging.ID3v2.Frames.PositionSynchronization();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, synchronization1, "TODO", property, validator);
            return synchronization1;
        }

        private IdSharp.Tagging.ID3v2.Frames.RecommendedBufferSize CreateRecommendedBufferSizeFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.RecommendedBufferSize size1 = new IdSharp.Tagging.ID3v2.Frames.RecommendedBufferSize();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, size1, "TODO", property, validator);
            return size1;
        }

        private IdSharp.Tagging.ID3v2.Frames.Reverb CreateReverbFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.Reverb reverb1 = new IdSharp.Tagging.ID3v2.Frames.Reverb();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, reverb1, "TODO", property, validator);
            return reverb1;
        }

        private IdSharp.Tagging.ID3v2.Frames.SeekNextTag CreateSeekFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            IdSharp.Tagging.ID3v2.Frames.SeekNextTag tag1 = new IdSharp.Tagging.ID3v2.Frames.SeekNextTag();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, tag1, "TODO", property, validator);
            return tag1;
        }

        private SynchronizedTempoCodes CreateSynchronizedTempoCodesFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            SynchronizedTempoCodes codes1 = new SynchronizedTempoCodes();
            this.Bind(id3v24FrameID, id3v23FrameID, id3v22FrameID, codes1, "TODO", property, validator);
            return codes1;
        }

        private TextFrame CreateTextFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            TextFrame frame1 = new TextFrame(id3v24FrameID, id3v23FrameID, id3v22FrameID);
            this.m_FrameBinder.Bind(frame1, "Value", property, validator);
            if (id3v24FrameID != null)
            {
                this.m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, frame1);
            }
            if (id3v23FrameID != null)
            {
                this.m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, frame1);
            }
            if (id3v22FrameID != null)
            {
                this.m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, frame1);
            }
            return frame1;
        }

        private UrlFrame CreateUrlFrame(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, string property, MethodInvoker validator)
        {
            UrlFrame frame1 = new UrlFrame(id3v24FrameID, id3v23FrameID, id3v22FrameID);
            this.m_FrameBinder.Bind(frame1, "Value", property, validator);
            if (id3v24FrameID != null)
            {
                this.m_ID3v24SingleOccurrenceFrames.Add(id3v24FrameID, frame1);
            }
            if (id3v23FrameID != null)
            {
                this.m_ID3v23SingleOccurrenceFrames.Add(id3v23FrameID, frame1);
            }
            if (id3v22FrameID != null)
            {
                this.m_ID3v22SingleOccurrenceFrames.Add(id3v22FrameID, frame1);
            }
            return frame1;
        }

        public void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler1 = this.PropertyChanged;
            if (handler1 != null)
            {
                handler1(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void FireWarning(string propertyName, string message)
        {
            InvalidDataEventHandler handler1 = this.InvalidData;
            if (handler1 != null)
            {
                handler1(this, new InvalidDataEventArgs(propertyName, message));
            }
        }

        public byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            using (MemoryStream stream1 = new MemoryStream())
            {
                Dictionary<string, IBindingList> dictionary1 = this.GetMultipleOccurrenceFrames(tagVersion);
                Dictionary<string, IFrame> dictionary2 = this.GetSingleOccurrenceFrames(tagVersion);
                Dictionary<string, IFrame>.ValueCollection.Enumerator enumerator1 = dictionary2.Values.GetEnumerator();
                try
                {
                    while (enumerator1.MoveNext())
                    {
                        byte[] buffer1 = enumerator1.Current.GetBytes(tagVersion);
                        stream1.Write(buffer1, 0, buffer1.Length);
                    }
                }
                finally
                {
                    enumerator1.Dispose();
                }
                foreach (IBindingList list1 in dictionary1.Values)
                {
                    for (int num1 = 0; num1 < list1.Count; num1++)
                    {
                        byte[] buffer2 = ((IFrame) list1[num1]).GetBytes(tagVersion);
                        stream1.Write(buffer2, 0, buffer2.Length);
                    }
                }
                List<UnknownFrame>.Enumerator enumerator3 = this.m_UnknownFrames.GetEnumerator();
                try
                {
                    while (enumerator3.MoveNext())
                    {
                        byte[] buffer3 = enumerator3.Current.GetBytes(tagVersion);
                        stream1.Write(buffer3, 0, buffer3.Length);
                    }
                }
                finally
                {
                    enumerator3.Dispose();
                }
                return stream1.ToArray();
            }
        }

        public IFrameList GetFrameList(string frameID)
        {
            frameID = frameID.ToUpper();
            if ((frameID != "APIC") && (frameID != "PIC"))
            {
                return null;
            }
            return this.m_AttachedPictureList;
        }

        private Dictionary<string, IBindingList> GetMultipleOccurrenceFrames(ID3v2TagVersion tagVersion)
        {
            if (tagVersion != ID3v2TagVersion.ID3v23)
            {
                if (tagVersion != ID3v2TagVersion.ID3v22)
                {
                    if (tagVersion != ID3v2TagVersion.ID3v24)
                    {
                        throw new ArgumentException("Unknown ID3v2 tag version");
                    }
                    return this.m_ID3v24MultipleOccurrenceFrames;
                }
                return this.m_ID3v22MultipleOccurrenceFrames;
            }
            return this.m_ID3v23MultipleOccurrenceFrames;
        }

        private Dictionary<string, IFrame> GetSingleOccurrenceFrames(ID3v2TagVersion tagVersion)
        {
            if (tagVersion != ID3v2TagVersion.ID3v23)
            {
                if (tagVersion != ID3v2TagVersion.ID3v22)
                {
                    if (tagVersion != ID3v2TagVersion.ID3v24)
                    {
                        throw new ArgumentException("Unknown ID3v2 tag version");
                    }
                    return this.m_ID3v24SingleOccurrenceFrames;
                }
                return this.m_ID3v22SingleOccurrenceFrames;
            }
            return this.m_ID3v23SingleOccurrenceFrames;
        }

        public void Read(Stream stream, ID3v2TagVersion tagVersion, TagReadingInfo tmpTagReadingInfo, int tmpReadUntil, int tmpFrameIDSize)
        {
            Dictionary<string, IBindingList> dictionary1 = this.GetMultipleOccurrenceFrames(tagVersion);
            Dictionary<string, IFrame> dictionary2 = this.GetSingleOccurrenceFrames(tagVersion);
            int num1 = 0;
            while (num1 < tmpReadUntil)
            {
                IFrame frame1;
                byte[] buffer1 = Utils.Read(stream, tmpFrameIDSize);
                if (tmpFrameIDSize == 4)
                {
                    if ((((buffer1[0] < 0x30) || (buffer1[0] > 90)) || ((buffer1[1] < 0x30) || (buffer1[1] > 90))) || (((buffer1[2] < 0x30) || (buffer1[2] > 90)) || ((buffer1[3] < 0x30) || (buffer1[3] > 90))))
                    {
                        if ((buffer1[0] != 0) && (buffer1[0] != 0xff))
                        {
                            string text1 = string.Format("Out of range FrameID - 0x{0:X}|0x{1:X}|0x{2:X}|0x{3:X}", new object[] { buffer1[0], buffer1[1], buffer1[2], buffer1[3] });
                            if (Utils.ISO88591GetString(buffer1) == "MP3e")
                            {
                                return;
                            }
                            Trace.WriteLine(text1 + " - " + Utils.ISO88591GetString(buffer1).TrimEnd(new char[1]));
                        }
                        return;
                    }
                }
                else if ((tmpFrameIDSize == 3) && ((((buffer1[0] < 0x30) || (buffer1[0] > 90)) || ((buffer1[1] < 0x30) || (buffer1[1] > 90))) || ((buffer1[2] < 0x30) || (buffer1[2] > 90))))
                {
                    if ((buffer1[0] != 0) && (buffer1[0] != 0xff))
                    {
                        string text3 = string.Format("Out of range FrameID - 0x{0:X}|0x{1:X}|0x{2:X}", buffer1[0], buffer1[1], buffer1[2]);
                        Trace.WriteLine(text3);
                        Trace.WriteLine(Utils.ISO88591GetString(buffer1));
                    }
                    return;
                }
                string text4 = Utils.ISO88591GetString(buffer1);
                do
                {
                    if (dictionary2.TryGetValue(text4, out frame1))
                    {
                        frame1.Read(tmpTagReadingInfo, stream);
                        num1 += frame1.FrameHeader.FrameSizeTotal;
                    }
                    else
                    {
                        IBindingList list1;
                        if (dictionary1.TryGetValue(text4, out list1))
                        {
                            frame1 = (IFrame) list1.AddNew();
                            frame1.Read(tmpTagReadingInfo, stream);
                            num1 += frame1.FrameHeader.FrameSizeTotal;
                        }
                        else if (tagVersion == ID3v2TagVersion.ID3v24)
                        {
                            string text5;
                            if (!this.m_ID3v24FrameAliases.TryGetValue(text4, out text5))
                            {
                                break;
                            }
                            text4 = text5;
                        }
                        else
                        {
                            string text6;
                            if ((tagVersion != ID3v2TagVersion.ID3v23) || !this.m_ID3v23FrameAliases.TryGetValue(text4, out text6))
                            {
                                break;
                            }
                            text4 = text6;
                        }
                    }
                }
                while (frame1 == null);
                if (frame1 == null)
                {
                    if (((((text4 != "NCON") && (text4 != "MJMD")) && ((text4 != "TT22") && (text4 != "PCST"))) && (((text4 != "TCAT") && (text4 != "TKWD")) && ((text4 != "TDES") && (text4 != "TGID")))) && ((((text4 != "WFED") && (text4 != "CM1")) && ((text4 != "TMB") && (text4 != "RTNG"))) && ((text4 != "XDOR") && (text4 != "XSOP"))))
                    {
                        bool flag1 = text4 != "TENK";
                    }
                    UnknownFrame frame2 = new UnknownFrame(text4, tmpTagReadingInfo, stream);
                    this.m_UnknownFrames.Add(frame2);
                    num1 += frame2.FrameHeader.FrameSizeTotal;
                }
            }
        }

        private void ValidateAudioFileUrl()
        {
            this.ValidateUrl("AudioFileUrl", this.AudioFileUrl);
        }

        private void ValidateAudioSourceUrl()
        {
            this.ValidateUrl("AudioSourceUrl", this.AudioSourceUrl);
        }

        private void ValidateBPM()
        {
            uint num1;
            string text1 = this.BPM;
            if (!string.IsNullOrEmpty(text1) && !uint.TryParse(text1, out num1))
            {
                this.FireWarning("BPM", "Value should be numeric");
            }
        }

        private void ValidateCopyright()
        {
            string text1 = this.Copyright;
            if (!string.IsNullOrEmpty(text1))
            {
                bool flag1 = false;
                if (text1.Length >= 6)
                {
                    int num1;
                    string text2 = text1.Substring(0, 4);
                    if ((int.TryParse(text2, out num1) && (num1 >= 0x3e8)) && ((num1 <= 0x270f) && (text1[4] == ' ')))
                    {
                        flag1 = true;
                    }
                }
                if (!flag1)
                {
                    this.FireWarning("Copyright", string.Format("The copyright field should begin with a year followed by the copyright owner{0}Example: 2007 Sony Records", Environment.NewLine));
                }
            }
        }

        private void ValidateCopyrightUrl()
        {
            this.ValidateUrl("CopyrightUrl", this.CopyrightUrl);
        }

        private void ValidateDateRecorded()
        {
        }

        private void ValidateDiscNumber()
        {
            this.ValidateFractionValue("DiscNumber", this.DiscNumber, "Value should contain either the disc number or disc number/total discs in the format ## or ##/##\nExample: 1 or 1/2");
        }

        private void ValidateFractionValue(string propertyName, string value, string message)
        {
            if (!string.IsNullOrEmpty(value))
            {
                bool flag1 = true;
                string[] textArray1 = value.Split(new char[] { '/' });
                if (textArray1.Length > 2)
                {
                    flag1 = false;
                }
                else
                {
                    int num1 = 0;
                    uint num2 = 0;
                    uint num3 = 0;
                    foreach (string text1 in textArray1)
                    {
                        uint num4;
                        if (!uint.TryParse(text1, out num4))
                        {
                            flag1 = false;
                            break;
                        }
                        switch (num1)
                        {
                            case 0:
                                num2 = num4;
                                goto Label_0063;

                            case 1:
                                num3 = num4;
                                break;
                        }
                    Label_0063:
                        num1++;
                    }
                    if (num2 == 0)
                    {
                        flag1 = false;
                    }
                    else if ((num1 == 2) && (num2 > num3))
                    {
                        flag1 = false;
                    }
                }
                if (!flag1)
                {
                    this.FireWarning(propertyName, message);
                }
            }
        }

        private void ValidateInternetRadioStationUrl()
        {
            this.ValidateUrl("InternetRadioStationUrl", this.InternetRadioStationUrl);
        }

        private void ValidateISRC()
        {
            string text1 = this.ISRC;
            if (!string.IsNullOrEmpty(text1) && (text1.Length != 12))
            {
                this.FireWarning("ISRC", "ISRC value should be 12 characters in length");
            }
        }

        private void ValidatePaymentUrl()
        {
            this.ValidateUrl("PaymentUrl", this.PaymentUrl);
        }

        private void ValidatePublisherUrl()
        {
            this.ValidateUrl("PublisherUrl", this.PublisherUrl);
        }

        private void ValidateRecordingTimestamp()
        {
            string text1 = this.RecordingTimestamp;
            if (text1 != null)
            {
                if (text1.Length >= 10)
                {
                    this.DateRecorded = text1.Substring(5, 2) + text1.Substring(8, 2);
                }
                else if (text1.Length >= 7)
                {
                    this.DateRecorded = text1.Substring(6, 2) + "00";
                }
                else
                {
                    this.DateRecorded = null;
                }
                if (text1.Length >= 0x10)
                {
                    this.TimeRecorded = text1.Substring(11, 2) + text1.Substring(14, 2);
                }
                else if (text1.Length >= 13)
                {
                    this.TimeRecorded = text1.Substring(11, 2) + "00";
                }
                else
                {
                    this.TimeRecorded = null;
                }
                if (text1.Length >= 0x13)
                {
                }
            }
            else
            {
                this.DateRecorded = null;
                this.TimeRecorded = null;
            }
        }

        private void ValidateReleaseTimestamp()
        {
            string text1 = this.ReleaseTimestamp;
            if (text1 != null)
            {
                if (text1.Length >= 4)
                {
                    this.Year = text1.Substring(0, 4);
                }
                else
                {
                    this.Year = null;
                }
            }
            else
            {
                this.Year = null;
            }
        }

        private void ValidateTimeRecorded()
        {
        }

        private void ValidateTrackNumber()
        {
            this.ValidateFractionValue("TrackNumber", this.TrackNumber, "Value should contain either the track number or track number/total tracks in the format ## or ##/##\nExample: 1 or 1/14");
        }

        private void ValidateUrl(string propertyName, string value)
        {
            if (!string.IsNullOrEmpty(value) && !Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                this.FireWarning(propertyName, "Value is not a valid relative or absolute URL");
            }
        }

        private void ValidateYear()
        {
            int num1;
            string text1 = this.Year;
            if (!string.IsNullOrEmpty(text1) && ((!int.TryParse(text1, out num1) || (num1 < 0x3e8)) || (num1 >= 0x2710)))
            {
                this.FireWarning("Year", string.Format("The year field should be a 4 digit number{0}Example: 2007", Environment.NewLine));
            }
        }


        public string Accompaniment
        {
            get
            {
                return this.m_Accompaniment.Value;
            }
            set
            {
                this.m_Accompaniment.Value = value;
            }
        }

        public string Album
        {
            get
            {
                return this.m_Album.Value;
            }
            set
            {
                this.m_Album.Value = value;
            }
        }

        public string AlbumSortOrder
        {
            get
            {
                return this.m_AlbumSortOrder.Value;
            }
            set
            {
                this.m_AlbumSortOrder.Value = value;
            }
        }

        public string Artist
        {
            get
            {
                return this.m_Artist.Value;
            }
            set
            {
                this.m_Artist.Value = value;
            }
        }

        public string ArtistSortOrder
        {
            get
            {
                return this.m_ArtistSortOrder.Value;
            }
            set
            {
                this.m_ArtistSortOrder.Value = value;
            }
        }

        public BindingList<IUrlFrame> ArtistUrlList
        {
            get
            {
                return this.m_ArtistUrlList;
            }
        }

        public BindingList<IAudioEncryption> AudioEncryptionList
        {
            get
            {
                return this.m_AudioEncryptionList;
            }
        }

        public string AudioFileUrl
        {
            get
            {
                return this.m_AudioFileUrl.Value;
            }
            set
            {
                this.m_AudioFileUrl.Value = value;
            }
        }

        public IAudioSeekPointIndex AudioSeekPointIndex
        {
            get
            {
                return this.m_AudioSeekPointIndex;
            }
        }

        public string AudioSourceUrl
        {
            get
            {
                return this.m_AudioSourceUrl.Value;
            }
            set
            {
                this.m_AudioSourceUrl.Value = value;
            }
        }

        public BindingList<IAudioText> AudioTextList
        {
            get
            {
                return this.m_AudioTextList;
            }
        }

        public string BPM
        {
            get
            {
                return this.m_BPM.Value;
            }
            set
            {
                this.m_BPM.Value = value;
            }
        }

        public BindingList<IComments> CommentsList
        {
            get
            {
                return this.m_CommentsList;
            }
        }

        public BindingList<ICommercial> CommercialInfoList
        {
            get
            {
                return this.m_CommercialInfoList;
            }
        }

        public BindingList<IUrlFrame> CommercialInfoUrlList
        {
            get
            {
                return this.m_CommercialInfoUrlList;
            }
        }

        public string Composer
        {
            get
            {
                return this.m_Composer.Value;
            }
            set
            {
                this.m_Composer.Value = value;
            }
        }

        public string Conductor
        {
            get
            {
                return this.m_Conductor.Value;
            }
            set
            {
                this.m_Conductor.Value = value;
            }
        }

        public string ContentGroup
        {
            get
            {
                return this.m_ContentGroup.Value;
            }
            set
            {
                this.m_ContentGroup.Value = value;
            }
        }

        public string Copyright
        {
            get
            {
                return this.m_Copyright.Value;
            }
            set
            {
                this.m_Copyright.Value = value;
            }
        }

        public string CopyrightUrl
        {
            get
            {
                return this.m_CopyrightUrl.Value;
            }
            set
            {
                this.m_CopyrightUrl.Value = value;
            }
        }

        public string DateRecorded
        {
            get
            {
                return this.m_DateRecorded.Value;
            }
            set
            {
                this.m_DateRecorded.Value = value;
            }
        }

        public string DiscNumber
        {
            get
            {
                return this.m_DiscNumber.Value;
            }
            set
            {
                this.m_DiscNumber.Value = value;
            }
        }

        public string EncodedByWho
        {
            get
            {
                return this.m_EncodedByWho.Value;
            }
            set
            {
                this.m_EncodedByWho.Value = value;
            }
        }

        public string EncoderSettings
        {
            get
            {
                return this.m_EncoderSettings.Value;
            }
            set
            {
                this.m_EncoderSettings.Value = value;
            }
        }

        public string EncodingTimestamp
        {
            get
            {
                return this.m_EncodingTimestamp.Value;
            }
            set
            {
                this.m_EncodingTimestamp.Value = value;
            }
        }

        public BindingList<IEncryptionMethod> EncryptionMethodList
        {
            get
            {
                return this.m_EncryptionMethodList;
            }
        }

        public BindingList<IEqualizationList> EqualizationList
        {
            get
            {
                return this.m_EqualizationList;
            }
        }

        public IEventTiming EventTiming
        {
            get
            {
                return this.m_EventTiming;
            }
        }

        public string FileOwnerName
        {
            get
            {
                return this.m_FileOwnerName.Value;
            }
            set
            {
                this.m_FileOwnerName.Value = value;
            }
        }

        public Nullable<long> FileSizeExcludingTag
        {
            get
            {
                long num1;
                if (long.TryParse(this.m_FileSizeExcludingTag.Value, out num1))
                {
                    return new Nullable<long>(num1);
                }
                return new Nullable<long>();
            }
            set
            {
                if (!value.HasValue)
                {
                    this.m_FileSizeExcludingTag.Value = null;
                }
                else
                {
                    Nullable<long> nullable1 = value;
                    if ((nullable1.GetValueOrDefault() < 0) && nullable1.HasValue)
                    {
                        throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                    }
                    this.m_FileSizeExcludingTag.Value = value.Value.ToString();
                }
            }
        }

        public string FileType
        {
            get
            {
                return this.m_FileType.Value;
            }
            set
            {
                this.m_FileType.Value = value;
            }
        }

        public BindingList<IGeneralEncapsulatedObject> GeneralEncapsulatedObjectList
        {
            get
            {
                return this.m_GeneralEncapsulatedObjectList;
            }
        }

        public string Genre
        {
            get
            {
                return this.m_Genre.Value;
            }
            set
            {
                this.m_Genre.Value = value;
            }
        }

        public BindingList<IGroupIdentification> GroupIdentificationList
        {
            get
            {
                return this.m_GroupIdentificationList;
            }
        }

        public string InitialKey
        {
            get
            {
                return this.m_InitialKey.Value;
            }
            set
            {
                this.m_InitialKey.Value = value;
            }
        }

        public string InternetRadioStationName
        {
            get
            {
                return this.m_InternetRadioStationName.Value;
            }
            set
            {
                this.m_InternetRadioStationName.Value = value;
            }
        }

        public string InternetRadioStationOwner
        {
            get
            {
                return this.m_InternetRadioStationOwner.Value;
            }
            set
            {
                this.m_InternetRadioStationOwner.Value = value;
            }
        }

        public string InternetRadioStationUrl
        {
            get
            {
                return this.m_InternetRadioStationUrl.Value;
            }
            set
            {
                this.m_InternetRadioStationUrl.Value = value;
            }
        }

        public IInvolvedPersonList InvolvedPersonList
        {
            get
            {
                return this.m_InvolvedPersonList;
            }
        }

        public bool IsPartOfCompilation
        {
            get
            {
                int num1;
                if (int.TryParse(this.m_IsPartOfCompilation.Value, out num1) && (num1 == 1))
                {
                    return true;
                }
                return false;
            }
            set
            {
                this.m_IsPartOfCompilation.Value = value ? "1" : "";
            }
        }

        public string ISRC
        {
            get
            {
                return this.m_ISRC.Value;
            }
            set
            {
                this.m_ISRC.Value = value;
            }
        }

        public ILanguageFrame Languages
        {
            get
            {
                return this.m_Languages;
            }
        }

        public Nullable<int> LengthMilliseconds
        {
            get
            {
                int num1;
                if (int.TryParse(this.m_LengthMilliseconds.Value, out num1))
                {
                    return new Nullable<int>(num1);
                }
                return new Nullable<int>();
            }
            set
            {
                if (!value.HasValue)
                {
                    this.m_LengthMilliseconds.Value = null;
                }
                else
                {
                    Nullable<int> nullable1 = value;
                    if ((nullable1.GetValueOrDefault() < 0) && nullable1.HasValue)
                    {
                        throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                    }
                    this.m_LengthMilliseconds.Value = value.Value.ToString();
                }
            }
        }

        public BindingList<ILinkedInformation> LinkedInformationList
        {
            get
            {
                return this.m_LinkedInformationList;
            }
        }

        public string Lyricist
        {
            get
            {
                return this.m_Lyricist.Value;
            }
            set
            {
                this.m_Lyricist.Value = value;
            }
        }

        public string MediaType
        {
            get
            {
                return this.m_MediaType.Value;
            }
            set
            {
                this.m_MediaType.Value = value;
            }
        }

        public string Mood
        {
            get
            {
                return this.m_Mood.Value;
            }
            set
            {
                this.m_Mood.Value = value;
            }
        }

        public IMpegLookupTable MpegLookupTable
        {
            get
            {
                return this.m_MpegLookupTable;
            }
        }

        public IMusicCDIdentifier MusicCDIdentifier
        {
            get
            {
                return this.m_MusicCDIdentifier;
            }
        }

        public IMusicianCreditsList MusicianCreditsList
        {
            get
            {
                return this.m_MusicianCreditsList;
            }
        }

        public string OriginalArtist
        {
            get
            {
                return this.m_OriginalArtist.Value;
            }
            set
            {
                this.m_OriginalArtist.Value = value;
            }
        }

        public string OriginalFileName
        {
            get
            {
                return this.m_OriginalFileName.Value;
            }
            set
            {
                this.m_OriginalFileName.Value = value;
            }
        }

        public string OriginalLyricist
        {
            get
            {
                return this.m_OriginalLyricist.Value;
            }
            set
            {
                this.m_OriginalLyricist.Value = value;
            }
        }

        public string OriginalReleaseTimestamp
        {
            get
            {
                return this.m_OriginalReleaseTimestamp.Value;
            }
            set
            {
                this.m_OriginalReleaseTimestamp.Value = value;
            }
        }

        public string OriginalReleaseYear
        {
            get
            {
                return this.m_OriginalReleaseYear.Value;
            }
            set
            {
                this.m_OriginalReleaseYear.Value = value;
            }
        }

        public string OriginalSourceTitle
        {
            get
            {
                return this.m_OriginalSourceTitle.Value;
            }
            set
            {
                this.m_OriginalSourceTitle.Value = value;
            }
        }

        public IOwnership Ownership
        {
            get
            {
                return this.m_Ownership;
            }
        }

        public string PaymentUrl
        {
            get
            {
                return this.m_PaymentUrl.Value;
            }
            set
            {
                this.m_PaymentUrl.Value = value;
            }
        }

        public BindingList<IAttachedPicture> PictureList
        {
            get
            {
                return this.m_AttachedPictureList;
            }
        }

        public IPlayCount PlayCount
        {
            get
            {
                return this.m_PlayCount;
            }
        }

        public Nullable<int> PlaylistDelayMilliseconds
        {
            get
            {
                int num1;
                if (int.TryParse(this.m_PlaylistDelayMilliseconds.Value, out num1))
                {
                    return new Nullable<int>(num1);
                }
                return new Nullable<int>();
            }
            set
            {
                if (!value.HasValue)
                {
                    this.m_PlaylistDelayMilliseconds.Value = null;
                }
                else
                {
                    Nullable<int> nullable1 = value;
                    if ((nullable1.GetValueOrDefault() < 0) && nullable1.HasValue)
                    {
                        throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                    }
                    this.m_PlaylistDelayMilliseconds.Value = value.Value.ToString();
                }
            }
        }

        public BindingList<IPopularimeter> PopularimeterList
        {
            get
            {
                return this.m_PopularimeterList;
            }
        }

        public IPositionSynchronization PositionSynchronization
        {
            get
            {
                return this.m_PositionSynchronization;
            }
        }

        public BindingList<IPrivateFrame> PrivateFrameList
        {
            get
            {
                return this.m_PrivateFrameList;
            }
        }

        public string ProducedNotice
        {
            get
            {
                return this.m_ProducedNotice.Value;
            }
            set
            {
                this.m_ProducedNotice.Value = value;
            }
        }

        public string Publisher
        {
            get
            {
                return this.m_Publisher.Value;
            }
            set
            {
                this.m_Publisher.Value = value;
            }
        }

        public string PublisherUrl
        {
            get
            {
                return this.m_PublisherUrl.Value;
            }
            set
            {
                this.m_PublisherUrl.Value = value;
            }
        }

        public IRecommendedBufferSize RecommendedBufferSize
        {
            get
            {
                return this.m_RecommendedBufferSize;
            }
        }

        public string RecordingDates
        {
            get
            {
                return this.m_RecordingDates.Value;
            }
            set
            {
                this.m_RecordingDates.Value = value;
            }
        }

        public string RecordingTimestamp
        {
            get
            {
                return this.m_RecordingTimestamp.Value;
            }
            set
            {
                this.m_RecordingTimestamp.Value = value;
            }
        }

        public BindingList<IRelativeVolumeAdjustment> RelativeVolumeAdjustmentList
        {
            get
            {
                return this.m_RelativeVolumeAdjustmentList;
            }
        }

        public string ReleaseTimestamp
        {
            get
            {
                return this.m_ReleaseTimestamp.Value;
            }
            set
            {
                this.m_ReleaseTimestamp.Value = value;
            }
        }

        public string RemixedBy
        {
            get
            {
                return this.m_RemixedBy.Value;
            }
            set
            {
                this.m_RemixedBy.Value = value;
            }
        }

        public IReverb Reverb
        {
            get
            {
                return this.m_Reverb;
            }
        }

        public ISeekNextTag SeekNextTag
        {
            get
            {
                return this.m_SeekNextTag;
            }
        }

        public string SetSubtitle
        {
            get
            {
                return this.m_SetSubtitle.Value;
            }
            set
            {
                this.m_SetSubtitle.Value = value;
            }
        }

        public BindingList<ISignature> SignatureList
        {
            get
            {
                return this.m_SignatureList;
            }
        }

        public string Subtitle
        {
            get
            {
                return this.m_Subtitle.Value;
            }
            set
            {
                this.m_Subtitle.Value = value;
            }
        }

        public BindingList<ISynchronizedText> SynchronizedLyrics
        {
            get
            {
                return this.m_SynchronizedLyricsList;
            }
        }

        public ISynchronizedTempoCodes SynchronizedTempoCodeList
        {
            get
            {
                return this.m_SynchronizedTempoCodes;
            }
        }

        public string TaggingTimestamp
        {
            get
            {
                return this.m_TaggingTimestamp.Value;
            }
            set
            {
                this.m_TaggingTimestamp.Value = value;
            }
        }

        public BindingList<ITermsOfUse> TermsOfUseList
        {
            get
            {
                return this.m_TermsOfUseList;
            }
        }

        public string TimeRecorded
        {
            get
            {
                return this.m_TimeRecorded.Value;
            }
            set
            {
                this.m_TimeRecorded.Value = value;
            }
        }

        public string Title
        {
            get
            {
                return this.m_Title.Value;
            }
            set
            {
                this.m_Title.Value = value;
            }
        }

        public string TitleSortOrder
        {
            get
            {
                return this.m_TitleSortOrder.Value;
            }
            set
            {
                this.m_TitleSortOrder.Value = value;
            }
        }

        public string TrackNumber
        {
            get
            {
                return this.m_TrackNumber.Value;
            }
            set
            {
                this.m_TrackNumber.Value = value;
            }
        }

        public BindingList<IUniqueFileIdentifier> UniqueFileIdentifierList
        {
            get
            {
                return this.m_UniqueFileIdentifierList;
            }
        }

        public BindingList<IUnsynchronizedText> UnsynchronizedLyricsList
        {
            get
            {
                return this.m_UnsynchronizedLyricsList;
            }
        }

        public BindingList<ITXXXFrame> UserDefinedText
        {
            get
            {
                return this.m_UserDefinedTextList;
            }
        }

        public BindingList<IWXXXFrame> UserDefinedUrlList
        {
            get
            {
                return this.m_UserDefinedUrlList;
            }
        }

        public string Year
        {
            get
            {
                return this.m_Year.Value;
            }
            set
            {
                this.m_Year.Value = value;
            }
        }


        private TextFrame m_Accompaniment;
        private TextFrame m_Album;
        private TextFrame m_AlbumSortOrder;
        private TextFrame m_Artist;
        private TextFrame m_ArtistSortOrder;
        private UrlBindingList m_ArtistUrlList;
        private AttachedPictureBindingList m_AttachedPictureList;
        private AudioEncryptionBindingList m_AudioEncryptionList;
        private IUrlFrame m_AudioFileUrl;
        private IdSharp.Tagging.ID3v2.Frames.AudioSeekPointIndex m_AudioSeekPointIndex;
        private IUrlFrame m_AudioSourceUrl;
        private AudioTextBindingList m_AudioTextList;
        private TextFrame m_BPM;
        private CommentsBindingList m_CommentsList;
        private CommercialBindingList m_CommercialInfoList;
        private UrlBindingList m_CommercialInfoUrlList;
        private TextFrame m_Composer;
        private TextFrame m_Conductor;
        private TextFrame m_ContentGroup;
        private TextFrame m_Copyright;
        private IUrlFrame m_CopyrightUrl;
        private TextFrame m_DateRecorded;
        private TextFrame m_DiscNumber;
        private TextFrame m_EncodedByWho;
        private TextFrame m_EncoderSettings;
        private TextFrame m_EncodingTimestamp;
        private EncryptedMetaFrameBindingList m_EncryptedMetaFrameList;
        private EncryptionMethodBindingList m_EncryptionMethodList;
        private EqualizationListBindingList m_EqualizationList;
        private IdSharp.Tagging.ID3v2.Frames.EventTiming m_EventTiming;
        private TextFrame m_FileOwnerName;
        private TextFrame m_FileSizeExcludingTag;
        private TextFrame m_FileType;
        private FrameBinder m_FrameBinder;
        private GeneralEncapsulatedObjectBindingList m_GeneralEncapsulatedObjectList;
        private TextFrame m_Genre;
        private GroupIdentificationBindingList m_GroupIdentificationList;
        private Dictionary<string, IBindingList> m_ID3v22MultipleOccurrenceFrames;
        private Dictionary<string, IFrame> m_ID3v22SingleOccurrenceFrames;
        private Dictionary<string, string> m_ID3v23FrameAliases;
        private Dictionary<string, IBindingList> m_ID3v23MultipleOccurrenceFrames;
        private Dictionary<string, IFrame> m_ID3v23SingleOccurrenceFrames;
        private Dictionary<string, string> m_ID3v24FrameAliases;
        private Dictionary<string, IBindingList> m_ID3v24MultipleOccurrenceFrames;
        private Dictionary<string, IFrame> m_ID3v24SingleOccurrenceFrames;
        private TextFrame m_InitialKey;
        private TextFrame m_InternetRadioStationName;
        private TextFrame m_InternetRadioStationOwner;
        private IUrlFrame m_InternetRadioStationUrl;
        private IdSharp.Tagging.ID3v2.Frames.InvolvedPersonList m_InvolvedPersonList;
        private TextFrame m_IsPartOfCompilation;
        private TextFrame m_ISRC;
        private LanguageFrame m_Languages;
        private TextFrame m_LengthMilliseconds;
        private LinkedInformationBindingList m_LinkedInformationList;
        private TextFrame m_Lyricist;
        private TextFrame m_MediaType;
        private TextFrame m_Mood;
        private IdSharp.Tagging.ID3v2.Frames.MpegLookupTable m_MpegLookupTable;
        private IdSharp.Tagging.ID3v2.Frames.MusicCDIdentifier m_MusicCDIdentifier;
        private IdSharp.Tagging.ID3v2.Frames.MusicianCreditsList m_MusicianCreditsList;
        private TextFrame m_OriginalArtist;
        private TextFrame m_OriginalFileName;
        private TextFrame m_OriginalLyricist;
        private TextFrame m_OriginalReleaseTimestamp;
        private TextFrame m_OriginalReleaseYear;
        private TextFrame m_OriginalSourceTitle;
        private IdSharp.Tagging.ID3v2.Frames.Ownership m_Ownership;
        private IUrlFrame m_PaymentUrl;
        private IdSharp.Tagging.ID3v2.Frames.PlayCount m_PlayCount;
        private TextFrame m_PlaylistDelayMilliseconds;
        private PopularimeterBindingList m_PopularimeterList;
        private IdSharp.Tagging.ID3v2.Frames.PositionSynchronization m_PositionSynchronization;
        private PrivateFrameBindingList m_PrivateFrameList;
        private TextFrame m_ProducedNotice;
        private TextFrame m_Publisher;
        private IUrlFrame m_PublisherUrl;
        private IdSharp.Tagging.ID3v2.Frames.RecommendedBufferSize m_RecommendedBufferSize;
        private TextFrame m_RecordingDates;
        private TextFrame m_RecordingTimestamp;
        private RelativeVolumeAdjustmentBindingList m_RelativeVolumeAdjustmentList;
        private TextFrame m_ReleaseTimestamp;
        private TextFrame m_RemixedBy;
        private IdSharp.Tagging.ID3v2.Frames.Reverb m_Reverb;
        private IdSharp.Tagging.ID3v2.Frames.SeekNextTag m_SeekNextTag;
        private TextFrame m_SetSubtitle;
        private SignatureBindingList m_SignatureList;
        private TextFrame m_Subtitle;
        private SynchronizedTextBindingList m_SynchronizedLyricsList;
        private SynchronizedTempoCodes m_SynchronizedTempoCodes;
        private TextFrame m_TaggingTimestamp;
        private TermsOfUseBindingList m_TermsOfUseList;
        private TextFrame m_TimeRecorded;
        private TextFrame m_Title;
        private TextFrame m_TitleSortOrder;
        private TextFrame m_TrackNumber;
        private UniqueFileIdentifierBindingList m_UniqueFileIdentifierList;
        private List<UnknownFrame> m_UnknownFrames;
        private UnsynchronizedLyricsBindingList m_UnsynchronizedLyricsList;
        private UserDefinedTextBindingList m_UserDefinedTextList;
        private UserDefinedUrlBindingList m_UserDefinedUrlList;
        private TextFrame m_Year;
    }
}

