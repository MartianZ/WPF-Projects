namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using IdSharp.Tagging.ID3v2;
    using System;

    public interface IEqualizationItem
    {
        int Adjustment { get; set; }

        short Frequency { get; set; }

        VolumeAdjustmentDirection VolumeAdjustment { get; set; }

    }
}

