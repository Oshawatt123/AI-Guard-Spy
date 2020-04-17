using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class hasLoot : BT_Behaviour
{
    private spyTree localBB;
    public hasLoot(Transform transform)
    {
        localBB = transform.GetComponent<spyTree>();
    }

    public override NodeState tick()
    {
        if(localBB.hasTreasure)
        {
            localBB.setMoveToLocation(localBB.exit.position);
            nodeState = NodeState.NODE_SUCCESS;
            return NodeState.NODE_SUCCESS;
        }
        nodeState = NodeState.NODE_FAILURE;
        return NodeState.NODE_FAILURE;
    }
}