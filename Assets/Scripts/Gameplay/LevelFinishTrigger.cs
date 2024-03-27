using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class LevelFinishTrigger : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private DataController dataController;

    private void Awake()
    {
        gameplayManager = GameplayManager.instance;
        dataController = DataController.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag is "Player" or "PlayerCar")
        {
            if (gameplayManager.missions[dataController.GetSelectedLevel()].missionEndingType ==
                Mission.MissionEndingType.ReachDestination)
            {
                Debug.Log("Level Complete After Reaching Destination");
                gameplayManager ??= GameplayManager.instance;
                gameplayManager.playerCar[dataController.GetSelectedVehicle()].gameObject.GetComponent<Rigidbody>()
                    .drag = 6;
                StartCoroutine(gameplayManager.LevelCompleteRoutine());
            }
        }
    }
}