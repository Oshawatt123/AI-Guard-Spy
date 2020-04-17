using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class needSleep : BT_Behaviour
{
    private Transform self;
    private guardTree localBB;
    public needSleep(Transform _self)
    {
        self = _self;
        localBB = self.GetComponent<guardTree>();
    }

    public override NodeState tick()
    {
        if(localBB.needsSleep())
        {
            localBB.getSleep();
            nodeState = NodeState.NODE_SUCCESS;
            return NodeState.NODE_SUCCESS;
        }
        nodeState = NodeState.NODE_FAILURE;
        return NodeState.NODE_FAILURE;
    }
}