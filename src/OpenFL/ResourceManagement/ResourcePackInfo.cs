using System.Xml.Serialization;

namespace OpenFL.ResourceManagement
{
    public class ResourcePackInfo
    {

        public string Name;

        [XmlIgnore]
        public string ResourceData;

        public string UnpackerConfig;

        public override string ToString()
        {
            return $"Name: {Name}, Config:{UnpackerConfig}";
        }

    }
}