using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    [SerializeField] private float slowDownFactor = 0.5f;
    [SerializeField] private float slowDownlength = 2f;

    void Update()
    {
        Time.timeScale += (1f / slowDownlength) * Time.unscaledDeltaTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

}
