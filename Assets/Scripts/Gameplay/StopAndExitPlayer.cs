using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAndExitPlayer : MonoBehaviour
{
    private DetectAndEnterCar carInOutController;

    private void Start()
    {
        carInOutController = DetectAndEnterCar.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("PlayerCar")) return;
        carInOutController.FaceAndExitCar();
        carInOutController.CarDoorOpenAnimationFast();
        gameObject.SetActive(false);
    }
}
