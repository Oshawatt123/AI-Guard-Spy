using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class guardTree : MonoBehaviour
{
    private BT_Tree tree = new BT_Tree();

    // local blackboard variables
    public Transform[] path;
    public float chaseCoolDown;
    private float chaseCoolDowntimer = 0;
    private Vector3 lastKnownLocation;

    // Start is called before the first frame update
    void Start()
    {
        CanSeePlayer CSP = new CanSeePlayer(transform);
        ChaseSpy CS = new ChaseSpy(transform);
        BT_Sequencer seq = new BT_Sequencer();
        seq.AddNode(CSP);
        seq.AddNode(CS);

        BT_Selector sel = new BT_Selector();
        sel.AddNode(seq);
        sel.AddNode(new GoToLastKnownLocation(transform));

        BT_Selector rootSelector = new BT_Selector();
        rootSelector.AddNode(sel);
        rootSelector.AddNode(new Patrol(transform));

        tree.AddNode(rootSelector);
    }

    // Update is called once per frame
    void Update()
    {
        tree.Tick();

        chaseCoolDowntimer -= Time.deltaTime;
    }

    // local blackboard methods

    public Transform[] getPath()
    {
        return path;
    }
    public void LostSight(Vector3 spyLastKnowLocation)
    {
        Debug.Log("Lost sight of spy");
        lastKnownLocation = spyLastKnowLocation;
        chaseCoolDowntimer = chaseCoolDown;
    }

    public bool inCoolDown()
    {
        return chaseCoolDowntimer > 0;
    }

    public Vector3 getLastKnownLocation()
    {
        return lastKnownLocation;
    }
}