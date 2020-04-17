using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class ChaseSpy : BT_Behaviour
{
    private NavMeshAgent agent;

    private Transform self;

    public ChaseSpy(Transform self)
    {
        agent = self.GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    public override NodeState tick()
    {
        agent.destination = GetPlayerPosition();

        Debug.Log("ChaseSpy SUCCESS");
        nodeState = NodeState.NODE_SUCCESS;
        return NodeState.NODE_SUCCESS;
    }
}
