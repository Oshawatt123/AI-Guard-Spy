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
        Chasing
    }

    private State currentState = State.Patrolling;

    private GameObject spy;

    public float FOV;
    public float viewDistance;
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
        if(currentState == State.Patrolling)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Debug.Log("Agent Met goal");
                GoToNextPoint();
            }

            if (Vector3.Distance(transform.position, spy.transform.position) < viewDistance && Vector3.Angle(transform.position, spy.transform.position) < FOV)
            {
                Debug.Log("Spy sighted!");
                currentState = State.Chasing;
            }
        }
        else if (currentState == State.Chasing)
        {
            agent.destination = spy.transform.position;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float y;
        y = viewDistance / Mathf.Tan(((180 - FOV) / 2)* Mathf.PI/180);
        Vector3 drawPoint = new Vector3(transform.position.x + y, transform.position.y, transform.position.z + viewDistance);
        Gizmos.DrawLine(transform.position, drawPoint);
        Vector3 drawPoint2 = new Vector3(transform.position.x - y, transform.position.y, transform.position.z + viewDistance);
        Gizmos.DrawLine(transform.position, drawPoint2);
    }
}
