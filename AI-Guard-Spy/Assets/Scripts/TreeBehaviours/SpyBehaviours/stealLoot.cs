using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class stealLoot : BT_Behaviour
{
    private spyTree localBB;
    private bool cracking;

    public stealLoot(Transform self)
    {
        localBB = self.GetComponent<spyTree>();
    }

    public override NodeState tick()
    {
        if(!cracking)
        {
            if (!localBB.crackingSafe())
            {
                localBB.startCrackingSafe();
                cracking = true;
            }
        }

        if(localBB.crackingSafe())
        {
            nodeState = NodeState.NODE_RUNNING;
            return NodeState.NODE_RUNNING;
        }

        localBB.crackSafe();
        cracking = false;
        nodeState = NodeState.NODE_SUCCESS;
        return NodeState.NODE_SUCCESS;
    }
}
