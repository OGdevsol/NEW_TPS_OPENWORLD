using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFailManager : MonoBehaviour
{
    public delegate void MissionFailDelegate();

    // Declare an event using the delegate type
    public static event MissionFailDelegate onMissionFail;
    
    private static MissionFailManager _instance;

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

    // Method to call when the level is complete
    public void FailMission()
    {
        // Trigger the event when the level is complete
        onMissionFail?.Invoke();
        Debug.Log("Mission Failed, You Died");
    }

}
