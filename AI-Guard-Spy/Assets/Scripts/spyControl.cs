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
            Debug.Log("Distance to guard: " + Vector3.Distance(transform.position, guard.transform.position));
            if (Vector3.Distance(transform.position, guard.transform.position) < viewDistance && Vector3.Angle(transform.position, guard.transform.position) < FOV)
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
        Gizmos.color = Color.red;
        float y;
        y = viewDistance / Mathf.Tan(((180 - FOV) / 2) * Mathf.PI / 180);
        Vector3 drawPoint = new Vector3(transform.position.x + y, transform.position.y, transform.position.z + viewDistance);
        Gizmos.DrawLine(transform.position, drawPoint);
        Vector3 drawPoint2 = new Vector3(transform.position.x - y, transform.position.y, transform.position.z + viewDistance);
        Gizmos.DrawLine(transform.position, drawPoint2);
    }
}
