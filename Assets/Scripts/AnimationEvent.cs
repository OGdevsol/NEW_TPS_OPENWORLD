using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    
    public void GetInCarStartAnimationEvent()
    {
    
        GameplayCarController.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

    }
    public void GetInCarEndAnimationEvent()
    {
    
        GameplayCarController.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    
    }
    public void GetOutOfCarStartAnimationEvent()
    {
    
        GameplayCarController.instance.getOutOfCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    
    }
    
    public void GetOutOfCarMidAnimationEvent()
    {
    
        GameplayCarController.instance.getOutOfCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    
    }

    /*public void PlayDoorOpenSound()
    {
        GameplayCarController.instance.doorOpen.Play();
    }
    public void PlayDoorCloseSound()
    {
        GameplayCarController.instance.doorClose.Play();
    }*/
   
}
