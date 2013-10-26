namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class PlayCount : IPlayCount, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PlayCount()
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
            if (!this.m_Value.HasValue)
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                Nullable<long> nullable1 = this.Value;
                if ((nullable1.GetValueOrDefault() <= 0xffffffff) && nullable1.HasValue)
                {
                    Utils.Write(stream1, Utils.Get4Bytes((uint) this.Value.Value));
                }
                else
                {
                    Utils.Write(stream1, Utils.GetBytesMinimal(this.Value.Value));
                }
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "CNT";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "PCNT";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            long num2 = 0;
            while (num1 > 0)
            {
                num2 = num2 << 8;
                num2 += Utils.ReadByte(stream, ref num1);
            }
            this.Value = new Nullable<long>(num2);
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public Nullable<long> Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                Nullable<long> nullable1 = value;
                if ((nullable1.GetValueOrDefault() < 0) && nullable1.HasValue)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_Value = value;
                this.FirePropertyChanged("Value");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private Nullable<long> m_Value;
    }
}

