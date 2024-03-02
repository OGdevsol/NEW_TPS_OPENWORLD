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

   private void OnTriggerEnter(Collider other)
   {
      x = GameplayManager.instance.enemiesInLevel
         .IndexOf(transform);
      GameplayManager.instance .enemiesInLevel.RemoveAt(x);
      print("File Removed from list");
//      print(gameplayManager.enemiesInLevel.Count);
      gameObject.SetActive(false);
      
   }
}
