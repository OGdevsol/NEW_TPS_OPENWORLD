using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicLevelTrigger : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private ShootBehaviour shootBehaviour;
    private GameUIManager gameUIManager;
    private GameplayCarController gameplayCarController;

    private void Awake()
    {
        gameplayManager = GameplayManager.instance;
        shootBehaviour = ShootBehaviour.instance;
        gameUIManager = GameUIManager.instance;
    }

    [System.Serializable]
    public enum TriggerType
    {
        OnlyCutscene,
        CutsceneWithActivations,
        LevelEndAfterCutscene,
        ActivateElements,
    }


    public TriggerType triggerType;
    public GameObject TriggerCutscene;
    public float TriggerCutsceneDuration;
    public List<GameObject> TriggerActivations;

    public TMP_Text TriggerTextObject;

    public string triggerText;
    //   [SerializeField] private List<GameObject> TriggerActivationsInLevel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag is "Player")
        {
            StartCoroutine(StartCutscene());
            Debug.Log("ACTIVATING CUTSCENE");
        }

        if (other.gameObject.tag is "PlayerCar")
        {
            //Get out of the car to proceed further
        }
    }

    private IEnumerator StartCutscene()
    {
        NullChecker();
        int j;

        int objective = TriggerActivations.Count;

        if (triggerType == TriggerType.OnlyCutscene)
        {
            BeforeCutsceneFunc();
            yield return new WaitForSeconds(TriggerCutsceneDuration);
            AfterCutSceneFunc();
        }
        else if (triggerType == TriggerType.LevelEndAfterCutscene)
        {
            BeforeCutsceneFunc();
            yield return new WaitForSeconds(TriggerCutsceneDuration);
            AfterCutSceneFunc();
            yield return new WaitForSeconds(2f);
            Debug.Log("LevelEndedAfterCutscene");
        }
        else if (triggerType == TriggerType.ActivateElements)
        {
            if (TriggerActivations.Count != 0)
            {
                for (j = 0; j < objective; j++)
                {
                    TriggerActivations[j].SetActive(true);
                }
            }

            TriggerTextObject.text = triggerText;
        }
    }

    private void BeforeCutsceneFunc()
    {
        gameUIManager.rccCanvas.enabled = false;
        gameUIManager.playerControllerCanvas.enabled = false;
        gameUIManager.gameplayUICanvas.enabled = false;
        TriggerCutscene.SetActive(true);
        shootBehaviour.activeWeapon = 0;
    }

    private void AfterCutSceneFunc()
    {
        TriggerCutscene.SetActive(false);

        gameObject.SetActive(false);
        if (gameplayCarController.inCar)
        {
            gameUIManager.rccCanvas.enabled = true;
            gameUIManager.gameplayUICanvas.enabled = true;
        }
        else
        {
            gameUIManager.playerControllerCanvas.enabled = true;
            gameUIManager.gameplayUICanvas.enabled = true;
            shootBehaviour.activeWeapon = 1;
        }
    }


    private void NullChecker()
    {
        shootBehaviour ??= ShootBehaviour.instance;
        gameUIManager ??= GameUIManager.instance;
        gameplayManager ??= GameplayManager.instance;
        gameplayCarController ??= GameplayCarController.instance;
    }
}