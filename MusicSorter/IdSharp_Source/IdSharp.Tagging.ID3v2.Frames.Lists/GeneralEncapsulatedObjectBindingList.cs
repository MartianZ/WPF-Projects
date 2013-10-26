namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class GeneralEncapsulatedObjectBindingList : BindingList<IGeneralEncapsulatedObject>
    {
        public GeneralEncapsulatedObjectBindingList()
        {
            base.AllowNew = true;
        }

        public GeneralEncapsulatedObjectBindingList(IList<IGeneralEncapsulatedObject> generalEncapsulatedObjectList) : base(generalEncapsulatedObjectList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IGeneralEncapsulatedObject obj1 = new GeneralEncapsulatedObject();
            base.Add(obj1);
            return obj1;
        }

        protected override void InsertItem(int index, IGeneralEncapsulatedObject item)
        {
            base.InsertItem(index, item);
        }

    }
}

