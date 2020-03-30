using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class guardTree : MonoBehaviour
{
    private BT_Tree tree = new BT_Tree();
    public Transform[] path;
    // Start is called before the first frame update
    void Start()
    {
        CanSeePlayer CSP = new CanSeePlayer(transform);
        ChaseSpy CS = new ChaseSpy(transform);
        BT_Sequencer seq = new BT_Sequencer();
        seq.AddNode(CSP);
        seq.AddNode(CS);

        BT_Selector rootSelector = new BT_Selector();
        rootSelector.AddNode(seq);
        rootSelector.AddNode(new Patrol(transform, path));

        tree.AddNode(rootSelector);
        tree.AddNode(new Patrol(transform, path));
    }

    // Update is called once per frame
    void Update()
    {
        tree.Tick();
    }
}