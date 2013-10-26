namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.ComInterop;
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames;
    using IdSharp.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class AttachedPictureBindingList : BindingList<IAttachedPicture>, IFrameList
    {
        public AttachedPictureBindingList()
        {
            base.AllowNew = true;
        }

        public AttachedPictureBindingList(IList<IAttachedPicture> attachedPictureList) : base(attachedPictureList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IAttachedPicture picture1 = new AttachedPicture();
            base.Add(picture1);
            return picture1;
        }

        private void AttachedPicture_DescriptionChanging(object sender, CancelDataEventArgs<string> e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                foreach (IAttachedPicture picture1 in base.Items)
                {
                    if (((picture1 != sender) && !string.IsNullOrEmpty(picture1.Description)) && (string.Compare(picture1.Description, e.Data, false) == 0))
                    {
                        return;
                    }
                }
            }
        }

        private void AttachedPicture_PictureTypeChanging(object sender, CancelDataEventArgs<PictureType> e)
        {
            foreach (IAttachedPicture picture1 in base.Items)
            {
                if ((((PictureType) e.Data) == PictureType.OtherFileIcon) && (picture1.PictureType == PictureType.OtherFileIcon))
                {
                    return;
                }
                if ((((PictureType) e.Data) == PictureType.FileIcon32x32Png) && (picture1.PictureType == PictureType.FileIcon32x32Png))
                {
                    return;
                }
            }
        }

        int IFrameList.Add(object value)
        {
            IAttachedPicture picture1 = (IAttachedPicture) value;
            base.Add(picture1);
            return base.IndexOf(picture1);
        }

        object IFrameList.AddNew()
        {
            return base.AddNew();
        }

        void IFrameList.Clear()
        {
            base.Clear();
        }

        void IFrameList.Remove(object value)
        {
            base.Remove((IAttachedPicture) value);
        }

        void IFrameList.RemoveAt(int index)
        {
            base.RemoveAt(index);
        }

        protected override void InsertItem(int index, IAttachedPicture item)
        {
            base.InsertItem(index, item);
        }


        int IFrameList.Count
        {
            get
            {
                return base.Count;
            }
        }

        object IFrameList.this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = (IAttachedPicture) value;
            }
        }

    }
}

