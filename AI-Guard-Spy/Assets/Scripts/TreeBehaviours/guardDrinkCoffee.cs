using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class guardDrinkCoffee : BT_Behaviour
{
    private Transform self;
    private guardTree localBB;
    public guardDrinkCoffee(Transform _self)
    {
        self = _self;
        localBB = self.GetComponent<guardTree>();
    }

    public override NodeState tick()
    {
        localBB.drinkCoffee();
        return NodeState.NODE_SUCCESS;
    }
}
