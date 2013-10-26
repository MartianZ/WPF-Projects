namespace zlib
{
    using System;

    internal sealed class Tree
    {
        static Tree()
        {
            Tree.L_CODES = 0x11e;
            Tree.HEAP_SIZE = (2 * Tree.L_CODES) + 1;
            Tree.extra_lbits = new int[] { 
                0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 
                3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0
             };
            Tree.extra_dbits = new int[] { 
                0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 
                7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13
             };
            Tree.extra_blbits = new int[] { 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                2, 3, 7
             };
            Tree.bl_order = new byte[] { 
                0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 
                14, 1, 15
             };
            Tree._dist_code = new byte[] { 
                0, 1, 2, 3, 4, 4, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 
                8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 
                10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 
                11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
                12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 
                12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 
                13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 
                13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
                0, 0, 0x10, 0x11, 0x12, 0x12, 0x13, 0x13, 20, 20, 20, 20, 0x15, 0x15, 0x15, 0x15, 
                0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 
                0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 
                0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d
             };
            Tree._length_code = new byte[] { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 9, 9, 10, 10, 11, 11, 
                12, 12, 12, 12, 13, 13, 13, 13, 14, 14, 14, 14, 15, 15, 15, 15, 
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 
                0x12, 0x12, 0x12, 0x12, 0x12, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 0x13, 0x13, 0x13, 0x13, 
                20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 
                0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 
                0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 
                0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 
                0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 
                0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 
                0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 
                0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1c
             };
            Tree.base_length = new int[] { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12, 14, 0x10, 20, 0x18, 0x1c, 
                0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0
             };
            Tree.base_dist = new int[] { 
                0, 1, 2, 3, 4, 6, 8, 12, 0x10, 0x18, 0x20, 0x30, 0x40, 0x60, 0x80, 0xc0, 
                0x100, 0x180, 0x200, 0x300, 0x400, 0x600, 0x800, 0xc00, 0x1000, 0x1800, 0x2000, 0x3000, 0x4000, 0x6000
             };
        }

        internal static int bi_reverse(int code, int len)
        {
            int num1 = 0;
            do
            {
                num1 |= code & 1;
                code = SupportClass.URShift(code, 1);
                num1 = num1 << 1;
            }
            while (--len > 0);
            return SupportClass.URShift(num1, 1);
        }

        internal void build_tree(Deflate s)
        {
            int num4;
            short[] numArray1 = this.dyn_tree;
            short[] numArray2 = this.stat_desc.static_tree;
            int num1 = this.stat_desc.elems;
            int num3 = -1;
            s.heap_len = 0;
            s.heap_max = Tree.HEAP_SIZE;
            int num2 = 0;
            while (num2 < num1)
            {
                if (numArray1[num2 * 2] != 0)
                {
                    s.heap[++s.heap_len] = num3 = num2;
                    s.depth[num2] = 0;
                }
                else
                {
                    numArray1[(num2 * 2) + 1] = 0;
                }
                num2++;
            }
            while (s.heap_len < 2)
            {
                num4 = s.heap[++s.heap_len] = (num3 < 2) ? ++num3 : 0;
                numArray1[num4 * 2] = 1;
                s.depth[num4] = 0;
                s.opt_len--;
                if (numArray2 != null)
                {
                    s.static_len -= numArray2[(num4 * 2) + 1];
                }
            }
            this.max_code = num3;
            num2 = s.heap_len / 2;
            while (num2 >= 1)
            {
                s.pqdownheap(numArray1, num2);
                num2--;
            }
            num4 = num1;
            do
            {
                num2 = s.heap[1];
                s.heap[1] = s.heap[s.heap_len--];
                s.pqdownheap(numArray1, 1);
                int num5 = s.heap[1];
                s.heap[--s.heap_max] = num2;
                s.heap[--s.heap_max] = num5;
                numArray1[num4 * 2] = (short) (numArray1[num2 * 2] + numArray1[num5 * 2]);
                s.depth[num4] = (byte) (Math.Max(s.depth[num2], s.depth[num5]) + 1);
                numArray1[(num2 * 2) + 1] = numArray1[(num5 * 2) + 1] = (short) num4;
                s.heap[1] = num4++;
                s.pqdownheap(numArray1, 1);
            }
            while (s.heap_len >= 2);
            s.heap[--s.heap_max] = s.heap[1];
            this.gen_bitlen(s);
            Tree.gen_codes(numArray1, num3, s.bl_count);
        }

        internal static int d_code(int dist)
        {
            if (dist >= 0x100)
            {
                return Tree._dist_code[0x100 + SupportClass.URShift(dist, 7)];
            }
            return Tree._dist_code[dist];
        }

        internal void gen_bitlen(Deflate s)
        {
            int num4;
            short[] numArray1 = this.dyn_tree;
            short[] numArray2 = this.stat_desc.static_tree;
            int[] numArray3 = this.stat_desc.extra_bits;
            int num1 = this.stat_desc.extra_base;
            int num2 = this.stat_desc.max_length;
            int num6 = 0;
            int num5 = 0;
            while (num5 <= 15)
            {
                s.bl_count[num5] = 0;
                num5++;
            }
            numArray1[(s.heap[s.heap_max] * 2) + 1] = 0;
            int num3 = s.heap_max + 1;
            while (num3 < Tree.HEAP_SIZE)
            {
                num4 = s.heap[num3];
                num5 = numArray1[(numArray1[(num4 * 2) + 1] * 2) + 1] + 1;
                if (num5 > num2)
                {
                    num5 = num2;
                    num6++;
                }
                numArray1[(num4 * 2) + 1] = (short) num5;
                if (num4 <= this.max_code)
                {
                    s.bl_count[num5] = (short) (s.bl_count[num5] + 1);
                    int num7 = 0;
                    if (num4 >= num1)
                    {
                        num7 = numArray3[num4 - num1];
                    }
                    short num8 = numArray1[num4 * 2];
                    s.opt_len += num8 * (num5 + num7);
                    if (numArray2 != null)
                    {
                        s.static_len += num8 * (numArray2[(num4 * 2) + 1] + num7);
                    }
                }
                num3++;
            }
            if (num6 != 0)
            {
                do
                {
                    num5 = num2 - 1;
                    while (s.bl_count[num5] == 0)
                    {
                        num5--;
                    }
                    s.bl_count[num5] = (short) (s.bl_count[num5] - 1);
                    s.bl_count[num5 + 1] = (short) (s.bl_count[num5 + 1] + 2);
                    s.bl_count[num2] = (short) (s.bl_count[num2] - 1);
                    num6 -= 2;
                }
                while (num6 > 0);
                for (num5 = num2; num5 != 0; num5--)
                {
                    num4 = s.bl_count[num5];
                    while (num4 != 0)
                    {
                        int num9 = s.heap[--num3];
                        if (num9 <= this.max_code)
                        {
                            if (numArray1[(num9 * 2) + 1] != num5)
                            {
                                s.opt_len += (num5 - numArray1[(num9 * 2) + 1]) * numArray1[num9 * 2];
                                numArray1[(num9 * 2) + 1] = (short) num5;
                            }
                            num4--;
                        }
                    }
                }
            }
        }

        internal static void gen_codes(short[] tree, int max_code, short[] bl_count)
        {
            short[] numArray1 = new short[0x10];
            short num1 = 0;
            for (int num2 = 1; num2 <= 15; num2++)
            {
                numArray1[num2] = num1 = (short) ((num1 + bl_count[num2 - 1]) << 1);
            }
            for (int num3 = 0; num3 <= max_code; num3++)
            {
                int num4 = tree[(num3 * 2) + 1];
                if (num4 != 0)
                {
                    short num5;
                    numArray1[num4] = (short) ((num5 = numArray1[num4]) + 1);
                    tree[num3 * 2] = (short) Tree.bi_reverse(num5, num4);
                }
            }
        }


        internal static readonly byte[] _dist_code;
        internal static readonly byte[] _length_code;
        internal static readonly int[] base_dist;
        internal static readonly int[] base_length;
        internal static readonly byte[] bl_order;
        internal const int Buf_size = 0x10;
        internal const int DIST_CODE_LEN = 0x200;
        internal short[] dyn_tree;
        internal const int END_BLOCK = 0x100;
        internal static readonly int[] extra_blbits;
        internal static readonly int[] extra_dbits;
        internal static readonly int[] extra_lbits;
        private static readonly int HEAP_SIZE;
        private static readonly int L_CODES;
        private const int LENGTH_CODES = 0x1d;
        private const int LITERALS = 0x100;
        private const int MAX_BITS = 15;
        internal const int MAX_BL_BITS = 7;
        internal int max_code;
        internal const int REP_3_6 = 0x10;
        internal const int REPZ_11_138 = 0x12;
        internal const int REPZ_3_10 = 0x11;
        internal StaticTree stat_desc;
    }
}

