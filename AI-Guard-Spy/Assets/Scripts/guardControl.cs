using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class guardControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] path;
    NavMeshAgent agent;
    private int destPoint = 0;

    private enum State
    {
        Patrolling,
        Chasing,
        Searching
    }

    private State currentState = State.Patrolling;

    private GameObject spy;
    private Vector3 spyLastLocation;

    public float FOV;
    public float viewDistance;

    public LayerMask layerMask;
    void Start()
    {
        spy = GameObject.FindGameObjectWithTag("Spy");
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        GoToNextPoint();
    }

    void GoToNextPoint()
    {
        if(path.Length == 0)
        {
            return;
        }

        agent.destination = path[destPoint].position;

        // loop through the points
        destPoint = (destPoint + 1) % path.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToPlayer = spy.transform.position - transform.position;
        RaycastHit hit;
        Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance, layerMask);

        if (currentState == State.Patrolling)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Debug.Log("Agent Met goal");
                GoToNextPoint();
            }

            // if the ray hits the spy chase commenceth
            if (hit.transform.CompareTag("Spy"))
            {
                Debug.Log("Spy sighted!");
                currentState = State.Chasing;
            }
        }
        else if (currentState == State.Chasing)
        {
            // if the ray hits keep chasing
            if (hit.transform.CompareTag("Spy"))
            {
                agent.destination = spy.transform.position;
            }
            else
            {
                currentState = State.Searching;
                getSpyLastLocation();
            }
        }
        else if (currentState == State.Searching)
        {
            agent.destination = spyLastLocation;

            // once we are at the last known location, have a look around
            if(!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Invoke("SearchArea", 3f);
            }
        }
    }

    private void getSpyLastLocation()
    {
        spyLastLocation = spy.transform.position;
    }

    private void SearchArea()
    {
        currentState = State.Patrolling;
    }

    private void OnDrawGizmos()
    {
        if(currentState == State.Chasing)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.magenta;
        }
        Gizmos.DrawLine(transform.position, spy.transform.position);
    }
}
