namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class Popularimeter : IPopularimeter, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Popularimeter()
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
            if ((this.m_Rating == 0) && (this.m_PlayCount == 0))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, this.m_UserEmail, true));
                stream1.WriteByte(this.m_Rating);
                Utils.Write(stream1, Utils.GetBytesMinimal(this.m_PlayCount));
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "POP";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "POPM";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 0)
            {
                this.UserEmail = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                if (num1 > 0)
                {
                    this.Rating = Utils.ReadByte(stream, ref num1);
                    if (num1 > 0)
                    {
                        byte[] buffer1 = Utils.Read(stream, num1);
                        this.PlayCount = Utils.ConvertToInt64(buffer1);
                    }
                    else
                    {
                        this.PlayCount = 0;
                    }
                }
                else
                {
                    this.Rating = 0;
                    this.PlayCount = 0;
                }
            }
            else
            {
                this.UserEmail = null;
                this.Rating = 0;
                this.PlayCount = 0;
            }
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public long PlayCount
        {
            get
            {
                return this.m_PlayCount;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_PlayCount = value;
                this.FirePropertyChanged("PlayCount");
            }
        }

        public byte Rating
        {
            get
            {
                return this.m_Rating;
            }
            set
            {
                this.m_Rating = value;
                this.FirePropertyChanged("Rating");
            }
        }

        public string UserEmail
        {
            get
            {
                return this.m_UserEmail;
            }
            set
            {
                this.m_UserEmail = value;
                this.FirePropertyChanged("UserEmail");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private long m_PlayCount;
        private byte m_Rating;
        private string m_UserEmail;
    }
}

