using System;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class GameUIManager : MonoBehaviour
{
    [HideInInspector]public ModeState modeState;
    private InGameSoundManager inGameSoundManager;

    [Header("____BUTTONS+____"), Space(10)]
    // public Levels[] levelsData;
    public GameObject player;

    public TouchButton pickWeaponButton;

    public Button getInCarButton;
    public Button getOutOfCarButton;
    public Button[] mobileButton;
    public Button skipButton;
    public Button[] controllerButtons;
    public Button pauseButton;
    public Button[] winPanelDeactivations;
    public Button[] losePanelDeactivations;
    public Button[] pausePanelDeactivations;
    public Button accept;
    public Button reject;
    public GameObject freeModePanel;
    public TMP_Text freeModePanelText;


    public Sprite redMark;
    public Sprite greenMark;

    public Image currentWeaponReference;
    public Sprite gunIcon;
    public Sprite punchIcon;


    public static GameUIManager instance;
    public List<GameObject> deathDeactivationButtons;

    [Header("____Canvases____"), Space(10)]
    public Canvas rccCanvas;

    public Canvas playerControllerCanvas;
    public Canvas gameplayUICanvas;
    public Canvas cutSceneUICanvas;
    public Canvas pickupCanvas;
    public Canvas screenHudCanvas;
    public Canvas hUDNavigationCanvas;
    public GameObject headshotEffect;

    public Canvas[] allCanvases;

    //  public Levels[] levels;
    public GameObject playerr;
    public GameObject enemyPrefab;

    [Header("____Panels____"), Space(10)] 
    public GameObject levelCompletePanel;
    public GameObject levelCompletePanelFreeMode;
    public GameObject levelFailPanelTime;
    public GameObject levelFailPanelDeath;
    public GameObject levelFailPanelBusted;
    public GameObject pausePanel;
    public GameObject loadingPanel;
    public GameObject phonePanel;
    public GameObject otherDoorPanel;
    public TMP_Text phonePanelText;

    private ShootBehaviour shootBehaviour;
    private GameUIManager gameUIManager;


    public enum ModeState
    {
        Missions,
        FreeMode
    }


    private void Awake()
    {
        instance = this;
        Transform personTransform;
        inGameSoundManager = InGameSoundManager.instance;
    }

    private void Start()
    {
        CheckAndApplyControlsSettingsOnStart();
        shootBehaviour = ShootBehaviour.instance;
    }

    private IEnumerator CutsceneRoutine(GameObject cutscene, float time)
    {
        cutscene.SetActive(true);

        yield return new WaitForSeconds(time);
        cutscene.SetActive(false);
    }

    public void FreeModeDeactivations()
    {
        DeactivateGameObjects(winPanelDeactivations);
        DeactivateGameObjects(losePanelDeactivations);
        DeactivateGameObjects(pausePanelDeactivations);
    }

    private void DeactivateGameObjects(Button[] gameObjects)
    {
        foreach (Button obj in gameObjects)
        {
            obj.gameObject.SetActive(false);
        }
    }

   

    public void DisableObjectsOnPlayerDeath()
    {
        int i;
        var deathDeactivationsLength=deathDeactivationButtons.Count;
        for (i = 0; i < deathDeactivationsLength; i++)
        {
            deathDeactivationButtons[i].SetActive(false);
        }
    }

    public void DisableAllCanvases()
    {
        int i;
        int canvasesLength = allCanvases.Length;
        for (i = 0; i < canvasesLength; i++)
        {
            allCanvases[i].enabled = false;
        }
    }

    public void OnClickPause()
    {
        inGameSoundManager.clickSound.Play();
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        AudioListener.volume = 0;
        
        ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon, 0);
    }

    public void OnClickResume()
    {
        Time.timeScale = 1;
        inGameSoundManager.clickSound.Play();
        pausePanel.SetActive(false);
        inGameSoundManager.SetInGameListenersVolume();
    }

    public void OnClickRestart()
    {

        if (DataController.instance.GetMode()==0)
        {
            SceneManager.LoadScene(1);
        }
        else  if (DataController.instance.GetMode()==1)
        {
            SceneManager.LoadScene(2);
        }
        
    }

    public IEnumerator OnClickContinueInFreeModeLevelCompleteRoutine()
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(2);
    }

    public void OnClickContinueFromFreeMode()
    {
        StartCoroutine(OnClickContinueInFreeModeLevelCompleteRoutine());
    }


    public void OnClickNext()
    {
        if (DataController.instance.GetMode()==0)
        {
            int selectedLevel = DataController.instance.GetSelectedLevel();
            selectedLevel = (selectedLevel < 9) ? selectedLevel + 1 : 0;
            DataController.instance.SetSelectedLevel(selectedLevel);
            SceneManager.LoadScene(1);
        }
        else if (DataController.instance.GetMode()==1)
        {
            StartCoroutine(LoadingToFreeModeOnNextClick());
        }
       
       
    }

    public IEnumerator LoadingToFreeModeOnNextClick()
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        SceneManager.LoadScene(2);
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene(0);
    }

    private readonly int defaultIndex = 0;

    public void CheckController()
    {
        var selectedController = GetSelectedController();
        for (int i = 0; i < controllerButtons.Length; i++)
        {
            controllerButtons[i].image.sprite = (i == selectedController) ? greenMark : redMark;
        }
    }

    public void OnClickControllerButton(int index)
    {
        PlayerPrefs.SetInt("Controller", index);
        CheckController();
        RCC_Settings.Instance.mobileController = index switch
        {
            0 => RCC_Settings.MobileController.TouchScreen,
            1 => RCC_Settings.MobileController.SteeringWheel,
            _ => RCC_Settings.Instance.mobileController
        };
    }

    public void CheckAndApplyControlsSettingsOnStart()
    {
        var selectedController = GetSelectedController();
        RCC_Settings.Instance.mobileController = (selectedController == 0)
            ? RCC_Settings.MobileController.TouchScreen
            : RCC_Settings.MobileController.SteeringWheel;
        CheckController();
    }

    private int GetSelectedController()
    {
        return PlayerPrefs.GetInt("Controller", defaultIndex);
    }

 
    public void CheckAndChangeCurrentWeapon()
    {
        currentWeaponReference.sprite = (ShootBehaviour.instance.activeWeapon == 0) ? gunIcon : punchIcon;
    }
}

