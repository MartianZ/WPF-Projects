namespace IdSharp.Tagging.ID3v2
{
    using System;

    public sealed class TagReadingInfo
    {
        public TagReadingInfo(ID3v2TagVersion tagVersion) : this(tagVersion, IdSharp.Tagging.ID3v2.TagVersionOptions.None)
        {
        }

        public TagReadingInfo(ID3v2TagVersion tagVersion, IdSharp.Tagging.ID3v2.TagVersionOptions tagVersionOptions)
        {
            this.m_TagVersion = tagVersion;
            this.m_TagVersionOptions = tagVersionOptions;
        }


        public ID3v2TagVersion TagVersion
        {
            get
            {
                return this.m_TagVersion;
            }
            set
            {
                this.m_TagVersion = value;
            }
        }

        public IdSharp.Tagging.ID3v2.TagVersionOptions TagVersionOptions
        {
            get
            {
                return this.m_TagVersionOptions;
            }
            set
            {
                this.m_TagVersionOptions = value;
            }
        }


        private ID3v2TagVersion m_TagVersion;
        private IdSharp.Tagging.ID3v2.TagVersionOptions m_TagVersionOptions;
    }
}

