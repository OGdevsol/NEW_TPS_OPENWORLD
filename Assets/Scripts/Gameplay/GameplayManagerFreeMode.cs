using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using EnemyAI;
using Gameplay;
using SickscoreGames.HUDNavigationSystem;
using UnityEditor;
//using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameplayManagerFreeMode : MonoBehaviour
{
    public static GameplayManagerFreeMode instance;

    public GameObject environment;
    public GameObject player;
    public GameObject[] playerCar;
    public List<Transform> hudElement;

    private StateController stateController;
    private DataController dataController;
    private GameUIManager gameUIManager;
    private GameplayCarControllerFreeMode gameplayCarController;
    private InteractiveWeapon interactiveWeapon;
    private HUDNavigationSystem hns;
    public AudioSource missionSound;
    public Transform impactDirection;
    private int waveToKeepActive;
    private GameplayCarControllerFreeMode gameplayCarControllerFreeMode;
    

    private void Awake()
    {
        Time.timeScale = 1;
        instance = this;
        environment.SetActive(true);
        StartCoroutine(FreeModeRoutine());
        GameUIManager.instance.modeState = GameUIManager.ModeState.FreeMode;
        Debug.LogError("Mode is " + DataController.instance.GetMode());
    }

    private void Start()
    {
        hns=HUDNavigationSystem.Instance;
        gameplayCarControllerFreeMode=GameplayCarControllerFreeMode.instance;
        Debug.Log("Start");
        hns.PlayerCamera =  gameplayCarControllerFreeMode.playerCamera.GetComponent<Camera>();
    }

    private void NullCheck()
    {
        dataController ??= DataController.instance;
        stateController ??= StateController.instance;
        gameUIManager ??= GameUIManager.instance;
        gameplayCarController ??= GameplayCarControllerFreeMode.instance;
        interactiveWeapon ??= InteractiveWeapon.instance;
    }

    #region CoRoutines

    private IEnumerator FreeModeRoutine()
    {
        NullCheck();
        StartCoroutine(InteractiveWeapon.instance.WeaponCoroutine());
        yield return new WaitForSecondsRealtime(0.25f);
        gameUIManager.FreeModeDeactivations();
        ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon, 0);
        Debug.Log("Weapon Changed");
    }

    #endregion

    public int freeModeLevel;
    public void StartLevel()
    {
        DataController.instance.SetSelectedLevel(freeModeLevel);
        StartCoroutine(LoadLevelFromFreeMode());
    }

    private IEnumerator LoadLevelFromFreeMode()
    {
        GameUIManager.instance.loadingPanel.SetActive(true);
        GameUIManager.instance.freeModePanel.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(1);
    }

    public void RejectLevel()
    {
        GameUIManager.instance.freeModePanel.SetActive(false);
        Time.timeScale = 1;
        AudioListener.volume = PlayerPrefs.GetFloat("Sound");
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}