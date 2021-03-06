﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class GoToPointOneTick : BT_Behaviour
{
    private Transform self;
    private localTree localBB;
    private NavMeshAgent agent;

    public GoToPointOneTick(Transform _self)
    {
        self = _self;
        localBB = self.GetComponent<localTree>();
        agent = self.GetComponent<NavMeshAgent>();
    }

    public override NodeState tick()
    {
        agent.destination = localBB.getMoveToLocation();
        //Debug.Log("GoToPoint : " + agent.destination);
        var path = agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.blue);
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            return NodeState.NODE_SUCCESS;
        }
        return NodeState.NODE_SUCCESS;
    }
}