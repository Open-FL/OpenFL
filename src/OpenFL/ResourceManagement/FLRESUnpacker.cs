using System.IO;

using Utility.ProgressFeedback;

namespace OpenFL.ResourceManagement
{
    public class FLRESUnpacker : ResourceTypeUnpacker
    {

        public override string UnpackerName => "flres";

        public override void Unpack(string targetDir, string name, Stream stream, IProgressIndicator progressIndicator)
        {
            progressIndicator.SetProgress($"[{UnpackerName}]Preparing Target Directory...", 1, 2);
            string filePath = Path.Combine(
                                           targetDir,
                                           name.Replace("/", "\\").StartsWith("\\")
                                               ? name.Replace("/", "\\").Substring(1)
                                               : name.Replace("/", "\\")
                                          );
            Directory.CreateDirectory(filePath);
            progressIndicator.SetProgress($"[{UnpackerName}]Unpacking: {name}", 2, 2);

            string packname = ResourceManager.Load(name);
            ResourceManager.Activate(packname, progressIndicator.CreateSubTask(), filePath);
            progressIndicator.Dispose();
        }

    }
}