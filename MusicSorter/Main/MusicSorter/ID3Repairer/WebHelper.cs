using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;
using System.Net;

namespace MusicSorter
{
    /// <summary>
    /// WebHelper类 用于Wpf下实现web操作
    /// </summary>
    public class WebHelper
    {
        /// <summary>
        /// 获得源代码，gb2312编码模式
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns></returns>
        public static string GetPageHTMLSource(string url)
        {
            try
            {
                
                WebClient WebClt = new WebClient();
                WebClt.Headers.Add("Cookie", "adid=; adSP=; adVer=; ac=; pt2gguin=; ptcz=; pgv_pvid=; pgv_flv=; pgv_r_cookie=; o_cookie=; uin_cookie=; euin_cookie=; pvid=; icache=; qqmusic_version=7; qqmusic_miniversion=72; qqmusic_uin=; qqmusic_key=; qqmusic_privatekey=; pgv_info=ssid=; uin=; skey=; ptisp=cnc; zzpaneluin=; zzpanelkey=; qqmusic_guid=; qqmusic_gkey=; qqmusic_gtime=0; qm_hideuin=; qm_method=1; user_status=2; TIMEPOINT_FOR_CLIENT=; userdata=1; start_time=; end_time=; year_pay=0; nickname=; vip=2; score=0; place=0; payway=0; yearstart=; yearend=; nowtime=; userqq=; viplevel=1; user_status_load_succ=true; closeflag=2; haspt=; TRANS_TIME_POINT=");

                Stream k = WebClt.OpenRead(url);
                StreamReader j = new StreamReader(k, Encoding.GetEncoding("gb2312"));

                string ok = "";
                string y;
                while ((y = j.ReadLine()) != null)
                {
                    ok = ok + y;
                }
                j.Close();
                return ok;
            }
            catch
            {
                return "";
            }
        }
        public static string Base64Encode(string AStr)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(AStr));
        }

        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        public static string Analytics(string url, string s)
        {

            try
            {

                WebClient WebClt = new WebClient();
                WebClt.Headers.Add(HttpRequestHeader.UserAgent, s);

                Stream k = WebClt.OpenRead(url);
                StreamReader j = new StreamReader(k, Encoding.GetEncoding("utf-8"));

                string ok = "";
                string y;
                while ((y = j.ReadLine()) != null)
                {
                    ok = ok + y;
                }
                j.Close();
                return ok;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// GB2312编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string Encode(string s)
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] byStr = System.Text.Encoding.GetEncoding("GB2312").GetBytes(s);
                for (int i = 0; i < byStr.Length; i++)
                {
                    sb.Append(@"%" + Convert.ToString(byStr[i], 16));
                }
                return (sb.ToString());
            }
            catch
            {
                return "";
            }
        }

        private static bool HasEnglishOrSimplifiedChinese(string s)
        {
            Regex r = new Regex(@"[\u4E00-\u9FA5|a-z|A-Z]+");
            return r.IsMatch(s);
        }

        /// <summary>
        /// WPF下兼容QQ音乐搜索的UrlEncode
        /// </summary>
        /// <param name="s">待编码的字符串</param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            if (HasEnglishOrSimplifiedChinese(str))
            {
                return Encode(str);
            }
            else
            {
                string outStr = "";
                if (!string.IsNullOrEmpty(str))
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        //將中文轉為10進制整數，然後轉為16進制unicode 
                        outStr += "%26%23" + ((int)str[i]).ToString() + "%3B";
                    }
                }
                return outStr;
            }
        }
    }


}
