namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public interface IFunction : IParsedObject
    {

        string Name { get; }

        void Process();

    }
}