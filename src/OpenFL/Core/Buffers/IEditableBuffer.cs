namespace OpenFL.Core.Buffers
{
    public interface IEditableBuffer
    {

        int DataSize { get; }

        void SetData(byte[] data);

        byte[] GetData();

    }
}