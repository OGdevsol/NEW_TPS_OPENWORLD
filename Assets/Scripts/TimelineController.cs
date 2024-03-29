using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
   [SerializeField] PlayableDirector playableDirector;   
                                                /// <summary> Don't forget to cache the data, do in start
                                                /// 
                                               
   private GameplayManager gameplayManager;
   private GameUIManager inGameUIManager;
   private DataController dataController;

   private void Awake()
   {
    
   }

   private void Start()
   {
       gameplayManager=GameplayManager.instance;
       inGameUIManager = GameUIManager.instance;
       dataController=DataController.instance;
       playableDirector = gameplayManager.missions[dataController.GetSelectedLevel()].waves[0].waveCutscene
           .GetComponent<PlayableDirector>();
   }

   public void ForPlayStartCutscene(float time)
   {
    
   
       time= gameplayManager.missions[dataController.GetSelectedLevel()].waves[0].cutsceneDuration;
      playableDirector.time =time;


      // Debug.LogError(time);
    // Debug.LogError("LEVEL TIME IS " + time);
  
   
 
   }
   
   
  
}
