namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class GroupIdentificationBindingList : BindingList<IGroupIdentification>
    {
        public GroupIdentificationBindingList()
        {
            base.AllowNew = true;
        }

        public GroupIdentificationBindingList(IList<IGroupIdentification> groupIdentificationList) : base(groupIdentificationList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IGroupIdentification identification1 = new GroupIdentification();
            base.Add(identification1);
            return identification1;
        }

        protected override void InsertItem(int index, IGroupIdentification item)
        {
            base.InsertItem(index, item);
        }

    }
}

