using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class Interactable : MonoBehaviour
{
   private int x;
   private GameplayManager gameplayManager;

   private void Awake()
   {
      gameplayManager=GameplayManager.instance;
   }

   private void Start()
   {
      
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         x = GameplayManager.instance. missions[DataController.instance.GetSelectedLevel()].enemiesInLevel
            .IndexOf(transform);
         GameplayManager.instance . missions[DataController.instance.GetSelectedLevel()].enemiesInLevel.RemoveAt(x);
         print("File Removed from list");
//      print(gameplayManager.enemiesInLevel.Count);
         gameObject.SetActive(false);
      }
     
      
   }
}
