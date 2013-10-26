namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;

    public interface IEqualizationList : IFrame, INotifyPropertyChanged
    {
        string Identification { get; set; }

        IdSharp.Tagging.ID3v2.InterpolationMethod InterpolationMethod { get; set; }

        BindingList<IEqualizationItem> Items { get; }

    }
}

