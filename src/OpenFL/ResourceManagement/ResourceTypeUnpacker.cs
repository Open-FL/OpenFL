using System.IO;

namespace OpenFL.ResourceManagement
{
    public abstract class ResourceTypeUnpacker
    {

        public abstract string UnpackerName { get; }

        public abstract void Unpack(string targetDir, string name, Stream stream, IProgressIndicator progressIndicator);

    }
}