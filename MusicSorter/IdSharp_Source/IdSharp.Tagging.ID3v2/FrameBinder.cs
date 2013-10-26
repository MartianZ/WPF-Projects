namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal sealed class FrameBinder
    {
        // Fields
        private FrameContainer m_FrameContainer;

        // Methods
        public FrameBinder(FrameContainer frameContainer)
        {
            this.m_FrameContainer = frameContainer;
        }

        public void Bind(INotifyPropertyChanged frame, string frameProperty, string tagProperty, MethodInvoker validator)
        {
            frame.PropertyChanged += delegate
            {
                this.m_FrameContainer.FirePropertyChanged(tagProperty);
                if (validator != null)
                {
                    validator();
                }
            };
        }
    }

 

}

