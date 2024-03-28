using System;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class GameUIManager : MonoBehaviour
{
    [Header("____BUTTONS____"), Space(10)]
   // public Levels[] levelsData;

    public GameObject player;
    public TouchButton pickWeaponButton;

    public Button getInCarButton;
    public Button getOutOfCarButton;
    
    
    
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

    [Header("____Win/Fail Panels____"), Space(10)]
    public GameObject levelCompletePanel;
    public GameObject levelFailPanelTime;
    public GameObject levelFailPanelDeath;
    public GameObject levelFailPanelBusted;
    

    private void Awake()
    {
        instance = this;
        Transform personTransform;
      
    }

    private IEnumerator CutsceneRoutine(GameObject cutscene ,  float time)
    {
        cutscene.SetActive(true);
      
        yield return new WaitForSeconds(time);
        cutscene.SetActive(false);
        
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
        for (i = 0; i < canvasesLength ; i++)
        {
            allCanvases[i].enabled = false;
        }
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(1);
    } 
    public void OnClickNext()
    {
        if (DataController.instance.GetSelectedLevel()<8)
        {
            DataController.instance.SetSelectedLevel(DataController.instance.GetSelectedLevel()+1);
            SceneManager.LoadScene(1);
        }
        else if(DataController.instance.GetSelectedLevel()>=8)
        {
            DataController.instance.SetSelectedLevel(0);
            SceneManager.LoadScene(1);
        }
       
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene(0);
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