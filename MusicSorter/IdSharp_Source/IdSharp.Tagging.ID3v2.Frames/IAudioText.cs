namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;

    public interface IAudioText : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        byte[] GetAudioData(AudioScramblingMode audioScramblingMode);
        void SetAudioData(string mimeType, byte[] audioData, bool isMpegOrAac);

        string EquivalentText { get; set; }

        string MimeType { get; set; }

    }
}

