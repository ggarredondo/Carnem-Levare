using System;

namespace TreeUtilities
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class NodeRelevance : Attribute
    {
        public Type[] RelevantBehaviourTrees { get; }

        public NodeRelevance(params Type[] relevantBehaviourTrees)
        {
            RelevantBehaviourTrees = relevantBehaviourTrees;
        }
    }
}
