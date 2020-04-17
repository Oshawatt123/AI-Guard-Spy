using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class GoToPoint : BT_Behaviour
{
    private Transform self;
    private localTree localBB;
    private NavMeshAgent agent;

    public GoToPoint(Transform _self)
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

        if (!agent.pathPending && agent.remainingDistance < 1.0f)
        {
            nodeState = NodeState.NODE_SUCCESS;
            return NodeState.NODE_SUCCESS;
        }
        nodeState = NodeState.NODE_RUNNING;
        return NodeState.NODE_RUNNING;
    }
}