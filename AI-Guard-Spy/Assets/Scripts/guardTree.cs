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
        BT_Selector rootSelector = new BT_Selector();
        rootSelector.Test();
        CanSeePlayer CSP = new CanSeePlayer(transform);
        CSP.Test();
        rootSelector.AddNode(CSP);

        tree.AddNode(rootSelector);
        tree.AddNode(new Patrol(transform, path));
    }

    // Update is called once per frame
    void Update()
    {
        tree.Tick();
    }
}
