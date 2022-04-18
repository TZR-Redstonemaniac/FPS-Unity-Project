using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    ////////////////////////////////////////Public Variables////////////////////////////////////////
    
    [SerializeField] private Slider slider;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    
    ////////////////////////////////////////Private Variables////////////////////////////////////////


    private void Start()
    {
        slider.maxValue = maxHealth;
        health = maxHealth;
    }

    public void SetHealth(int setHealth)
    {
        slider.value = setHealth;
    }
    
}
