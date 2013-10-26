/*
 © 4321.La
 * 千千静听歌词搜索服务加密部分算法破解
 */

using System;
using System.Collections.Generic;
using System.Text;
namespace QianQianLrc
{
    public class EncodeHelper
    {
        public static string ToQianQianHexString(string s, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = encoding.GetBytes(s);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        public static string CreateQianQianCode(string singer, string title, int lrcId)
        {
            string qqHexStr = ToQianQianHexString(singer + title, Encoding.UTF8);
            if (qqHexStr.Length % 2 == 1)
            {
                qqHexStr += "0";
            }
                
            int length = qqHexStr.Length / 2;

            int[] song = new int[length];
            for (int i = 0; i < length; i++)
            {
                song[i] = int.Parse(qqHexStr.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            int t1 = 0, t2 = 0, t3 = 0;
            t1 = (lrcId & 0x0000FF00) >> 8;


            if ((lrcId & 0x00FF0000) == 0)
            {
                t3 = 0x000000FF & ~t1;
            }
            else
            {
                t3 = 0x000000FF & ((lrcId & 0x00FF0000) >> 16);
            }

            t3 = t3 | ((0x000000FF & lrcId) << 8);
            t3 = t3 << 8;
            t3 = t3 | (0x000000FF & t1);
            t3 = t3 << 8;
            if ((lrcId & 0xFF000000) == 0)
            {
                t3 = t3 | (0x000000FF & (~lrcId));
            }
            else
            {
                t3 = t3 | (0x000000FF & (lrcId >> 24));
            }

            int j = length - 1;
            while (j >= 0)
            {
                int c = song[j];
                if (c >= 0x80) c = c - 0x100;

                t1 = (int)((c + t2) & 0x00000000FFFFFFFF);
                t2 = (int)((t2 << (j % 2 + 4)) & 0x00000000FFFFFFFF);
                t2 = (int)((t1 + t2) & 0x00000000FFFFFFFF);
                j -= 1;
            }


            j = 0;
            t1 = 0;
            while (j <= length - 1)
            {
                int c = song[j];
                if (c >= 0x80) c = c - 0x100;
                int t4 = (int)((c + t1) & 0x00000000FFFFFFFF);
                t1 = (int)((t1 << (j % 2 + 3)) & 0x00000000FFFFFFFF);
                t1 = (int)((t1 + t4) & 0x00000000FFFFFFFF);
                j += 1;
            }
            /*
             [Window Title]
            Projectlrc

            [Content]
            t12 1619049903

            [OK]*/

            long t5 = Conv(t2 ^ t3);
            t5 = Conv(t5 + (t1 | lrcId));
            t5 = Conv(t5 * (t1 | t3));
            t5 = Conv(t5 * (t2 ^ lrcId));

            long t6 = t5;
            if (t6 > 0x80000000)
                t5 = t6 - 0x100000000;
            return t5.ToString();
        }

        public static long Conv(long i)
        {
            long r = i % 0x100000000;
            if (i >= 0 && r > 0x80000000)
                r = r - 0x100000000;

            if (i < 0 && r < 0x80000000)
                r = r + 0x100000000;
            return r;
        }
    }
}
