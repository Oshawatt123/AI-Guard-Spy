using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState
    {
        NODE_FAILURE,
        NODE_SUCCESS,
        NODE_RUNNING,
    }


    public enum NodeType
    {
        NODE_BEHAVIOUR,
        NODE_SELECTOR,
        NODE_SEQUENCER,
        NODE_UNDEFINED
    }
    public class BT_Node
    {
        public NodeType nodeType = NodeType.NODE_UNDEFINED;
        public NodeState nodeState = NodeState.NODE_FAILURE;
        public List<BT_Node> childNodes = new List<BT_Node>();
        public Vector3 GetPlayerPosition() { return GameObject.FindGameObjectWithTag("Spy").transform.position; }

        public virtual NodeState tick()
        {
            nodeState = NodeState.NODE_FAILURE;
            return NodeState.NODE_FAILURE;
        }

        public virtual void Test()
        {
            Debug.Log("BT_Node Test");
        }
    }

    public class BT_Selector : BT_Node
    {        
        NodeState childNodeReturnValue;

        int currentNodeIndex = 0;

        public BT_Selector()
        {
            nodeType = NodeType.NODE_SELECTOR;
        }

        public void AddNode(BT_Node node)
        {
            childNodes.Add(node);
        }
        public override NodeState tick()
        {
            // tick the node we are on
            Debug.Log("Selector child index : " + currentNodeIndex);
            childNodeReturnValue = childNodes[currentNodeIndex].tick();

            // if they're running still, then we say we're still running
            if (childNodeReturnValue == NodeState.NODE_RUNNING)
            {
                Debug.Log("Selector RUNNING");
                currentNodeIndex = 0;
                nodeState = NodeState.NODE_RUNNING;
                return NodeState.NODE_RUNNING;
            }

            // if the node fails, move on to the next node if available
            if(childNodeReturnValue == NodeState.NODE_FAILURE)
            {
                // keep on going
                currentNodeIndex += 1;

                // until we hit the end
                if (currentNodeIndex > childNodes.Count - 1)
                {
                    currentNodeIndex = 0;
                    Debug.Log("Selector FAILURE");
                    nodeState = NodeState.NODE_FAILURE;
                    return NodeState.NODE_FAILURE;
                }

                Debug.Log("Selector RUNNING");
                nodeState = NodeState.NODE_RUNNING;
                return NodeState.NODE_RUNNING;
            }

            // at this point, we know that a node has succeeded so we return a success
            currentNodeIndex = 0;
            Debug.Log("Selector SUCCESS");
            nodeState = NodeState.NODE_SUCCESS;
            return NodeState.NODE_SUCCESS;
        }

        public override void Test()
        {
            Debug.Log("BT_Selector Test");
        }
    }

    public class BT_Sequencer : BT_Node
    {
        NodeState childNodeReturnValue;
        int currentNodeIndex = 0;

        public BT_Sequencer()
        {
            nodeType = NodeType.NODE_SEQUENCER;
        }

        public void AddNode(BT_Node node)
        {
            childNodes.Add(node);
        }
        public override NodeState tick()
        {
            // tick the node we are on
            childNodeReturnValue = childNodes[currentNodeIndex].tick();

            // if they're running still, then we say we're still running
            if (childNodeReturnValue == NodeState.NODE_RUNNING)
            {
                Debug.Log("Sequencer RUNNING");
                nodeState = NodeState.NODE_RUNNING;
                return NodeState.NODE_RUNNING;
            }

            // if they succeed, we need to see if all have succeeded
            if (childNodeReturnValue == NodeState.NODE_SUCCESS)
            {
                currentNodeIndex += 1;
                // all nodes have returned success, so we return success
                if (currentNodeIndex >= childNodes.Count)
                {
                    currentNodeIndex = 0;
                    Debug.Log("Sequencer SUCCESS");
                    nodeState = NodeState.NODE_SUCCESS;
                    return NodeState.NODE_SUCCESS;
                }
                // if we've got more to process we continue running
                Debug.Log("Sequencer RUNNING");
                nodeState = NodeState.NODE_RUNNING;
                return NodeState.NODE_RUNNING;
            }

            // if anything fails, we fail immediately
            Debug.Log("Sequencer FAILURE");
            nodeState = NodeState.NODE_FAILURE;
            return NodeState.NODE_FAILURE;
        }
    }
    public class BT_Behaviour : BT_Node
    {
        public BT_Behaviour()
        {
            nodeType = NodeType.NODE_BEHAVIOUR;
        }
        public override NodeState tick()
        {
            Debug.Log("No behaviour defined, failure inferred");
            nodeState = NodeState.NODE_FAILURE;
            return NodeState.NODE_FAILURE;
        }
        public override void Test()
        {
            Debug.Log("BT_Behaviour Test");
        }
    }

    public class BT_Tree
    {
        BT_Node root;

        public void SetRoot(BT_Node node)
        {
            root = node;
        }

        public BT_Node getRoot()
        {
            return root;
        }

        public void Tick()
        {
            root.tick();
        }
    }
}