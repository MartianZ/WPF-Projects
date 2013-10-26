namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class LinkedInformationBindingList : BindingList<ILinkedInformation>
    {
        public LinkedInformationBindingList()
        {
            base.AllowNew = true;
        }

        public LinkedInformationBindingList(IList<ILinkedInformation> linkedInformationList) : base(linkedInformationList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ILinkedInformation information1 = new LinkedInformation();
            base.Add(information1);
            return information1;
        }

        protected override void InsertItem(int index, ILinkedInformation item)
        {
            base.InsertItem(index, item);
        }

    }
}

