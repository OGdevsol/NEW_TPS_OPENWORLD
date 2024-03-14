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
    [SerializeField] private AudioSource[] audios;

    private void Awake()
    {
        gameplayManager??=GameplayManager.instance;
        dataController??=DataController.instance;
    }

    private void Start()
    {
    //    StartCoroutine(MissionIntroductionCoRoutine());
    }

    private IEnumerator MissionIntroductionCoRoutine()
    {
        if (gameplayManager.bShouldPlayCutscene)
        {
            yield return new WaitForSecondsRealtime(gameplayManager.missions[dataController.GetSelectedLevel()].waves[0]
                .cutsceneDuration);
            yield return new WaitForSecondsRealtime(playAfterWaitDuration);
            audios?[0].Play();
        }
        
        if (!gameplayManager.bShouldPlayCutscene)
        {
           
            yield return new WaitForSecondsRealtime(playAfterWaitDuration);
            audios?[0].Play();

        }
       
    }
}
