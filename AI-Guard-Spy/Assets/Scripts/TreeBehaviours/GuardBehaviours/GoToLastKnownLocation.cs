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

            var path = agent.path;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }

            Debug.Log("LastKnownLocation SUCCESS");
            nodeState = NodeState.NODE_SUCCESS;
            return NodeState.NODE_SUCCESS;
        }
        Debug.Log("LastKnownLocation FAILURE");
        nodeState = NodeState.NODE_FAILURE;
        return NodeState.NODE_FAILURE;
    }
}
