using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreeModeLevel : MonoBehaviour
{
  private GameUIManager gameUiManager;
  private DataController dataController;
 
  
  public string levelObjective;
  public int levelNo;
  private int level;
  private Coroutine myRoutine;

  private void Awake()
  {
    gameUiManager=GameUIManager.instance;
    dataController=DataController.instance;
  }

  private void OnTriggerEnter(Collider other)
  {
  
    if (other.gameObject.tag is "Player" or "PlayerCar")
    {
      GameplayManagerFreeMode.instance.missionSound.Play();
      GameplayManagerFreeMode.instance.freeModeLevel = levelNo;
      GameUIManager.instance.freeModePanel.SetActive(true);
      GameUIManager.instance.freeModePanelText.text = levelObjective;
     myRoutine= StartCoroutine(GeneralWait());


    }
  
  }

  private IEnumerator GeneralWait()
  {
    yield return new WaitForSecondsRealtime(0.25f);
    Time.timeScale = 0;
    AudioListener.volume = 0;

  }

  private void OnTriggerExit(Collider other)
  {
   
    if (other.gameObject.tag is "Player" or "PlayerCar")
    {
      GameUIManager.instance.freeModePanel.SetActive(false);
     

    }
  }

  /*public void StartLevel()
  {
    DataController.instance.SetSelectedLevel(level);
    StartCoroutine(LoadLevelFromFreeMode());

  }*/

  /*public void RejectLevel()
  {
    GameUIManager.instance.freeModePanel.SetActive(false);
    Time.timeScale = 1;
    AudioListener.volume = PlayerPrefs.GetFloat("Sound");
  }*/


  /*private IEnumerator LoadLevelFromFreeMode()
  {
    GameUIManager.instance.loadingPanel.SetActive(true);
    GameUIManager.instance.freeModePanel.SetActive(false);
    yield return new WaitForSecondsRealtime(3f);
    SceneManager.LoadScene(1);

  }*/
}
