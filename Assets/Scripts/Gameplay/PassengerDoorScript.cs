using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerDoorScript : MonoBehaviour
{
    private GameUIManager gameUIManager;
    private bool inRange;

 

    private void Start()
    {
        gameUIManager = GameUIManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            gameUIManager.otherDoorPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            gameUIManager.otherDoorPanel.SetActive(false);
        }
    }
}