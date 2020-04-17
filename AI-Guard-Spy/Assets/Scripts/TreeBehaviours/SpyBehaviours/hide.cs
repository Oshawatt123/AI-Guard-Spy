using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class hide : BT_Behaviour
{
    private float hideTime;
    private bool hiding;

    private Transform self;
    private spyTree localBB;

    public hide(Transform transform)
    {
        self = transform;
        localBB = self.GetComponent<spyTree>();
    }

    public override NodeState tick()
    {
        if(!hiding)
        {
            // start hiding
            localBB.startHiding();
            hiding = true;
        }

        if(localBB.Hiding())
        {
            nodeState = NodeState.NODE_RUNNING;
            return NodeState.NODE_RUNNING;
        }

        localBB.StopHiding();
        hiding = false;
        nodeState = NodeState.NODE_SUCCESS;
        return NodeState.NODE_SUCCESS;

    }

}
