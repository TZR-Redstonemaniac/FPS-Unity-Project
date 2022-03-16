using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleAnimationPlayer : MonoBehaviour
{
    public Animator anim;

    public GrapplingGun grapplingGun;

    public bool grappleActivated = false;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grappleActivated = grapplingGun.IsGrappling();
        
        anim.SetBool("Grapple", grappleActivated);
    }
}
