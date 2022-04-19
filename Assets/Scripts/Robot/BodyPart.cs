using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{

    ////////////////////////////////////////Public Variables////////////////////////////////////////

    [SerializeField] private RobotLife life;
    [SerializeField] private float damageMultiplier;

    ////////////////////////////////////////Private Variables////////////////////////////////////////



    public void TakeDamage(float damage)
    {
        life.TakeDamage(damage * damageMultiplier);
    }
}
