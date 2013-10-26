namespace IdSharp.Tagging.ID3v1
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal sealed class ID3v1 : IID3v1, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ID3v1()
        {
            this.m_TagVersion = ID3v1TagVersion.ID3v11;
            this.m_GenreIndex = 12;
        }

        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler1 = this.PropertyChanged;
            if (handler1 != null)
            {
                handler1(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static string GetString(byte[] byteArray)
        {
            return Encoding.GetEncoding(0x6faf).GetString(byteArray).TrimEnd(new char[1]).TrimEnd(new char[] { ' ' });
        }

        private static string GetString(Stream stream, int length)
        {
            byte[] buffer1 = new byte[length];
            stream.Read(buffer1, 0, length);
            return IdSharp.Tagging.ID3v1.ID3v1.GetString(buffer1);
        }

        private static string GetString(string value, int maxLength)
        {
            if (value == null)
            {
                return null;
            }
            value = value.Trim();
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength).Trim();
            }
            return value;
        }

        public void Read(string path)
        {
            using (FileStream stream1 = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this.ReadStream(stream1);
            }
        }

        public void ReadStream(Stream stream)
        {
            if (stream.Length >= 0x80)
            {
                stream.Seek((long) (-128), SeekOrigin.End);
                if (IdSharp.Tagging.ID3v1.ID3v1.GetString(stream, 3) == "TAG")
                {
                    this.Title = IdSharp.Tagging.ID3v1.ID3v1.GetString(stream, 30);
                    this.Artist = IdSharp.Tagging.ID3v1.ID3v1.GetString(stream, 30);
                    this.Album = IdSharp.Tagging.ID3v1.ID3v1.GetString(stream, 30);
                    this.Year = IdSharp.Tagging.ID3v1.ID3v1.GetString(stream, 4);
                    byte[] buffer1 = new byte[30];
                    stream.Read(buffer1, 0, 30);
                    string text1 = IdSharp.Tagging.ID3v1.ID3v1.GetString(buffer1);
                    if ((buffer1[0x1c] == 0) && (buffer1[0x1d] != 0))
                    {
                        this.Comment = text1.Substring(0, 0x1c).TrimEnd(new char[1]).TrimEnd(new char[] { ' ' });
                        this.TrackNumber = buffer1[0x1d];
                        this.TagVersion = ID3v1TagVersion.ID3v11;
                    }
                    else
                    {
                        this.Comment = text1;
                        this.TrackNumber = 0;
                        this.TagVersion = ID3v1TagVersion.ID3v10;
                    }
                    int num1 = stream.ReadByte();
                    if ((num1 < 0) || (num1 > 0x93))
                    {
                        num1 = 12;
                    }
                    this.GenreIndex = num1;
                }
                else
                {
                    this.Reset();
                }
            }
            else
            {
                this.Reset();
            }
        }

        public void Reset()
        {
            this.Title = null;
            this.Artist = null;
            this.Album = null;
            this.Year = null;
            this.Comment = null;
            this.TrackNumber = 0;
            this.GenreIndex = 12;
            this.TagVersion = ID3v1TagVersion.ID3v11;
        }

        private static byte[] SafeGetBytes(string value)
        {
            if (value == null)
            {
                return new byte[0];
            }
            return Encoding.GetEncoding(0x6faf).GetBytes(value);
        }

        public void Save(string path)
        {
            using (FileStream stream1 = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                byte[] buffer6;
                stream1.Seek((long) -ID3v1Helper.GetTagSize(stream1), SeekOrigin.End);
                byte[] buffer1 = Encoding.ASCII.GetBytes("TAG");
                byte[] buffer2 = IdSharp.Tagging.ID3v1.ID3v1.SafeGetBytes(this.m_Title);
                byte[] buffer3 = IdSharp.Tagging.ID3v1.ID3v1.SafeGetBytes(this.m_Artist);
                byte[] buffer4 = IdSharp.Tagging.ID3v1.ID3v1.SafeGetBytes(this.m_Album);
                byte[] buffer5 = IdSharp.Tagging.ID3v1.ID3v1.SafeGetBytes(this.m_Year);
                stream1.Write(buffer1, 0, 3);
                IdSharp.Tagging.ID3v1.ID3v1.WriteBytesPadded(stream1, buffer2, 30);
                IdSharp.Tagging.ID3v1.ID3v1.WriteBytesPadded(stream1, buffer3, 30);
                IdSharp.Tagging.ID3v1.ID3v1.WriteBytesPadded(stream1, buffer4, 30);
                IdSharp.Tagging.ID3v1.ID3v1.WriteBytesPadded(stream1, buffer5, 4);
                if (this.m_TagVersion == ID3v1TagVersion.ID3v11)
                {
                    buffer6 = IdSharp.Tagging.ID3v1.ID3v1.SafeGetBytes(this.m_Comment);
                    IdSharp.Tagging.ID3v1.ID3v1.WriteBytesPadded(stream1, buffer6, 0x1c);
                    stream1.WriteByte(0);
                    stream1.WriteByte((byte) this.m_TrackNumber);
                }
                else
                {
                    buffer6 = IdSharp.Tagging.ID3v1.ID3v1.SafeGetBytes(this.m_Comment);
                    IdSharp.Tagging.ID3v1.ID3v1.WriteBytesPadded(stream1, buffer6, 30);
                }
                stream1.WriteByte((byte) this.m_GenreIndex);
            }
        }

        private static void WriteBytesPadded(Stream stream, byte[] byteArray, int length)
        {
            int num1 = 0;
            while (((num1 < length) && (num1 < byteArray.Length)) && (byteArray[num1] != 0))
            {
                stream.WriteByte(byteArray[num1]);
                num1++;
            }
            while (num1 < length)
            {
                stream.WriteByte(0);
                num1++;
            }
        }


        public string Album
        {
            get
            {
                return this.m_Album;
            }
            set
            {
                this.m_Album = IdSharp.Tagging.ID3v1.ID3v1.GetString(value, 30);
                this.FirePropertyChanged("Album");
            }
        }

        public string Artist
        {
            get
            {
                return this.m_Artist;
            }
            set
            {
                this.m_Artist = IdSharp.Tagging.ID3v1.ID3v1.GetString(value, 30);
                this.FirePropertyChanged("Artist");
            }
        }

        public string Comment
        {
            get
            {
                return this.m_Comment;
            }
            set
            {
                if (this.m_TagVersion == ID3v1TagVersion.ID3v11)
                {
                    this.m_Comment = IdSharp.Tagging.ID3v1.ID3v1.GetString(value, 0x1c);
                }
                else
                {
                    this.m_Comment = IdSharp.Tagging.ID3v1.ID3v1.GetString(value, 30);
                }
                this.FirePropertyChanged("Comment");
            }
        }

        public int GenreIndex
        {
            get
            {
                return this.m_GenreIndex;
            }
            set
            {
                if ((value >= 0) && (value <= 0x93))
                {
                    this.m_GenreIndex = value;
                }
                this.FirePropertyChanged("GenreIndex");
            }
        }

        public ID3v1TagVersion TagVersion
        {
            get
            {
                return this.m_TagVersion;
            }
            set
            {
                this.m_TagVersion = value;
                this.FirePropertyChanged("TagVersion");
                if (value == ID3v1TagVersion.ID3v11)
                {
                    this.Comment = this.m_Comment;
                }
            }
        }

        public string Title
        {
            get
            {
                return this.m_Title;
            }
            set
            {
                this.m_Title = IdSharp.Tagging.ID3v1.ID3v1.GetString(value, 30);
                this.FirePropertyChanged("Title");
            }
        }

        public int TrackNumber
        {
            get
            {
                return this.m_TrackNumber;
            }
            set
            {
                if ((value >= 0) && (value <= 0xff))
                {
                    this.m_TrackNumber = value;
                    if (this.m_TagVersion == ID3v1TagVersion.ID3v10)
                    {
                        this.TagVersion = ID3v1TagVersion.ID3v11;
                    }
                }
                this.FirePropertyChanged("TrackNumber");
            }
        }

        public string Year
        {
            get
            {
                return this.m_Year;
            }
            set
            {
                this.m_Year = IdSharp.Tagging.ID3v1.ID3v1.GetString(value, 4);
                this.FirePropertyChanged("Year");
            }
        }


        private string m_Album;
        private string m_Artist;
        private string m_Comment;
        private int m_GenreIndex;
        private ID3v1TagVersion m_TagVersion;
        private string m_Title;
        private int m_TrackNumber;
        private string m_Year;
    }
}

