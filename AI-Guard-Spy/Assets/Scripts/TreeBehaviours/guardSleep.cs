using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class guardSleep : BT_Behaviour
{
    private Transform self;
    private guardTree localBB;
    private bool startedSleeping = false;
    public guardSleep(Transform _self)
    {
        self = _self;
        localBB = self.GetComponent<guardTree>();
    }

    public override NodeState tick()
    {
        if(!startedSleeping)
        {
            localBB.startSleep();
            startedSleeping = true;
        }

        if (localBB.sleeping())
        {
            return NodeState.NODE_RUNNING;
        }
        localBB.endSleep();
        startedSleeping = false;
        return NodeState.NODE_SUCCESS;
    }
}