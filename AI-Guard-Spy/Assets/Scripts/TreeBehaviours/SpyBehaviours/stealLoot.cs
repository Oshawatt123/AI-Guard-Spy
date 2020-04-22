using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;

public class stealLoot : BT_Behaviour
{
    private spyTree localBB;
    private NavMeshAgent agent;
    private List<Transform> guards = new List<Transform>();
    private bool cracking;

    private float amplitude = 5f;

    public stealLoot(Transform self)
    {
        localBB = self.GetComponent<spyTree>();
        guards = localBB.guards;

        agent = self.GetComponent<NavMeshAgent>();
    }

    public override NodeState tick()
    {
        if(!cracking)
        {
            if (!localBB.crackingSafe())
            {
                localBB.startCrackingSafe();
                cracking = true;
            }
        }

        if(localBB.crackingSafe())
        {
            if(MakeNoise(amplitude))
            {
                nodeState = NodeState.NODE_FAILURE;
                return NodeState.NODE_FAILURE;
            }
            nodeState = NodeState.NODE_RUNNING;
            return NodeState.NODE_RUNNING;
        }

        localBB.crackSafe();
        cracking = false;
        nodeState = NodeState.NODE_SUCCESS;
        return NodeState.NODE_SUCCESS;
    }


    private bool MakeNoise(float amplitude)
    {
        // cache current destination
        Vector3 currentDestination = agent.destination;

        // check if the guard can hear us
        foreach (Transform guard in guards)
        {
            agent.destination = guard.position;

            var path = agent.path;


            float guardHearingDistance = calculatePathLength(agent.path.corners);
            bool noisePathPending = agent.pathPending;

            if (!agent.pathPending && guardHearingDistance < amplitude)
            {
                guard.GetComponent<guardTree>().PlayerHeard();
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
                }
                agent.destination = currentDestination;
                return true;
            }
            else
            {
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.blue);
                }
            }
        }

        agent.destination = currentDestination;
        return false;
        // reset current pathfinding
    }

    private float calculatePathLength(Vector3[] path)
    {
        float pathLength = 0;

        Vector3 prevPoint = new Vector3();
        Vector3 currPoint = new Vector3();

        for (int i = 0; i < path.Length; i++)
        {
            if (i == 0)
            {
                prevPoint = path[0];
            }
            else
            {
                currPoint = path[i];
                pathLength += Vector3.Distance(prevPoint, currPoint);

                currPoint = prevPoint;
            }
        }


        return pathLength;
    }
}
