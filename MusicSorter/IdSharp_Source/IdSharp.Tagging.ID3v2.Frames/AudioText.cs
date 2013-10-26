namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class AudioText : IAudioText, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        static AudioText()
        {
            AudioText.m_ScrambleTable = new byte[0x7f];
            AudioText.m_ScrambleTable[0] = 0xfe;
            int num1 = 0;
            while (true)
            {
                byte num2 = AudioText.NextByte(AudioText.m_ScrambleTable[num1]);
                if (num2 == 0xfe)
                {
                    return;
                }
                AudioText.m_ScrambleTable[num1 + 1] = num2;
                num1++;
            }
        }

        public AudioText()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
        }

        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler1 = this.PropertyChanged;
            if (handler1 != null)
            {
                handler1(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public byte[] GetAudioData(AudioScramblingMode audioScramblingMode)
        {
            if (audioScramblingMode == AudioScramblingMode.Default)
            {
                audioScramblingMode = this.m_IsMpegOrAac ? AudioScramblingMode.Unsynchronization : AudioScramblingMode.Scrambling;
            }
            switch (audioScramblingMode)
            {
                case AudioScramblingMode.Unsynchronization:
                    return Utils.ReadUnsynchronized(this.m_AudioData);

                case AudioScramblingMode.Scrambling:
                    return AudioText.Scramble(this.m_AudioData);
            }
            if (this.m_AudioData == null)
            {
                return null;
            }
            return (byte[]) this.m_AudioData.Clone();
        }

        public byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if ((this.m_AudioData == null) || (this.m_AudioData.Length == 0))
            {
                return new byte[0];
            }
            string text1 = this.GetFrameID(tagVersion);
            if (text1 == null)
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                byte[] buffer1 = Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, this.MimeType, true);
                byte[] buffer2 = Utils.GetStringBytes(tagVersion, this.TextEncoding, this.EquivalentText, true);
                stream1.Write(buffer1, 0, buffer1.Length);
                stream1.WriteByte(this.m_IsMpegOrAac ? ((byte) 0) : ((byte) 1));
                stream1.Write(buffer2, 0, buffer2.Length);
                stream1.Write(this.m_AudioData, 0, this.m_AudioData.Length);
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, text1);
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return null;

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "ATXT";
            }
            throw new ArgumentException("Unknown tag version");
        }

        private static byte NextByte(byte n)
        {
            byte num1 = (byte) ((n >> 7) & 1);
            byte num2 = (byte) ((n >> 6) & 1);
            byte num3 = (byte) ((n >> 5) & 1);
            byte num4 = (byte) ((n >> 4) & 1);
            byte num5 = (byte) ((n >> 3) & 1);
            byte num6 = (byte) ((n >> 2) & 1);
            byte num7 = (byte) ((n >> 1) & 1);
            byte num8 = (byte) (n & 1);
            return (byte) (((((((((num2 ^ num3) << 7) + ((num3 ^ num4) << 6)) + ((num4 ^ num5) << 5)) + ((num5 ^ num6) << 4)) + ((num6 ^ num7) << 3)) + ((num7 ^ num8) << 2)) + ((num1 ^ num3) << 1)) + (num2 ^ num4));
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 0)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                if (num1 > 0)
                {
                    this.MimeType = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                    if (num1 > 1)
                    {
                        byte num2 = Utils.ReadByte(stream, ref num1);
                        this.m_IsMpegOrAac = (num2 & 1) == 0;
                        this.EquivalentText = Utils.ReadString(this.TextEncoding, stream, ref num1);
                        if (num1 > 0)
                        {
                            this.m_AudioData = Utils.Read(stream, num1);
                            num1 = 0;
                        }
                    }
                    else
                    {
                        this.EquivalentText = null;
                        this.m_AudioData = null;
                    }
                }
                else
                {
                    this.MimeType = null;
                    this.EquivalentText = null;
                    this.m_AudioData = null;
                }
            }
            else
            {
                this.TextEncoding = EncodingType.ISO88591;
                this.MimeType = null;
                this.EquivalentText = null;
                this.m_AudioData = null;
            }
            if (num1 > 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
        }

        private static byte[] Scramble(byte[] audioData)
        {
            byte[] buffer1 = new byte[audioData.Length];
            int num1 = 0;
            for (int num2 = 0; num1 < audioData.Length; num2++)
            {
                buffer1[num1] = (byte) (audioData[num1] ^ AudioText.m_ScrambleTable[num2]);
                if (num2 == 0x7e)
                {
                    num2 = -1;
                }
                num1++;
            }
            return buffer1;
        }

        public void SetAudioData(string mimeType, byte[] audioData, bool isMpegOrAac)
        {
            this.MimeType = mimeType;
            this.m_IsMpegOrAac = isMpegOrAac;
            if (audioData == null)
            {
                this.m_AudioData = null;
            }
            else if (this.m_IsMpegOrAac)
            {
                this.m_AudioData = Utils.ConvertToUnsynchronized(this.m_AudioData);
            }
            else
            {
                this.m_AudioData = AudioText.Scramble(this.m_AudioData);
            }
            this.FirePropertyChanged("AudioData");
        }


        public string EquivalentText
        {
            get
            {
                return this.m_EquivalentText;
            }
            set
            {
                this.m_EquivalentText = value;
                this.FirePropertyChanged("EquivalentText");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public string MimeType
        {
            get
            {
                return this.m_MimeType;
            }
            set
            {
                this.m_MimeType = value;
                this.FirePropertyChanged("MimeType");
            }
        }

        public EncodingType TextEncoding
        {
            get
            {
                return this.m_TextEncoding;
            }
            set
            {
                this.m_TextEncoding = value;
                this.FirePropertyChanged("TextEncoding");
            }
        }


        private byte[] m_AudioData;
        private string m_EquivalentText;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private bool m_IsMpegOrAac;
        private string m_MimeType;
        private static readonly byte[] m_ScrambleTable;
        private EncodingType m_TextEncoding;
    }
}