/*public void OnClickNext()
{
    if (DataController.instance.GetSelectedLevel() < 9)
    {
        DataController.instance.SetSelectedLevel(DataController.instance.GetSelectedLevel() + 1);
        SceneManager.LoadScene(1);
    }
    else if (DataController.instance.GetSelectedLevel() >= 9)
    {
        DataController.instance.SetSelectedLevel(0);
        SceneManager.LoadScene(1);
    }
}*/

/*public void CheckAndChangeCurrentWeapon()
{
    if (ShootBehaviour.instance.activeWeapon == 0)
    {
        currentWeaponReference.sprite = gunIcon;
    }
    else if (ShootBehaviour.instance.activeWeapon == 1)
    {
        currentWeaponReference.sprite = punchIcon;
    }
}*/
/*public void FreeModeDeactivations()
{
    int i;
    int winPanelDeactivationsLengths= winPanelDeactivations.Length;
    int losePanelDeactivationsLengths= losePanelDeactivations.Length;
    int  pausePanelDeactivationsLengths=pausePanelDeactivations.Length;
        
        
    for (i = 0; i < winPanelDeactivationsLengths; i++)
    {
        winPanelDeactivations[i].gameObject.SetActive(false);
    }

    for ( i = 0; i < losePanelDeactivationsLengths; i++)
    {
        losePanelDeactivations[i].gameObject.SetActive(false);
    }

    for ( i = 0; i <pausePanelDeactivationsLengths; i++)
    {
        pausePanelDeactivations[i].gameObject.SetActive(false);
    }
}*/