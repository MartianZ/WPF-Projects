namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class UniqueFileIdentifier : IUniqueFileIdentifier, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public UniqueFileIdentifier()
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
            if ((this.m_Identifier == null) || (this.m_Identifier.Length == 0))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_OwnerIdentifier));
                stream1.WriteByte(0);
                Utils.Write(stream1, this.m_Identifier);
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "UFI";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "UFID";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 0)
            {
                this.OwnerIdentifier = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                if (num1 > 0)
                {
                    this.Identifier = Utils.Read(stream, num1);
                }
            }
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public byte[] Identifier
        {
            get
            {
                if (this.m_Identifier == null)
                {
                    return null;
                }
                return (byte[]) this.m_Identifier.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_Identifier = null;
                }
                else
                {
                    this.m_Identifier = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("Identifier");
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
        private byte[] m_Identifier;
        private string m_OwnerIdentifier;
    }
}

