namespace IdSharp.Tagging.ID3v2.Frames
{
    using System.ComponentModel;

    public interface IMusicianCreditsList : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        BindingList<IMusicianCreditsItem> Items { get; }

    }
}

