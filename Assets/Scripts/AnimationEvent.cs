using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    
    public void GetInCarStartAnimationEvent()
    {

        if (GameUIManager.instance.modeState==GameUIManager.ModeState.Missions)
        {
            GameplayCarController.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
        else  if (GameUIManager.instance.modeState==GameUIManager.ModeState.FreeMode)
        {
            GameplayCarControllerFreeMode.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
       

    }
    public void GetInCarEndAnimationEvent()
    {
        if (GameUIManager.instance.modeState==GameUIManager.ModeState.Missions)
        {
            GameplayCarController.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        else   if (GameUIManager.instance.modeState==GameUIManager.ModeState.FreeMode)
        {
            GameplayCarControllerFreeMode.instance.getInCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
    
       
    
    }
    public void GetOutOfCarStartAnimationEvent()
    {
        if (GameUIManager.instance.modeState==GameUIManager.ModeState.Missions)
        {
            GameplayCarController.instance.getOutOfCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        else   if (GameUIManager.instance.modeState==GameUIManager.ModeState.FreeMode)
        {
            GameplayCarControllerFreeMode.instance.getOutOfCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
    
       
    
    }
    
    public void GetOutOfCarMidAnimationEvent()
    {
        if (GameUIManager.instance.modeState==GameUIManager.ModeState.Missions)
        {
            GameplayCarController.instance.getOutOfCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
        else   if (GameUIManager.instance.modeState==GameUIManager.ModeState.FreeMode)
        {
            GameplayCarControllerFreeMode.instance.getOutOfCarPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    
      
    
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
