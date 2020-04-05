﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spyHealth : MonoBehaviour
{
    private float health = 100;

    private void OnTriggerStay(Collider other)
    {
        Debug.LogError(other.gameObject.name);
        if(other.gameObject.CompareTag("Guard"))
        {
            health -= 1.0f;
            if(health <= 0)
            {
                if(!(transform.parent.GetComponent<spyControl>().Won()))
                    GameObject.Find("Canvas").GetComponent<EndGame>().SpyLoss();
                transform.parent.GetComponent<spyControl>().Die();
            }
        }
    }
}