using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public List<GameObject> deathDeactivationButtons;
    public GameObject headshotEffect;

    private void Awake()
    {
        instance = this;
    }


    public void DisableObjectsOnPlayerDeath()
    {
        for (int i = 0; i < deathDeactivationButtons.Count; i++)
        {
            deathDeactivationButtons[i].SetActive(false);
        }
    }
    
    
    
}
