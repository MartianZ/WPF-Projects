namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IReverb : IFrame, INotifyPropertyChanged
    {
        byte PremixLeftToRight { get; set; }

        byte PremixRightToLeft { get; set; }

        byte ReverbBouncesLeft { get; set; }

        byte ReverbBouncesRight { get; set; }

        byte ReverbFeedbackLeftToLeft { get; set; }

        byte ReverbFeedbackLeftToRight { get; set; }

        byte ReverbFeedbackRightToLeft { get; set; }

        byte ReverbFeedbackRightToRight { get; set; }

        short ReverbLeftMilliseconds { get; set; }

        short ReverbRightMilliseconds { get; set; }

    }
}

