namespace IdSharp.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal sealed class PresetGuesser
    {
        static PresetGuesser()
        {
            PresetGuesser.m_PresetGuessTable = new List<PresetGuessRow>();
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 0xff, 0x3a, 1, 1, 3, 2, 0xcd, LamePreset.Insane));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 0xff, 0x3a, 1, 1, 3, 2, 0xce, LamePreset.Insane));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 0xff, 0x39, 1, 1, 3, 4, 0xcd, LamePreset.Insane));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 0, 0x4e, 3, 2, 3, 2, 0xc3, LamePreset.Extreme));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, 0, 0x4e, 3, 2, 3, 2, 0xc4, LamePreset.Extreme));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 0x4e, 3, 1, 3, 2, 0xc4, LamePreset.Extreme));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 0, 0x4e, 4, 2, 3, 2, 0xc3, LamePreset.FastExtreme));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 0, 0x4e, 4, 2, 3, 2, 0xc4, LamePreset.FastExtreme));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 0, 0x4e, 3, 2, 3, 4, 190, LamePreset.Standard));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 0x4e, 3, 1, 3, 4, 190, LamePreset.Standard));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 0, 0x4e, 4, 2, 3, 4, 190, LamePreset.FastStandard));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 0x44, 3, 2, 3, 4, 180, LamePreset.Medium));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0, 0x44, 4, 2, 3, 4, 180, LamePreset.FastMedium));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 0, 0x58, 4, 1, 3, 3, 0xc3, LamePreset.R3mix));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, 0, 0x58, 4, 1, 3, 3, 0xc4, LamePreset.R3mix));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 0xff, 0x63, 1, 1, 1, 2, 0, LamePreset.Studio));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0xff, 0x3a, 2, 1, 3, 2, 0xce, LamePreset.Studio));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 0xff, 0x3a, 2, 1, 3, 2, 0xcd, LamePreset.Studio));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 0xff, 0x39, 2, 1, 3, 4, 0xcd, LamePreset.Studio));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 0xc0, 0x58, 1, 1, 1, 2, 0, LamePreset.CD));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0xc0, 0x3a, 2, 2, 3, 2, 0xc4, LamePreset.CD));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 0xc0, 0x3a, 2, 2, 3, 2, 0xc3, LamePreset.CD));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 0xc0, 0x39, 2, 1, 3, 4, 0xc3, LamePreset.CD));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 160, 0x4e, 1, 1, 3, 2, 180, LamePreset.Hifi));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 160, 0x3a, 2, 2, 3, 2, 180, LamePreset.Hifi));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 160, 0x39, 2, 1, 3, 4, 180, LamePreset.Hifi));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 0x80, 0x43, 1, 1, 3, 2, 180, LamePreset.Tape));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 0x80, 0x43, 1, 1, 3, 2, 150, LamePreset.Radio));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, LameVersionGroup.lvg3902_391, 0x70, 0x43, 1, 1, 3, 2, 150, LamePreset.FM));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 0x70, 0x3a, 2, 2, 3, 2, 160, LamePreset.TapeRadioFM));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 0x70, 0x39, 2, 1, 3, 4, 160, LamePreset.TapeRadioFM));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 0x38, 0x3a, 2, 2, 0, 2, 100, LamePreset.Voice));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 0x38, 0x39, 2, 1, 0, 4, 150, LamePreset.Voice));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg390_3901_392, 40, 0x41, 1, 1, 0, 2, 0x4b, LamePreset.MWUS));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3902_391, 40, 0x41, 1, 1, 0, 2, 0x4c, LamePreset.MWUS));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, LameVersionGroup.lvg3931_3903up, 40, 0x3a, 2, 2, 0, 2, 70, LamePreset.MWUS));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 40, 0x39, 2, 1, 0, 4, 0x69, LamePreset.MWUS));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0x18, 0x3a, 2, 2, 0, 2, 40, LamePreset.MWEU));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 0x18, 0x3a, 2, 2, 0, 2, 0x27, LamePreset.MWEU));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 0x18, 0x39, 2, 1, 0, 4, 0x3b, LamePreset.MWEU));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg3931_3903up, 0x10, 0x3a, 2, 2, 0, 2, 0x26, LamePreset.Phone));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg393, 0x10, 0x3a, 2, 2, 0, 2, 0x25, LamePreset.Phone));
            PresetGuesser.m_PresetGuessTable.Add(new PresetGuessRow(LameVersionGroup.lvg394up, 0x10, 0x39, 2, 1, 0, 4, 0x38, LamePreset.Phone));
        }

        private LamePreset BestGuessTwoVersions(LameVersionGroup AGroup1, LameVersionGroup AGroup2, byte ABitrate, byte AQuality, byte AEncodingMethod, byte ANoiseShaping, byte AStereoMode, byte AATHType, byte ALowpassDiv100, out bool ANonBitrate)
        {
            LamePreset preset1;
            LamePreset preset2 = this.GuessForVersion(AGroup1, ABitrate, AQuality, AEncodingMethod, ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            bool flag1 = ANonBitrate;
            LamePreset preset3 = this.GuessForVersion(AGroup2, ABitrate, AQuality, AEncodingMethod, ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            bool flag2 = ANonBitrate;
            if ((preset2 == LamePreset.Unknown) || (flag1 && (preset3 != LamePreset.Unknown)))
            {
                preset1 = preset3;
                ANonBitrate = flag2;
                return preset1;
            }
            preset1 = preset2;
            ANonBitrate = flag1;
            return preset1;
        }

        private LamePreset GuessForVersion(LameVersionGroup AVersionGroup, byte ABitrate, byte AQuality, byte AEncodingMethod, byte ANoiseShaping, byte AStereoMode, byte AATHType, byte ALowpassDiv100, out bool ANonBitrate)
        {
            LamePreset preset1 = LamePreset.Unknown;
            LamePreset preset2 = LamePreset.Unknown;
            ANonBitrate = false;
            foreach (PresetGuessRow row1 in PresetGuesser.m_PresetGuessTable)
            {
                if (((row1.HasVersionGroup(AVersionGroup) && (row1.TVs[1] == AQuality)) && ((row1.TVs[2] == AEncodingMethod) && (row1.TVs[3] == ANoiseShaping))) && (((row1.TVs[4] == AStereoMode) && (row1.TVs[5] == AATHType)) && (row1.TVs[6] == ALowpassDiv100)))
                {
                    if (row1.TVs[0] == ABitrate)
                    {
                        preset1 = row1.Res;
                        break;
                    }
                    if ((AEncodingMethod == 3) || (AEncodingMethod == 4))
                    {
                        preset2 = row1.Res;
                    }
                }
            }
            if ((preset1 == LamePreset.Unknown) && (preset2 != LamePreset.Unknown))
            {
                ANonBitrate = true;
                preset1 = preset2;
            }
            return preset1;
        }

        public LamePreset GuessPreset(string AVersionString, byte ABitrate, byte AQuality, byte AEncodingMethod, byte ANoiseShaping, byte AStereoMode, byte AATHType, byte ALowpassDiv100, out bool ANonBitrate)
        {
            string text1 = AVersionString.Substring(0, 4);
            string text2 = AVersionString.Substring(0, 5);
            if (((text1 == "3.90") && (text2 != "3.90.")) || (text1 == "3.92"))
            {
                return this.GuessForVersion(LameVersionGroup.lvg390_3901_392, ABitrate, AQuality, AEncodingMethod, ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
            if (text2 == "3.90.")
            {
                return this.BestGuessTwoVersions(LameVersionGroup.lvg3902_391, LameVersionGroup.lvg3931_3903up, ABitrate, AQuality, AEncodingMethod, ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
            switch (text1)
            {
                case "3.91":
                    return this.GuessForVersion(LameVersionGroup.lvg3902_391, ABitrate, AQuality, AEncodingMethod, ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);

                case "3.93":
                    return this.BestGuessTwoVersions(LameVersionGroup.lvg3931_3903up, LameVersionGroup.lvg393, ABitrate, AQuality, AEncodingMethod, ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
            if (string.Compare(text1, "3.94") >= 0)
            {
                return this.GuessForVersion(LameVersionGroup.lvg394up, ABitrate, AQuality, AEncodingMethod, ANoiseShaping, AStereoMode, AATHType, ALowpassDiv100, out ANonBitrate);
            }
            LamePreset preset1 = LamePreset.Unknown;
            ANonBitrate = false;
            return preset1;
        }


        private static List<PresetGuessRow> m_PresetGuessTable;
    }
}

