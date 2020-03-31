using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CanSeePlayer : BT_Behaviour
{
    private Transform self;
    private bool sawLastFrame;

    public CanSeePlayer(Transform guardTransform)
    {
        self = guardTransform;
    }
    public override NodeState tick()
    {
        Vector3 directionToPlayer = GetPlayerPosition() - self.position;
        RaycastHit hit;

        // if the raycast hits anything
        if(Physics.Raycast(self.position, directionToPlayer, out hit, 10))
        {
            // if we see the spy
            if (hit.transform.CompareTag("Spy"))
            {
                sawLastFrame = true;
                Debug.Log("CanSeePlayer SUCCESS");
                return NodeState.NODE_SUCCESS;
            }
        }

        // if we dont see the spy, have we just lost sight?
        if (sawLastFrame)
        {
            // start the timer on the local blackboard
            self.GetComponent<guardTree>().LostSight(GetPlayerPosition());
            sawLastFrame = false;
        }

        Debug.Log("CanSeePlayer FAILURE");
        return NodeState.NODE_FAILURE;
    }
}