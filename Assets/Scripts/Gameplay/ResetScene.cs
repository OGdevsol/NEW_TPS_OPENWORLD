using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
   public void RestartScene()
   {
      if (DataController.instance.GetSelectedLevel()==0)
      {
         Debug.LogError("Selected Level Was" + DataController.instance.GetSelectedLevel());

         DataController.instance.SetSelectedLevel(1);
         SceneManager.LoadScene(0);
      }
    else  if (DataController.instance.GetSelectedLevel()==1)
      {
         Debug.LogError("Selected Level Was" + DataController.instance.GetSelectedLevel());
         DataController.instance.SetSelectedLevel(2);
         SceneManager.LoadScene(0);
      }
      else  if (DataController.instance.GetSelectedLevel()==2)
      {
         Debug.LogError("Selected Level Was" + DataController.instance.GetSelectedLevel());
         DataController.instance.SetSelectedLevel(0);
         SceneManager.LoadScene(0);
      }
   }

}
