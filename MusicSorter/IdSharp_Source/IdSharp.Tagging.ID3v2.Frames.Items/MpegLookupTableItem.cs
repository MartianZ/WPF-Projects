namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class MpegLookupTableItem : IMpegLookupTableItem, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler1 = this.PropertyChanged;
            if (handler1 != null)
            {
                handler1(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public long DeviationInBytes
        {
            get
            {
                return this.m_DeviationInBytes;
            }
            set
            {
                this.m_DeviationInBytes = value;
                this.FirePropertyChanged("DeviationInBytes");
            }
        }

        public long DeviationInMilliseconds
        {
            get
            {
                return this.m_DeviationInMilliseconds;
            }
            set
            {
                this.m_DeviationInMilliseconds = value;
                this.FirePropertyChanged("DeviationInMilliseconds");
            }
        }


        private long m_DeviationInBytes;
        private long m_DeviationInMilliseconds;
    }
}

