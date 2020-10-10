namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public abstract class FLParsedObject : ALoggable<LogType>, IParsedObject
    {

        protected FLParsedObject(string name) : base(OpenFLDebugConfig.Settings, name)
        {
        }

        public FLProgram Root { get; private set; }

        public virtual void SetRoot(FLProgram root)
        {
            Root = root;
        }

    }
}