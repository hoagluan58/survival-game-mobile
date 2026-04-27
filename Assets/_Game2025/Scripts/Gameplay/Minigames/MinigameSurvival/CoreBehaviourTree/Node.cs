using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public abstract class Node : MonoBehaviour
    {
        public NodeState nodeState;
        public bool started = false;
        [SerializeField] protected LogicNode parent;
        protected string nodeName;
        //public Action OnInterrupt;

        [Button]
        public void LoadComponents()
        {
            GetAllChildNode();
        }

        protected virtual void GetAllChildNode()
        {
            //to override
        }

        public NodeState UpdateNode()
        {

            if (!started)
            {
                OnStart();
                started = true;
            }

            nodeState = OnUpdate();

            if (nodeState != NodeState.RUNNING)
            {
                OnStop();
                started = false;
            }

            return nodeState;
        }

        public virtual void Init(LogicNode parent)
        {
            nodeName = transform.name;
            this.parent = parent;
        }

        protected virtual void StartNode()
        {
            if (transform.name != nodeName) return;
            transform.name = transform.name + " isRunning";
        }
        protected virtual void EndNode()
        {
            if (nodeState != NodeState.RUNNING) return;
            transform.name = nodeName;
        }
        public NodeState NodeState => nodeState;

        protected virtual void OnStart()
        {
            nodeState = NodeState.RUNNING;
        }
        protected virtual void OnStop()
        {

        }

        protected abstract NodeState OnUpdate();

        public virtual void Abort()
        {
            BehaviorTree.Traverse(this, (node) => {
                node.started = false;
                node.nodeState = NodeState.RUNNING;
                node.OnStop();
            });
        }

    }

    public enum NodeState
    {
        FAILURE, SUCCESS, RUNNING,
    }

}
