namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using IdSharp.Tagging.ID3v2;
    using System;

    internal sealed class EqualizationItem : IEqualizationItem
    {
        public int Adjustment
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                throw new NotImplementedException();
            }
        }

        public short Frequency
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                throw new NotImplementedException();
            }
        }

        public VolumeAdjustmentDirection VolumeAdjustment
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}

