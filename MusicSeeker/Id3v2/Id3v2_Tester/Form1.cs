using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Id3v2_Tester
{
    public partial class Form1 : Form
    {
        [DllImport("ID3v2Library.dll")]
        public static extern int ID3v2_Create();
        [DllImport("ID3v2Library.dll")]
        public static extern int ID3v2_Load(int Tag, string FileName);
        [DllImport("ID3v2Library.dll")]
        public static extern bool ID3v2_Free(int Tag);
        [DllImport("ID3v2Library.dll")]
        public static extern string ID3v2_GetAsciiText(int Tag, string FrameName);

        //ID3v2_SetFrameData(Tag: Pointer; FrameIndex: Cardinal; var Data: TStream): Integer; stdcall; 
        [DllImport("ID3v2Library.dll")]
        public static extern int ID3v2_GetFrameData(int Tag, Int64 FrameIndex, out string Data);

        //ID3v1_GetTitle(Tag: Pointer): PChar; stdcall; 
        [DllImport("ID3v2Library.dll")]
        public static extern string ID3v1_GetTitle(int Tag);

        [DllImport("ProjectID3.dll")]
        public static extern void WriteITunesLyrics(string FileName, string Content);

        [DllImport("ProjectID3.dll")]
        public static extern void SetCover(string FileName, string Path);


        [DllImport("ProjectID3.dll")]
        public static extern void GetLyrics(string Title, string Artist,StringBuilder resulta);


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SetCover(@"C:\1.mp3", @"C:\temp.jpg");
            StringBuilder resulta = new StringBuilder();
            resulta.Capacity = 65536;

            GetLyrics("NO, Thank You", "樱高轻音部", resulta);

            MessageBox.Show(resulta.ToString());

            //MessageBox.Show(System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "http://stream11.qqmusic.qq.com/30462600.mp3";
            //stream18.qqmusic.qq.com /30668288.mp3

            ///30668288.mp3
            ///http://stream11.qqmusic.qq.com/30668288.mp3


            string filename = @"c:\a.mp3";
            System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            Myrq.Referer = "";
            Myrq.Headers.Add("Cache-Control: no-cache");
            Myrq.Headers.Add("Down-Param: uin=1234567&qqkey=&downkey=&&");
            Myrq.Headers.Add("Cookie: qqmusic_uin=1234567; qqmusic_key=; qqmusic_privatekey=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA; qqmusic_fromtag=11;  ");
            System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
            long totalBytes = myrp.ContentLength;
            int Maximum = (int)totalBytes;
            int starttime = System.DateTime.Now.Second;
            System.IO.Stream st = myrp.GetResponseStream();
            System.IO.Stream so = new System.IO.FileStream(@filename, System.IO.FileMode.Create);
            long totalDownloadedByte = 0;
            long lasttotalDownloadedByte = 0;
            byte[] by = new byte[4096];
            int osize = st.Read(by, 0, (int)by.Length);
            while (osize > 0)
            {

                totalDownloadedByte = osize + totalDownloadedByte;
                //System.Windows.Forms.Application.DoEvents(); //不加多线程用这个临时调试
                int endtime = System.DateTime.Now.Second;

                if ((endtime - starttime) >= 1)
                {
                    Console.Write("AAA");
                }
                so.Write(by, 0, osize);
                if (Maximum != 0)
                {

                }
                osize = st.Read(by, 0, (int)by.Length);


            }
            so.Close();
            so.Dispose();
            st.Close();
            st.Dispose();

        }
    }
}
