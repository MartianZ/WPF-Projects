namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IOwnership : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string CurrencyCode { get; set; }

        DateTime DateOfPurchase { get; set; }

        double PricePaid { get; set; }

        string Seller { get; set; }

    }
}

