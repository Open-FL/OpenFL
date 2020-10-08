using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public interface IFunction : IParsedObject
    {

        string Name { get; }

        FLExecutableElementModifiers Modifiers { get; }

        void Process();

    }
}