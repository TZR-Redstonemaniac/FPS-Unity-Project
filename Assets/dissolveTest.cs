using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dissolveTest : MonoBehaviour
{
    
    //////////////////////////////////////////////Public Variables//////////////////////////////////////////////

    [SerializeField] private Material mat;
    [SerializeField] private bool dead;
    [SerializeField] private float dissolveSpeed;
    [Range(-1f, 1f)] [SerializeField] private float DissolveAmount;

    //////////////////////////////////////////////Private Variables//////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        if (dead)
            DissolveAmount += dissolveSpeed * Time.deltaTime;
        else
            DissolveAmount -= dissolveSpeed * Time.deltaTime;

        DissolveAmount = Mathf.Clamp(DissolveAmount, -1, 1);
        
        mat.SetFloat("_Dissolve", DissolveAmount);
    }
}
