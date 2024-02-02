using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController instance;
    private string selectedLevel = "SelectedLevel";
    

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;    
        }
        
        
    }






    public void SetSelectedLevel(int selectedLevelIndex)
    {
        PlayerPrefs.SetInt(selectedLevel,selectedLevelIndex);
    }

    public int GetSelectedLevel()
    {
        return PlayerPrefs.GetInt(selectedLevel);
    }
}
