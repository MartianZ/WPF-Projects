namespace zlib
{
    using System;

    internal sealed class InfCodes
    {
        static InfCodes()
        {
            InfCodes.inflate_mask = new int[] { 
                0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 
                0xffff
             };
        }

        internal InfCodes(int bl, int bd, int[] tl, int[] td, ZStream z)
        {
            this.mode = 0;
            this.lbits = (byte) bl;
            this.dbits = (byte) bd;
            this.ltree = tl;
            this.ltree_index = 0;
            this.dtree = td;
            this.dtree_index = 0;
        }

        internal InfCodes(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, ZStream z)
        {
            this.mode = 0;
            this.lbits = (byte) bl;
            this.dbits = (byte) bd;
            this.ltree = tl;
            this.ltree_index = tl_index;
            this.dtree = td;
            this.dtree_index = td_index;
        }

        internal void free(ZStream z)
        {
        }

        internal int inflate_fast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InfBlocks s, ZStream z)
        {
            int num12;
            int num6 = z.next_in_index;
            int num7 = z.avail_in;
            int num4 = s.bitb;
            int num5 = s.bitk;
            int num8 = s.write;
            int num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
            int num10 = InfCodes.inflate_mask[bl];
            int num11 = InfCodes.inflate_mask[bd];
        Label_0092:
            while (num5 < 20)
            {
                num7--;
                num4 |= (z.next_in[num6++] & 0xff) << (num5 & 0x1f);
                num5 += 8;
            }
            int num1 = num4 & num10;
            int[] numArray1 = tl;
            int num2 = tl_index;
            int num3 = numArray1[(num2 + num1) * 3];
            if (num3 == 0)
            {
                num4 = num4 >> (numArray1[((num2 + num1) * 3) + 1] & 0x1f);
                num5 -= numArray1[((num2 + num1) * 3) + 1];
                s.window[num8++] = (byte) numArray1[((num2 + num1) * 3) + 2];
                num9--;
                goto Label_05E0;
            }
        Label_00F1:
            num4 = num4 >> (numArray1[((num2 + num1) * 3) + 1] & 0x1f);
            num5 -= numArray1[((num2 + num1) * 3) + 1];
            if ((num3 & 0x10) == 0)
            {
                if ((num3 & 0x40) == 0)
                {
                    num1 += numArray1[((num2 + num1) * 3) + 2];
                    num1 += num4 & InfCodes.inflate_mask[num3];
                    num3 = numArray1[(num2 + num1) * 3];
                    if (num3 != 0)
                    {
                        goto Label_00F1;
                    }
                    num4 = num4 >> (numArray1[((num2 + num1) * 3) + 1] & 0x1f);
                    num5 -= numArray1[((num2 + num1) * 3) + 1];
                    s.window[num8++] = (byte) numArray1[((num2 + num1) * 3) + 2];
                    num9--;
                }
                else
                {
                    if ((num3 & 0x20) != 0)
                    {
                        num12 = z.avail_in - num7;
                        num12 = ((num5 >> 3) < num12) ? (num5 >> 3) : num12;
                        num7 += num12;
                        num6 -= num12;
                        num5 -= num12 << 3;
                        s.bitb = num4;
                        s.bitk = num5;
                        z.avail_in = num7;
                        z.total_in += num6 - z.next_in_index;
                        z.next_in_index = num6;
                        s.write = num8;
                        return 1;
                    }
                    z.msg = "invalid literal/length code";
                    num12 = z.avail_in - num7;
                    num12 = ((num5 >> 3) < num12) ? (num5 >> 3) : num12;
                    num7 += num12;
                    num6 -= num12;
                    num5 -= num12 << 3;
                    s.bitb = num4;
                    s.bitk = num5;
                    z.avail_in = num7;
                    z.total_in += num6 - z.next_in_index;
                    z.next_in_index = num6;
                    s.write = num8;
                    return -3;
                }
                goto Label_05E0;
            }
            num3 &= 15;
            num12 = numArray1[((num2 + num1) * 3) + 2] + (num4 & InfCodes.inflate_mask[num3]);
            num4 = num4 >> (num3 & 0x1f);
            num5 -= num3;
            while (num5 < 15)
            {
                num7--;
                num4 |= (z.next_in[num6++] & 0xff) << (num5 & 0x1f);
                num5 += 8;
            }
            num1 = num4 & num11;
            numArray1 = td;
            num2 = td_index;
            num3 = numArray1[(num2 + num1) * 3];
        Label_018B:
            num4 = num4 >> (numArray1[((num2 + num1) * 3) + 1] & 0x1f);
            num5 -= numArray1[((num2 + num1) * 3) + 1];
            if ((num3 & 0x10) != 0)
            {
                int num14;
                num3 &= 15;
                while (num5 < num3)
                {
                    num7--;
                    num4 |= (z.next_in[num6++] & 0xff) << (num5 & 0x1f);
                    num5 += 8;
                }
                int num13 = numArray1[((num2 + num1) * 3) + 2] + (num4 & InfCodes.inflate_mask[num3]);
                num4 = num4 >> (num3 & 0x1f);
                num5 -= num3;
                num9 -= num12;
                if (num8 >= num13)
                {
                    num14 = num8 - num13;
                    if (((num8 - num14) > 0) && (2 > (num8 - num14)))
                    {
                        s.window[num8++] = s.window[num14++];
                        num12--;
                        s.window[num8++] = s.window[num14++];
                        num12--;
                    }
                    else
                    {
                        Array.Copy(s.window, num14, s.window, num8, 2);
                        num8 += 2;
                        num14 += 2;
                        num12 -= 2;
                    }
                }
                else
                {
                    num14 = num8 - num13;
                    do
                    {
                        num14 += s.end;
                    }
                    while (num14 < 0);
                    num3 = s.end - num14;
                    if (num12 > num3)
                    {
                        num12 -= num3;
                        if (((num8 - num14) > 0) && (num3 > (num8 - num14)))
                        {
                            do
                            {
                                s.window[num8++] = s.window[num14++];
                            }
                            while (--num3 != 0);
                        }
                        else
                        {
                            Array.Copy(s.window, num14, s.window, num8, num3);
                            num8 += num3;
                            num14 += num3;
                            num3 = 0;
                        }
                        num14 = 0;
                    }
                }
                if (((num8 - num14) > 0) && (num12 > (num8 - num14)))
                {
                    do
                    {
                        s.window[num8++] = s.window[num14++];
                    }
                    while (--num12 != 0);
                }
                else
                {
                    Array.Copy(s.window, num14, s.window, num8, num12);
                    num8 += num12;
                    num14 += num12;
                    num12 = 0;
                }
            }
            else
            {
                if ((num3 & 0x40) == 0)
                {
                    num1 += numArray1[((num2 + num1) * 3) + 2];
                    num1 += num4 & InfCodes.inflate_mask[num3];
                    num3 = numArray1[(num2 + num1) * 3];
                    goto Label_018B;
                }
                z.msg = "invalid distance code";
                num12 = z.avail_in - num7;
                num12 = ((num5 >> 3) < num12) ? (num5 >> 3) : num12;
                num7 += num12;
                num6 -= num12;
                num5 -= num12 << 3;
                s.bitb = num4;
                s.bitk = num5;
                z.avail_in = num7;
                z.total_in += num6 - z.next_in_index;
                z.next_in_index = num6;
                s.write = num8;
                return -3;
            }
        Label_05E0:
            if ((num9 < 0x102) || (num7 < 10))
            {
                num12 = z.avail_in - num7;
                num12 = ((num5 >> 3) < num12) ? (num5 >> 3) : num12;
                num7 += num12;
                num6 -= num12;
                num5 -= num12 << 3;
                s.bitb = num4;
                s.bitk = num5;
                z.avail_in = num7;
                z.total_in += num6 - z.next_in_index;
                z.next_in_index = num6;
                s.write = num8;
                return 0;
            }
            goto Label_0092;
        }

        internal int proc(InfBlocks s, ZStream z, int r)
        {
            int num1;
            int num10;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            num6 = z.next_in_index;
            int num7 = z.avail_in;
            num4 = s.bitb;
            num5 = s.bitk;
            int num8 = s.write;
            int num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
        Label_0051:
            switch (this.mode)
            {
                case 0:
                    if ((num9 < 0x102) || (num7 < 10))
                    {
                        break;
                    }
                    s.bitb = num4;
                    s.bitk = num5;
                    z.avail_in = num7;
                    z.total_in += num6 - z.next_in_index;
                    z.next_in_index = num6;
                    s.write = num8;
                    r = this.inflate_fast(this.lbits, this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, s, z);
                    num6 = z.next_in_index;
                    num7 = z.avail_in;
                    num4 = s.bitb;
                    num5 = s.bitk;
                    num8 = s.write;
                    num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                    if (r == 0)
                    {
                        break;
                    }
                    this.mode = (r == 1) ? 7 : 9;
                    goto Label_0051;

                case 1:
                    goto Label_0199;

                case 2:
                    num1 = this.get_Renamed;
                    while (num5 < num1)
                    {
                        if (num7 != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            s.bitb = num4;
                            s.bitk = num5;
                            z.avail_in = num7;
                            z.total_in += num6 - z.next_in_index;
                            z.next_in_index = num6;
                            s.write = num8;
                            return s.inflate_flush(z, r);
                        }
                        num7--;
                        num4 |= (z.next_in[num6++] & 0xff) << (num5 & 0x1f);
                        num5 += 8;
                    }
                    this.len += num4 & InfCodes.inflate_mask[num1];
                    num4 = num4 >> (num1 & 0x1f);
                    num5 -= num1;
                    this.need = this.dbits;
                    this.tree = this.dtree;
                    this.tree_index = this.dtree_index;
                    this.mode = 3;
                    goto Label_0412;

                case 3:
                    goto Label_0412;

                case 4:
                    num1 = this.get_Renamed;
                    while (num5 < num1)
                    {
                        if (num7 != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            s.bitb = num4;
                            s.bitk = num5;
                            z.avail_in = num7;
                            z.total_in += num6 - z.next_in_index;
                            z.next_in_index = num6;
                            s.write = num8;
                            return s.inflate_flush(z, r);
                        }
                        num7--;
                        num4 |= (z.next_in[num6++] & 0xff) << (num5 & 0x1f);
                        num5 += 8;
                    }
                    this.dist += num4 & InfCodes.inflate_mask[num1];
                    num4 = num4 >> (num1 & 0x1f);
                    num5 -= num1;
                    this.mode = 5;
                    goto Label_0635;

                case 5:
                    goto Label_0635;

                case 6:
                    if (num9 == 0)
                    {
                        if ((num8 == s.end) && (s.read != 0))
                        {
                            num8 = 0;
                            num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                        }
                        if (num9 == 0)
                        {
                            s.write = num8;
                            r = s.inflate_flush(z, r);
                            num8 = s.write;
                            num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                            if ((num8 == s.end) && (s.read != 0))
                            {
                                num8 = 0;
                                num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                            }
                            if (num9 == 0)
                            {
                                s.bitb = num4;
                                s.bitk = num5;
                                z.avail_in = num7;
                                z.total_in += num6 - z.next_in_index;
                                z.next_in_index = num6;
                                s.write = num8;
                                return s.inflate_flush(z, r);
                            }
                        }
                    }
                    r = 0;
                    s.window[num8++] = (byte) this.lit;
                    num9--;
                    this.mode = 0;
                    goto Label_0051;

                case 7:
                    if (num5 > 7)
                    {
                        num5 -= 8;
                        num7++;
                        num6--;
                    }
                    s.write = num8;
                    r = s.inflate_flush(z, r);
                    num8 = s.write;
                    num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                    if (s.read != s.write)
                    {
                        s.bitb = num4;
                        s.bitk = num5;
                        z.avail_in = num7;
                        z.total_in += num6 - z.next_in_index;
                        z.next_in_index = num6;
                        s.write = num8;
                        return s.inflate_flush(z, r);
                    }
                    this.mode = 8;
                    goto Label_098A;

                case 8:
                    goto Label_098A;

                case 9:
                    r = -3;
                    s.bitb = num4;
                    s.bitk = num5;
                    z.avail_in = num7;
                    z.total_in += num6 - z.next_in_index;
                    z.next_in_index = num6;
                    s.write = num8;
                    return s.inflate_flush(z, r);

                default:
                    r = -2;
                    s.bitb = num4;
                    s.bitk = num5;
                    z.avail_in = num7;
                    z.total_in += num6 - z.next_in_index;
                    z.next_in_index = num6;
                    s.write = num8;
                    return s.inflate_flush(z, r);
            }
            this.need = this.lbits;
            this.tree = this.ltree;
            this.tree_index = this.ltree_index;
            this.mode = 1;
        Label_0199:
            num1 = this.need;
            while (num5 < num1)
            {
                if (num7 != 0)
                {
                    r = 0;
                }
                else
                {
                    s.bitb = num4;
                    s.bitk = num5;
                    z.avail_in = num7;
                    z.total_in += num6 - z.next_in_index;
                    z.next_in_index = num6;
                    s.write = num8;
                    return s.inflate_flush(z, r);
                }
                num7--;
                num4 |= (z.next_in[num6++] & 0xff) << (num5 & 0x1f);
                num5 += 8;
            }
            int num2 = (this.tree_index + (num4 & InfCodes.inflate_mask[num1])) * 3;
            num4 = SupportClass.URShift(num4, this.tree[num2 + 1]);
            num5 -= this.tree[num2 + 1];
            int num3 = this.tree[num2];
            if (num3 == 0)
            {
                this.lit = this.tree[num2 + 2];
                this.mode = 6;
                goto Label_0051;
            }
            if ((num3 & 0x10) != 0)
            {
                this.get_Renamed = num3 & 15;
                this.len = this.tree[num2 + 2];
                this.mode = 2;
                goto Label_0051;
            }
            if ((num3 & 0x40) == 0)
            {
                this.need = num3;
                this.tree_index = (num2 / 3) + this.tree[num2 + 2];
                goto Label_0051;
            }
            if ((num3 & 0x20) != 0)
            {
                this.mode = 7;
                goto Label_0051;
            }
            this.mode = 9;
            z.msg = "invalid literal/length code";
            r = -3;
            s.bitb = num4;
            s.bitk = num5;
            z.avail_in = num7;
            z.total_in += num6 - z.next_in_index;
            z.next_in_index = num6;
            s.write = num8;
            return s.inflate_flush(z, r);
        Label_0412:
            num1 = this.need;
            while (num5 < num1)
            {
                if (num7 != 0)
                {
                    r = 0;
                }
                else
                {
                    s.bitb = num4;
                    s.bitk = num5;
                    z.avail_in = num7;
                    z.total_in += num6 - z.next_in_index;
                    z.next_in_index = num6;
                    s.write = num8;
                    return s.inflate_flush(z, r);
                }
                num7--;
                num4 |= (z.next_in[num6++] & 0xff) << (num5 & 0x1f);
                num5 += 8;
            }
            num2 = (this.tree_index + (num4 & InfCodes.inflate_mask[num1])) * 3;
            num4 = num4 >> (this.tree[num2 + 1] & 0x1f);
            num5 -= this.tree[num2 + 1];
            num3 = this.tree[num2];
            if ((num3 & 0x10) != 0)
            {
                this.get_Renamed = num3 & 15;
                this.dist = this.tree[num2 + 2];
                this.mode = 4;
                goto Label_0051;
            }
            if ((num3 & 0x40) == 0)
            {
                this.need = num3;
                this.tree_index = (num2 / 3) + this.tree[num2 + 2];
                goto Label_0051;
            }
            this.mode = 9;
            z.msg = "invalid distance code";
            r = -3;
            s.bitb = num4;
            s.bitk = num5;
            z.avail_in = num7;
            z.total_in += num6 - z.next_in_index;
            z.next_in_index = num6;
            s.write = num8;
            return s.inflate_flush(z, r);
        Label_0635:
            num10 = num8 - this.dist;
            while (num10 < 0)
            {
                num10 += s.end;
            }
            while (this.len != 0)
            {
                if (num9 == 0)
                {
                    if ((num8 == s.end) && (s.read != 0))
                    {
                        num8 = 0;
                        num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                    }
                    if (num9 == 0)
                    {
                        s.write = num8;
                        r = s.inflate_flush(z, r);
                        num8 = s.write;
                        num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                        if ((num8 == s.end) && (s.read != 0))
                        {
                            num8 = 0;
                            num9 = (num8 < s.read) ? ((s.read - num8) - 1) : (s.end - num8);
                        }
                        if (num9 == 0)
                        {
                            s.bitb = num4;
                            s.bitk = num5;
                            z.avail_in = num7;
                            z.total_in += num6 - z.next_in_index;
                            z.next_in_index = num6;
                            s.write = num8;
                            return s.inflate_flush(z, r);
                        }
                    }
                }
                s.window[num8++] = s.window[num10++];
                num9--;
                if (num10 == s.end)
                {
                    num10 = 0;
                }
                this.len--;
            }
            this.mode = 0;
            goto Label_0051;
        Label_098A:
            r = 1;
            s.bitb = num4;
            s.bitk = num5;
            z.avail_in = num7;
            z.total_in += num6 - z.next_in_index;
            z.next_in_index = num6;
            s.write = num8;
            return s.inflate_flush(z, r);
        }


        private const int BADCODE = 9;
        private const int COPY = 5;
        internal byte dbits;
        internal int dist;
        private const int DIST = 3;
        private const int DISTEXT = 4;
        internal int[] dtree;
        internal int dtree_index;
        private const int END = 8;
        internal int get_Renamed;
        private static readonly int[] inflate_mask;
        internal byte lbits;
        internal int len;
        private const int LEN = 1;
        private const int LENEXT = 2;
        internal int lit;
        private const int LIT = 6;
        internal int[] ltree;
        internal int ltree_index;
        internal int mode;
        internal int need;
        private const int START = 0;
        internal int[] tree;
        internal int tree_index;
        private const int WASH = 7;
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

