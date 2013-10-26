namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class MusicCDIdentifier : IMusicCDIdentifier, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MusicCDIdentifier()
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

        public byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if ((this.TOC == null) || (this.TOC.Length == 0))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.Write(this.TOC, 0, this.TOC.Length);
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "MCI";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "MCDI";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            if (this.m_FrameHeader.FrameSizeExcludingAdditions > 0)
            {
                this.TOC = Utils.Read(stream, this.m_FrameHeader.FrameSizeExcludingAdditions);
            }
            else
            {
                this.TOC = null;
            }
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public byte[] TOC
        {
            get
            {
                if (this.m_TOC == null)
                {
                    return null;
                }
                return (byte[]) this.m_TOC.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_TOC = null;
                }
                else
                {
                    this.m_TOC = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("TOC");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private byte[] m_TOC;
    }
}

