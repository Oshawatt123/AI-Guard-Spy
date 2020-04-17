using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class spyTree : localTree
{
    public List<Transform> guards = new List<Transform>();

    public List<Transform> targets = new List<Transform>();
    public int targetIndex;

    public Transform[] hidingSpots;
    public Transform exit;

    public float crackSafeTime;
    private float crackSafeTimer;

    public bool hasTreasure;

    public float hideTime;
    private float hideTimer;

    public bool win = false;

    // Start is called before the first frame update
    void Start()
    {
        BT_Selector rootSel = new BT_Selector();

        BT_Sequencer seq1 = new BT_Sequencer();
        seq1.AddNode(new CanSeeGuard(transform));
        BT_Sequencer seq11 = new BT_Sequencer();
        seq11.AddNode(new GoToPoint(transform));
        seq11.AddNode(new hide(transform));
        seq1.AddNode(seq11);

        BT_Selector sel2 = new BT_Selector();

        BT_Sequencer seq21 = new BT_Sequencer();
        seq21.AddNode(new hasLoot(transform));
        seq21.AddNode(new GoToPoint(transform));
        seq21.AddNode(new hideLoot(transform));

        BT_Sequencer seq22 = new BT_Sequencer();
        seq22.AddNode(new getLootPosition(transform));
        BT_Sequencer seq222 = new BT_Sequencer();
        seq222.AddNode(new GoToPoint(transform));
        seq222.AddNode(new stealLoot(transform));
        seq22.AddNode(seq222);

        sel2.AddNode(seq21);
        sel2.AddNode(seq22);

        rootSel.AddNode(seq1);
        rootSel.AddNode(sel2);

        tree.SetRoot(rootSel);

        hideTime = Random.Range(3, 8);
        crackSafeTime = Random.Range(5, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if(!win)
        {
            tree.Tick();
            if(targetIndex >= targets.Count)
            {
                win = true;
            }
        }

        crackSafeTimer -= Time.deltaTime;
        hideTimer -= Time.deltaTime;
    }

    public List<Transform> getGuards()
    {
        return guards;
    }

    public void setGoToHidingSpot()
    {
        Vector3 closestSpot = hidingSpots[0].position;

        foreach (Transform spot in hidingSpots)
        {
            if (Vector3.Distance(spot.position, transform.position) < Vector3.Distance(closestSpot, transform.position))
            {
                closestSpot = spot.position;
            }
        }

        setMoveToLocation(closestSpot);
    }

    public void startHiding()
    {
        hideTimer = hideTime;
    }

    public bool Hiding()
    {
        return hideTimer <= 0 ? false : true;
    }

    public void StopHiding()
    {
        tree.resetTree();
    }

    public void startCrackingSafe()
    {
        crackSafeTimer = crackSafeTime;
    }

    public bool crackingSafe()
    {
        return crackSafeTimer <= 0 ? false : true;
    }

    public void crackSafe()
    {
        Debug.LogError("cracking safe");
        hasTreasure = true;
    }
}