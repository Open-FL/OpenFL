using OpenFL.Core.ElementModifiers;

namespace OpenFL.Core.DataObjects.SerializableDataObjects
{
    public class SerializableExternalFLFunction : SerializableNamedObject
    {

        public SerializableExternalFLFunction(
            string name, SerializableFLProgram externalProgram,
            FLExecutableElementModifiers mod) : base(name)
        {
            ExternalProgram = externalProgram;
            Modifiers = mod;
        }

        public SerializableFLProgram ExternalProgram { get; }

        public FLExecutableElementModifiers Modifiers { get; }


        public override string ToString()
        {
            return $"{FLKeywords.DefineScriptKey} " + Name + ": " + ExternalProgram.FileName;
        }

    }
}