using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPortalAppeared : MonoBehaviour
{
    public AnimationPlayer animPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animPlayer.playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animPlayer.playerInTrigger = false;   
        }
    }
}
