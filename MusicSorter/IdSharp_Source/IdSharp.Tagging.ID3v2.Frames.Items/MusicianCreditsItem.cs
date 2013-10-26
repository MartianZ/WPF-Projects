namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class MusicianCreditsItem : IMusicianCreditsItem, INotifyPropertyChanged
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


        public string Artists
        {
            get
            {
                return this.m_Artists;
            }
            set
            {
                this.m_Artists = value;
                this.FirePropertyChanged("Artists");
            }
        }

        public string Instrument
        {
            get
            {
                return this.m_Instrument;
            }
            set
            {
                this.m_Instrument = value;
                this.FirePropertyChanged("Instrument");
            }
        }


        private string m_Artists;
        private string m_Instrument;
    }
}

