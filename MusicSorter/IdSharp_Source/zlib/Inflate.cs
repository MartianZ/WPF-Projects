namespace zlib
{
    using System;

    internal sealed class Inflate
    {
        static Inflate()
        {
            byte[] buffer1 = new byte[4];
            buffer1[2] = (byte) SupportClass.Identity((long) 0xff);
            buffer1[3] = (byte) SupportClass.Identity((long) 0xff);
            Inflate.mark = buffer1;
        }

        public Inflate()
        {
            this.was = new long[1];
        }

        internal int inflate(ZStream z, int f)
        {
            if (((z == null) || (z.istate == null)) || (z.next_in == null))
            {
                return -2;
            }
            f = (f == 4) ? -5 : 0;
            int num1 = -5;
        Label_0024:
            switch (z.istate.mode)
            {
                case 0:
                    if (z.avail_in == 0)
                    {
                        return num1;
                    }
                    num1 = f;
                    z.avail_in--;
                    z.total_in++;
                    if (((z.istate.method = z.next_in[z.next_in_index++]) & 15) != 8)
                    {
                        z.istate.mode = 13;
                        z.msg = "unknown compression method";
                        z.istate.marker = 5;
                        goto Label_0024;
                    }
                    if (((z.istate.method >> 4) + 8) > z.istate.wbits)
                    {
                        z.istate.mode = 13;
                        z.msg = "invalid window size";
                        z.istate.marker = 5;
                        goto Label_0024;
                    }
                    z.istate.mode = 1;
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_01EE;

                case 3:
                    goto Label_0258;

                case 4:
                    goto Label_02CA;

                case 5:
                    goto Label_033B;

                case 6:
                    z.istate.mode = 13;
                    z.msg = "need dictionary";
                    z.istate.marker = 0;
                    return -2;

                case 7:
                    num1 = z.istate.blocks.proc(z, num1);
                    if (num1 != -3)
                    {
                        if (num1 == 0)
                        {
                            num1 = f;
                        }
                        if (num1 != 1)
                        {
                            return num1;
                        }
                        num1 = f;
                        z.istate.blocks.reset(z, z.istate.was);
                        if (z.istate.nowrap != 0)
                        {
                            z.istate.mode = 12;
                            goto Label_0024;
                        }
                        z.istate.mode = 8;
                        goto Label_0468;
                    }
                    z.istate.mode = 13;
                    z.istate.marker = 0;
                    goto Label_0024;

                case 8:
                    goto Label_0468;

                case 9:
                    goto Label_04D3;

                case 10:
                    goto Label_0546;

                case 11:
                    goto Label_05B8;

                case 12:
                    goto Label_0667;

                case 13:
                    return -3;

                default:
                    return -2;
            }
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            int num2 = z.next_in[z.next_in_index++] & 0xff;
            if ((((z.istate.method << 8) + num2) % 0x1f) != 0)
            {
                z.istate.mode = 13;
                z.msg = "incorrect header check";
                z.istate.marker = 5;
                goto Label_0024;
            }
            if ((num2 & 0x20) == 0)
            {
                z.istate.mode = 7;
                goto Label_0024;
            }
            z.istate.mode = 2;
        Label_01EE:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need = ((z.next_in[z.next_in_index++] & 0xff) << 0x18) & -16777216;
            z.istate.mode = 3;
        Label_0258:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need += ((z.next_in[z.next_in_index++] & 0xff) << 0x10) & 0xff0000;
            z.istate.mode = 4;
        Label_02CA:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need += ((z.next_in[z.next_in_index++] & 0xff) << 8) & 0xff00;
            z.istate.mode = 5;
        Label_033B:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need += z.next_in[z.next_in_index++] & 0xff;
            z.adler = z.istate.need;
            z.istate.mode = 6;
            return 2;
        Label_0468:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need = ((z.next_in[z.next_in_index++] & 0xff) << 0x18) & -16777216;
            z.istate.mode = 9;
        Label_04D3:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need += ((z.next_in[z.next_in_index++] & 0xff) << 0x10) & 0xff0000;
            z.istate.mode = 10;
        Label_0546:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need += ((z.next_in[z.next_in_index++] & 0xff) << 8) & 0xff00;
            z.istate.mode = 11;
        Label_05B8:
            if (z.avail_in == 0)
            {
                return num1;
            }
            num1 = f;
            z.avail_in--;
            z.total_in++;
            z.istate.need += z.next_in[z.next_in_index++] & 0xff;
            if (((int) z.istate.was[0]) != ((int) z.istate.need))
            {
                z.istate.mode = 13;
                z.msg = "incorrect data check";
                z.istate.marker = 5;
                goto Label_0024;
            }
            z.istate.mode = 12;
        Label_0667:
            return 1;
        }

        internal int inflateEnd(ZStream z)
        {
            if (this.blocks != null)
            {
                this.blocks.free(z);
            }
            this.blocks = null;
            return 0;
        }

        internal int inflateInit(ZStream z, int w)
        {
            z.msg = null;
            this.blocks = null;
            this.nowrap = 0;
            if (w < 0)
            {
                w = -w;
                this.nowrap = 1;
            }
            if ((w < 8) || (w > 15))
            {
                this.inflateEnd(z);
                return -2;
            }
            this.wbits = w;
            z.istate.blocks = new InfBlocks(z, (z.istate.nowrap != 0) ? null : this, 1 << (w & 0x1f));
            this.inflateReset(z);
            return 0;
        }

        internal int inflateReset(ZStream z)
        {
            if ((z == null) || (z.istate == null))
            {
                return -2;
            }
            z.total_in = z.total_out = 0;
            z.msg = null;
            z.istate.mode = (z.istate.nowrap != 0) ? 7 : 0;
            z.istate.blocks.reset(z, null);
            return 0;
        }

        internal int inflateSetDictionary(ZStream z, byte[] dictionary, int dictLength)
        {
            int num1 = 0;
            int num2 = dictLength;
            if (((z == null) || (z.istate == null)) || (z.istate.mode != 6))
            {
                return -2;
            }
            if (z._adler.adler32((long) 1, dictionary, 0, dictLength) != z.adler)
            {
                return -3;
            }
            z.adler = z._adler.adler32((long) 0, null, 0, 0);
            if (num2 >= (1 << (z.istate.wbits & 0x1f)))
            {
                num2 = (1 << (z.istate.wbits & 0x1f)) - 1;
                num1 = dictLength - num2;
            }
            z.istate.blocks.set_dictionary(dictionary, num1, num2);
            z.istate.mode = 7;
            return 0;
        }

        internal int inflateSync(ZStream z)
        {
            if ((z == null) || (z.istate == null))
            {
                return -2;
            }
            if (z.istate.mode != 13)
            {
                z.istate.mode = 13;
                z.istate.marker = 0;
            }
            int num1 = z.avail_in;
            if (num1 == 0)
            {
                return -5;
            }
            int num2 = z.next_in_index;
            int num3 = z.istate.marker;
            while ((num1 != 0) && (num3 < 4))
            {
                if (z.next_in[num2] == Inflate.mark[num3])
                {
                    num3++;
                }
                else if (z.next_in[num2] != 0)
                {
                    num3 = 0;
                }
                else
                {
                    num3 = 4 - num3;
                }
                num2++;
                num1--;
            }
            z.total_in += num2 - z.next_in_index;
            z.next_in_index = num2;
            z.avail_in = num1;
            z.istate.marker = num3;
            if (num3 != 4)
            {
                return -3;
            }
            long num4 = z.total_in;
            long num5 = z.total_out;
            this.inflateReset(z);
            z.total_in = num4;
            z.total_out = num5;
            z.istate.mode = 7;
            return 0;
        }

        internal int inflateSyncPoint(ZStream z)
        {
            if (((z != null) && (z.istate != null)) && (z.istate.blocks != null))
            {
                return z.istate.blocks.sync_point();
            }
            return -2;
        }


        private const int BAD = 13;
        internal InfBlocks blocks;
        private const int BLOCKS = 7;
        private const int CHECK1 = 11;
        private const int CHECK2 = 10;
        private const int CHECK3 = 9;
        private const int CHECK4 = 8;
        private const int DICT0 = 6;
        private const int DICT1 = 5;
        private const int DICT2 = 4;
        private const int DICT3 = 3;
        private const int DICT4 = 2;
        private const int DONE = 12;
        private const int FLAG = 1;
        private static byte[] mark;
        internal int marker;
        private const int MAX_WBITS = 15;
        internal int method;
        private const int METHOD = 0;
        internal int mode;
        internal long need;
        internal int nowrap;
        private const int PRESET_DICT = 0x20;
        internal long[] was;
        internal int wbits;
        private const int Z_BUF_ERROR = -5;
        private const int Z_DATA_ERROR = -3;
        private const int Z_DEFLATED = 8;
        private const int Z_ERRNO = -1;
        internal const int Z_FINISH = 4;
        internal const int Z_FULL_FLUSH = 3;
        private const int Z_MEM_ERROR = -4;
        private const int Z_NEED_DICT = 2;
        internal const int Z_NO_FLUSH = 0;
        private const int Z_OK = 0;
        internal const int Z_PARTIAL_FLUSH = 1;
        private const int Z_STREAM_END = 1;
        private const int Z_STREAM_ERROR = -2;
        internal const int Z_SYNC_FLUSH = 2;
        private const int Z_VERSION_ERROR = -6;
    }
}

