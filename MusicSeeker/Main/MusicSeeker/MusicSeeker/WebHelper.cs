using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Security.Cryptography;

namespace MusicSeeker
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
                string ss = Base64Encode(s);
                WebClient WebClt = new WebClient();
                WebClt.Headers.Add(HttpRequestHeader.UserAgent, ss);

                Stream k = WebClt.OpenRead(url + "&sign=" + MD5(MD5(ss)));
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
        /// WPF下兼容的UrlEncode
        /// </summary>
        /// <param name="s">待编码的字符串</param>
        /// <returns></returns>
        public static string Encode(string s)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] byStr = System.Text.Encoding.Default.GetBytes(s);
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
    }
}
