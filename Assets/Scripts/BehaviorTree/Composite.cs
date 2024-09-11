using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence():base() { }
        public Sequence(List<Node> children):base(children) { }
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            foreach(Node node in childrenNodes)
            {
                switch(node.Evaluate())
                {
                    case NodeState.Failure:
                        state=NodeState.Failure;
                        return state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        anyChildIsRunning=true;
                        continue;
                    default:
                        state = NodeState.Success;
                        return state;
                }                
            }
            state = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            return state;
        }
    }

    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }
        public override NodeState Evaluate()
        {
            foreach (Node node in childrenNodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        state = NodeState.Success;
                        return state;
                    case NodeState.Running:
                        state = NodeState.Running;
                        return state;
                    default:
                        continue;
                }
            }
            state = NodeState.Failure;
            return state;
        }
    }
}

