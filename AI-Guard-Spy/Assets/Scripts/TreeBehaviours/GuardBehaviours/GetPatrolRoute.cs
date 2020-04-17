using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GetPatrolRoute : BT_Behaviour
{
    int destPoint = 0;
    public List<Transform> path = new List<Transform>();

    private Transform self;

    public GetPatrolRoute(Transform _self)
    {
        self = _self;
        path = self.GetComponent<guardTree>().getPath();
    }
    void GoToNextPoint()
    {
        if (path.Count == 0)
        {
            return;
        }

        // loop through the points
        Debug.Log("Finding next point");
        destPoint = (destPoint + 1) % path.Count;
        self.GetComponent<guardTree>().setMoveToLocation(path[destPoint].position);
    }

    public override NodeState tick()
    {
        GoToNextPoint();
        nodeState = NodeState.NODE_SUCCESS;
        return NodeState.NODE_SUCCESS;
    }
}