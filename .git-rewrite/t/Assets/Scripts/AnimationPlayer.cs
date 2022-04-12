using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{

    public Animator anim;

    public bool playerInTrigger = false;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

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

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("playerInTrigger", playerInTrigger);
    }
}
