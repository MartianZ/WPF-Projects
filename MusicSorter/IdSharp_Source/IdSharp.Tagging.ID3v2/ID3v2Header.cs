namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Diagnostics;
    using System.IO;

    internal sealed class ID3v2Header : IID3v2Header
    {
        public ID3v2Header()
        {
            this.Clear();
        }

        public ID3v2Header(Stream stream, bool readIdentifier)
        {
            this.Read(stream, readIdentifier);
        }

        private void Clear()
        {
            this.m_TagVersion = ID3v2TagVersion.ID3v23;
            this.m_TagVersionRevision = 0;
            this.m_TagSize = 0;
            this.m_UsesUnsynchronization = false;
            this.m_HasExtendedHeader = false;
            this.m_IsExperimental = false;
        }

        public byte[] GetBytes()
        {
            byte[] buffer1 = new byte[10];
            buffer1[0] = 0x49;
            buffer1[1] = 0x44;
            buffer1[2] = 0x33;
            buffer1[3] = (byte) this.m_TagVersion;
            buffer1[4] = this.m_TagVersionRevision;
            buffer1[5] = 0;
            if (this.m_UsesUnsynchronization)
            {
                buffer1[5] = (byte) (buffer1[5] + 0x80);
            }
            if (this.m_HasExtendedHeader)
            {
                buffer1[5] = (byte) (buffer1[5] + 0x40);
            }
            if (this.m_IsExperimental)
            {
                buffer1[5] = (byte) (buffer1[5] + 0x20);
            }
            buffer1[6] = (byte) ((this.m_TagSize >> 0x15) & 0x7f);
            buffer1[7] = (byte) ((this.m_TagSize >> 14) & 0x7f);
            buffer1[8] = (byte) ((this.m_TagSize >> 7) & 0x7f);
            buffer1[9] = (byte) (this.m_TagSize & 0x7f);
            return buffer1;
        }

        public void Read(Stream stream)
        {
            byte[] buffer1 = Utils.Read(stream, 3);
            if (((buffer1[0] != 0x49) || (buffer1[1] != 0x44)) || (buffer1[2] != 0x33))
            {
                string text1 = "'ID3' marker not found";
                Trace.WriteLine(text1);
                throw new InvalidDataException(text1);
            }
            this.Read(stream, false);
        }

        private void Read(Stream stream, bool readIdentifier)
        {
            if (readIdentifier)
            {
                this.Read(stream);
            }
            else
            {
                byte[] buffer1 = Utils.Read(stream, 7);
                if ((buffer1[0] < 2) || (buffer1[0] > 4))
                {
                    string text1 = string.Format("ID3 Version '{0}' not recognized (valid versions are 2, 3, and 4)", buffer1[0]);
                    Trace.WriteLine(text1);
                    throw new InvalidDataException(text1);
                }
                this.m_TagVersion = (ID3v2TagVersion) buffer1[0];
                this.m_TagVersionRevision = buffer1[1];
                switch (this.m_TagVersion)
                {
                    case ID3v2TagVersion.ID3v22:
                        this.m_UsesUnsynchronization = (buffer1[2] & 0x80) == 0x80;
                        this.m_IsFooterPresent = false;
                        this.m_IsCompressed = (buffer1[2] & 0x40) == 0x40;
                        break;

                    case ID3v2TagVersion.ID3v23:
                        this.UsesUnsynchronization = (buffer1[2] & 0x80) == 0x80;
                        this.m_HasExtendedHeader = (buffer1[2] & 0x40) == 0x40;
                        this.m_IsExperimental = (buffer1[2] & 0x20) == 0x20;
                        this.m_IsFooterPresent = false;
                        this.m_IsCompressed = false;
                        break;

                    case ID3v2TagVersion.ID3v24:
                        this.m_UsesUnsynchronization = (buffer1[2] & 0x80) == 0x80;
                        this.m_HasExtendedHeader = (buffer1[2] & 0x40) == 0x40;
                        this.m_IsExperimental = (buffer1[2] & 0x20) == 0x20;
                        this.m_IsFooterPresent = (buffer1[2] & 0x10) == 0x10;
                        this.m_IsCompressed = false;
                        break;
                }
                this.m_TagSize = buffer1[3] << 0x15;
                this.m_TagSize += buffer1[4] << 14;
                this.m_TagSize += buffer1[5] << 7;
                this.m_TagSize += buffer1[6];
            }
        }


        public bool HasExtendedHeader
        {
            get
            {
                return this.m_HasExtendedHeader;
            }
            set
            {
                this.m_HasExtendedHeader = value;
            }
        }

        public bool IsCompressed
        {
            get
            {
                if (this.m_TagVersion == ID3v2TagVersion.ID3v22)
                {
                    return this.m_IsCompressed;
                }
                return false;
            }
            set
            {
                if (this.m_TagVersion == ID3v2TagVersion.ID3v22)
                {
                    this.m_IsCompressed = value;
                }
                else
                {
                    this.m_IsCompressed = false;
                }
            }
        }

        public bool IsExperimental
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_IsExperimental;
                }
                return false;
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_IsExperimental = value;
                }
                else
                {
                    this.m_IsExperimental = false;
                }
            }
        }

        public bool IsFooterPresent
        {
            get
            {
                if (this.m_TagVersion == ID3v2TagVersion.ID3v24)
                {
                    return this.m_IsFooterPresent;
                }
                return false;
            }
            set
            {
                if (this.m_TagVersion == ID3v2TagVersion.ID3v24)
                {
                    this.m_IsFooterPresent = value;
                }
                else
                {
                    this.m_IsFooterPresent = false;
                }
            }
        }

        public int TagSize
        {
            get
            {
                return this.m_TagSize;
            }
            set
            {
                if (value > 0xfffffff)
                {
                    string text1 = string.Format("Argument 'value' out of range.  Maximum tag size is {0}.", 0xfffffff);
                    Trace.WriteLine(text1);
                    throw new ArgumentOutOfRangeException("value", value, text1);
                }
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }
                this.m_TagSize = value;
            }
        }

        public ID3v2TagVersion TagVersion
        {
            get
            {
                return this.m_TagVersion;
            }
            set
            {
                this.m_TagVersion = value;
            }
        }

        public byte TagVersionRevision
        {
            get
            {
                return this.m_TagVersionRevision;
            }
            set
            {
                this.m_TagVersionRevision = value;
            }
        }

        public bool UsesUnsynchronization
        {
            get
            {
                return this.m_UsesUnsynchronization;
            }
            set
            {
                this.m_UsesUnsynchronization = value;
            }
        }


        private bool m_HasExtendedHeader;
        private bool m_IsCompressed;
        private bool m_IsExperimental;
        private bool m_IsFooterPresent;
        private int m_TagSize;
        private ID3v2TagVersion m_TagVersion;
        private byte m_TagVersionRevision;
        private bool m_UsesUnsynchronization;
    }
}

