//bit 641997 UA
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThresholdScript : MonoBehaviour
{
    [Tooltip("Add buttons with shapes in this list")]
    public List<Button> buttons;

    private void Start()
    {
        ChangeButtonsAlphaHit();
    }

    private void ChangeButtonsAlphaHit()
    {
//        Debug.Log("Buttons with shapes AlphaHit changed");
        Image imageComponent;
        foreach (var shapedButton in buttons)
        {
            imageComponent = shapedButton.GetComponent<Image>();
            if(imageComponent != null)
            {
                imageComponent.alphaHitTestMinimumThreshold = 0.1f;
            }
            else
            {
                Debug.LogWarning("No Image component found on GameObject: " + shapedButton.name + "641997");
            }
        }
    }
}
