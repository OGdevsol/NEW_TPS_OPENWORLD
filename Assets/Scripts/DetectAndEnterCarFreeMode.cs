using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DetectAndEnterCarFreeMode : MonoBehaviour
{
    private Animator animator;
    private GameUIManager gameUIManager;
    private GameplayCarControllerFreeMode gameplayCarController;
    private ShootBehaviour shootBehaviour;
    public TMP_Text pickUpWeaponText;
    private GameplayManagerFreeMode gameplayManager;
    private DataController dataController;
    public GameObject carDoor;
  


  
        private void Awake()
    {
      
    }

        private void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            gameUIManager = GameUIManager.instance;
            gameplayCarController=GameplayCarControllerFreeMode.instance;
            shootBehaviour=ShootBehaviour.instance;
            gameplayManager=GameplayManagerFreeMode.instance;
            dataController=DataController.instance;
        }

        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CarDoor")
        {

            AssureReferences();

            gameUIManager.getInCarButton.gameObject.SetActive(true);
          //  gameplayCarController.SwitchCameraToCar();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CarDoor")
        {
         
            gameUIManager.getInCarButton.gameObject.SetActive(false);
          //  gameplayCarController.SwitchCameraToPlayer();
        }
    }

    public void FaceAndEnterCar()
    {
       
            gameplayCarController.SwitchCameraToCar();
            
            gameplayManager.playerCar[dataController.GetSelectedVehicle()].GetComponent<Rigidbody>().drag = 0;
      
        
    }

    public void CarDoorOpenAnimation()
    {
         gameplayManager.playerCar[dataController.GetSelectedVehicle()].GetComponentInChildren<CarDoorElement>().transform.gameObject.GetComponent<Animator>().Play("OpenAnimation",-1,0f);
     
    }
    public void CarDoorOpenAnimationFast()
    {
        gameplayManager.playerCar[dataController.GetSelectedVehicle()].GetComponentInChildren<CarDoorElement>().transform.gameObject.GetComponent<Animator>().Play("CloseAnimation",-1,0f);
     
    }

    
    
    
    private IEnumerator PickUpWeaponText () 
    {
        pickUpWeaponText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        pickUpWeaponText.gameObject.SetActive(false);
        
    }

    public void FaceAndExitCar()
    {
        gameplayCarController.SwitchCameraToPlayer();
        gameplayManager.playerCar[dataController.GetSelectedVehicle()].GetComponent<Rigidbody>().drag = 5;
    }

    private void AssureReferences()
    {
        gameUIManager??=GameUIManager.instance;
       gameplayCarController??=GameplayCarControllerFreeMode.instance;
    }
}