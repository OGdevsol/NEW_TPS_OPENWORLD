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
    public ModeState modeState;
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

    [Header("____Panels____"), Space(10)] public GameObject levelCompletePanel;
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
        for (int i = 0; i < winPanelDeactivations.Length; i++)
        {
            winPanelDeactivations[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < losePanelDeactivations.Length; i++)
        {
            losePanelDeactivations[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < pausePanelDeactivations.Length; i++)
        {
            pausePanelDeactivations[i].gameObject.SetActive(false);
        }
    }

    public void DisableObjectsOnPlayerDeath()
    {
        for (int i = 0; i < deathDeactivationButtons.Count; i++)
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
        //  ShootBehaviour.instance.activeWeapon = 0;
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
        SceneManager.LoadScene(1);
    }

    public void OnClickNext()
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
        int selectedController = GetSelectedController();
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
        if (ShootBehaviour.instance.activeWeapon == 0)
        {
            currentWeaponReference.sprite = gunIcon;
        }
        else if (ShootBehaviour.instance.activeWeapon == 1)
        {
            currentWeaponReference.sprite = punchIcon;
        }
    }
}


/*if (levelsData[DataController.instance.GetSelectedLevel()].LevelLogic==Levels.levelLogic.hasCutscene)
      {
          levelsData[DataController.instance.GetSelectedLevel()].cutscene.SetActive(true);

          for (int i = 0; i < levelsData[DataController.instance.GetSelectedLevel()].dropPeople.Length; i++)
          {
              player.transform.position = levelsData[DataController.instance.GetSelectedLevel()].pickPos.position;
              levelsData[DataController.instance.GetSelectedLevel()].dropPeople[i].SetActive(true);
          
          }
      }

      if (levelsData[DataController.instance.GetSelectedLevel()].LevelLogic==Levels.levelLogic.noCutscene)
      {
          for (int i = 0; i < levelsData[DataController.instance.GetSelectedLevel()].dropPeople.Length; i++)
          {
              player.transform.position = levelsData[DataController.instance.GetSelectedLevel()].pickPos.position;
              levelsData[DataController.instance.GetSelectedLevel()].dropPeople[i].SetActive(true);
          }
      }*/


/*
if (levels[1].cutSceneEnum==Levels.CuttSenEnum.Yes)
{
  //  StartCoroutine(CutsceneRoutine(levels[0].cutsceen, levels[0].cutsceneDuration));
    // cutscene end
    //level start
  //  playerr.transform.position = levels[0].startPos.position;
 //   SpawnEnemies();
 Debug.Log(levels[1].enemies.Length);
 Debug.Log(levels[1].cutSceneEnum);
    
   
}
else if (levels[1].cutSceneEnum==Levels.CuttSenEnum.No)
{
 //   playerr.transform.position = levels[0].startPos.position;
    Debug.Log(levels[1].enemies.Length);
    Debug.Log(levels[1].cutSceneEnum);
  
}
*/
/*
   public void SpawnEnemies()
   {
       for (int i = 0; i < levels[0].enemies.Length; i++)
       {
           GameObject enemy = Instantiate(enemyPrefab, levels[0].enemies[i].position);
           levels[0].enemies[i].weapon.SetActive(true);
           SpawnEnemies();
       }
   }
   */

/*
public void CheckController()
{
       
    for (int i = 0; i < controllerButtons.Length; i++)
    {
        controllerButtons[i].image.sprite = redMark;
    }
    if (PlayerPrefs.HasKey("Controller"))
    {
        controllerButtons[PlayerPrefs.GetInt("Controller")].image.sprite = greenMark;
            
    }
    else
    {
        controllerButtons[defaultIndex].image.sprite = greenMark;
    }
}

public void OnClickControllerButton(int index)
{
    PlayerPrefs.SetInt("Controller", index);
    for (int i = 0; i < controllerButtons.Length; i++)
    {
        controllerButtons[i].image.sprite = redMark;
    }
    controllerButtons[GetSelectedController()].image.sprite = greenMark;
    if (GetSelectedController()==0)
    {
        RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.TouchScreen;
    }
    else if(GetSelectedController()==1)
    {
        RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.SteeringWheel;
    }
       
      

}

private int GetSelectedController()
{
    return PlayerPrefs.GetInt("Controller");
}

public void CheckAndApplyControlsSettingsOnStart()
{
      
    if (GetSelectedController()==0)
    {
        RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.TouchScreen;
    }
    else if(GetSelectedController()==1)
    {
        RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.SteeringWheel;
    }
    foreach (var t in controllerButtons)
    {
        t.image.sprite = redMark;
    }
    controllerButtons[GetSelectedController()].image.sprite = greenMark;
}#1#*/