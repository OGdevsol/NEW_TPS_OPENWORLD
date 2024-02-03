using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFailManager : MonoBehaviour
{
    public delegate void MissionFailDelegate();

    // Declare an event using the delegate type
    public static event MissionFailDelegate onMissionFail;
    
    private static MissionFailManager _instance;
    private SimplePlayerHealth simplePlayerHealth;
    public static MissionFailManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MissionFailManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("MissionFailManager");
                    _instance = obj.AddComponent<MissionFailManager>();
                }
            }
            return _instance;
        }
    }


    
    
   

}
