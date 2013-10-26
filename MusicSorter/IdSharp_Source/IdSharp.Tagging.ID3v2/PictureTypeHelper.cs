namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Collections.Generic;

    public static class PictureTypeHelper
    {
        static PictureTypeHelper()
        {
            PictureTypeHelper.m_PictureTypeDictionary = new SortedDictionary<string, PictureType>();
            PictureTypeHelper.m_PictureTypeDictionary.Add("Other", PictureType.Other);
            PictureTypeHelper.m_PictureTypeDictionary.Add("File icon (32x32 PNG)", PictureType.FileIcon32x32Png);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Other file icon", PictureType.OtherFileIcon);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Cover (front)", PictureType.CoverFront);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Cover (back)", PictureType.CoverBack);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Leaflet page", PictureType.LeafletPage);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Media (e.g. label side of CD)", PictureType.MediaLabelSideOfCD);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Lead artist/performer", PictureType.LeadArtistPerformer);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Artist/performer", PictureType.ArtistPerformer);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Conductor", PictureType.Conductor);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Band/orchestra", PictureType.BandOrchestra);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Composer", PictureType.Composer);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Lyricist", PictureType.Lyricist);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Recording location", PictureType.RecordingLocation);
            PictureTypeHelper.m_PictureTypeDictionary.Add("During recording", PictureType.DuringRecording);
            PictureTypeHelper.m_PictureTypeDictionary.Add("During performance", PictureType.DuringPerformance);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Movie/video screen capture", PictureType.MovieVideoScreenCapture);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Illustration", PictureType.Illustration);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Band/artist logo", PictureType.BandArtistLogo);
            PictureTypeHelper.m_PictureTypeDictionary.Add("Publisher/studio logo", PictureType.PublisherStudioLogo);
            PictureTypeHelper.m_PictureTypes = new string[PictureTypeHelper.m_PictureTypeDictionary.Count];
            int num1 = 0;
            foreach (string text1 in PictureTypeHelper.m_PictureTypeDictionary.Keys)
            {
                PictureTypeHelper.m_PictureTypes[num1++] = text1;
            }
        }

        public static PictureType GetPictureTypeFromString(string pictureTypeString)
        {
            PictureType type1;
            if (PictureTypeHelper.m_PictureTypeDictionary.TryGetValue(pictureTypeString, out type1))
            {
                return type1;
            }
            return PictureType.Other;
        }

        public static string GetStringFromPictureType(PictureType pictureType)
        {
            foreach (KeyValuePair<string, PictureType> pair1 in PictureTypeHelper.m_PictureTypeDictionary)
            {
                if (((PictureType) pair1.Value) == pictureType)
                {
                    return pair1.Key;
                }
            }
            return "Other";
        }


        public static string[] PictureTypes
        {
            get
            {
                return PictureTypeHelper.m_PictureTypes;
            }
        }


        private static readonly SortedDictionary<string, PictureType> m_PictureTypeDictionary;
        private static readonly string[] m_PictureTypes;
    }
}

