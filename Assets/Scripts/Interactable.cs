using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class Interactable : MonoBehaviour
{
   private int x;
   private GameplayManager gameplayManager;
   private DataController dataController;

   private void Awake()
   {
      gameplayManager=GameplayManager.instance;
      dataController=DataController.instance;
   }

   private void Start()
   {
      
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player") || other.CompareTag("PlayerCar"))
      {
         x = gameplayManager. missions[dataController.GetSelectedLevel()].enemiesInLevel
            .IndexOf(transform);
         gameplayManager . missions[dataController.GetSelectedLevel()].enemiesInLevel.RemoveAt(x);
         print("Interactable Removed from list");
         gameObject.SetActive(false);
      }
     
      
   }
}
