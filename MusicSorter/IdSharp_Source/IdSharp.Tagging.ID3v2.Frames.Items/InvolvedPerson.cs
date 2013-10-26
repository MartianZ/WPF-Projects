namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class InvolvedPerson : IInvolvedPerson, INotifyPropertyChanged
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


        public string Involvement
        {
            get
            {
                return this.m_Involvement;
            }
            set
            {
                this.m_Involvement = value;
                this.FirePropertyChanged("Involvement");
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
                this.FirePropertyChanged("Name");
            }
        }


        private string m_Involvement;
        private string m_Name;
    }
}

