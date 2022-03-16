using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{

    ////////////////////////////////////////Public Variables////////////////////////////////////////

    [SerializeField] private RobotLife life;

    ////////////////////////////////////////Private Variables////////////////////////////////////////



    public void TakeDamage(int damage)
    {
        life.TakeDamage(damage);
    }
}
