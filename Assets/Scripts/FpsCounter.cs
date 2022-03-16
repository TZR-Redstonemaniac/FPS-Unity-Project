using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{

    public TextMeshProUGUI fpsCount;
    
    private int lastFrameIndex;
    private float[] frameTimeDeltaArray;

    private void Awake()
    {
        frameTimeDeltaArray = new float[50];
    }

    private void Update()
    {
        frameTimeDeltaArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameTimeDeltaArray.Length;

        fpsCount.text = Mathf.Round(CalculateFps()).ToString();
    }

    float CalculateFps()
    {
        float total = 0f;
        foreach (float deltaTime in frameTimeDeltaArray)
        {
            total += deltaTime;
        }
        return frameTimeDeltaArray.Length / total;
    }
}
