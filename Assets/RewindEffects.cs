using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindEffects : MonoBehaviour
{

    public TimeBody timeBody;
    public GameObject rewindCanvas;

    // Update is called once per frame
    void Update()
    {
        if(timeBody.isRewinding)
            rewindCanvas.SetActive(true);
        else
            rewindCanvas.SetActive(false);
    }
}
