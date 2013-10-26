namespace zlib
{
    using System;

    internal sealed class Adler32
    {
        internal long adler32(long adler, byte[] buf, int index, int len)
        {
            if (buf == null)
            {
                return (long) 1;
            }
            long num1 = adler & 0xffff;
            long num2 = (adler >> 0x10) & 0xffff;
            while (len > 0)
            {
                int num3 = (len < 0x15b0) ? len : 0x15b0;
                len -= num3;
                while (num3 >= 0x10)
                {
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num1 += buf[index++] & 0xff;
                    num2 += num1;
                    num3 -= 0x10;
                }
                if (num3 != 0)
                {
                    do
                    {
                        num1 += buf[index++] & 0xff;
                        num2 += num1;
                    }
                    while (--num3 != 0);
                }
                num1 = num1 % ((long) 0xfff1);
                num2 = num2 % ((long) 0xfff1);
            }
            return ((num2 << 0x10) | num1);
        }


        private const int BASE = 0xfff1;
        private const int NMAX = 0x15b0;
    }
}

