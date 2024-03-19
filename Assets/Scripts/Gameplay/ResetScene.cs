using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public void RestartScene()
    {
        int selectedLevel = DataController.instance.GetSelectedLevel();
        int nextLevel = (selectedLevel + 1) % 5; // Assuming there are 4 levels, adjust accordingly if necessary

        Debug.LogError("Selected Level Was " + selectedLevel);
        DataController.instance.SetSelectedLevel(nextLevel);
        SceneManager.LoadScene(0);
       
    }
}