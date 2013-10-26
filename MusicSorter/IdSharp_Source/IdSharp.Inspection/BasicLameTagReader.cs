namespace IdSharp.Inspection
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class BasicLameTagReader
    {
        public BasicLameTagReader(string path)
        {
            this.m_IsLameTagFound = true;
            this.m_Tag = new LameTag();
            using (BinaryReader reader1 = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                int num1 = ID3v2Helper.GetTagSize(reader1.BaseStream);
                reader1.BaseStream.Seek((long) num1, SeekOrigin.Begin);
                StartOfFile file1 = StartOfFile.FromBinaryReader(reader1);
                reader1.BaseStream.Seek((long) num1, SeekOrigin.Begin);
                string text1 = Encoding.ASCII.GetString(file1.Info1);
                string text2 = Encoding.ASCII.GetString(file1.Info2);
                string text3 = Encoding.ASCII.GetString(file1.Info3);
                switch (text1)
                {
                    case "Xing":
                    case "Info":
                        reader1.BaseStream.Seek((long) 13, SeekOrigin.Current);
                        break;

                    default:
                        switch (text2)
                        {
                            case "Xing":
                            case "Info":
                                reader1.BaseStream.Seek((long) 0x15, SeekOrigin.Current);
                                break;
                        }
                        switch (text3)
                        {
                            case "Xing":
                            case "Info":
                                reader1.BaseStream.Seek((long) 0x24, SeekOrigin.Current);
                                break;
                        }
                        this.m_IsLameTagFound = true;
                        break;
                }
                reader1.BaseStream.Seek((long) 0x77, SeekOrigin.Current);
                this.m_Tag = LameTag.FromBinaryReader(reader1);
                reader1.BaseStream.Seek((long) -Marshal.SizeOf(typeof(LameTag)), SeekOrigin.Current);
                OldLameHeader header1 = OldLameHeader.FromBinaryReader(reader1);
                this.m_VersionStringNonLameTag = Encoding.ASCII.GetString(header1.VersionString);
            }
            if (this.m_Tag.VersionString[1] == 0x2e)
            {
                byte[] buffer1 = new byte[6];
                int num2 = 0;
                while ((num2 < 4) || ((num2 == 4) && (this.m_Tag.VersionString[num2] == 0x62)))
                {
                    buffer1[num2] = this.m_Tag.VersionString[num2];
                    num2++;
                }
                Array.Resize<byte>(ref buffer1, num2);
                this.m_VersionString = Encoding.ASCII.GetString(buffer1);
            }
            else
            {
                this.m_VersionString = "";
            }
            if (Encoding.ASCII.GetString(this.m_Tag.Encoder) != "LAME")
            {
                this.m_IsLameTagFound = false;
            }
            this.m_Preset = (ushort) (((this.m_Tag.Surround_Preset[0] << 8) + this.m_Tag.Surround_Preset[1]) & 0x7ff);
            this.m_PresetGuess = new PresetGuesser().GuessPreset(this.VersionStringNonLameTag, this.m_Tag.Bitrate, this.m_Tag.Quality, this.m_Tag.TagRevision_EncodingMethod, this.m_Tag.NoiseShaping, this.m_Tag.StereoMode, this.m_Tag.EncodingFlags_ATHType, this.m_Tag.Lowpass, out this.m_IsPresetGuessNonBitrate);
        }


        public byte Bitrate
        {
            get
            {
                return this.m_Tag.Bitrate;
            }
        }

        public byte EncodingMethod
        {
            get
            {
                return this.m_Tag.TagRevision_EncodingMethod;
            }
        }

        public bool IsLameTagFound
        {
            get
            {
                return this.m_IsLameTagFound;
            }
        }

        public bool IsPresetGuessNonBitrate
        {
            get
            {
                return this.m_IsPresetGuessNonBitrate;
            }
        }

        public ushort Preset
        {
            get
            {
                return this.m_Preset;
            }
        }

        public LamePreset PresetGuess
        {
            get
            {
                return this.m_PresetGuess;
            }
        }

        public string VersionString
        {
            get
            {
                return this.m_VersionString;
            }
        }

        public string VersionStringNonLameTag
        {
            get
            {
                return this.m_VersionStringNonLameTag;
            }
        }


        private const byte Info1Offset = 13;
        private const byte Info2Offset = 0x15;
        private const byte Info3Offset = 0x24;
        private const byte LAMETagOffset = 0x77;
        private bool m_IsLameTagFound;
        private bool m_IsPresetGuessNonBitrate;
        private ushort m_Preset;
        private LamePreset m_PresetGuess;
        private LameTag m_Tag;
        private string m_VersionString;
        private string m_VersionStringNonLameTag;
    }
}

