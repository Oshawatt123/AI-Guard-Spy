using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class hideLoot : BT_Behaviour
{
    private spyTree localBB;
    public hideLoot(Transform transform)
    {
        localBB = transform.GetComponent<spyTree>();
    }

    public override NodeState tick()
    {
        localBB.targetIndex += 1;
        localBB.hasTreasure = false;
        nodeState = NodeState.NODE_SUCCESS;
        return NodeState.NODE_SUCCESS;
    }
}