using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class guardTree : MonoBehaviour
{

    public Transform player;

    private BT_Tree tree = new BT_Tree();

    // local blackboard variables
    public Transform path;
    private List<Transform> _path = new List<Transform>();
    private float chaseCoolDown;
    private float chaseCoolDowntimer = 0;
    private Vector3 lastKnownLocation;

    private Vector3 moveToLocation;

    private bool canHearPlayer;

    public Transform sleepingArea;
    private float fatigue;
    public float fatigueThreshold;

    public Transform coffeeTable;
    private float energy;
    public float energyThreshold;

    public float sleepAmount;
    private float sleepTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // build the behaviour tree

        CanSeePlayer CSP = new CanSeePlayer(transform);
        GoToPointOneTick CS = new GoToPointOneTick(transform);
        BT_Sequencer seq = new BT_Sequencer();
        seq.AddNode(CSP);
        seq.AddNode(CS);

        CanHearSpy CHP = new CanHearSpy(transform);
        GoToPointOneTick GTPOT = new GoToPointOneTick(transform);
        BT_Sequencer seq1 = new BT_Sequencer();
        seq1.AddNode(CHP);
        seq1.AddNode(GTPOT);

        BT_Selector sel = new BT_Selector();
        sel.AddNode(seq);
        sel.AddNode(seq1);
        sel.AddNode(new GoToLastKnownLocation(transform));

        BT_Sequencer seq2 = new BT_Sequencer();
        seq2.AddNode(new GetPatrolRoute(transform));
        seq2.AddNode(new GoToPoint(transform));

        BT_Sequencer checkSleepSequencer = new BT_Sequencer();
        checkSleepSequencer.AddNode(new needSleep(transform));
        BT_Sequencer sleepSequencer = new BT_Sequencer();
        sleepSequencer.AddNode(new GoToPoint(transform));
        sleepSequencer.AddNode(new guardSleep(transform));
        checkSleepSequencer.AddNode(sleepSequencer);

        BT_Sequencer checkCoffeeSequencer = new BT_Sequencer();
        checkCoffeeSequencer.AddNode(new needCoffee(transform));
        BT_Sequencer coffeeSequencer = new BT_Sequencer();
        coffeeSequencer.AddNode(new GoToPoint(transform));
        coffeeSequencer.AddNode(new guardDrinkCoffee(transform));
        checkCoffeeSequencer.AddNode(coffeeSequencer);

        BT_Selector IdleSelector = new BT_Selector();
        IdleSelector.AddNode(checkSleepSequencer);
        IdleSelector.AddNode(checkCoffeeSequencer);
        IdleSelector.AddNode(seq2);

        BT_Selector rootSelector = new BT_Selector();
        rootSelector.AddNode(sel);
        rootSelector.AddNode(IdleSelector);

        tree.SetRoot(rootSelector);


        fatigue = Random.Range(60, 80);
        energy = Random.Range(60, 80);
        chaseCoolDown = Random.Range(1, 3);

        generatePath();
    }

    private void generatePath()
    {
        // yes, this is hugely wasteful of resources
        foreach (Transform child in path)
        {
            _path.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tree.Tick();

        chaseCoolDowntimer -= Time.deltaTime;
        sleepTimer -= Time.deltaTime;

        fatigue -= Time.deltaTime;
        energy -= Time.deltaTime;
    }

    // ######### local blackboard methods ######### //

    public List<Transform> getPath()
    {
        return _path;
    }
    public void LostSight(Vector3 spyLastKnowLocation)
    {
        Debug.Log("Lost sight of spy");
        moveToLocation = spyLastKnowLocation;
        lastKnownLocation = moveToLocation;
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

    public void setMoveToLocation(Vector3 location)
    {
        moveToLocation = location;
    }

    public Vector3 getMoveToLocation()
    {
        return moveToLocation;
    }

    public void PlayerHeard()
    {
        canHearPlayer = true;
    }

    public bool bPlayerHeard()
    {
        return canHearPlayer;
    }

    public void resetHearing()
    {
        canHearPlayer = false;
    }

    public bool needsCoffee()
    {
        return energy < energyThreshold;
    }

    public void getCoffee()
    {
        moveToLocation = coffeeTable.position;
    }

    public void drinkCoffee()
    {
        energy = Random.Range(60, 80);
    }

    public bool needsSleep()
    {
        Debug.LogError((fatigue < fatigueThreshold));
        return fatigue < fatigueThreshold;
    }

    public void getSleep()
    {
        moveToLocation = sleepingArea.position;
    }

    public void startSleep()
    {
        sleepTimer = sleepAmount;
    }

    public bool sleeping()
    {
        return sleepTimer > 0;
    }

    public void endSleep()
    {
        fatigue = Random.Range(60, 80);
    }

}