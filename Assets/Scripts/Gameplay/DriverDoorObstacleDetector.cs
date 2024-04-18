using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverDoorObstacleDetector : MonoBehaviour
{

    private GameUIManager gameUIManager;
    public float raycastDistance = 1f;
    private RaycastHit hit; // Declare the RaycastHit variable outside of the Update method
    

    private void Start()
    {
        gameUIManager=GameUIManager.instance;
    }

    private void Update()
    {
       
        bool hitSomething = Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance);

        
        if (hitSomething)
        {
            if (hit.collider.CompareTag("Player"))
            {
               
                return;
            }
            else //if (hit.collider.CompareTag("Obstacle"))
            {
                gameUIManager.getOutOfCarButton.interactable = false;
                //  Debug.Log("Obstacle detected near the driver door!");
            }
        }
        else
        {
            gameUIManager.getOutOfCarButton.interactable = true;
           // Debug.Log("No obstacle detected near the driver door.");
        }

      
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, hitSomething ? Color.red : Color.green);
    }
}