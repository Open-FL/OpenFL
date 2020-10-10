using System.Drawing;

using OpenCL.NET.Memory;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromBitmapFLBuffer : SerializableFLBuffer, IBitmapBasedBuffer
    {

        public readonly int Size;

        public SerializableFromBitmapFLBuffer(string name, Bitmap bmp, FLBufferModifiers modifiers, int size) : base(
             name,
             modifiers
            )
        {
            Bitmap = bmp;
            Size = size;
        }

        public virtual Bitmap Bitmap { get; }


        public virtual Bitmap GetBitmap(int width, int height)
        {
            return new Bitmap(Bitmap, width, height);
        }

        public override FLBuffer GetBuffer()
        {
            MemoryFlag flag = Modifiers.IsReadOnly ? MemoryFlag.ReadOnly : MemoryFlag.ReadWrite;
            if (IsArray)
            {
                return new LazyLoadingFLBuffer(
                                               root =>
                                               {
                                                   FLBuffer buf = new FLBuffer(
                                                                               root.Instance,
                                                                               Bitmap,
                                                                               "BitmapBuffer." + Name,
                                                                               flag
                                                                              );
                                                   return buf;
                                               },
                                               Modifiers.InitializeOnStart
                                              );
            }

            return new LazyLoadingFLBuffer(
                                           root =>
                                           {
                                               Bitmap bmp = new Bitmap(Bitmap, root.Dimensions.x, root.Dimensions.y);
                                               FLBuffer buf = new FLBuffer(
                                                                           root.Instance,
                                                                           bmp,
                                                                           "BitmapBuffer." + Name,
                                                                           flag
                                                                          );
                                               bmp.Dispose();
                                               return buf;
                                           },
                                           Modifiers.InitializeOnStart
                                          );
        }

    }
}