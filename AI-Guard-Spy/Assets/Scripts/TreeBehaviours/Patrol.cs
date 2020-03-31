﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class Patrol : BT_Behaviour
{
    int destPoint = 0;
    public Transform[] path;
    private NavMeshAgent agent;

    private Transform self;

    public Patrol(Transform self)
    {
        agent = self.GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        path = self.GetComponent<guardTree>().getPath();
    }
    void GoToNextPoint()
    {
        if (path.Length == 0)
        {
            return;
        }

        agent.destination = path[destPoint].position;

        // loop through the points
        Debug.Log("Finding next point");
        destPoint = (destPoint + 1) % path.Length;
    }

    public override NodeState tick()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Debug.Log("Agent Met goal");
            GoToNextPoint();
        }
        Debug.Log("Patrol SUCCESS");
        return NodeState.NODE_SUCCESS;
    }
}
