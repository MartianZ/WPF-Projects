namespace IdSharp.Inspection
{
    using System;

    internal sealed class PresetGuessRow
    {
        public PresetGuessRow(LameVersionGroup vg1, byte tv1, byte tv2, byte tv3, byte tv4, byte tv5, byte tv6, byte tv7, LamePreset result)
        {
            this.VGs = new LameVersionGroup[3];
            this.TVs = new byte[7];
            this.Initialize(vg1, LameVersionGroup.None, LameVersionGroup.None, tv1, tv2, tv3, tv4, tv5, tv6, tv7, result);
        }

        public PresetGuessRow(LameVersionGroup vg1, LameVersionGroup vg2, byte tv1, byte tv2, byte tv3, byte tv4, byte tv5, byte tv6, byte tv7, LamePreset result)
        {
            this.VGs = new LameVersionGroup[3];
            this.TVs = new byte[7];
            this.Initialize(vg1, vg2, LameVersionGroup.None, tv1, tv2, tv3, tv4, tv5, tv6, tv7, result);
        }

        public PresetGuessRow(LameVersionGroup vg1, LameVersionGroup vg2, LameVersionGroup vg3, byte tv1, byte tv2, byte tv3, byte tv4, byte tv5, byte tv6, byte tv7, LamePreset result)
        {
            this.VGs = new LameVersionGroup[3];
            this.TVs = new byte[7];
            this.Initialize(vg1, vg2, vg3, tv1, tv2, tv3, tv4, tv5, tv6, tv7, result);
        }

        public bool HasVersionGroup(LameVersionGroup vg1)
        {
            for (int num1 = 0; num1 < 3; num1++)
            {
                if (vg1 == this.VGs[num1])
                {
                    return true;
                }
            }
            return false;
        }

        private void Initialize(LameVersionGroup vg1, LameVersionGroup vg2, LameVersionGroup vg3, byte tv1, byte tv2, byte tv3, byte tv4, byte tv5, byte tv6, byte tv7, LamePreset result)
        {
            this.VGs[0] = vg1;
            this.VGs[1] = vg2;
            this.VGs[2] = vg3;
            this.TVs[0] = tv1;
            this.TVs[1] = tv2;
            this.TVs[2] = tv3;
            this.TVs[3] = tv4;
            this.TVs[4] = tv5;
            this.TVs[5] = tv6;
            this.TVs[6] = tv7;
            this.Res = result;
        }


        public LamePreset Res;
        public byte[] TVs;
        public LameVersionGroup[] VGs;
    }
}

