namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;

    public interface IPriceInformation : INotifyPropertyChanged
    {
        string CurrencyCode { get; set; }

        double Price { get; set; }

    }
}

