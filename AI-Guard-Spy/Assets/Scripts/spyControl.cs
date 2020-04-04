using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spyControl : MonoBehaviour
{
    public Transform[] targets;
    private int targetIndex = 0;
    NavMeshAgent agent;

    public float FOV;
    public float viewDistance;

    public GameObject guard;

    private bool spotted = false;

    private bool crackingSafe = false;
    public float safeCrackTime;
    private float safeCrackTimer = 0;
    public float safeCrackNoise;

    private bool hasTreasure = false;

    public float hidingTime;
    public Transform hidingSpot;
    public Transform exit;

    public bool guardHearsMe = false;
    public float guardHearingDistance = 0;
    public bool noisePathPending = true;

    private float health = 100;

    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = targets[targetIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        var path = agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }

        // if we have the treasure, beeline the exit
        if (hasTreasure)
        {
            Debug.Log("RUNNING RUNIING RUN- RUNNIG RUNING");
            agent.destination = exit.position;
            if(!agent.pathPending && agent.remainingDistance < 0.5)
            {
                // get next treasure
                targetIndex += 1;
                hasTreasure = false;
                agent.destination = targets[targetIndex].position;
            }
        }

        else if (!spotted)
        {
            Vector3 directionToGuard = guard.transform.position - transform.position;
            RaycastHit hit;

            // check if we see the guard
            if (Physics.Raycast(transform.position, directionToGuard, out hit, viewDistance, layerMask))
            {
                // if the ray hits the spy chase commenceth
                if (hit.transform.CompareTag("Guard"))
                {
                    Debug.Log("Spy sighted!");
                    spotted = true;
                    crackingSafe = false;
                }
            }

            // check if we are at the safe
            if(Vector3.Distance(transform.position, targets[targetIndex].position) < 1 && !crackingSafe)
            {
                crackingSafe = true;
                safeCrackTimer = safeCrackTime;
            }

            // check if we are cracking the safe
            if (crackingSafe)
            {
                agent.speed = 0;
                MakeNoise(safeCrackNoise);
                safeCrackTimer -= Time.deltaTime;
                if (safeCrackTimer < 0)
                {
                    crackingSafe = false;
                    hasTreasure = true;
                    agent.speed = 5;
                }
            }

        }

        // if we are spotted
        else
        {
            agent.speed = 5f;
            agent.destination = hidingSpot.position;
            if (!agent.pathPending && agent.remainingDistance < 0.5)
            {
                // we are at the hiding spot. hide for some time, and go back to what we were doing
                StartCoroutine(hide());
            }
        }        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Guard"))
        {
            health -= 1;
        }
    }

    private void MakeNoise(float amplitude)
    {
        // cache current destination
        //Vector3 currentDestination = agent.destination;

        // check if the guard can hear us
        agent.destination = guard.transform.position;

        var path = agent.path;


        guardHearingDistance = calculatePathLength(agent.path.corners);
        noisePathPending = agent.pathPending;

        if (!agent.pathPending && guardHearingDistance < amplitude)
        {
            guardHearsMe = true;
            guard.GetComponent<guardTree>().PlayerHeard();
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
            }
        }
        else
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.blue);
            }
        }

        // reset current pathfinding
        //agent.destination = currentDestination;
    }

    private float calculatePathLength(Vector3[] path)
    {
        float pathLength = 0;

        Vector3 prevPoint = new Vector3();
        Vector3 currPoint = new Vector3();

        for(int i = 0; i < path.Length; i++)
        {
            if(i == 0)
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

    IEnumerator hide()
    {
        Debug.Log("Hiding");
        yield return new WaitForSeconds(hidingTime);
        Debug.Log("Stopped hiding");
        agent.destination = targets[targetIndex].position;
        spotted = false;
    }
}
