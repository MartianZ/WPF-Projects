namespace IdSharp.Tagging.ID3v2
{
    using System;

    public interface ITagRestrictions
    {
        IdSharp.Tagging.ID3v2.ImageEncodingRestriction ImageEncodingRestriction { get; set; }

        IdSharp.Tagging.ID3v2.ImageSizeRestriction ImageSizeRestriction { get; set; }

        IdSharp.Tagging.ID3v2.TagSizeRestriction TagSizeRestriction { get; set; }

        IdSharp.Tagging.ID3v2.TextEncodingRestriction TextEncodingRestriction { get; set; }

        IdSharp.Tagging.ID3v2.TextFieldsSizeRestriction TextFieldsSizeRestriction { get; set; }

    }
}

