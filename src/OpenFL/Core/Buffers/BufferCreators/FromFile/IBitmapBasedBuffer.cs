using System.Drawing;

namespace OpenFL.Core.Buffers.BufferCreators.BuiltIn.FromFile
{
    public interface IBitmapBasedBuffer
    {

        Bitmap GetBitmap(int width, int height);

    }
}