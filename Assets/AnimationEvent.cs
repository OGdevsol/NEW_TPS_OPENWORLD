using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void GetInCarStartAnimationEvent()
    {
    
        GameplayCarController.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        Debug.Log("Enabling SMR");
    }
    public void GetInCarEndAnimationEvent()
    {
    
        GameplayCarController.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        Debug.Log("Disabling SMR");
    }

   
}
