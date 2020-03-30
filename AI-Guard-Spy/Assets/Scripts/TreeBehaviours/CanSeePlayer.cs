using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CanSeePlayer : BT_Behaviour
{
    private Transform self;
    public CanSeePlayer(Transform guardTransform)
    {
        self = guardTransform;
    }
    public override NodeState tick()
    {
        Vector3 directionToPlayer = GetPlayerPosition() - self.position;
        RaycastHit hit;
        Physics.Raycast(self.position, directionToPlayer, out hit, 10);

        if(hit.transform.CompareTag("Spy"))
        {
            return NodeState.NODE_SUCCESS;
        }

        return NodeState.NODE_FAILURE;
    }
}