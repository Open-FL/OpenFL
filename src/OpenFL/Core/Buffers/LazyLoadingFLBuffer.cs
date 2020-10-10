﻿using System;

using OpenFL.Core.DataObjects.ExecutableDataObjects;

namespace OpenFL.Core.Buffers
{
    public class LazyLoadingFLBuffer : FLBuffer, IDisposable, IWarmable, IEditableBuffer
    {

        public delegate FLBuffer BufferLoader(FLProgram root);

        private readonly bool WarmOnStart;
        private MemoryBuffer _buffer;

        protected BufferLoader Loader;


        public LazyLoadingFLBuffer(BufferLoader loader, bool warmOnStart) : base(default, -1, -1, -1)
        {
            WarmOnStart = warmOnStart;
            Loader = loader;
        }

        public override MemoryBuffer Buffer
        {
            get
            {
                InitializeBuffer();
                return _buffer;
            }
            protected set
            {
                _buffer?.Dispose();
                _buffer = value;
            }
        }

        public override void Dispose()
        {
            _buffer?.Dispose();
            _buffer = null;
            Loader = null;
        }


        public int DataSize => (int) Buffer.Size;

        public void SetData(byte[] data)
        {
            MemoryBuffer buf = Buffer;

            if (data.Length != buf.Size)
            {
                throw new InvalidOperationException("The passed data has not the same size as the buffer has.");
            }

            if ((buf.Flags & MemoryFlag.ReadOnly) != 0)
            {
                throw new InvalidOperationException("Can not write to a ReadOnly Buffer");
            }

            CLAPI.WriteToBuffer(Root.Instance, buf, data);
        }

        public byte[] GetData()
        {
            if ((Buffer.Flags & MemoryFlag.WriteOnly) != 0)
            {
                throw new InvalidOperationException("Can not read a WriteOnly Buffer");
            }

            return CLAPI.ReadBuffer<byte>(Root.Instance, Buffer, (int) Buffer.Size);
        }

        public void Warm(bool force)
        {
            if (!WarmOnStart && !force)
            {
                return;
            }

            InitializeBuffer();
        }

        private void InitializeBuffer()
        {
            if (_buffer == null)
            {
                FLBuffer i = Loader(Root);
                _buffer = i.Buffer;
                Width = i.Width;
                Height = i.Height;
                Depth = i.Depth;
            }
        }

        internal override void ReplaceUnderlyingBuffer(MemoryBuffer buf, int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Buffer = buf;
        }

    }
}