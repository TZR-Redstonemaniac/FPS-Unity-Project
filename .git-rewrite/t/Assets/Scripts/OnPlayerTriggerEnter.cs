using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerTriggerEnter : MonoBehaviour
{
    public bool playerInTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger = false;
        }
    }
}
