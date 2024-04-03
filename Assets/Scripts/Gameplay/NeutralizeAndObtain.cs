using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

public class NeutralizeAndObtain : MonoBehaviour
{
    private int x;
    private GameplayManager gameplayManager;

    private void Awake()
    {
        gameplayManager=GameplayManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        x =  GameplayManager.instance. missions[DataController.instance.GetSelectedLevel()].enemiesInLevel
            .IndexOf(transform);
        if (other.CompareTag("PlayerCar"))
        {
            Debug.Log("Milton Detecting Player");

            var wpm = GetComponent<WaypointMover>();
            var animator = GetComponent<Animator>();
            var hne = GetComponent<HUDNavigationElement>();

            if (!wpm.isDead)
            {
                animator.enabled = false;
                hne.enabled = false;

                var rigidbodies = GetComponentsInChildren<Rigidbody>();
                foreach (var rb in rigidbodies)
                {
                    rb.isKinematic = false;
                }

                /*Interactable interactable = GetComponentInChildren<Interactable>();
                if (interactable)
                {
                    interactable.gameObject.SetActive(true);
                }*/
                GameplayManager.instance. missions[DataController.instance.GetSelectedLevel()].enemiesInLevel.RemoveAt(x);
                
            }

            wpm.isDead = true;
            CheckEnemiesInLevel();
           
            
            
            
        }
    }
    private void CheckEnemiesInLevel()
    {
            
        if (gameplayManager.missions[DataController.instance.GetSelectedLevel()].enemiesInLevel.Count == 0)
        {
            StartCoroutine(GameplayManager.instance.LevelCompleteRoutine());
        }
        else
        {
            print("Enemies Still Remaining");
        }
    }
}




/*private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Milton Detecting Player");


        WaypointMover wpm = gameObject.GetComponent<WaypointMover>();
        Animator animator = gameObject.GetComponent<Animator>();
        HUDNavigationElement hne = gameObject.GetComponent<HUDNavigationElement>();
        if (!wpm.isDead)
        {
            animator.enabled = false;
            hne.enabled = false;


            foreach (Rigidbody member in gameObject.GetComponentsInChildren<Rigidbody>())
            {
                member.isKinematic = false;
            }

            Interactable interactable = gameObject.GetComponentInChildren<Interactable>();
            interactable.gameObject.SetActive(true);
        }

        wpm.isDead = true;
    }
}*/