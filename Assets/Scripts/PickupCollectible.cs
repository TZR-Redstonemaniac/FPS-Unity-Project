using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollectible : MonoBehaviour
{
    public CollectiblesCollected CollectiblesCollected;

    private void OnTriggerEnter(Collider other)
    {
        CollectiblesCollected.collectiblesCollected++;
        Destroy(gameObject);
    }
}
