using System.Drawing;

using OpenCL.Memory;

using OpenFL.Core.ElementModifiers;

using Utility.IO.Callbacks;

using Image = System.Drawing.Image;

namespace OpenFL.Core.Buffers
{
    public class LazyFromFileFLBuffer : LazyLoadingFLBuffer
    {

        public readonly string File;


        public LazyFromFileFLBuffer(string file, bool isArray, int size, FLBufferModifiers modifiers) : base(
                                                                                                             null,
                                                                                                             modifiers
                                                                                                                 .InitializeOnStart
                                                                                                            )
        {
            File = file;
            MemoryFlag flag = modifiers.IsReadOnly ? MemoryFlag.ReadOnly : MemoryFlag.ReadWrite;
            if (isArray)
            {
                Loader = root =>
                         {
                             Bitmap bmp = new Bitmap(Image.FromStream(IOManager.GetStream(File)));
                             FLBuffer buf = new FLBuffer(root.Instance, bmp, DefinedBufferName + ":" + File, flag);
                             bmp.Dispose();
                             return buf;
                         };
            }
            else
            {
                Loader = root =>
                         {
                             if (File == "INPUT")
                             {
                                 return root.Input;
                             }

                             Bitmap bmp = new Bitmap(
                                                     Image.FromStream(IOManager.GetStream(File)),
                                                     root.Dimensions.x,
                                                     root.Dimensions.y
                                                    );
                             FLBuffer buf = new FLBuffer(root.Instance, bmp, DefinedBufferName + ":" + File, flag);
                             bmp.Dispose();
                             return buf;
                         };
            }
        }

    }
}