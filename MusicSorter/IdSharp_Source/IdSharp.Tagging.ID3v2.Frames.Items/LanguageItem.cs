namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class LanguageItem : ILanguageItem, INotifyPropertyChanged
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


        public string LanguageCode
        {
            get
            {
                return this.m_LanguageCode;
            }
            set
            {
                string text1;
                this.m_LanguageCode = value;
                if (LanguageHelper.Languages.TryGetValue(this.m_LanguageCode.ToLower(), out text1))
                {
                    this.LanguageDisplay = text1;
                }
                else
                {
                    this.LanguageDisplay = this.m_LanguageCode;
                }
                this.FirePropertyChanged("LanguageCode");
            }
        }

        string LanguageDisplay
        {
            public get
            {
                return this.m_LanguageDisplay;
            }
            private set
            {
                this.m_LanguageDisplay = value;
                this.FirePropertyChanged("LanguageDisplay");
            }
        }


        private string m_LanguageCode;
        private string m_LanguageDisplay;
    }
}

