using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

public class GameplayCarController : MonoBehaviour
{
    public static GameplayCarController instance;
    public Transform rccCam;
    public Transform playerCamera;
    private bool isSwitchingCamera = false; // Flag to track if camera is currently being switched.
    private float switchDuration = 1.530f; // Duration of camera switch animation.
   
    public GameObject getInCarPlayer;
    public GameObject getOutOfCarPlayer;
    public Transform playerOutOfCarPosition;
  [HideInInspector]  public bool inCar;

  private GameUIManager gameUIManager;
  private HUDNavigationSystem hns;
  private GameplayManager gameplayManager;
  private ShootBehaviour shootBehaviour;
    
    private void Awake()
    {
        instance = this;
        rccCam = FindObjectOfType<RCC_Camera>().transform;
        gameUIManager=GameUIManager.instance;
        gameplayManager=GameplayManager.instance;
        hns = FindObjectOfType<HUDNavigationSystem>();



    }
    // Method to smoothly switch from player camera to RCC camera.
    public void SwitchCameraToCar()
    {
        if (rccCam != null && playerCamera.GetComponent<Camera>() && !isSwitchingCamera)
        {
            StartCoroutine(InterpolateCamera(playerCamera.transform, rccCam.transform));
        }
    }

    // Method to smoothly switch from RCC camera to player camera.
    public void SwitchCameraToPlayer()
    {
        if (rccCam != null && rccCam.gameObject.GetComponentInChildren<Camera>().enabled && !isSwitchingCamera)
        {
            StartCoroutine(InterpolateCamera(rccCam.transform, playerCamera.transform));
        }
    }

    private void ShowPlayerEnteringCar()
    {
       
        GameplayManager.instance.player.GetComponentInChildren<SkinnedMeshRenderer>().enabled  = false;
        
        getInCarPlayer.SetActive(true);
        ShootBehaviour.instance.activeWeapon = 0;
        inCar = true;
        // FindObjectOfType<InteractiveWeapon>().transform.gameObject.SetActive(false);



    }
    
    private void ShowPlayerExitingCar()
    {
        GameplayManager.instance.player.GetComponentInChildren<SkinnedMeshRenderer>().enabled  = false;
        getOutOfCarPlayer.SetActive(true);
        inCar = false;
      
        //  FindObjectOfType<InteractiveWeapon>().transform.gameObject.SetActive(true);


    }

    private IEnumerator InterpolateCamera(Transform fromTransform, Transform toTransform)
{
    NullChecker();
    isSwitchingCamera = true;
    float switchTimer = 0f;
    Vector3 initialPos = fromTransform.position;
    Quaternion initialRot = fromTransform.rotation;

    while (switchTimer < switchDuration)
    {
        switchTimer += Time.deltaTime;
        if (fromTransform==rccCam.transform)
        {
//            Debug.Log("SWITCHING FROM RCCCCCCCCCCCCC");
            ShowPlayerExitingCar();
          
        }
        if (fromTransform==playerCamera.transform)
        {
           ShowPlayerEnteringCar();
          
        }
        float t = Mathf.Clamp01(switchTimer / switchDuration);

        fromTransform.position = Vector3.Lerp(initialPos, toTransform.position, t);
        fromTransform.rotation = Quaternion.Lerp(initialRot, toTransform.rotation, t);

        yield return null;
    }

    fromTransform.position = toTransform.position;
    fromTransform.rotation = toTransform.rotation;

    isSwitchingCamera = false;

    // Disable the camera component of the `fromTransform`.
    if (fromTransform.GetComponent<Camera>() != null)
    {
        fromTransform.GetComponent<Camera>().enabled = false;
        Debug.Log("Disabled camera component on fromTransform.");
    }
    else if (fromTransform.GetComponentInChildren<Camera>() != null)
    {
        fromTransform.GetComponentInChildren<Camera>().enabled = false;
        Debug.Log("Disabled camera component on a child of fromTransform.");
    }
    else
    {
        Debug.LogWarning("No camera component found on fromTransform or its children.");
    }

    // Enable the camera component of the `toTransform`.
    if (toTransform.GetComponent<Camera>() != null)
    {
        toTransform.GetComponent<Camera>().enabled = true;
        Debug.Log("Enabled camera component on toTransform.");
    }
    else if (toTransform.GetComponentInChildren<Camera>() != null)
    {
        toTransform.GetComponentInChildren<Camera>().enabled = true;
        Debug.Log("Enabled camera component on a child of toTransform.");
    }
    else
    {
        Debug.LogWarning("No camera component found on toTransform or its children.");
    }

    // Additional debug logs to check the camera transition.
    Debug.Log("Camera transition complete.");
    Debug.Log("From transform: " + fromTransform.name);
    Debug.Log("To transform: " + toTransform.name);

    // Update other game state based on camera transition.
    UpdateGameState(fromTransform, toTransform);
}

private void UpdateGameState(Transform fromTransform, Transform toTransform)
{
    
    if (fromTransform == playerCamera.transform)
    {
       // GameplayManager.instance.player.SetActive(false);
       gameplayManager.player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
       gameplayManager.player.GetComponentInChildren<MeshRenderer>().enabled = false;
       gameplayManager.player.GetComponentInChildren<HUDNavigationElement>().enabled = false;
        // Update game state when transitioning from player to car camera.
       // Debug.Log("Switching From Player To RCC");
        gameUIManager.rccCanvas.enabled = true;
        gameUIManager.playerControllerCanvas.enabled = false;
        gameUIManager.gameplayUICanvas.enabled = true;
        gameUIManager.getOutOfCarButton.gameObject.SetActive(true);
        gameUIManager.getInCarButton.gameObject.SetActive(false);
        hns.PlayerCamera = rccCam.GetComponentInChildren<Camera>();
        hns.PlayerController = rccCam.transform;
        getInCarPlayer.SetActive(false);
    }

    if (fromTransform == rccCam.transform)
    {
        // Update game state when transitioning from car to player camera.
        gameUIManager.rccCanvas.enabled = false;
        gameUIManager.playerControllerCanvas.enabled = true;
        gameUIManager.gameplayUICanvas.enabled = true;
        gameUIManager.getOutOfCarButton.gameObject.SetActive(false);
      
        gameplayManager.player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        gameplayManager.player.GetComponentInChildren<MeshRenderer>().enabled = true;
        gameplayManager.player.GetComponentInChildren<HUDNavigationElement>().enabled = true;
        getOutOfCarPlayer.SetActive(false);
        gameplayManager.player.transform.position=playerOutOfCarPosition.position;
          hns.PlayerCamera = playerCamera.GetComponent<Camera>();
        hns.PlayerController = gameplayManager.player.transform;
        shootBehaviour.activeWeapon = 1;
    }
}



private void NullChecker()
    {
        gameUIManager??=GameUIManager.instance;
        hns??= FindObjectOfType<HUDNavigationSystem>();
        gameplayManager??=GameplayManager.instance;
        shootBehaviour??=ShootBehaviour.instance;
        

       
    }

}
