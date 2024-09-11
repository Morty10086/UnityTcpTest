using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace BehaviorTree
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    public class Node
    {
        protected NodeState state;
        public Node parent;
        protected List<Node> childrenNodes=new List<Node>();
        private Dictionary<string, object> dataDic = new();
        public Node() 
        {
            parent = null;
        }
        public Node(List<Node> children)
        {
            foreach(Node child in children)
            {
                this.AddChild(child);
            }
        }
        private void AddChild(Node child)
        {
            child.parent = this;
            this.childrenNodes.Add(child);
        }

        public virtual NodeState Evaluate() => NodeState.Failure;

        public void AddData(string key,object value)
        {
            if(!dataDic.ContainsKey(key))
            {
                dataDic.Add(key, value);
            }
        }
        public object GetData(string key)
        {
            object value = null;
            if(dataDic.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while(node != null)
            {
                value=node.GetData(key);
                if(value != null) 
                    return value;
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if(dataDic.ContainsKey(key))
            {
                dataDic.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
               bool cleared=node.ClearData(key);
                if(cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }
}

