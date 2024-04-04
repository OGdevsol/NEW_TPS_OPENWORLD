using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class MissionInstructionsAndEffects : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private DataController dataController;
    [SerializeField] private float playAfterWaitDuration;
    [SerializeField] public AudioSource[] audios;
    public string levelObjective;

    private void Awake()
    {
        gameplayManager??=GameplayManager.instance;
        dataController??=DataController.instance;
    }

    private void Start()
    {
       StartCoroutine(MissionIntroductionCoRoutine());
    }

    private IEnumerator MissionIntroductionCoRoutine()
    {
      
            yield return new WaitForSecondsRealtime(gameplayManager.missions[dataController.GetSelectedLevel()].waves[0]
                .cutsceneDuration);
            yield return new WaitForSecondsRealtime(playAfterWaitDuration);
            if (audios.Length>0)
            {
                audios?[0].Play();
            }
        
        
        
       
           
           

        
       
    }
}
