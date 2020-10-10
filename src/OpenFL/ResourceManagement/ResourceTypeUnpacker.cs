using System.IO;

using Utility.ProgressFeedback;

namespace OpenFL.ResourceManagement
{
    public abstract class ResourceTypeUnpacker
    {

        public abstract string UnpackerName { get; }

        public abstract void Unpack(string targetDir, string name, Stream stream, IProgressIndicator progressIndicator);

    }
}