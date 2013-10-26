namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class UserDefinedTextBindingList : BindingList<ITXXXFrame>
    {
        public UserDefinedTextBindingList()
        {
            base.AllowNew = true;
        }

        public UserDefinedTextBindingList(IList<ITXXXFrame> userDefineTextList) : base(userDefineTextList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ITXXXFrame frame1 = new TXXXFrame();
            base.Add(frame1);
            return frame1;
        }

        protected override void InsertItem(int index, ITXXXFrame item)
        {
            using (IEnumerator<ITXXXFrame> enumerator1 = base.Items.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    ITXXXFrame local1 = enumerator1.Current;
                }
            }
            base.InsertItem(index, item);
        }

    }
}

