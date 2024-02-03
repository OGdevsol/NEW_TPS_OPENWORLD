using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class MissionCompleteManager : MonoBehaviour
    {
       // public static MissionCompleteManager instance;
        public delegate void MissionCompleteDelegate();

        // Declare an event using the delegate type
        public static event MissionCompleteDelegate onMissionComplete;
        
        private static MissionCompleteManager _instance;

        public static MissionCompleteManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MissionCompleteManager>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("MissionCompleteManager");
                        _instance = obj.AddComponent<MissionCompleteManager>();
                    }
                }
                return _instance;
            }
        }

        
        

        // Method to call when the level is complete
        public void CompleteMission()
        {
            // Trigger the event when the level is complete
            onMissionComplete?.Invoke();
            Debug.Log("LevelComplete"); ///Paste all level complete logic here
        }

       
        
       
    }
}
