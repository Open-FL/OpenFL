using OpenFL.Core.DataObjects.ExecutableDataObjects;

namespace OpenFL.Core.Buffers
{
    public interface IWarmable : IParsedObject
    {

        void Warm(bool force);

    }
}