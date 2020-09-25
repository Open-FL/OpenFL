using OpenFL.Core.Buffers;

namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public class FLExecutionContext
    {

        public FLExecutionContext(byte[] activeChannels, FLBuffer activeBuffer)
        {
            ActiveChannels = activeChannels;
            ActiveBuffer = activeBuffer;
        }

        public byte[] ActiveChannels { get; }

        public FLBuffer ActiveBuffer { get; }

    }
}