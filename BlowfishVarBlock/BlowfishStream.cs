﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BlowfishVarBlock
{
    public class BlowfishStream : Stream
    {
        class CBState : IAsyncResult
        {
            internal AsyncCallback callback;
            internal object state;
            internal byte[] buffer;
            internal IAsyncResult result;
            internal CBState(AsyncCallback callback, object state, byte[] buffer)
            {
                this.callback = callback;
                this.state = state;
                this.buffer = buffer;
            }
            #region IAsyncResult Members

            public object AsyncState
            {
                get
                {
                    return state;
                }
            }

            public bool CompletedSynchronously
            {
                get
                {
                    return result.CompletedSynchronously;
                }
            }

            public System.Threading.WaitHandle AsyncWaitHandle
            {
                get
                {
                    return result.AsyncWaitHandle;
                }
            }

            public bool IsCompleted
            {
                get
                {
                    return result.IsCompleted;
                }
            }

            #endregion
        }

        public enum Target
        {
            Encrypted,
            Normal
        };

        Stream stream;
        Blowfish bf;
        Target target;

        BlowfishStream(Stream stream, Blowfish bf, Target target)
        {
            this.stream = stream;
            this.bf = bf;
            this.target = target;
        }

        /// <summary>
        /// Returns true if the stream support reads.
        /// </summary>
        public override bool CanRead
        {
            get { return stream.CanRead; }
        }

        /// <summary>
        /// Returns true is the stream supports seeks.
        /// </summary>
        public override bool CanSeek
        {
            get { return stream.CanSeek; }
        }

        /// <summary>
        /// Returns true if the stream supports writes.
        /// </summary>
        public override bool CanWrite
        {
            get { return stream.CanWrite; }
        }

        /// <summary>
        /// Returns the length of the stream.
        /// </summary>
        public override long Length
        {
            get { return stream.Length; }
        }

        /// <summary>
        /// Gets or Sets the posistion of the stream.
        /// </summary>
        public override long Position
        {
            get { return stream.Position; }
            set { stream.Position = value; }
        }

        /// <summary>
        /// Flushes the stream.
        /// </summary>
        public override void Flush()
        {
            stream.Flush();
        }

        /// <summary>
        /// Read data from the stream and encrypt it.
        /// </summary>
        /// <param name="buffer">The buffer to read into.</param>
        /// <param name="offset">The offset in the buffer to begin storing data.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = stream.Read(buffer, offset, count);
            if (target == Target.Normal)
                bf.Encipher(buffer, bytesRead);
            else
                bf.Decipher(buffer, bytesRead);
            return bytesRead;
        }

        /// <summary>
        /// Write data to the stream after decrypting it.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to write.</param>
        /// <param name="offset">The offset in the buffer where the data begins.</param>
        /// <param name="count">The number of bytes to write.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (target == Target.Normal)
                bf.Decipher(buffer, count);
            else
                bf.Encipher(buffer, count);
            stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            CBState cbs = new CBState(callback, state, buffer);
            cbs.result = base.BeginRead(buffer, offset, count, new AsyncCallback(ReadComplete), cbs);
            return cbs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            CBState cbs = (CBState)asyncResult.AsyncState;
            int bytesRead = base.EndRead(cbs.result);
            if (target == Target.Normal)
                bf.Encipher(cbs.buffer, bytesRead);
            else
                bf.Decipher(cbs.buffer, bytesRead);
            return bytesRead;
        }


        /// <summary>
        /// The Read has completed.
        /// </summary>
        /// <param name="result">The result of the async write.</param>
        private void ReadComplete(IAsyncResult result)
        {
            CBState cbs = (CBState)result.AsyncState;
            cbs.callback(cbs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (target == Target.Normal)
                bf.Decipher(buffer, count);
            else
                bf.Encipher(buffer, count);
            return base.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Move the current stream posistion to the specified location.
        /// </summary>
        /// <param name="offset">The offset from the origin to seek.</param>
        /// <param name="origin">The origin to seek from.</param>
        /// <returns>The new position.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        /// <summary>
        /// Set the stream length.
        /// </summary>
        /// <param name="value">The length to set.</param>
        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }
    }
}
