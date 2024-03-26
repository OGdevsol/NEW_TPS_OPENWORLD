using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float totalTime; // Initial time in seconds (e.g., 10 minutes)

    private GameplayManager gameplayManager;
    private DataController dataController;
    private float currentTime;
    [SerializeField] private DOTweenAnimation warningAnim;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private TMP_Text timerText;
    private Animator timerAnimator;
    private bool beyondWarning;
    private bool levelIsFail;

    private void Awake()
    {
        gameplayManager = GameplayManager.instance;
        dataController = DataController.instance;
    }


    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        CheckTimerStatus();
    }

    private void Initialize()
    {
        Debug.LogError(totalTime);

        gameplayManager ??= GameplayManager.instance;
        dataController ??= DataController.instance;

        timerAnimator = timerObject.GetComponent<Animator>();
        totalTime = gameplayManager.missions[dataController.GetSelectedLevel()].missionTime;
        Debug.LogError(totalTime);
        CheckCutscene();
    }

    private void CheckCutscene()
    {
        if (gameplayManager.bShouldPlayCutscene)
        {
            StartCoroutine(TimerRoutine());
        }
        else
        {
           
            currentTime = totalTime;
          
        }
    }

    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSecondsRealtime(gameplayManager.missions[dataController.GetSelectedLevel()].waves[0]
            .cutsceneDuration);
    
        currentTime = totalTime;
     
    }


    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CheckTimerStatus()
    {
        if (levelIsFail || currentTime <= 0)  // If the level has already failed or the timer has run out, no need to proceed further
        {
           
            return;
        }
       

        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (!beyondWarning && Mathf.FloorToInt(currentTime) == 15)  // Trigger the warning animation when there are exactly 10 seconds remaining
        {
           
            timerAnimator.Play("Warning");
            Debug.LogError("10 SECONDS REMAINING");
            beyondWarning = true;
        }

        if (Mathf.FloorToInt(currentTime) == 0)    // Handle level fail when timer runs out
        {
         
            levelIsFail = true;
            // timerObject.SetActive(false);
            Debug.LogError("Level Fail");
        }
    }
}