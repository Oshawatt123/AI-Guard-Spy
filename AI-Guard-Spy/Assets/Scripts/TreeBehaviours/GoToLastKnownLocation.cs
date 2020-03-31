using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class GoToLastKnownLocation : BT_Behaviour
{
    private Transform self;
    private guardTree localBB;
    private NavMeshAgent agent;
    public GoToLastKnownLocation(Transform _self)
    {
        self = _self;
        agent = self.GetComponent<NavMeshAgent>();
        localBB = self.GetComponent<guardTree>();
    }
    public override NodeState tick()
    {
        if(localBB.inCoolDown())
        {
            agent.destination = localBB.getLastKnownLocation();
            Debug.Log("LastKnownLocation SUCCESS");
            return NodeState.NODE_SUCCESS;
        }
        Debug.Log("LastKnownLocation FAILURE");
        return NodeState.NODE_FAILURE;
    }
}
