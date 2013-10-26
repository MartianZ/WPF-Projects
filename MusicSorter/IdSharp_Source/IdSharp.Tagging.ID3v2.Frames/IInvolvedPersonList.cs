namespace IdSharp.Tagging.ID3v2.Frames
{
    using System.ComponentModel;

    public interface IInvolvedPersonList : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        BindingList<IInvolvedPerson> Items { get; }

    }
}

