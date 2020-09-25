using System.IO;

using Utility.ProgressFeedback;

namespace OpenFL.ResourceManagement
{
    public class DefaultUnpacker : ResourceTypeUnpacker
    {

        public override string UnpackerName => "default";

        public override void Unpack(string targetDir, string name, Stream stream, IProgressIndicator progressIndicator)
        {
            progressIndicator.SetProgress($"[{UnpackerName}]Preparing Target Directory...", 1, 2);
            string filePath = Path.Combine(
                                           targetDir,
                                           name.Replace("/", "\\").StartsWith("\\")
                                               ? name.Replace("/", "\\").Substring(1)
                                               : name.Replace("/", "\\")
                                          );
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            Stream s = File.OpenWrite(filePath);
            progressIndicator.SetProgress($"[{UnpackerName}]Unpacking: {name}", 2, 2);
            stream.CopyTo(s);
            s.Close();
            stream.Close();
            progressIndicator.Dispose();
        }

    }
}