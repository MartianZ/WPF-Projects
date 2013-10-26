namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class RelativeVolumeAdjustment : IRelativeVolumeAdjustment, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelativeVolumeAdjustment()
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
            return new byte[0];
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "RVA";

                case ID3v2TagVersion.ID3v23:
                    return "RVAD";

                case ID3v2TagVersion.ID3v24:
                    return "RVA2";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 0)
            {
                bool flag1 = this.m_FrameHeader.TagVersion == ID3v2TagVersion.ID3v24;
                if (flag1)
                {
                    this.Identification = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                    while (num1 >= 3)
                    {
                        Utils.ReadByte(stream, ref num1);
                        Utils.ReadInt16(stream, ref num1);
                        if (num1 > 0)
                        {
                            byte num2 = Utils.ReadByte(stream, ref num1);
                            if ((num2 == 0) || (num1 < num2))
                            {
                                break;
                            }
                            byte[] buffer1 = Utils.Read(stream, num2);
                            num1 -= buffer1.Length;
                        }
                    }
                    if (num1 > 0)
                    {
                        stream.Seek((long) (num1 - this.m_FrameHeader.FrameSizeExcludingAdditions), SeekOrigin.Current);
                        num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
                        flag1 = false;
                    }
                }
                if (!flag1)
                {
                    byte num3 = Utils.ReadByte(stream, ref num1);
                    if (num1 > 0)
                    {
                        byte num4 = Utils.ReadByte(stream, ref num1);
                        int num5 = num4 / 8;
                        if (num1 >= num5)
                        {
                            byte[] buffer2 = Utils.Read(stream, num5, ref num1);
                            this.FrontRightAdjustment = (decimal) (Utils.ConvertToInt64(buffer2) * (Utils.IsBitSet(num3, 0) ? ((long) 1) : ((long) (-1))));
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer3 = Utils.Read(stream, num5, ref num1);
                            this.FrontLeftAdjustment = (decimal) (Utils.ConvertToInt64(buffer3) * (Utils.IsBitSet(num3, 1) ? ((long) 1) : ((long) (-1))));
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer4 = Utils.Read(stream, num5, ref num1);
                            this.FrontRightPeak = (decimal) Utils.ConvertToInt64(buffer4);
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer5 = Utils.Read(stream, num5, ref num1);
                            this.FrontLeftPeak = (decimal) Utils.ConvertToInt64(buffer5);
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer6 = Utils.Read(stream, num5, ref num1);
                            this.BackRightAdjustment = (decimal) (Utils.ConvertToInt64(buffer6) * (Utils.IsBitSet(num3, 2) ? ((long) 1) : ((long) (-1))));
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer7 = Utils.Read(stream, num5, ref num1);
                            this.BackLeftAdjustment = (decimal) (Utils.ConvertToInt64(buffer7) * (Utils.IsBitSet(num3, 3) ? ((long) 1) : ((long) (-1))));
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer8 = Utils.Read(stream, num5, ref num1);
                            this.BackRightPeak = (decimal) Utils.ConvertToInt64(buffer8);
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer9 = Utils.Read(stream, num5, ref num1);
                            this.BackLeftPeak = (decimal) Utils.ConvertToInt64(buffer9);
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer10 = Utils.Read(stream, num5, ref num1);
                            this.FrontCenterAdjustment = (decimal) (Utils.ConvertToInt64(buffer10) * (Utils.IsBitSet(num3, 4) ? ((long) 1) : ((long) (-1))));
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer11 = Utils.Read(stream, num5, ref num1);
                            this.FrontCenterPeak = (decimal) Utils.ConvertToInt64(buffer11);
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer12 = Utils.Read(stream, num5, ref num1);
                            this.SubwooferAdjustment = (decimal) (Utils.ConvertToInt64(buffer12) * (Utils.IsBitSet(num3, 5) ? ((long) 1) : ((long) (-1))));
                        }
                        if (num1 >= num5)
                        {
                            byte[] buffer13 = Utils.Read(stream, num5, ref num1);
                            this.SubwooferPeak = (decimal) Utils.ConvertToInt64(buffer13);
                        }
                    }
                }
                if (num1 > 0)
                {
                    Trace.WriteLine("Invalid RVA2/RVAD/RVA frame");
                    stream.Seek((long) num1, SeekOrigin.Current);
                }
            }
        }

        private void WriteID3v24ChannelItem(MemoryStream memoryStream, ChannelType channelType, decimal adjustment, decimal peak)
        {
            if ((adjustment != new decimal(0)) || (peak != new decimal(0)))
            {
                memoryStream.WriteByte((byte) channelType);
                if (adjustment <= new decimal(0x40))
                {
                    bool flag1 = adjustment >= new decimal(-64);
                }
                throw new NotImplementedException();
            }
        }


        public decimal BackCenterAdjustment
        {
            get
            {
                return this.m_BackCenterAdjustment;
            }
            set
            {
                this.m_BackCenterAdjustment = value;
                this.FirePropertyChanged("BackCenterAdjustment");
            }
        }

        public decimal BackCenterPeak
        {
            get
            {
                return this.m_BackCenterPeak;
            }
            set
            {
                this.m_BackCenterPeak = value;
                this.FirePropertyChanged("BackCenterPeak");
            }
        }

        public decimal BackLeftAdjustment
        {
            get
            {
                return this.m_BackLeftAdjustment;
            }
            set
            {
                this.m_BackLeftAdjustment = value;
                this.FirePropertyChanged("BackLeftAdjustment");
            }
        }

        public decimal BackLeftPeak
        {
            get
            {
                return this.m_BackLeftPeak;
            }
            set
            {
                this.m_BackLeftPeak = value;
                this.FirePropertyChanged("BackLeftPeak");
            }
        }

        public decimal BackRightAdjustment
        {
            get
            {
                return this.m_BackRightAdjustment;
            }
            set
            {
                this.m_BackRightAdjustment = value;
                this.FirePropertyChanged("BackRightAdjustment");
            }
        }

        public decimal BackRightPeak
        {
            get
            {
                return this.m_BackRightPeak;
            }
            set
            {
                this.m_BackRightPeak = value;
                this.FirePropertyChanged("BackRightPeak");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public decimal FrontCenterAdjustment
        {
            get
            {
                return this.m_FrontCenterAdjustment;
            }
            set
            {
                this.m_FrontCenterAdjustment = value;
                this.FirePropertyChanged("FrontCenterAdjustment");
            }
        }

        public decimal FrontCenterPeak
        {
            get
            {
                return this.m_FrontCenterPeak;
            }
            set
            {
                this.m_FrontCenterPeak = value;
                this.FirePropertyChanged("FrontCenterPeak");
            }
        }

        public decimal FrontLeftAdjustment
        {
            get
            {
                return this.m_FrontLeftAdjustment;
            }
            set
            {
                this.m_FrontLeftAdjustment = value;
                this.FirePropertyChanged("FrontLeftAdjustment");
            }
        }

        public decimal FrontLeftPeak
        {
            get
            {
                return this.m_FrontLeftPeak;
            }
            set
            {
                this.m_FrontLeftPeak = value;
                this.FirePropertyChanged("FrontLeftPeak");
            }
        }

        public decimal FrontRightAdjustment
        {
            get
            {
                return this.m_FrontRightAdjustment;
            }
            set
            {
                this.m_FrontRightAdjustment = value;
                this.FirePropertyChanged("FrontRightAdjustment");
            }
        }

        public decimal FrontRightPeak
        {
            get
            {
                return this.m_FrontRightPeak;
            }
            set
            {
                this.m_FrontRightPeak = value;
                this.FirePropertyChanged("FrontRightPeak");
            }
        }

        public string Identification
        {
            get
            {
                return this.m_Identification;
            }
            set
            {
                this.m_Identification = value;
                this.FirePropertyChanged("Identification");
            }
        }

        public decimal MasterAdjustment
        {
            get
            {
                return this.m_MasterAdjustment;
            }
            set
            {
                this.m_MasterAdjustment = value;
                this.FirePropertyChanged("MasterAdjustment");
            }
        }

        public decimal MasterPeak
        {
            get
            {
                return this.m_MasterPeak;
            }
            set
            {
                this.m_MasterPeak = value;
                this.FirePropertyChanged("MasterPeak");
            }
        }

        public decimal OtherAdjustment
        {
            get
            {
                return this.m_OtherAdjustment;
            }
            set
            {
                this.m_OtherAdjustment = value;
                this.FirePropertyChanged("OtherAdjustment");
            }
        }

        public decimal OtherPeak
        {
            get
            {
                return this.m_OtherPeak;
            }
            set
            {
                this.m_OtherPeak = value;
                this.FirePropertyChanged("OtherPeak");
            }
        }

        public decimal SubwooferAdjustment
        {
            get
            {
                return this.m_SubwooferAdjustment;
            }
            set
            {
                this.m_SubwooferAdjustment = value;
                this.FirePropertyChanged("SubwooferAdjustment");
            }
        }

        public decimal SubwooferPeak
        {
            get
            {
                return this.m_SubwooferPeak;
            }
            set
            {
                this.m_SubwooferPeak = value;
                this.FirePropertyChanged("SubwooferPeak");
            }
        }


        private decimal m_BackCenterAdjustment;
        private decimal m_BackCenterPeak;
        private decimal m_BackLeftAdjustment;
        private decimal m_BackLeftPeak;
        private decimal m_BackRightAdjustment;
        private decimal m_BackRightPeak;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private decimal m_FrontCenterAdjustment;
        private decimal m_FrontCenterPeak;
        private decimal m_FrontLeftAdjustment;
        private decimal m_FrontLeftPeak;
        private decimal m_FrontRightAdjustment;
        private decimal m_FrontRightPeak;
        private const byte m_ID3v24BitsRepresentingPeak = 0x10;
        private string m_Identification;
        private decimal m_MasterAdjustment;
        private decimal m_MasterPeak;
        private decimal m_OtherAdjustment;
        private decimal m_OtherPeak;
        private decimal m_SubwooferAdjustment;
        private decimal m_SubwooferPeak;
    }
}

