using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
   public static CollisionScript instance;

   private void Awake()
   {
      instance = this;
   }
  
   public GameObject hitEffect;
   
   
private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("StreetLight"))
    {
        HandleStreetLightCollision(other.gameObject);
    }
    else if (other.CompareTag("Population"))
    {
        RCC_CarControllerV3 playerCarController = GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].GetComponent<RCC_CarControllerV3>();
        WaypointMover waypointMover = other.GetComponent<WaypointMover>();

        if (playerCarController.speed >= 10 && !waypointMover.isDead)
        {
            KillPopulation(other.gameObject);
        }
        else if (!waypointMover.isDead && playerCarController.speed < 10)
        {
            StartCoroutine(GestureCoRoutine(other));
        }
    }

   
    
}

private void HandleStreetLightCollision(GameObject streetLight)
{
    Rigidbody streetLightRigidbody = streetLight.GetComponent<Rigidbody>();
    if (streetLightRigidbody != null)
    {
        StartCoroutine(HitCoRoutine());
        streetLightRigidbody.isKinematic = false;
        streetLightRigidbody.AddRelativeForce(-GameplayManager.instance.impactDirection.up * 10f, ForceMode.Impulse);
        StartCoroutine(colliderCoroutine(streetLight.GetComponent<Collider>()));
        StartCoroutine(DestroyStreetlight(streetLight));
    }
}

public IEnumerator GestureCoRoutine(Collider other)
{
    WaypointMover waypointMover = other.GetComponent<WaypointMover>();
    if (waypointMover && !waypointMover.isDead && other.gameObject.activeInHierarchy)
    {
        Animation animation = other.GetComponent<Animation>();
        Animator animator = other.GetComponent<Animator>();

        animation.Stop();
        waypointMover.enabled = false;
        animator.enabled = true;
        animator.Play("AngryGesture", -1, 0f);
        yield return new WaitForSeconds(4f);

        if (waypointMover && !waypointMover.isDead && other.gameObject.activeInHierarchy &&
            Vector3.Distance(other.transform.position, GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].transform.position) > 5)
        {
            animator.enabled = false;
            animation.Play();
            waypointMover.enabled = true;
        }
        // Optionally, you can handle else case here if needed.
    }
}

private void KillPopulation(GameObject population)
{
    Animation animation = population.GetComponent<Animation>();
    WaypointMover waypointMover = population.GetComponent<WaypointMover>();

    animation.enabled = false;
    waypointMover.enabled = false;
    waypointMover.isDead = true;

    foreach (Rigidbody member in population.GetComponentsInChildren<Rigidbody>())
    {
        member.isKinematic = false;
    }

    Instantiate(hitEffect, population.transform.position, population.transform.rotation);
    StartCoroutine(DeadBodyRoutine(population.GetComponent<Collider>()));
}

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
    yield return new WaitForSecondsRealtime(3.5f);
}

private IEnumerator DeadBodyRoutine(Collider other)
{
    yield return new WaitForSeconds(4f);
    other.gameObject.SetActive(false);
}
   
   
}










   /*private void OnTriggerEnter(Collider other)
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
         if (GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].GetComponent<RCC_CarControllerV3>().speed>=10 && !other.gameObject.GetComponent<WaypointMover>().isDead)
         {
            other.gameObject.GetComponent<Animation>().enabled = false;
            other.gameObject.GetComponent<WaypointMover>().enabled = false;
            other.gameObject.GetComponent<WaypointMover>().isDead = true;
            foreach (Rigidbody member in other.gameObject.GetComponentsInChildren<Rigidbody>())
            {
               member.isKinematic = false;
               // member.velocity = Vector3.zero;
            }
            Instantiate(hitEffect, other.transform.position, other.transform.rotation);
            Debug.Log("HIGH SPEED, KILLING CHRACTER");
         
          
//            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
       //     other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
         //  other.gameObject.GetComponent<BoxCollider>().enabled = false;
           

            StartCoroutine(DeadBodyRoutine(other));
         }
         if (GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].GetComponent<RCC_CarControllerV3>().speed<10 && !other.gameObject.GetComponent<WaypointMover>().isDead)
         {
            Debug.Log("Low SPEED, Playing Animation and Returning to Roaming");

            if (!other.gameObject.GetComponent<WaypointMover>().isDead)
            {
               StartCoroutine(GestureCoRoutine(other)) ;
            }
           
         }
         
        
         
      }

    
   }

   
  
   public IEnumerator GestureCoRoutine(Collider other)
   {
      if (!other.gameObject.GetComponent<WaypointMover>().isDead)
      {
         other.gameObject.GetComponent<Animation>().Stop();
         other.gameObject.GetComponent<WaypointMover>().enabled = false;
         other.GetComponent<Animator>().enabled = true;
         other.GetComponent<Animator>().Play("AngryGesture", -1,0f);
         yield return new WaitForSeconds(4f);
         /*if (!other.gameObject.GetComponent<WaypointMover>().isDead)
         {#1#
         if (!other.gameObject.GetComponent<WaypointMover>().isDead && other.gameObject.activeInHierarchy)
         {
            if ( Vector3.Distance(other.transform.position,GameplayManager.instance.playerCar[DataController.instance.GetSelectedVehicle()].transform.position)<=5)
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
         else
         {
            
         }

        
            
           
        // }
    
       
      }
     
      
   }
   private IEnumerator DeadBodyRoutine(Collider other)
   {
      yield return new WaitForSeconds(1.5f);
      other.gameObject.SetActive(false);
      
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
   #1#

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

   }*/
   
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
