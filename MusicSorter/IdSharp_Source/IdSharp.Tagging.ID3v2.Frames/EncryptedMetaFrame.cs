namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class EncryptedMetaFrame : IEncryptedMetaFrame, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EncryptedMetaFrame()
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
            throw new NotImplementedException();
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "CRM";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return null;
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            throw new NotImplementedException();
        }


        public string ContentExplanation
        {
            get
            {
                return this.m_ContentExplanation;
            }
            set
            {
                this.m_ContentExplanation = value;
                this.FirePropertyChanged("ContentExplanation");
            }
        }

        public byte[] EncryptedData
        {
            get
            {
                if (this.m_EncryptedData == null)
                {
                    return null;
                }
                return (byte[]) this.m_EncryptedData.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_EncryptedData = null;
                }
                else
                {
                    this.m_EncryptedData = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("EncryptedData");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
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


        private string m_ContentExplanation;
        private byte[] m_EncryptedData;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_OwnerIdentifier;
    }
}

