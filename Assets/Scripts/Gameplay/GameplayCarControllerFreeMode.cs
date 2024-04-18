using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

public class GameplayCarControllerFreeMode : MonoBehaviour
{
    public static GameplayCarControllerFreeMode instance;
    private InGameSoundManager inGameSoundManager;
    public Transform rccCam;
    public Transform playerCamera;
    private bool isSwitchingCamera = false; // Flag to track if camera is currently being switched.
    public float getInCarDuration;
    public float getOutOfCarDuration;
    private float switchDuration = 1.45f; // Duration of camera switch animation.

    public GameObject getInCarPlayer;
    public GameObject getOutOfCarPlayer;
    public Transform playerOutOfCarPosition;
    public GameObject doorIndicator;

    [HideInInspector] public bool inCar;

    // public AudioSource doorOpen;
//  public AudioSource doorClose;
    private GameUIManager gameUIManager;
    private HUDNavigationSystem hns;
    private GameplayManagerFreeMode gameplayManager;
    private ShootBehaviour shootBehaviour;

    private void Awake()
    {
        instance = this;
        rccCam = FindObjectOfType<RCC_Camera>().transform;
        gameUIManager = GameUIManager.instance;
        gameplayManager = GameplayManagerFreeMode.instance;
        inGameSoundManager = InGameSoundManager.instance;
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

    // ReSharper disable Unity.PerformanceAnalysis
    private void ShowPlayerEnteringCar()
    {
        gameUIManager.getInCarButton.gameObject.SetActive(false);
        if (shootBehaviour.activeWeapon != 0)
        {
            shootBehaviour.ChangeWeapon(shootBehaviour.activeWeapon, 0);
        }

        GameplayManagerFreeMode.instance.player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        if (ShootBehaviour.instance.activeWeapon != 0)
        {
            GameplayManagerFreeMode.instance.player.GetComponentInChildren<InteractiveWeapon>().transform.gameObject
                .GetComponentInChildren<MeshRenderer>().enabled = false;
        }


        getInCarPlayer.SetActive(true);
        //  ShootBehaviour.instance.activeWeapon = 0;
        inCar = true;
        // FindObjectOfType<InteractiveWeapon>().transform.gameObject.SetActive(false);
        doorIndicator.SetActive(false);
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ShowPlayerExitingCar()
    {
        gameUIManager.getOutOfCarButton.gameObject.SetActive(false);
        GameplayManagerFreeMode.instance.player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        getOutOfCarPlayer.SetActive(true);
        inCar = false;

        //  FindObjectOfType<InteractiveWeapon>().transform.gameObject.SetActive(true);
       
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator InterpolateCamera(Transform fromTransform, Transform toTransform)
    {
        if (fromTransform == rccCam.transform)
        {
            switchDuration = getOutOfCarDuration;
            Debug.Log(getOutOfCarDuration);
        }

        else if (fromTransform == playerCamera.transform)
        {
            switchDuration = getInCarDuration;
//        Debug.Log(getInCarDuration);
        }

        NullChecker();
        isSwitchingCamera = true;
        float switchTimer = 0f;
        Vector3 initialPos = fromTransform.position;
        Quaternion initialRot = fromTransform.rotation;

        while (switchTimer < switchDuration)
        {
            switchTimer += Time.deltaTime;
            if (fromTransform == rccCam.transform)
            {
                Debug.Log("Switching " + "FROM " + "rcc");
                ShowPlayerExitingCar();
            }

            if (fromTransform == playerCamera.transform)
            {
                Debug.Log("Switching " + "FROM " + "player");
                ShowPlayerEnteringCar();
            }

            float t = Mathf.Clamp01(switchTimer / switchDuration);

            fromTransform.localPosition = Vector3.Lerp(initialPos, toTransform.localPosition, t);
            fromTransform.rotation = Quaternion.Lerp(initialRot, toTransform.rotation, t);

            yield return null;
        }

        fromTransform.localPosition = toTransform.localPosition;
        fromTransform.rotation = toTransform.rotation;

        isSwitchingCamera = false;

        // Disable the camera component of the `fromTransform`.
        if (fromTransform.GetComponent<Camera>() != null)
        {
            fromTransform.GetComponent<Camera>().enabled = false;
        }
        else if (fromTransform.GetComponentInChildren<Camera>() != null)
        {
            fromTransform.GetComponentInChildren<Camera>().enabled = false;
        }
        else
        {
            Debug.LogWarning("No camera component found on fromTransform or its children.");
        }

        // Enable the camera component of the `toTransform`.
        if (toTransform.GetComponent<Camera>() != null)
        {
            toTransform.GetComponent<Camera>().enabled = true;
        }
        else if (toTransform.GetComponentInChildren<Camera>() != null)
        {
            toTransform.GetComponentInChildren<Camera>().enabled = true;
        }
        else
        {
            Debug.LogWarning("No camera component found on toTransform or its children.");
        }

        // Additional debug logs to check the camera transition.


        // Update other game state based on camera transition.
        UpdateGameState(fromTransform, toTransform);
    }

    private void UpdateGameState(Transform fromTransform, Transform toTransform)
    {
        Debug.Log("Updating Game State Start");
        if (fromTransform == playerCamera.transform)
        {
            // GameplayManager.instance.player.SetActive(false);
            gameplayManager.player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            if (ShootBehaviour.instance.activeWeapon != 0)
            {
                gameplayManager.player.GetComponentInChildren<MeshRenderer>().enabled = false;
                //    gameplayManager.player.GetComponentInChildren<HUDNavigationElement>().enabled = false;
            }


            // Update game state when transitioning from player to car camera.

            gameUIManager.rccCanvas.enabled = true;
            gameUIManager.playerControllerCanvas.enabled = false;
            gameUIManager.gameplayUICanvas.enabled = true;
            gameUIManager.getOutOfCarButton.gameObject.SetActive(true);
            gameUIManager.getInCarButton.gameObject.SetActive(false);
            
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

            getOutOfCarPlayer.SetActive(false);
            gameplayManager.player.transform.position = playerOutOfCarPosition.position;
           

            Debug.Log("Active Weapon Is " + ShootBehaviour.instance.activeWeapon);

            if (ShootBehaviour.instance.activeWeapon != 0)
            {
                GameplayManager.instance.player.GetComponentInChildren<InteractiveWeapon>().transform.gameObject
                    .GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }
        Debug.Log("Game State Updated");
        if (inCar)
        {
            Debug.Log("In Car");
            rccCam.gameObject.GetComponentInChildren<AudioListener>().enabled = true;
            playerCamera.GetComponent<AudioListener>().enabled = false;
            inGameSoundManager.SetInGameListenersVolume();
            hns.PlayerCamera = rccCam.GetComponentInChildren<Camera>();
            hns.PlayerController = rccCam.transform;
            gameUIManager.getOutOfCarButton.gameObject.SetActive(true);
           
        }
        else if (!inCar)
        {
            Debug.Log("Out Of Car");
            rccCam.gameObject.GetComponentInChildren<AudioListener>().enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = true;
            inGameSoundManager.SetInGameListenersVolume();
            hns.PlayerCamera = playerCamera.GetComponent<Camera>();
            hns.PlayerController = gameplayManager.player.transform;
            doorIndicator.SetActive(true);
        }
    }


    private void NullChecker()
    {
        gameUIManager ??= GameUIManager.instance;
        hns ??= FindObjectOfType<HUDNavigationSystem>();
        gameplayManager ??= GameplayManagerFreeMode.instance;
        shootBehaviour ??= ShootBehaviour.instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        throw new NotImplementedException();
    }
}