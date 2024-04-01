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
    #region Variables

    public static GameplayManagerFreeMode instance;
    public GameObject environment;

    [Header("____Player And Enemy Data____"), Space(10)]
    public GameObject player;


    public GameObject[] playerCar;


    public List<Transform> hudElement;


    private RCC_CarControllerV3 rccCar;

    private Coroutine myRoutine;


    private StateController stateController;
    private DataController dataController;
    private GameUIManager gameUIManager;
    private GameplayCarControllerFreeMode gameplayCarController;
    private InteractiveWeapon interactiveWeapon;
    private HUDNavigationSystem hns;

    private int y;

    private int
        waveToKeepActive; //Introduce removeAt0 functionality by adding the wave to a list and then initializing waves again if waves are >0

    #endregion


    private void Awake()
    {
        instance = this;
        environment.SetActive(true);
        StartCoroutine(FreeModeRoutine());
        GameUIManager.instance.modeState = GameUIManager.ModeState.FreeMode;

        /*DataCache();
      
        Time.timeScale = 1;
        environment.SetActive(true);


        rccCar = FindObjectOfType<RCC_CarControllerV3>();
        hns = FindObjectOfType<HUDNavigationSystem>();
        //  dataController.SetMode(1);


        myRoutine = StartCoroutine(noCutsceneRoutine());*/
        Debug.LogError("Mode is " + dataController.GetMode());
    }

    private void Start()
    {
        Debug.Log("Start");
        HUDNavigationSystem.Instance.PlayerCamera =
            GameplayCarControllerFreeMode.instance.playerCamera.GetComponent<Camera>();
       
    }

    private void DataCache()
    {
        dataController = DataController.instance;
        stateController = StateController.instance;
        gameUIManager = GameUIManager.instance;
        gameplayCarController = GameplayCarControllerFreeMode.instance;
        interactiveWeapon = InteractiveWeapon.instance;
    }

    private void NullCheck()
    {
        dataController ??= DataController.instance;
        stateController ??= StateController.instance;
        gameUIManager ??= GameUIManager.instance;
        gameplayCarController ??= GameplayCarControllerFreeMode.instance;
        interactiveWeapon ??= InteractiveWeapon.instance;
    }

    public Mission[] missions;

    #region CoRoutines

    private IEnumerator FreeModeRoutine()
    {
        
        NullCheck();
        StartCoroutine(InteractiveWeapon.instance.WeaponCoroutine());

        yield return new WaitForSecondsRealtime(0.25f);

        ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon, 0);

        gameUIManager.FreeModeDeactivations();

    }

    #endregion

    /*public void LevelStartFreeMode(Collider other)
    {
        DataController.instance.SetSelectedLevel(other.GetComponent<FreeModeLevel>().levelNo); 
        GameUIManager.instance.freeModePanelText.text = other.GetComponent<FreeModeLevel>().levelObjective;
        StartCoroutine(LoadLevelFromFreeMode());
    }
    public void StartLevel()
    {
     //   Debug.LogError(level);
        
      

    }

    public void RejectLevel()
    {
        GameUIManager.instance.freeModePanel.SetActive(false);
        Time.timeScale = 1;
        AudioListener.volume = PlayerPrefs.GetFloat("Sound");
    }


    private IEnumerator LoadLevelFromFreeMode()
    {
        GameUIManager.instance.loadingPanel.SetActive(true);
        GameUIManager.instance.freeModePanel.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(1);

    }*/
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