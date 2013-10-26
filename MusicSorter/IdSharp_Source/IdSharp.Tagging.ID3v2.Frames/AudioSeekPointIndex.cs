namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class AudioSeekPointIndex : IAudioSeekPointIndex, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AudioSeekPointIndex()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_FractionAtIndex = new BindingList<short>();
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
            if (((this.IndexedDataLength != 0) && (this.BitsPerIndexPoint != 0)) && (this.FractionAtIndex.Count != 0))
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
                    return null;

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "ASPI";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FractionAtIndex.Clear();
            throw new NotImplementedException();
        }


        public byte BitsPerIndexPoint
        {
            get
            {
                return this.m_BitsPerIndexPoint;
            }
            set
            {
                this.m_BitsPerIndexPoint = value;
                this.FirePropertyChanged("BitsPerIndexPoint");
            }
        }

        public BindingList<short> FractionAtIndex
        {
            get
            {
                return this.m_FractionAtIndex;
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public int IndexedDataLength
        {
            get
            {
                return this.m_IndexedDataLength;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_IndexedDataLength = value;
                this.FirePropertyChanged("IndexedDataLength");
            }
        }

        public int IndexedDataStart
        {
            get
            {
                return this.m_IndexedDataStart;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_IndexedDataStart = value;
                this.FirePropertyChanged("IndexedDataStart");
            }
        }


        private byte m_BitsPerIndexPoint;
        private BindingList<short> m_FractionAtIndex;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private int m_IndexedDataLength;
        private int m_IndexedDataStart;
    }
}

