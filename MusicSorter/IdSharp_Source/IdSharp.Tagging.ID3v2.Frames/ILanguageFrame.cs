namespace IdSharp.Tagging.ID3v2.Frames
{
    using System.ComponentModel;

    public interface ILanguageFrame : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        BindingList<ILanguageItem> Items { get; }

    }
}

