using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {        
        private Node root=null;
        protected virtual void Start()
        {
            root = SetUpTree();
        }
        protected virtual void Update()
        {
            if(root != null) 
            {
                root.Evaluate();
            }
        }
        protected abstract Node SetUpTree();
    }
}

