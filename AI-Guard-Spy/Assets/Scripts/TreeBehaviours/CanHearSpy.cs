using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class CanHearSpy : BT_Behaviour
{
    private Transform self;
    private guardTree localBB;
    private NavMeshAgent agent;
    private float hearingSensitivity;
    private Vector3 currentDestination;

    public CanHearSpy(Transform _self)
    {
        self = _self;
        localBB = self.GetComponent<guardTree>();
        agent = self.GetComponent<NavMeshAgent>();
    }

    public override NodeState tick()
    {
        if(localBB.bPlayerHeard())
        {
            localBB.setMoveToLocation(GetPlayerPosition());
            localBB.resetHearing();
            return NodeState.NODE_SUCCESS;
        }

        return NodeState.NODE_FAILURE;
    }
}