namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;

    public interface ICommercial : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string ContactUrl { get; set; }

        string Description { get; set; }

        string NameOfSeller { get; set; }

        BindingList<IPriceInformation> PriceList { get; }

        IdSharp.Tagging.ID3v2.ReceivedAs ReceivedAs { get; set; }

        byte[] SellerLogo { get; set; }

        string SellerLogoMimeType { get; set; }

        DateTime ValidUntil { get; set; }

    }
}

