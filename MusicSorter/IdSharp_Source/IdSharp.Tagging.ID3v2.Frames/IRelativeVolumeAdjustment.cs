namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IRelativeVolumeAdjustment : IFrame, INotifyPropertyChanged
    {
        decimal BackCenterAdjustment { get; set; }

        decimal BackCenterPeak { get; set; }

        decimal BackLeftAdjustment { get; set; }

        decimal BackLeftPeak { get; set; }

        decimal BackRightAdjustment { get; set; }

        decimal BackRightPeak { get; set; }

        decimal FrontCenterAdjustment { get; set; }

        decimal FrontCenterPeak { get; set; }

        decimal FrontLeftAdjustment { get; set; }

        decimal FrontLeftPeak { get; set; }

        decimal FrontRightAdjustment { get; set; }

        decimal FrontRightPeak { get; set; }

        string Identification { get; set; }

        decimal MasterAdjustment { get; set; }

        decimal MasterPeak { get; set; }

        decimal OtherAdjustment { get; set; }

        decimal OtherPeak { get; set; }

        decimal SubwooferAdjustment { get; set; }

        decimal SubwooferPeak { get; set; }

    }
}

