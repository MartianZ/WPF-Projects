namespace zlib
{
    using System;
    using System.IO;

    internal class ZOutputStream : Stream
    {
        public ZOutputStream(Stream out_Renamed)
        {
            this.z = new ZStream();
            this.bufsize = 0x200;
            this.buf1 = new byte[1];
            this.InitBlock();
            this.out_Renamed = out_Renamed;
            this.z.inflateInit();
            this.compress = false;
        }

        public ZOutputStream(Stream out_Renamed, int level)
        {
            this.z = new ZStream();
            this.bufsize = 0x200;
            this.buf1 = new byte[1];
            this.InitBlock();
            this.out_Renamed = out_Renamed;
            this.z.deflateInit(level);
            this.compress = true;
        }

        public override void Close()
        {
            try
            {
                this.finish();
            }
            catch
            {
                return;
            }
            finally
            {
                this.end();
                this.out_Renamed.Close();
                this.out_Renamed = null;
            }
        }

        public virtual void end()
        {
            if (this.compress)
            {
                this.z.deflateEnd();
            }
            else
            {
                this.z.inflateEnd();
            }
            this.z.free();
            this.z = null;
        }

        public virtual void finish()
        {
            do
            {
                int num1;
                this.z.next_out = this.buf;
                this.z.next_out_index = 0;
                this.z.avail_out = this.bufsize;
                if (this.compress)
                {
                    num1 = this.z.deflate(4);
                }
                else
                {
                    num1 = this.z.inflate(4);
                }
                if ((num1 != 1) && (num1 != 0))
                {
                    throw new ZStreamException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
                }
                if ((this.bufsize - this.z.avail_out) > 0)
                {
                    this.out_Renamed.Write(this.buf, 0, this.bufsize - this.z.avail_out);
                }
            }
            while ((this.z.avail_in > 0) || (this.z.avail_out == 0));
            try
            {
                this.Flush();
            }
            catch
            {
            }
        }

        public override void Flush()
        {
            this.out_Renamed.Flush();
        }

        private void InitBlock()
        {
            this.flush_Renamed_Field = 0;
            this.buf = new byte[this.bufsize];
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return (long) 0;
        }

        public override void SetLength(long value)
        {
        }

        public override void Write(byte[] b1, int off, int len)
        {
            if (len != 0)
            {
                byte[] buffer1 = new byte[b1.Length];
                Array.Copy(b1, buffer1, b1.Length);
                this.z.next_in = buffer1;
                this.z.next_in_index = off;
                this.z.avail_in = len;
                do
                {
                    int num1;
                    this.z.next_out = this.buf;
                    this.z.next_out_index = 0;
                    this.z.avail_out = this.bufsize;
                    if (this.compress)
                    {
                        num1 = this.z.deflate(this.flush_Renamed_Field);
                    }
                    else
                    {
                        num1 = this.z.inflate(this.flush_Renamed_Field);
                    }
                    if ((num1 != 0) && (num1 != 1))
                    {
                        throw new ZStreamException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
                    }
                    this.out_Renamed.Write(this.buf, 0, this.bufsize - this.z.avail_out);
                }
                while ((this.z.avail_in > 0) || (this.z.avail_out == 0));
            }
        }

        public override void WriteByte(byte b)
        {
            this.WriteByte((int) b);
        }

        public void WriteByte(int b)
        {
            this.buf1[0] = (byte) b;
            this.Write(this.buf1, 0, 1);
        }


        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public virtual int FlushMode
        {
            get
            {
                return this.flush_Renamed_Field;
            }
            set
            {
                this.flush_Renamed_Field = value;
            }
        }

        public override long Length
        {
            get
            {
                return (long) 0;
            }
        }

        public override long Position
        {
            get
            {
                return (long) 0;
            }
            set
            {
            }
        }

        public virtual long TotalIn
        {
            get
            {
                return this.z.total_in;
            }
        }

        public virtual long TotalOut
        {
            get
            {
                return this.z.total_out;
            }
        }


        protected internal byte[] buf;
        protected internal byte[] buf1;
        protected internal int bufsize;
        protected internal bool compress;
        protected internal int flush_Renamed_Field;
        private Stream out_Renamed;
        protected internal ZStream z;
    }
}

