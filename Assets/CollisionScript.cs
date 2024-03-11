using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
 //  public GameObject hitSpark;
   //public AudioSource hitSound;
   /*private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.tag=="StreetLight")
      {
        // collision.gameObject.GetComponent<BoxCollider>().enabled = false;
         Debug.Log("Hitting Streetlight");
         if (collision.gameObject.GetComponent<Rigidbody>())
         {
           
           // collision.gameObject.AddComponent<Rigidbody>();
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right*2f,ForceMode.Impulse);
           // collision.gameObject.GetComponent<Rigidbody>().AddRelativeForce(GameplayManager.instance.impactDirection.forward*0.5f,ForceMode.Impulse);
         // collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(5f,collision.gameObject.transform.position,5f);
            StartCoroutine(DestroyStreetlight(collision.gameObject));
         }
      }
   }*/

   public GameObject hitEffect;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.tag=="StreetLight")
      {
         // collision.gameObject.GetComponent<BoxCollider>().enabled = false;
         Debug.Log("Hitting Streetlight");
         if (other.gameObject.GetComponent<Rigidbody>())
         {
            StartCoroutine(HitCoRoutine());
            Debug.Log("Hitting Streetlight2");
            // collision.gameObject.AddComponent<Rigidbody>();
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
         //   other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right*2000f,ForceMode.Impulse);
             other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(-GameplayManager.instance.impactDirection.up*10f,ForceMode.Impulse);
             StartCoroutine(colliderCoroutine(other));
            // collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(5f,collision.gameObject.transform.position,5f);
            StartCoroutine(DestroyStreetlight(other.gameObject));
         }
      }
      if (other.gameObject.tag == "Population")
      {
         Debug.Log("HITTING POPULATION");
         if (GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].GetComponent<RCC_CarControllerV3>().speed>=15)
         {
            foreach (Rigidbody member in other.gameObject.GetComponentsInChildren<Rigidbody>())
            {
               member.isKinematic = false;
               // member.velocity = Vector3.zero;
            }
            Instantiate(hitEffect, other.transform.position, other.transform.rotation);
            Debug.Log("HIGH SPEED, KILLING CHRACTER");
            other.gameObject.GetComponent<Animation>().enabled = false;
            other.gameObject.GetComponent<WaypointMover>().enabled = false;
//            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
       //     other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
           other.gameObject.GetComponent<BoxCollider>().enabled = false;
           

            StartCoroutine(DeadBodyRoutine(other));
         }
         if (GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].GetComponent<RCC_CarControllerV3>().speed<15)
         {
            Debug.Log("Low SPEED, Playing Animation and Returning to Roaming");
           
            StartCoroutine(GestureCoRoutine(other)) ;
         }
         
        
         
      }

    
   }

  
   private IEnumerator GestureCoRoutine(Collider other)
   {
      other.gameObject.GetComponent<Animation>().Stop();
      other.gameObject.GetComponent<WaypointMover>().enabled = false;
      other.GetComponent<Animator>().enabled = true;
      other.GetComponent<Animator>().Play("AngryGesture", -1,0f);
      yield return new WaitForSeconds(4f);
    
      if (Vector3.Distance(other.transform.position,GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].transform.position)<=5)
      {
         StartCoroutine(GestureCoRoutine(other)) ;
      }
      else
      {
         other.GetComponent<Animator>().enabled = false;
         other.gameObject.GetComponent<Animation>().Play();
         other.gameObject.GetComponent<WaypointMover>().enabled = true;
      }
      
   }
   private IEnumerator DeadBodyRoutine(Collider other)
   {
      yield return new WaitForSeconds(1.5f);
      Destroy(other.gameObject);
      
   }

   /*private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.tag == "Population")
      {
         Debug.Log("HITTING POPULATION");
         
         collision.gameObject.GetComponent<Animation>().Stop();
         collision.gameObject.GetComponent<WaypointMover>().enabled = false;
         foreach (Rigidbody member in collision.gameObject.GetComponentsInChildren<Rigidbody>())
         {
            member.isKinematic = false;
            member.velocity = Vector3.zero;
         }

         
      }
   }
   */

   private IEnumerator colliderCoroutine(Collider other)
   {
      yield return new WaitForSeconds(0.3f);
      other.isTrigger = false;
   }

   private IEnumerator DestroyStreetlight(GameObject streetlight)


   {
      yield return new WaitForSeconds(3f);
      streetlight.SetActive(false);
   }



   private IEnumerator HitCoRoutine()
   {
     // hitSpark.gameObject.SetActive(true);
    //  hitSpark.GetComponent<ParticleSystem>().Play();
   //   hitSound.Play();
      yield return new WaitForSecondsRealtime(3.5f);
    //  hitSpark.gameObject.SetActive(false);
    //  hitSpark.GetComponent<ParticleSystem>().Play();

   }
   
   
}