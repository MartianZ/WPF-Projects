namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal sealed class AttachedPicture : IAttachedPicture, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AttachedPicture()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_TextEncoding = EncodingType.Unicode;
            this.m_PictureType = IdSharp.Tagging.ID3v2.PictureType.CoverFront;
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
            if ((this.m_PictureData == null) || (this.m_PictureData.Length == 0))
            {
                return new byte[0];
            }
            this.TextEncoding = EncodingType.ISO88591;
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.m_TextEncoding);
                if (tagVersion == ID3v2TagVersion.ID3v22)
                {
                    string text1 = this.PictureExtension;
                    if (string.IsNullOrEmpty(text1) || (text1.Length < 3))
                    {
                        text1 = "   ";
                    }
                    else if (text1.Length > 3)
                    {
                        text1 = text1.Substring(0, 3);
                    }
                    Utils.Write(stream1, Encoding.ASCII.GetBytes(text1));
                }
                else
                {
                    this.SetMimeType();
                    Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_MimeType));
                    stream1.WriteByte(0);
                }
                stream1.WriteByte((byte) this.m_PictureType);
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.m_TextEncoding, this.m_Description, true));
                Utils.Write(stream1, this.m_PictureData);
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "PIC";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "APIC";
            }
            throw new ArgumentException("Unknown tag version");
        }

        private void LoadPicture()
        {
            this.m_PictureCached = true;
            if (this.m_PictureData == null)
            {
                this.Picture = null;
            }
            else
            {
                using (MemoryStream stream1 = new MemoryStream(this.m_PictureData))
                {
                    bool flag1 = false;
                    try
                    {
                        this.m_LoadingPicture = true;
                        try
                        {
                            this.Picture = Image.FromStream(stream1);
                        }
                        finally
                        {
                            this.m_LoadingPicture = false;
                        }
                    }
                    catch (OutOfMemoryException)
                    {
                        string text1 = string.Format("OutOfMemoryException caught in APIC's PictureData setter", new object[0]);
                        Trace.WriteLine(text1);
                        flag1 = true;
                    }
                    catch (ArgumentException)
                    {
                        string text2 = string.Format("ArgumentException caught in APIC's PictureData setter", new object[0]);
                        Trace.WriteLine(text2);
                        flag1 = true;
                    }
                    if (flag1)
                    {
                        if (this.m_Picture != null)
                        {
                            this.m_Picture.Dispose();
                        }
                        this.m_Picture = null;
                        try
                        {
                            string text3 = Utils.ISO88591GetString(this.m_PictureData);
                            if (text3.Contains("://"))
                            {
                                this.MimeType = "-->";
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 >= 6)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v22)
                {
                    Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    num1 -= 3;
                }
                else
                {
                    this.MimeType = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                }
                this.PictureType = (IdSharp.Tagging.ID3v2.PictureType) Utils.ReadByte(stream, ref num1);
                this.Description = Utils.ReadString(this.TextEncoding, stream, ref num1);
                if (num1 > 0)
                {
                    byte[] buffer1 = Utils.Read(stream, num1);
                    num1 = 0;
                    this.m_ReadingTag = true;
                    try
                    {
                        this.m_PictureCached = false;
                        this.PictureData = buffer1;
                        goto Label_00D1;
                    }
                    finally
                    {
                        this.m_ReadingTag = false;
                    }
                }
                this.PictureData = null;
            }
            else
            {
                this.TextEncoding = EncodingType.ISO88591;
                this.Description = null;
                this.MimeType = null;
                this.PictureType = IdSharp.Tagging.ID3v2.PictureType.CoverFront;
                this.PictureData = null;
            }
        Label_00D1:
            if (num1 > 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
        }

        private void SetMimeType()
        {
            if (this.m_Picture != null)
            {
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Bmp))
                {
                    this.MimeType = "image/bmp";
                }
                else if (this.m_Picture.RawFormat.Equals(ImageFormat.Emf))
                {
                    this.MimeType = "image/x-emf";
                }
                else if (!this.m_Picture.RawFormat.Equals(ImageFormat.Exif))
                {
                    if (this.m_Picture.RawFormat.Equals(ImageFormat.Gif))
                    {
                        this.MimeType = "image/gif";
                    }
                    else if (!this.m_Picture.RawFormat.Equals(ImageFormat.Icon))
                    {
                        if (this.m_Picture.RawFormat.Equals(ImageFormat.Jpeg))
                        {
                            this.MimeType = "image/jpeg";
                        }
                        else if (this.m_Picture.RawFormat.Equals(ImageFormat.MemoryBmp))
                        {
                            this.MimeType = "image/bmp";
                        }
                        else if (this.m_Picture.RawFormat.Equals(ImageFormat.Png))
                        {
                            this.MimeType = "image/png";
                        }
                        else if (this.m_Picture.RawFormat.Equals(ImageFormat.Tiff))
                        {
                            this.MimeType = "image/tiff";
                        }
                        else if (this.m_Picture.RawFormat.Equals(ImageFormat.Wmf))
                        {
                            this.MimeType = "image/x-wmf";
                        }
                    }
                }
            }
        }


        public string Description
        {
            get
            {
                return this.m_Description;
            }
            set
            {
                if (this.m_Description != value)
                {
                    this.m_Description = value;
                    this.FirePropertyChanged("Description");
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

        public string MimeType
        {
            get
            {
                return this.m_MimeType;
            }
            set
            {
                if (this.m_MimeType != value)
                {
                    this.m_MimeType = value;
                    this.FirePropertyChanged("MimeType");
                }
            }
        }

        public Image Picture
        {
            get
            {
                if (!this.m_PictureCached)
                {
                    this.LoadPicture();
                }
                if (this.m_Picture != null)
                {
                    return (Image) this.m_Picture.Clone();
                }
                return null;
            }
            set
            {
                if (this.m_Picture != value)
                {
                    if (this.m_Picture != null)
                    {
                        this.m_Picture.Dispose();
                    }
                    this.m_Picture = value;
                    if (value == null)
                    {
                        this.m_PictureData = null;
                    }
                    else if (!this.m_LoadingPicture)
                    {
                        using (MemoryStream stream1 = new MemoryStream())
                        {
                            value.Save(stream1, value.RawFormat);
                            this.m_PictureData = stream1.ToArray();
                        }
                        this.SetMimeType();
                    }
                }
                this.FirePropertyChanged("Picture");
            }
        }

        public byte[] PictureData
        {
            get
            {
                if (this.m_PictureData == null)
                {
                    return null;
                }
                return (byte[]) this.m_PictureData.Clone();
            }
            set
            {
                if (this.m_PictureData != value)
                {
                    this.m_PictureData = value;
                    if ((value != null) && !this.m_ReadingTag)
                    {
                        this.LoadPicture();
                    }
                    this.FirePropertyChanged("PictureData");
                }
            }
        }

        public string PictureExtension
        {
            get
            {
                if (this.m_Picture == null)
                {
                    return null;
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Bmp))
                {
                    return "bmp";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Emf))
                {
                    return "emf";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Exif))
                {
                    return null;
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Gif))
                {
                    return "gif";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Icon))
                {
                    return "ico";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Jpeg))
                {
                    return "jpg";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.MemoryBmp))
                {
                    return "bmp";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Png))
                {
                    return "png";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Tiff))
                {
                    return "tif";
                }
                if (this.m_Picture.RawFormat.Equals(ImageFormat.Wmf))
                {
                    return "wmf";
                }
                return "";
            }
        }

        public IdSharp.Tagging.ID3v2.PictureType PictureType
        {
            get
            {
                return this.m_PictureType;
            }
            set
            {
                if (this.m_PictureType != value)
                {
                    this.m_PictureType = value;
                    this.FirePropertyChanged("PictureType");
                }
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
                if (this.m_TextEncoding != value)
                {
                    this.m_TextEncoding = value;
                    this.FirePropertyChanged("TextEncoding");
                }
            }
        }


        private string m_Description;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private bool m_LoadingPicture;
        private string m_MimeType;
        private Image m_Picture;
        private bool m_PictureCached;
        private byte[] m_PictureData;
        private IdSharp.Tagging.ID3v2.PictureType m_PictureType;
        private bool m_ReadingTag;
        private EncodingType m_TextEncoding;
    }
}

