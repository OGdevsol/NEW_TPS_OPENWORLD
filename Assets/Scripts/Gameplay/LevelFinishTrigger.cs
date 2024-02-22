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
        gameplayManager=GameplayManager.instance;
        dataController=DataController.instance;
    }

    private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.tag=="Player" || other.gameObject.tag=="PlayerCar")
      {
        Debug.Log("Level Complete");
        gameplayManager.playerCar[dataController.GetSelectedVehicle()].gameObject.GetComponent<Rigidbody>().drag = 6;

      }
   }
}
