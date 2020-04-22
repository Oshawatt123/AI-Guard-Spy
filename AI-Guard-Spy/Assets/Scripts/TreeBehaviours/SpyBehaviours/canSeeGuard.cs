using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CanSeeGuard : BT_Behaviour
{
    private Transform self;
    private spyTree localBB;

    public CanSeeGuard(Transform transform)
    {
        self = transform;
        localBB = self.GetComponent<spyTree>();
    }
    public override NodeState tick()
    {
        // for every guard
        List<Transform> guards = localBB.guards;

        foreach(Transform guard in guards)
        {
            RaycastHit hit;
            Vector3 directionToGuard = guard.position - self.position;


            // if the raycast hits anything
            if (Physics.Raycast(self.position, directionToGuard, out hit, 15))
            {
                // if we see the spy
                if (hit.transform.CompareTag("Guard"))
                {
                    // go to hiding place
                    localBB.setGoToHidingSpot();
                    nodeState = NodeState.NODE_SUCCESS;
                    return NodeState.NODE_SUCCESS;
                }
            }
        }
        nodeState = NodeState.NODE_FAILURE;
        return NodeState.NODE_FAILURE;
    }
}