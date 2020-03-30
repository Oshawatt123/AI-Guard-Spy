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

    public class BT_Node
    {
        public Vector3 GetPlayerPosition() { return GameObject.FindGameObjectWithTag("Spy").transform.position; }

        public virtual NodeState tick()
        {
            return NodeState.NODE_FAILURE;
        }

        public virtual void Test()
        {
            Debug.Log("BT_Node Test");
        }
    }

    public class BT_Selector : BT_Node
    {
        List<BT_Node> selectorNodes = new List<BT_Node>();
        NodeState childNodeReturnValue;
        int currentNodeIndex = 0;
        public void AddNode(BT_Node node)
        {
            selectorNodes.Add(node);
        }
        public override NodeState tick()
        {
            // tick the node we are on
            childNodeReturnValue = selectorNodes[currentNodeIndex].tick();

            // if they're running still, then we say we're still running
            if (childNodeReturnValue == NodeState.NODE_RUNNING)
                return NodeState.NODE_RUNNING;

            // if the node fails, move on to the next node if available
            if(childNodeReturnValue == NodeState.NODE_FAILURE)
            {
                currentNodeIndex += 1;
                if (currentNodeIndex > selectorNodes.Count)
                    return NodeState.NODE_FAILURE;

                currentNodeIndex = 0;
                return NodeState.NODE_RUNNING;
            }

            // at this point, we know that a node has succeeded so we return a success
            currentNodeIndex = 0;
            return NodeState.NODE_SUCCESS;
        }

        public override void Test()
        {
            Debug.Log("BT_Selector Test");
        }
    }

    public class BT_Sequencer : BT_Node
    {
        List<BT_Node> sequencerNodes = new List<BT_Node>();
        NodeState childNodeReturnValue;
        int currentNodeIndex = 0;
        public void AddNode(BT_Node node)
        {
            sequencerNodes.Add(node);
        }
        public override NodeState tick()
        {
            // tick the node we are on
            childNodeReturnValue = sequencerNodes[currentNodeIndex].tick();

            // if they're running still, then we say we're still running
            if (childNodeReturnValue == NodeState.NODE_RUNNING)
                return NodeState.NODE_RUNNING;

            // if they succeed, we need to see if all have succeeded
            if (childNodeReturnValue == NodeState.NODE_SUCCESS)
            {
                currentNodeIndex += 1;
                // all nodes have returned success, so we return success
                if (currentNodeIndex >= sequencerNodes.Count)
                {
                    return NodeState.NODE_SUCCESS;
                }
                // if we've got more to process we continue running
                return NodeState.NODE_RUNNING;
            }

            // if anything fails, we fail immediately
            return NodeState.NODE_FAILURE;
        }
    }
    public class BT_Behaviour : BT_Node
    {

        public override NodeState tick()
        {
            Debug.Log("No behaviour defined, failure inferred");
            return NodeState.NODE_FAILURE;
        }
        public override void Test()
        {
            Debug.Log("BT_Behaviour Test");
        }
    }

    public class BT_Tree
    {
        List<BT_Node> tree = new List<BT_Node>();

        public void AddNode(BT_Node node)
        {
            tree.Add(node);
        }

        public void Tick()
        {
            foreach(BT_Node node in tree)
            {
                node.tick();
            }
        }
    }
}