namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class PriceInformationBindingList : BindingList<IPriceInformation>
    {
        public PriceInformationBindingList()
        {
            base.AllowNew = true;
        }

        public PriceInformationBindingList(IList<IPriceInformation> priceInformationList) : base(priceInformationList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IPriceInformation information1 = new PriceInformation();
            base.Add(information1);
            return information1;
        }

        protected override void InsertItem(int index, IPriceInformation item)
        {
            base.InsertItem(index, item);
        }

    }
}

