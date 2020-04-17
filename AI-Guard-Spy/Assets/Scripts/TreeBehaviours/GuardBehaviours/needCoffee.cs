using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class needCoffee : BT_Behaviour
{
    private Transform self;
    private guardTree localBB;
    public needCoffee(Transform _self)
    {
        self = _self;
        localBB = self.GetComponent<guardTree>();
    }

    public override NodeState tick()
    {
        if(localBB.needsCoffee())
        {
            localBB.getCoffee();
            nodeState = NodeState.NODE_SUCCESS;
            return NodeState.NODE_SUCCESS;
        }
        nodeState = NodeState.NODE_FAILURE;
        return NodeState.NODE_FAILURE;
    }
}
