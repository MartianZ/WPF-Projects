using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MusicSeeker
{
    /// <summary>
    /// iniClass 的摘要说明。
    /// </summary>
    // TODO: 在此处添加构造函数逻辑
    static public class INIClass
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public static void IniWriteValue(string Key, string Value)
        {
            WritePrivateProfileString("MusicSeekerConfiguration", Key, Value, Environment.CurrentDirectory + @"\Config.ini");
        }
        /// <summary>
        /// 读出INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        public static bool IniReadValue(string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Key == "LyricsServer" ? "Lyrics" : "MusicSeekerConfiguration", Key, "", temp, 500, Environment.CurrentDirectory + @"\Config.ini");
            return temp.ToString() == "false" ? false : true;
        }

    }
    
}