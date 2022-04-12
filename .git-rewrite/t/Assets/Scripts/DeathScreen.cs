using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{

    public GameObject deathScreen;
    public GameObject normalUI;
    public OnPlayerTriggerEnter deathTrigger;

    private void Update()
    {
        if (deathTrigger.playerInTrigger)
        {
            deathScreen.gameObject.SetActive(true);
            normalUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
