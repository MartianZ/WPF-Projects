using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MusicSorter
{
    /// <summary>
    /// 获取MP3文件的ID3 V1版本的TAG信息的类
    /// </summary>
    internal class Mp3TagID3V1
    {
        //流派分类
        readonly string[] GENRE = {"Blues","Classic Rock","Country","Dance","Disco","Funk","Grunge","Hip-Hop",
            "Jazz","Metal","New Age","Oldies","Other", "Pop", "R&B", "Rap", "Reggae", "Rock", "Techno", 
            "Industrial", "Alternative"
            };

        private string title = string.Empty;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
        }

        private string artist = string.Empty;
        /// <summary>
        /// 艺术家，演唱者
        /// </summary>
        public string Artist
        {
            get { return artist; }
        }

        private string album = string.Empty;
        /// <summary>
        /// 所属专辑
        /// </summary>
        public string Album
        {
            get { return album; }
        }

        private string pubYear = string.Empty;
        /// <summary>
        /// 发行年份
        /// </summary>
        public string PublishYear
        {
            get { return pubYear; }
        }

        private string comment = string.Empty;
        /// <summary>
        /// 备注、说明
        /// </summary>
        public string Comment
        {
            get
            {
                if (comment.Length == 30)
                {
                    //如果是 ID3 V1.1的版本，那么comment只占前28个byte，第30个byte存放音轨信息
                    if (TagVersion(comment)) return comment.Substring(0, 28).TrimEnd();
                }

                return comment.TrimEnd();
            }
        }

        /// <summary>
        /// 音轨
        /// </summary>
        public string Track
        {
            get
            {
                if (comment.Length == 30)
                {
                    //如果是 ID3 V1.1的版本，读取音轨信息
                    if (TagVersion(comment)) return ((int)comment[29]).ToString();
                }

                return string.Empty;
            }
        }

        private string genre;
        /// <summary>
        /// 流派
        /// </summary>
        public string Genre
        {
            get { return genre; }
        }

        /// <summary>
        /// 判断MP3的TAG信息的版本，是V1.0 还是 V1.1
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>true表示是 1.1，false表示是 1.0</returns>
        private bool TagVersion(string comment)
        {
            if (comment[28].Equals('\0') && comment[29] > 0 || comment[28] == 32 && comment[29] != 32)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mp3FilePath">MP3文件的完整路径</param>
        public Mp3TagID3V1(string mp3FilePath)
        {
            byte[] tagBody = new byte[128];
            string tagFlag;

            if (!File.Exists(mp3FilePath))
                return;
              
            //读取MP3文件的最后128个字节的内容
            using (FileStream fs = new FileStream(mp3FilePath, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(-128, SeekOrigin.End);
                fs.Read(tagBody, 0, 128);
                fs.Close();
            }

            //取TAG段的前三个字节
            tagFlag = Encoding.Default.GetString(tagBody, 0, 3);

            //如果没有TAG信息，则直接返回
            if (!"TAG".Equals(tagFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
               
            }

            //按照MP3 ID3 V1 的tag定义，依次读取相关的信息
            this.title = Encoding.Default.GetString(tagBody, 3, 30).Replace("\0","");
            this.artist = Encoding.Default.GetString(tagBody, 33, 30).TrimEnd().Replace("\0", "");
            this.album = Encoding.Default.GetString(tagBody, 62, 30).TrimEnd().Replace("\0", "");
            this.pubYear = Encoding.Default.GetString(tagBody, 93, 4).TrimEnd().Replace("\0", "");
            this.comment = Encoding.Default.GetString(tagBody, 97, 30);
            Int16 g = (Int16)tagBody[127];
            this.genre = g >= GENRE.Length ? "未知" : GENRE[g];
        }
    }
}
