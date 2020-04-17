using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class localTree : MonoBehaviour
{

    public BT_Tree tree = new BT_Tree();

    public Vector3 moveToLocation;

    public void setMoveToLocation(Vector3 location)
    {
        moveToLocation = location;
    }

    public Vector3 getMoveToLocation()
    {
        return moveToLocation;
    }

}
