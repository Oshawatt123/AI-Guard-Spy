using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spyControl : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    public float FOV;
    public float viewDistance;

    public GameObject guard;

    private bool spotted = false;

    public Transform hidingSpot;

    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!spotted)
        {
            Vector3 directionToGuard = guard.transform.position - transform.position;
            RaycastHit hit;
            Physics.Raycast(transform.position, directionToGuard, out hit, viewDistance, layerMask);

            // if the ray hits the spy chase commenceth
            if (hit.transform.CompareTag("Guard"))
            {
                Debug.Log("Spy sighted!");
                spotted = true;
            }
        }
        else
        {
            agent.destination = hidingSpot.position;
        }
    }

    private void OnDrawGizmos()
    {

    }
}
