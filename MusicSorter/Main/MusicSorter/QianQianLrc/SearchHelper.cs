/*
 © 4321.La
 * 千千静听歌词搜索服务加密部分算法破解
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace QianQianLrc
{
    public static class SearchHelper
    {
        //TODO:服务器地址 电信 网通 增加选项
        private static readonly string SearchPath = "http://ttlrccnc.qianqian.com/dll/lyricsvr.dll?sh?Artist={0}&Title={1}&Flags=0";
        private static readonly string DownloadPath = "http://ttlrccnc.qianqian.com/dll/lyricsvr.dll?dl?Id={0}&Code={1}";

        public static string DownloadLrc(string singer, string title)
        {
            XmlDocument xml = SearchLrc(singer, title);
            string retStr = null;
            if (xml == null)
                return retStr;

            XmlNodeList list = xml.SelectNodes("/result/lrc");
            if (list.Count > 0)
            {
                int sLrcId = -1;
                string xSinger = null, xTitle = null;
                foreach (XmlNode node in list)
                {
                    if (node != null && node.Attributes != null && node.Attributes["id"] != null)
                    {
                        int lrcId = -1;
                        if (int.TryParse(node.Attributes["id"].Value, out lrcId))
                        {
                            //优先选择双语对照、中文与其他语言对照的版本
                            //这里可以写更多的artificial intelligence
                            if (sLrcId == -1 
                                || node.Attributes["artist"].Value.IndexOf("中") > 0
                                || node.Attributes["artist"].Value.IndexOf("双") > 0)
                            {
                                sLrcId = lrcId;
                                xSinger = node.Attributes["artist"].Value;
                                xTitle = node.Attributes["title"].Value;
                            }
                        }
                    }
                }

                if (sLrcId > -1)
                {
                    string temp = string.Format(DownloadPath, sLrcId,
                        EncodeHelper.CreateQianQianCode(xSinger, xTitle, sLrcId));
                    retStr = RequestALink(temp);
                }
            }
            return retStr;
        }

        public static string DownloadLRCLite(int sLrcId, string xSinger, string xTitle)
        {
            string temp = string.Format(DownloadPath, sLrcId,
                        EncodeHelper.CreateQianQianCode(xSinger, xTitle, sLrcId));
            return RequestALink(temp);
        }


        public static XmlDocument SearchLrc(string singer, string title)
        {
            singer = singer.ToLower().Replace(" ", "").Replace("'", "");
            title = title.ToLower().Replace(" ", "").Replace("'", "");


            string[] CharToDelete = 
            {       "`", "~", "!", "@", "#", "$", "%", 
                    "^", "&", "*", "(", ")", "-", "_", "=",
                    "+", ",", "<", ".", ">", "/", "?", ";", 
                    ":", "\"", "[", "{", "]", "}", "\\", "|", "€",
                    "　","。","，","、","；","：","？","！","…","—","·",
                    "ˉ","¨","‘","’","“","”","々","～","‖","∶","＂","＇",
                    "｀","｜","〃","〔","〕","〈","〉","《","》","「","」",
                    "『","』","．","〖","〗","【","】","（","）","［","］",
                    "｛","｝","≈","≡","≠","＝","≤","≥","＜","＞","≮","≯","∷","±",
                    "＋","－","×","÷","／","∫","∮","∝","∞","∧","∨","∑","∏","∪",
                    "∩","∈","∵","∴","⊥","∥","∠","⌒","⊙","≌","∽","√","§","№",
                    "☆","★","○","●","◎","◇","◆","□","℃","‰","■","△","▲","※","→",
                    "←","↑","↓","〓","¤","°","＃","＆","＠","＼","︿","＿","￣","―",
                    "♂","♀","Ⅰ","Ⅱ","Ⅲ","Ⅳ","Ⅴ","Ⅵ","Ⅶ","Ⅷ","Ⅸ","Ⅹ","Ⅺ",
                    "Ⅻ","⒈","⒉","⒊","⒋","⒌","⒍","⒎","⒏","⒐","⒑","⒒","⒓",
                    "⒔","⒕","⒖","⒗","⒘","⒙","⒚","⒛","㈠","㈡","㈢","㈣","㈤",
                    "㈥","㈦","㈧","㈨","㈩","①","②","③","④","⑤","⑥","⑦","⑧","⑨","⑩",
                    "⑴","⑵","⑶","⑷","⑸","⑹","⑺","⑻","⑼","⑽","⑾","⑿","⒀",
                    "⒁","⒂","⒃","⒄","⒅","⒆","⒇","┌","┍","┎","┏","┐","┑","┒",
                    "┓","─","┄","┈","└","┕","┖","┗","┘","┙","┚","┛","━","┅","┉",
                    "├","┝","┞","┟","┠","┡","┢","┣","│","┆","┊","┤","┥","┦","┧","┨",
                    "┩","┪","┫","┃","┇","┋","┬","┭","┮","┯","┰","┱","┲","┳","┴","┵",
                    "┶","┷","┸","┹","┺","┻","┼","┽","┾","┿","╀","╁","╂","╃","╄","╅",
                    "╆","╇","╈","╉","╊","╋", "\"", "-"
            };

            for (int i = 0; i < CharToDelete.Length; i++)
            {
                singer = singer.Replace(CharToDelete[i], "");
                title = title.Replace(CharToDelete[i], "");
            }
            string x = RequestALink(string.Format(SearchPath,
                EncodeHelper.ToQianQianHexString(singer, Encoding.Unicode),
                EncodeHelper.ToQianQianHexString(title, Encoding.Unicode)));
            XmlDocument xml = new XmlDocument();
           
            try
            {
                xml.LoadXml(x);
            }
            catch (Exception)
            {
            }
            return xml;
        }


        private static string RequestALink(string link)
        {
            WebRequest request = WebRequest.Create(link);
            StringBuilder sb = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {

                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line + "\n");
                    }
                }
            }
            catch
            {
                return null;
            }
            return sb.ToString();
        }


    }
}
