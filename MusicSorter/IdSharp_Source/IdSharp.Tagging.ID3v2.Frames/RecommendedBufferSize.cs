namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class RecommendedBufferSize : IRecommendedBufferSize, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RecommendedBufferSize()
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
            if (this.BufferSize != 0)
            {
                throw new NotImplementedException();
            }
            return new byte[0];
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "BUF";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "RBUF";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            throw new NotImplementedException();
        }


        public int BufferSize
        {
            get
            {
                return this.m_BufferSize;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_BufferSize = value;
                this.FirePropertyChanged("BufferSize");
            }
        }

        public bool EmbeddedInfo
        {
            get
            {
                return this.m_EmbeddedInfo;
            }
            set
            {
                this.m_EmbeddedInfo = value;
                this.FirePropertyChanged("EmbeddedInfo");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public Nullable<int> OffsetToNextTag
        {
            get
            {
                return this.m_OffsetToNextTag;
            }
            set
            {
                Nullable<int> nullable1 = value;
                if ((nullable1.GetValueOrDefault() < 0) && nullable1.HasValue)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_OffsetToNextTag = value;
                this.FirePropertyChanged("OffsetToNextTag");
            }
        }


        private int m_BufferSize;
        private bool m_EmbeddedInfo;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private Nullable<int> m_OffsetToNextTag;
    }
}

