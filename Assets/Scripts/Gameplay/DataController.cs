using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
  
   public static DataController instance;
    private string selectedLevel = "SelectedLevel";
    private string selectedVehicle = "SelectedVehicle";

    private void Awake()
    {
        instance = this;
    }

 
    public void SetSelectedLevel(int selectedLevelIndex)
    {
        PlayerPrefs.SetInt(selectedLevel, selectedLevelIndex);
    }

    public int GetSelectedLevel()
    {
        return PlayerPrefs.GetInt(selectedLevel);
    }
    
    public void SetSelectedVehicle(int selectedVehicleIndex)
    {
        PlayerPrefs.SetInt(selectedVehicle, selectedVehicleIndex);
    }

    public int GetSelectedVehicle()
    {
        return PlayerPrefs.GetInt(selectedVehicle);
    }
    public void SetMode(int index)
    {
        PlayerPrefs.SetInt("Mode",index);
    }

    public int GetMode()
    {
        return PlayerPrefs.GetInt("Mode");
    }
}