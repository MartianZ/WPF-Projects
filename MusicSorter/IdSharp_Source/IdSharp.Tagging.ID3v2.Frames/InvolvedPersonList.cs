namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class InvolvedPersonList : IInvolvedPersonList, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public InvolvedPersonList()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_InvolvedPersons = new InvolvedPersonBindingList();
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
            if (this.Items.Count == 0)
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.m_TextEncoding);
                bool flag1 = false;
                using (IEnumerator<IInvolvedPerson> enumerator1 = this.Items.GetEnumerator())
                {
                    while (enumerator1.MoveNext())
                    {
                        IInvolvedPerson person1 = enumerator1.Current;
                        if (!string.IsNullOrEmpty(person1.Involvement) || !string.IsNullOrEmpty(person1.Name))
                        {
                            Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.m_TextEncoding, person1.Involvement, true));
                            Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.m_TextEncoding, person1.Name, true));
                            flag1 = true;
                        }
                    }
                }
                if (!flag1)
                {
                    return new byte[0];
                }
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "IPL";

                case ID3v2TagVersion.ID3v23:
                    return "IPLS";

                case ID3v2TagVersion.ID3v24:
                    return "TIPL";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            this.m_InvolvedPersons.Clear();
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 0)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                while (num1 > 0)
                {
                    string text1 = Utils.ReadString(this.TextEncoding, stream, ref num1);
                    string text2 = Utils.ReadString(this.TextEncoding, stream, ref num1);
                    if (!string.IsNullOrEmpty(text1) || !string.IsNullOrEmpty(text2))
                    {
                        IInvolvedPerson person1 = this.m_InvolvedPersons.AddNew();
                        person1.Involvement = text1;
                        person1.Name = text2;
                    }
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

        public BindingList<IInvolvedPerson> Items
        {
            get
            {
                return this.m_InvolvedPersons;
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


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private InvolvedPersonBindingList m_InvolvedPersons;
        private EncodingType m_TextEncoding;
    }
}

