namespace IdSharp.Inspection
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class DescriptiveLameTagReader
    {
        public DescriptiveLameTagReader(string path)
        {
            this.m_Mpeg = new MpegAudio(path);
            this.m_BasicReader = new BasicLameTagReader(path);
            this.DeterminePresetRelatedValues();
        }

        private string DeterminePreset(out IdSharp.Inspection.UsePresetGuess usePresetGuess)
        {
            string text1;
            usePresetGuess = IdSharp.Inspection.UsePresetGuess.NotNeeded;
            int num1 = this.m_BasicReader.Preset;
            if ((num1 >= 8) && (num1 <= 320))
            {
                text1 = num1.ToString();
                if (this.m_BasicReader.EncodingMethod == 1)
                {
                    text1 = "cbr " + text1;
                }
                usePresetGuess = IdSharp.Inspection.UsePresetGuess.UseGuess;
            }
            else
            {
                switch (num1)
                {
                    case 0x3e8:
                        text1 = "r3mix";
                        goto Label_0208;

                    case 0x3e9:
                        text1 = "--alt-preset standard";
                        goto Label_0208;

                    case 0x3ea:
                        text1 = "--alt-preset extreme";
                        goto Label_0208;

                    case 0x3eb:
                        text1 = "--alt-preset insane";
                        goto Label_0208;

                    case 0x3ec:
                        text1 = "--alt-preset fast standard";
                        goto Label_0208;

                    case 0x3ed:
                        text1 = "--alt-preset fast extreme";
                        goto Label_0208;

                    case 0x3ee:
                        text1 = "preset medium";
                        goto Label_0208;

                    case 0x3ef:
                        text1 = "preset fast medium";
                        goto Label_0208;

                    case 0x3f2:
                        text1 = "preset portable";
                        goto Label_0208;

                    case 0x3f7:
                        text1 = "preset radio";
                        goto Label_0208;

                    case 500:
                        text1 = "V0: preset extreme";
                        goto Label_0208;

                    case 490:
                        text1 = "V1";
                        goto Label_0208;

                    case 460:
                        text1 = "V4: preset medium";
                        goto Label_0208;

                    case 470:
                        text1 = "V3";
                        goto Label_0208;

                    case 480:
                        text1 = "V2: preset standard";
                        goto Label_0208;

                    case 430:
                        text1 = "V7";
                        goto Label_0208;

                    case 440:
                        text1 = "V6";
                        goto Label_0208;

                    case 450:
                        text1 = "V5";
                        goto Label_0208;

                    case 0:
                        text1 = "<not stored>";
                        usePresetGuess = IdSharp.Inspection.UsePresetGuess.UseGuess;
                        goto Label_0208;

                    case 410:
                        text1 = "V9";
                        goto Label_0208;

                    case 420:
                        text1 = "V8";
                        goto Label_0208;
                }
                text1 = string.Format("<unrecognised value {0}>", num1);
                usePresetGuess = IdSharp.Inspection.UsePresetGuess.UseGuess;
            }
        Label_0208:
            if ((this.m_BasicReader.EncodingMethod != 4) || ((((num1 != 410) && (num1 != 420)) && ((num1 != 430) && (num1 != 440))) && ((((num1 != 450) && (num1 != 460)) && ((num1 != 470) && (num1 != 480))) && ((num1 != 490) && (num1 != 500)))))
            {
                return text1;
            }
            return (text1 + " (fast mode)");
        }

        private string DeterminePresetGuess(ref IdSharp.Inspection.UsePresetGuess usePresetGuess)
        {
            switch (this.m_BasicReader.PresetGuess)
            {
                case LamePreset.Insane:
                    return "--alt-preset insane";

                case LamePreset.Extreme:
                    return "--alt-preset extreme";

                case LamePreset.FastExtreme:
                    return "--alt-preset fast extreme";

                case LamePreset.Standard:
                    return "--alt-preset standard";

                case LamePreset.FastStandard:
                    return "--alt-preset fast standard";

                case LamePreset.Medium:
                    return "preset medium";

                case LamePreset.FastMedium:
                    return "preset fast medium";

                case LamePreset.R3mix:
                    return "r3mix";

                case LamePreset.Studio:
                    return "preset studio";

                case LamePreset.CD:
                    return "preset cd";

                case LamePreset.Hifi:
                    return "preset hifi";

                case LamePreset.Tape:
                    return "preset tape";

                case LamePreset.Radio:
                    return "preset radio";

                case LamePreset.FM:
                    return "preset fm";

                case LamePreset.TapeRadioFM:
                    return "preset tape OR preset radio OR preset fm";

                case LamePreset.Voice:
                    return "preset voice";

                case LamePreset.MWUS:
                    return "preset mw-us";

                case LamePreset.MWEU:
                    return "preset phon+ OR preset lw OR preset mw-eu OR preset sw";

                case LamePreset.Phone:
                    return "preset phone";
            }
            string text1 = "";
            if (this.m_BasicReader.Preset == 0)
            {
                usePresetGuess = IdSharp.Inspection.UsePresetGuess.UnableToGuess;
                return text1;
            }
            usePresetGuess = IdSharp.Inspection.UsePresetGuess.NotNeeded;
            return text1;
        }

        private void DeterminePresetRelatedValues()
        {
            this.m_Preset = this.DeterminePreset(out this.m_UsePresetGuess);
            if (this.m_UsePresetGuess == IdSharp.Inspection.UsePresetGuess.NotNeeded)
            {
                this.m_PresetGuess = "";
            }
            else
            {
                this.m_PresetGuess = this.DeterminePresetGuess(ref this.m_UsePresetGuess);
                if (this.m_BasicReader.IsPresetGuessNonBitrate)
                {
                    this.m_PresetGuess = this.m_PresetGuess + string.Format(" -b {0}", this.m_BasicReader.Bitrate);
                }
            }
        }


        public bool IsLameTagFound
        {
            get
            {
                return this.m_BasicReader.IsLameTagFound;
            }
        }

        public bool IsPresetGuessNonBitrate
        {
            get
            {
                return this.m_BasicReader.IsPresetGuessNonBitrate;
            }
        }

        public string LameTagInfoEncoder
        {
            get
            {
                if (!this.IsLameTagFound)
                {
                    return this.m_Mpeg.Encoder;
                }
                string text1 = "LAME";
                if (string.Compare(this.VersionString, "3.90") < 0)
                {
                    if (!string.IsNullOrEmpty(this.VersionStringNonLameTag) && (this.VersionStringNonLameTag[1] == '.'))
                    {
                        text1 = text1 + " " + this.VersionStringNonLameTag;
                    }
                    return text1;
                }
                if (!string.IsNullOrEmpty(this.VersionString))
                {
                    text1 = text1 + " " + this.VersionString;
                }
                return text1;
            }
        }

        public string LameTagInfoPreset
        {
            get
            {
                string text1 = "";
                if (!this.IsLameTagFound || (string.Compare(this.VersionString, "3.90") < 0))
                {
                    return text1;
                }
                text1 = this.Preset;
                if (this.UsePresetGuess != IdSharp.Inspection.UsePresetGuess.UseGuess)
                {
                    return text1;
                }
                if (this.IsPresetGuessNonBitrate)
                {
                    return (this.PresetGuess + " (modified)");
                }
                return (this.PresetGuess + " (guessed)");
            }
        }

        public string LameTagInfoVersion
        {
            get
            {
                return (this.m_Mpeg.Version + " " + this.m_Mpeg.Layer);
            }
        }

        public string Preset
        {
            get
            {
                return this.m_Preset;
            }
        }

        public string PresetGuess
        {
            get
            {
                return this.m_PresetGuess;
            }
        }

        public IdSharp.Inspection.UsePresetGuess UsePresetGuess
        {
            get
            {
                return this.m_UsePresetGuess;
            }
        }

        public string VersionString
        {
            get
            {
                return this.m_BasicReader.VersionString;
            }
        }

        public string VersionStringNonLameTag
        {
            get
            {
                return this.m_BasicReader.VersionStringNonLameTag;
            }
        }


        private BasicLameTagReader m_BasicReader;
        private MpegAudio m_Mpeg;
        private string m_Preset;
        private string m_PresetGuess;
        private IdSharp.Inspection.UsePresetGuess m_UsePresetGuess;
    }
}

