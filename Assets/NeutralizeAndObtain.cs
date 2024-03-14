using System;
using System.Collections;
using System.Collections.Generic;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

public class NeutralizeAndObtain : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Milton Detecting Player");

            WaypointMover wpm = GetComponent<WaypointMover>();
            Animator animator = GetComponent<Animator>();
            HUDNavigationElement hne = GetComponent<HUDNavigationElement>();

            if (!wpm.isDead)
            {
                animator.enabled = false;
                hne.enabled = false;

                Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigidbodies)
                {
                    rb.isKinematic = false;
                }

                Interactable interactable = GetComponentInChildren<Interactable>();
                if (interactable)
                {
                    interactable.gameObject.SetActive(true);
                }
            }

            wpm.isDead = true;
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