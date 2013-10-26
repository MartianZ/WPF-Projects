namespace zlib
{
    using System;

    internal sealed class ZStream
    {
        static ZStream()
        {
            ZStream.DEF_WBITS = 15;
        }

        public ZStream()
        {
            this._adler = new Adler32();
        }

        public int deflate(int flush)
        {
            if (this.dstate == null)
            {
                return -2;
            }
            return this.dstate.deflate(this, flush);
        }

        public int deflateEnd()
        {
            if (this.dstate == null)
            {
                return -2;
            }
            int num1 = this.dstate.deflateEnd();
            this.dstate = null;
            return num1;
        }

        public int deflateInit(int level)
        {
            return this.deflateInit(level, 15);
        }

        public int deflateInit(int level, int bits)
        {
            this.dstate = new Deflate();
            return this.dstate.deflateInit(this, level, bits);
        }

        public int deflateParams(int level, int strategy)
        {
            if (this.dstate == null)
            {
                return -2;
            }
            return this.dstate.deflateParams(this, level, strategy);
        }

        public int deflateSetDictionary(byte[] dictionary, int dictLength)
        {
            if (this.dstate == null)
            {
                return -2;
            }
            return this.dstate.deflateSetDictionary(this, dictionary, dictLength);
        }

        internal void flush_pending()
        {
            int num1 = this.dstate.pending;
            if (num1 > this.avail_out)
            {
                num1 = this.avail_out;
            }
            if (num1 != 0)
            {
                if (((this.dstate.pending_buf.Length <= this.dstate.pending_out) || (this.next_out.Length <= this.next_out_index)) || ((this.dstate.pending_buf.Length < (this.dstate.pending_out + num1)) || (this.next_out.Length < (this.next_out_index + num1))))
                {
                    Console.Out.WriteLine(string.Concat(new object[] { this.dstate.pending_buf.Length, ", ", this.dstate.pending_out, ", ", this.next_out.Length, ", ", this.next_out_index, ", ", num1 }));
                    Console.Out.WriteLine("avail_out=" + this.avail_out);
                }
                Array.Copy(this.dstate.pending_buf, this.dstate.pending_out, this.next_out, this.next_out_index, num1);
                this.next_out_index += num1;
                this.dstate.pending_out += num1;
                this.total_out += num1;
                this.avail_out -= num1;
                this.dstate.pending -= num1;
                if (this.dstate.pending == 0)
                {
                    this.dstate.pending_out = 0;
                }
            }
        }

        public void free()
        {
            this.next_in = null;
            this.next_out = null;
            this.msg = null;
            this._adler = null;
        }

        public int inflate(int f)
        {
            if (this.istate == null)
            {
                return -2;
            }
            return this.istate.inflate(this, f);
        }

        public int inflateEnd()
        {
            if (this.istate == null)
            {
                return -2;
            }
            int num1 = this.istate.inflateEnd(this);
            this.istate = null;
            return num1;
        }

        public int inflateInit()
        {
            return this.inflateInit(ZStream.DEF_WBITS);
        }

        public int inflateInit(int w)
        {
            this.istate = new Inflate();
            return this.istate.inflateInit(this, w);
        }

        public int inflateSetDictionary(byte[] dictionary, int dictLength)
        {
            if (this.istate == null)
            {
                return -2;
            }
            return this.istate.inflateSetDictionary(this, dictionary, dictLength);
        }

        public int inflateSync()
        {
            if (this.istate == null)
            {
                return -2;
            }
            return this.istate.inflateSync(this);
        }

        internal int read_buf(byte[] buf, int start, int size)
        {
            int num1 = this.avail_in;
            if (num1 > size)
            {
                num1 = size;
            }
            if (num1 == 0)
            {
                return 0;
            }
            this.avail_in -= num1;
            if (this.dstate.noheader == 0)
            {
                this.adler = this._adler.adler32(this.adler, this.next_in, this.next_in_index, num1);
            }
            Array.Copy(this.next_in, this.next_in_index, buf, start, num1);
            this.next_in_index += num1;
            this.total_in += num1;
            return num1;
        }


        internal Adler32 _adler;
        public long adler;
        public int avail_in;
        public int avail_out;
        internal int data_type;
        private static readonly int DEF_WBITS;
        internal Deflate dstate;
        internal Inflate istate;
        private const int MAX_MEM_LEVEL = 9;
        private const int MAX_WBITS = 15;
        public string msg;
        public byte[] next_in;
        public int next_in_index;
        public byte[] next_out;
        public int next_out_index;
        public long total_in;
        public long total_out;
        private const int Z_BUF_ERROR = -5;
        private const int Z_DATA_ERROR = -3;
        private const int Z_ERRNO = -1;
        private const int Z_FINISH = 4;
        private const int Z_FULL_FLUSH = 3;
        private const int Z_MEM_ERROR = -4;
        private const int Z_NEED_DICT = 2;
        private const int Z_NO_FLUSH = 0;
        private const int Z_OK = 0;
        private const int Z_PARTIAL_FLUSH = 1;
        private const int Z_STREAM_END = 1;
        private const int Z_STREAM_ERROR = -2;
        private const int Z_SYNC_FLUSH = 2;
        private const int Z_VERSION_ERROR = -6;
    }
}

