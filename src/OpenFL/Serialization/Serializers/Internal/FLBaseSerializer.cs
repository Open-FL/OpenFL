using System.Collections.Generic;
using System.Linq;

namespace OpenFL.Serialization.Serializers.Internal
{
    public abstract class FLBaseSerializer : ASerializer
    {

        protected List<string> idMap { get; private set; }

        public virtual void SetIdMap(string[] map)
        {
            idMap = map.ToList();
        }

        protected string ResolveId(int id)
        {
            return idMap[id];
        }

        protected int ResolveName(string name)
        {
            return idMap.IndexOf(name);
        }

    }
}