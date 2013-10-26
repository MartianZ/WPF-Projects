namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;

    public interface IInvolvedPerson : INotifyPropertyChanged
    {
        string Involvement { get; set; }

        string Name { get; set; }

    }
}

