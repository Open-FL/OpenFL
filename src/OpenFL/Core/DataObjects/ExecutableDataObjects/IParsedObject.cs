namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public interface IParsedObject
    {

        FLProgram Root { get; }

        void SetRoot(FLProgram root);

    }
}