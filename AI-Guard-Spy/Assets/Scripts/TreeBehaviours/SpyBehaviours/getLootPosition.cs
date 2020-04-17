using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class getLootPosition : BT_Behaviour
{
    private List<Transform> targets = new List<Transform>();
    private spyTree localbb;

    public getLootPosition(Transform self)
    {
        localbb = self.GetComponent<spyTree>();
        targets = localbb.targets;
    }

    public override NodeState tick()
    {
        Debug.LogError("Getting loot position");
        Debug.Log(localbb.targetIndex);

        if(!localbb.hasTreasure)
        {
            localbb.setMoveToLocation(targets[localbb.targetIndex].position);
            nodeState = NodeState.NODE_SUCCESS;
            return NodeState.NODE_SUCCESS;
        }
        nodeState = NodeState.NODE_FAILURE;
        return NodeState.NODE_FAILURE;
    }
}