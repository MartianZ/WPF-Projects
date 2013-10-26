namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class GroupIdentification : IGroupIdentification, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GroupIdentification()
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
            using (MemoryStream stream1 = new MemoryStream())
            {
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, this.m_OwnerIdentifier, true));
                stream1.WriteByte(this.m_GroupSymbol);
                if (this.m_GroupDependentData != null)
                {
                    Utils.Write(stream1, this.m_GroupDependentData);
                }
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
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
                    return "GRID";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            throw new NotImplementedException();
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public byte[] GroupDependentData
        {
            get
            {
                if (this.m_GroupDependentData == null)
                {
                    return null;
                }
                return (byte[]) this.m_GroupDependentData.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_GroupDependentData = null;
                }
                else
                {
                    this.m_GroupDependentData = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("GroupDependentData");
            }
        }

        public byte GroupSymbol
        {
            get
            {
                return this.m_GroupSymbol;
            }
            set
            {
                this.m_GroupSymbol = value;
                this.FirePropertyChanged("GroupSymbol");
            }
        }

        public string OwnerIdentifier
        {
            get
            {
                return this.m_OwnerIdentifier;
            }
            set
            {
                this.m_OwnerIdentifier = value;
                this.FirePropertyChanged("OwnerIdentifier");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private byte[] m_GroupDependentData;
        private byte m_GroupSymbol;
        private string m_OwnerIdentifier;
    }
}

