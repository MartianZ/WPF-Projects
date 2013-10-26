namespace IdSharp.Tagging.ID3v2
{
    using IdSharp.ComInterop;
    using IdSharp.Tagging.ID3v2.Frames;
    using IdSharp.Utils;
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    [Guid("56461D17-88CC-4c56-8EEE-1E3A1AA68FEF"), ComVisible(true)]
    public interface IFrameContainer : INotifyPropertyChanged, INotifyInvalidData
    {
        BindingList<IUniqueFileIdentifier> UniqueFileIdentifierList { get; }
        string Album { get; set; }
        string BPM { get; set; }
        string Composer { get; set; }
        string Genre { get; set; }
        string Copyright { get; set; }
        Nullable<int> PlaylistDelayMilliseconds { get; set; }
        string EncodedByWho { get; set; }
        string Lyricist { get; set; }
        string FileType { get; set; }
        string ContentGroup { get; set; }
        string Title { get; set; }
        string Subtitle { get; set; }
        string InitialKey { get; set; }
        ILanguageFrame Languages { get; }
        Nullable<int> LengthMilliseconds { get; set; }
        string MediaType { get; set; }
        string OriginalSourceTitle { get; set; }
        string OriginalFileName { get; set; }
        string OriginalLyricist { get; set; }
        string OriginalArtist { get; set; }
        string OriginalReleaseYear { get; set; }
        string FileOwnerName { get; set; }
        string Artist { get; set; }
        string Accompaniment { get; set; }
        string Conductor { get; set; }
        string RemixedBy { get; set; }
        string DiscNumber { get; set; }
        string Publisher { get; set; }
        string TrackNumber { get; set; }
        string RecordingDates { get; set; }
        string InternetRadioStationName { get; set; }
        string InternetRadioStationOwner { get; set; }
        Nullable<long> FileSizeExcludingTag { get; set; }
        string ISRC { get; set; }
        string EncoderSettings { get; set; }
        string Year { get; set; }
        string DateRecorded { get; set; }
        string TimeRecorded { get; set; }
        string ReleaseTimestamp { get; set; }
        string OriginalReleaseTimestamp { get; set; }
        string RecordingTimestamp { get; set; }
        bool IsPartOfCompilation { get; set; }
        BindingList<ITXXXFrame> UserDefinedText { get; }
        BindingList<IUrlFrame> CommercialInfoUrlList { get; }
        string CopyrightUrl { get; set; }
        string AudioFileUrl { get; set; }
        BindingList<IUrlFrame> ArtistUrlList { get; }
        string AudioSourceUrl { get; set; }
        string InternetRadioStationUrl { get; set; }
        string PaymentUrl { get; set; }
        string PublisherUrl { get; set; }
        BindingList<IWXXXFrame> UserDefinedUrlList { get; }
        IInvolvedPersonList InvolvedPersonList { get; }
        IMusicCDIdentifier MusicCDIdentifier { get; }
        IEventTiming EventTiming { get; }
        IMpegLookupTable MpegLookupTable { get; }
        ISynchronizedTempoCodes SynchronizedTempoCodeList { get; }
        BindingList<IUnsynchronizedText> UnsynchronizedLyricsList { get; }
        BindingList<ISynchronizedText> SynchronizedLyrics { get; }
        BindingList<IComments> CommentsList { get; }
        BindingList<IRelativeVolumeAdjustment> RelativeVolumeAdjustmentList { get; }
        BindingList<IEqualizationList> EqualizationList { get; }
        IReverb Reverb { get; }
        [DispId(1)]
        BindingList<IAttachedPicture> PictureList { get; }
        BindingList<IGeneralEncapsulatedObject> GeneralEncapsulatedObjectList { get; }
        IPlayCount PlayCount { get; }
        BindingList<IPopularimeter> PopularimeterList { get; }
        IRecommendedBufferSize RecommendedBufferSize { get; }
        BindingList<IAudioEncryption> AudioEncryptionList { get; }
        BindingList<ILinkedInformation> LinkedInformationList { get; }
        IPositionSynchronization PositionSynchronization { get; }
        IAudioSeekPointIndex AudioSeekPointIndex { get; }
        BindingList<ITermsOfUse> TermsOfUseList { get; }
        BindingList<ICommercial> CommercialInfoList { get; }
        BindingList<IEncryptionMethod> EncryptionMethodList { get; }
        BindingList<IGroupIdentification> GroupIdentificationList { get; }
        BindingList<IPrivateFrame> PrivateFrameList { get; }
        string EncodingTimestamp { get; set; }
        string TaggingTimestamp { get; set; }
        string Mood { get; set; }
        string AlbumSortOrder { get; set; }
        string ArtistSortOrder { get; set; }
        string TitleSortOrder { get; set; }
        string ProducedNotice { get; set; }
        string SetSubtitle { get; set; }
        IOwnership Ownership { get; }
        ISeekNextTag SeekNextTag { get; }
        BindingList<ISignature> SignatureList { get; }
        IMusicianCreditsList MusicianCreditsList { get; }
        BindingList<IAudioText> AudioTextList { get; }
        void FirePropertyChanged(string propertyName);
        IFrameList GetFrameList(string frameID);
    }
}

