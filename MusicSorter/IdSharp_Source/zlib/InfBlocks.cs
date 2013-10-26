namespace zlib
{
    using System;

    internal sealed class InfBlocks
    {
        static InfBlocks()
        {
            InfBlocks.inflate_mask = new int[] { 
                0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 
                0xffff
             };
            InfBlocks.border = new int[] { 
                0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 
                14, 1, 15
             };
        }

        internal InfBlocks(ZStream z, object checkfn, int w)
        {
            this.bb = new int[1];
            this.tb = new int[1];
            this.hufts = new int[0x10e0];
            this.window = new byte[w];
            this.end = w;
            this.checkfn = checkfn;
            this.mode = 0;
            this.reset(z, null);
        }

        internal void free(ZStream z)
        {
            this.reset(z, null);
            this.window = null;
            this.hufts = null;
        }

        internal int inflate_flush(ZStream z, int r)
        {
            int num2 = z.next_out_index;
            int num3 = this.read;
            int num1 = ((num3 <= this.write) ? this.write : this.end) - num3;
            if (num1 > z.avail_out)
            {
                num1 = z.avail_out;
            }
            if ((num1 != 0) && (r == -5))
            {
                r = 0;
            }
            z.avail_out -= num1;
            z.total_out += num1;
            if (this.checkfn != null)
            {
                z.adler = this.check = z._adler.adler32(this.check, this.window, num3, num1);
            }
            Array.Copy(this.window, num3, z.next_out, num2, num1);
            num2 += num1;
            num3 += num1;
            if (num3 == this.end)
            {
                num3 = 0;
                if (this.write == this.end)
                {
                    this.write = 0;
                }
                num1 = this.write - num3;
                if (num1 > z.avail_out)
                {
                    num1 = z.avail_out;
                }
                if ((num1 != 0) && (r == -5))
                {
                    r = 0;
                }
                z.avail_out -= num1;
                z.total_out += num1;
                if (this.checkfn != null)
                {
                    z.adler = this.check = z._adler.adler32(this.check, this.window, num3, num1);
                }
                Array.Copy(this.window, num3, z.next_out, num2, num1);
                num2 += num1;
                num3 += num1;
            }
            z.next_out_index = num2;
            this.read = num3;
            return r;
        }

        internal int proc(ZStream z, int r)
        {
            int num1;
            int num4 = z.next_in_index;
            int num5 = z.avail_in;
            int num2 = this.bitb;
            int num3 = this.bitk;
            int num6 = this.write;
            int num7 = (num6 < this.read) ? ((this.read - num6) - 1) : (this.end - num6);
        Label_0047:
            switch (this.mode)
            {
                case 0:
                    while (num3 < 3)
                    {
                        if (num5 != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            this.bitb = num2;
                            this.bitk = num3;
                            z.avail_in = num5;
                            z.total_in += num4 - z.next_in_index;
                            z.next_in_index = num4;
                            this.write = num6;
                            return this.inflate_flush(z, r);
                        }
                        num5--;
                        num2 |= (z.next_in[num4++] & 0xff) << (num3 & 0x1f);
                        num3 += 8;
                    }
                    num1 = num2 & 7;
                    this.last = num1 & 1;
                    switch (SupportClass.URShift(num1, 1))
                    {
                        case 0:
                            num2 = SupportClass.URShift(num2, 3);
                            num3 -= 3;
                            num1 = num3 & 7;
                            num2 = SupportClass.URShift(num2, num1);
                            num3 -= num1;
                            this.mode = 1;
                            goto Label_0047;

                        case 1:
                        {
                            int[] numArray1 = new int[1];
                            int[] numArray2 = new int[1];
                            int[][] numArrayArray1 = new int[1][];
                            int[][] numArrayArray2 = new int[1][];
                            InfTree.inflate_trees_fixed(numArray1, numArray2, numArrayArray1, numArrayArray2, z);
                            this.codes = new InfCodes(numArray1[0], numArray2[0], numArrayArray1[0], numArrayArray2[0], z);
                            num2 = SupportClass.URShift(num2, 3);
                            num3 -= 3;
                            this.mode = 6;
                            goto Label_0047;
                        }
                        case 2:
                            num2 = SupportClass.URShift(num2, 3);
                            num3 -= 3;
                            this.mode = 3;
                            goto Label_0047;

                        case 3:
                            num2 = SupportClass.URShift(num2, 3);
                            num3 -= 3;
                            this.mode = 9;
                            z.msg = "invalid block type";
                            r = -3;
                            this.bitb = num2;
                            this.bitk = num3;
                            z.avail_in = num5;
                            z.total_in += num4 - z.next_in_index;
                            z.next_in_index = num4;
                            this.write = num6;
                            return this.inflate_flush(z, r);
                    }
                    goto Label_0047;

                case 1:
                    while (num3 < 0x20)
                    {
                        if (num5 != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            this.bitb = num2;
                            this.bitk = num3;
                            z.avail_in = num5;
                            z.total_in += num4 - z.next_in_index;
                            z.next_in_index = num4;
                            this.write = num6;
                            return this.inflate_flush(z, r);
                        }
                        num5--;
                        num2 |= (z.next_in[num4++] & 0xff) << (num3 & 0x1f);
                        num3 += 8;
                    }
                    if ((SupportClass.URShift(~num2, 0x10) & 0xffff) != (num2 & 0xffff))
                    {
                        this.mode = 9;
                        z.msg = "invalid stored block lengths";
                        r = -3;
                        this.bitb = num2;
                        this.bitk = num3;
                        z.avail_in = num5;
                        z.total_in += num4 - z.next_in_index;
                        z.next_in_index = num4;
                        this.write = num6;
                        return this.inflate_flush(z, r);
                    }
                    this.left = num2 & 0xffff;
                    num2 = num3 = 0;
                    this.mode = (this.left != 0) ? 2 : ((this.last != 0) ? 7 : 0);
                    goto Label_0047;

                case 2:
                    if (num5 == 0)
                    {
                        this.bitb = num2;
                        this.bitk = num3;
                        z.avail_in = num5;
                        z.total_in += num4 - z.next_in_index;
                        z.next_in_index = num4;
                        this.write = num6;
                        return this.inflate_flush(z, r);
                    }
                    if (num7 == 0)
                    {
                        if ((num6 == this.end) && (this.read != 0))
                        {
                            num6 = 0;
                            num7 = (num6 < this.read) ? ((this.read - num6) - 1) : (this.end - num6);
                        }
                        if (num7 == 0)
                        {
                            this.write = num6;
                            r = this.inflate_flush(z, r);
                            num6 = this.write;
                            num7 = (num6 < this.read) ? ((this.read - num6) - 1) : (this.end - num6);
                            if ((num6 == this.end) && (this.read != 0))
                            {
                                num6 = 0;
                                num7 = (num6 < this.read) ? ((this.read - num6) - 1) : (this.end - num6);
                            }
                            if (num7 == 0)
                            {
                                this.bitb = num2;
                                this.bitk = num3;
                                z.avail_in = num5;
                                z.total_in += num4 - z.next_in_index;
                                z.next_in_index = num4;
                                this.write = num6;
                                return this.inflate_flush(z, r);
                            }
                        }
                    }
                    r = 0;
                    num1 = this.left;
                    if (num1 > num5)
                    {
                        num1 = num5;
                    }
                    if (num1 > num7)
                    {
                        num1 = num7;
                    }
                    Array.Copy(z.next_in, num4, this.window, num6, num1);
                    num4 += num1;
                    num5 -= num1;
                    num6 += num1;
                    num7 -= num1;
                    this.left -= num1;
                    if (this.left == 0)
                    {
                        this.mode = (this.last != 0) ? 7 : 0;
                    }
                    goto Label_0047;

                case 3:
                    while (num3 < 14)
                    {
                        if (num5 != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            this.bitb = num2;
                            this.bitk = num3;
                            z.avail_in = num5;
                            z.total_in += num4 - z.next_in_index;
                            z.next_in_index = num4;
                            this.write = num6;
                            return this.inflate_flush(z, r);
                        }
                        num5--;
                        num2 |= (z.next_in[num4++] & 0xff) << (num3 & 0x1f);
                        num3 += 8;
                    }
                    this.table = num1 = num2 & 0x3fff;
                    if (((num1 & 0x1f) > 0x1d) || (((num1 >> 5) & 0x1f) > 0x1d))
                    {
                        this.mode = 9;
                        z.msg = "too many length or distance symbols";
                        r = -3;
                        this.bitb = num2;
                        this.bitk = num3;
                        z.avail_in = num5;
                        z.total_in += num4 - z.next_in_index;
                        z.next_in_index = num4;
                        this.write = num6;
                        return this.inflate_flush(z, r);
                    }
                    num1 = (0x102 + (num1 & 0x1f)) + ((num1 >> 5) & 0x1f);
                    this.blens = new int[num1];
                    num2 = SupportClass.URShift(num2, 14);
                    num3 -= 14;
                    this.index = 0;
                    this.mode = 4;
                    goto Label_06E1;

                case 4:
                    goto Label_06E1;

                case 5:
                    goto Label_07B9;

                case 6:
                    goto Label_0B63;

                case 7:
                    goto Label_0C2C;

                case 8:
                    goto Label_0CC1;

                case 9:
                    r = -3;
                    this.bitb = num2;
                    this.bitk = num3;
                    z.avail_in = num5;
                    z.total_in += num4 - z.next_in_index;
                    z.next_in_index = num4;
                    this.write = num6;
                    return this.inflate_flush(z, r);

                default:
                    r = -2;
                    this.bitb = num2;
                    this.bitk = num3;
                    z.avail_in = num5;
                    z.total_in += num4 - z.next_in_index;
                    z.next_in_index = num4;
                    this.write = num6;
                    return this.inflate_flush(z, r);
            }
        Label_06E1:
            if (this.index < (4 + SupportClass.URShift(this.table, 10)))
            {
                while (num3 < 3)
                {
                    if (num5 != 0)
                    {
                        r = 0;
                    }
                    else
                    {
                        this.bitb = num2;
                        this.bitk = num3;
                        z.avail_in = num5;
                        z.total_in += num4 - z.next_in_index;
                        z.next_in_index = num4;
                        this.write = num6;
                        return this.inflate_flush(z, r);
                    }
                    num5--;
                    num2 |= (z.next_in[num4++] & 0xff) << (num3 & 0x1f);
                    num3 += 8;
                }
                this.blens[InfBlocks.border[this.index++]] = num2 & 7;
                num2 = SupportClass.URShift(num2, 3);
                num3 -= 3;
                goto Label_06E1;
            }
            while (this.index < 0x13)
            {
                this.blens[InfBlocks.border[this.index++]] = 0;
            }
            this.bb[0] = 7;
            num1 = InfTree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, z);
            if (num1 != 0)
            {
                r = num1;
                if (r == -3)
                {
                    this.blens = null;
                    this.mode = 9;
                }
                this.bitb = num2;
                this.bitk = num3;
                z.avail_in = num5;
                z.total_in += num4 - z.next_in_index;
                z.next_in_index = num4;
                this.write = num6;
                return this.inflate_flush(z, r);
            }
            this.index = 0;
            this.mode = 5;
        Label_07B9:
            num1 = this.table;
            if (this.index < ((0x102 + (num1 & 0x1f)) + ((num1 >> 5) & 0x1f)))
            {
                num1 = this.bb[0];
                while (num3 < num1)
                {
                    if (num5 != 0)
                    {
                        r = 0;
                    }
                    else
                    {
                        this.bitb = num2;
                        this.bitk = num3;
                        z.avail_in = num5;
                        z.total_in += num4 - z.next_in_index;
                        z.next_in_index = num4;
                        this.write = num6;
                        return this.inflate_flush(z, r);
                    }
                    num5--;
                    num2 |= (z.next_in[num4++] & 0xff) << (num3 & 0x1f);
                    num3 += 8;
                }
                int num17 = this.tb[0];
                num1 = this.hufts[((this.tb[0] + (num2 & InfBlocks.inflate_mask[num1])) * 3) + 1];
                int num10 = this.hufts[((this.tb[0] + (num2 & InfBlocks.inflate_mask[num1])) * 3) + 2];
                if (num10 < 0x10)
                {
                    num2 = SupportClass.URShift(num2, num1);
                    num3 -= num1;
                    this.blens[this.index++] = num10;
                    goto Label_07B9;
                }
                int num8 = (num10 == 0x12) ? 7 : (num10 - 14);
                int num9 = (num10 == 0x12) ? 11 : 3;
                while (num3 < (num1 + num8))
                {
                    if (num5 != 0)
                    {
                        r = 0;
                    }
                    else
                    {
                        this.bitb = num2;
                        this.bitk = num3;
                        z.avail_in = num5;
                        z.total_in += num4 - z.next_in_index;
                        z.next_in_index = num4;
                        this.write = num6;
                        return this.inflate_flush(z, r);
                    }
                    num5--;
                    num2 |= (z.next_in[num4++] & 0xff) << (num3 & 0x1f);
                    num3 += 8;
                }
                num2 = SupportClass.URShift(num2, num1);
                num3 -= num1;
                num9 += num2 & InfBlocks.inflate_mask[num8];
                num2 = SupportClass.URShift(num2, num8);
                num3 -= num8;
                num8 = this.index;
                num1 = this.table;
                if (((num8 + num9) > ((0x102 + (num1 & 0x1f)) + ((num1 >> 5) & 0x1f))) || ((num10 == 0x10) && (num8 < 1)))
                {
                    this.blens = null;
                    this.mode = 9;
                    z.msg = "invalid bit length repeat";
                    r = -3;
                    this.bitb = num2;
                    this.bitk = num3;
                    z.avail_in = num5;
                    z.total_in += num4 - z.next_in_index;
                    z.next_in_index = num4;
                    this.write = num6;
                    return this.inflate_flush(z, r);
                }
                num10 = (num10 == 0x10) ? this.blens[num8 - 1] : 0;
                do
                {
                    this.blens[num8++] = num10;
                }
                while (--num9 != 0);
                this.index = num8;
                goto Label_07B9;
            }
            this.tb[0] = -1;
            int[] numArray3 = new int[1];
            int[] numArray4 = new int[1];
            int[] numArray5 = new int[1];
            int[] numArray6 = new int[1];
            numArray3[0] = 9;
            numArray4[0] = 6;
            num1 = this.table;
            num1 = InfTree.inflate_trees_dynamic(0x101 + (num1 & 0x1f), 1 + ((num1 >> 5) & 0x1f), this.blens, numArray3, numArray4, numArray5, numArray6, this.hufts, z);
            switch (num1)
            {
                case 0:
                    this.codes = new InfCodes(numArray3[0], numArray4[0], this.hufts, numArray5[0], this.hufts, numArray6[0], z);
                    this.blens = null;
                    this.mode = 6;
                    goto Label_0B63;

                case -3:
                    this.blens = null;
                    this.mode = 9;
                    break;
            }
            r = num1;
            this.bitb = num2;
            this.bitk = num3;
            z.avail_in = num5;
            z.total_in += num4 - z.next_in_index;
            z.next_in_index = num4;
            this.write = num6;
            return this.inflate_flush(z, r);
        Label_0B63:
            this.bitb = num2;
            this.bitk = num3;
            z.avail_in = num5;
            z.total_in += num4 - z.next_in_index;
            z.next_in_index = num4;
            this.write = num6;
            if ((r = this.codes.proc(this, z, r)) != 1)
            {
                return this.inflate_flush(z, r);
            }
            r = 0;
            this.codes.free(z);
            num4 = z.next_in_index;
            num5 = z.avail_in;
            num2 = this.bitb;
            num3 = this.bitk;
            num6 = this.write;
            num7 = (num6 < this.read) ? ((this.read - num6) - 1) : (this.end - num6);
            if (this.last == 0)
            {
                this.mode = 0;
                goto Label_0047;
            }
            this.mode = 7;
        Label_0C2C:
            this.write = num6;
            r = this.inflate_flush(z, r);
            num6 = this.write;
            num7 = (num6 < this.read) ? ((this.read - num6) - 1) : (this.end - num6);
            if (this.read != this.write)
            {
                this.bitb = num2;
                this.bitk = num3;
                z.avail_in = num5;
                z.total_in += num4 - z.next_in_index;
                z.next_in_index = num4;
                this.write = num6;
                return this.inflate_flush(z, r);
            }
            this.mode = 8;
        Label_0CC1:
            r = 1;
            this.bitb = num2;
            this.bitk = num3;
            z.avail_in = num5;
            z.total_in += num4 - z.next_in_index;
            z.next_in_index = num4;
            this.write = num6;
            return this.inflate_flush(z, r);
        }

        internal void reset(ZStream z, long[] c)
        {
            if (c != null)
            {
                c[0] = this.check;
            }
            if ((this.mode == 4) || (this.mode == 5))
            {
                this.blens = null;
            }
            if (this.mode == 6)
            {
                this.codes.free(z);
            }
            this.mode = 0;
            this.bitk = 0;
            this.bitb = 0;
            this.read = this.write = 0;
            if (this.checkfn != null)
            {
                z.adler = this.check = z._adler.adler32((long) 0, null, 0, 0);
            }
        }

        internal void set_dictionary(byte[] d, int start, int n)
        {
            Array.Copy(d, start, this.window, 0, n);
            this.read = this.write = n;
        }

        internal int sync_point()
        {
            if (this.mode != 1)
            {
                return 0;
            }
            return 1;
        }


        private const int BAD = 9;
        internal int[] bb;
        internal int bitb;
        internal int bitk;
        internal int[] blens;
        internal static readonly int[] border;
        private const int BTREE = 4;
        internal long check;
        internal object checkfn;
        internal InfCodes codes;
        private const int CODES = 6;
        private const int DONE = 8;
        private const int DRY = 7;
        private const int DTREE = 5;
        internal int end;
        internal int[] hufts;
        internal int index;
        private static readonly int[] inflate_mask;
        internal int last;
        internal int left;
        private const int LENS = 1;
        private const int MANY = 0x5a0;
        internal int mode;
        internal int read;
        private const int STORED = 2;
        internal int table;
        private const int TABLE = 3;
        internal int[] tb;
        private const int TYPE = 0;
        internal byte[] window;
        internal int write;
        private const int Z_BUF_ERROR = -5;
        private const int Z_DATA_ERROR = -3;
        private const int Z_ERRNO = -1;
        private const int Z_MEM_ERROR = -4;
        private const int Z_NEED_DICT = 2;
        private const int Z_OK = 0;
        private const int Z_STREAM_END = 1;
        private const int Z_STREAM_ERROR = -2;
        private const int Z_VERSION_ERROR = -6;
    }
}

