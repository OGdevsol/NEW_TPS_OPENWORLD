using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("____Main Menu____"), Space(10)]
    public GameObject[] panels;
    public GameObject loadingPanel;
    public float waitTime;
[SerializeField] private Image selectedPlayerProfilePic;
[SerializeField]   private TMP_Text selectedPlayerName;
public Button[] controllerTypeButtons;
    [Header("____Character Selection____"), Space(10)]
    public Button continueButton;
    public Image mainCharacterReferenceImage;
    public List<Button> genderButtons;
    public List<Button> characterButtons;
    public Sprite redMark;
    public Sprite greenMark;
    public GameObject[] genderPanels;
    public  TMP_InputField playerNameField;

    private List<GameObject> characterChildObjects = new List<GameObject>();


    private string player = "SelectedPlayer";
    private string playerName = "playerName";

    /*private void Awake()
    {
        AdsManager.instance.InitializeAdmob();
    }*/

    private void Start()
    {
       CheckForPlayerData();
        CacheChildren();
        CheckAndApplyControlsSettingsOnStart();
    }

    #region Main Menu

    public void EnablePanel(int panelIndex)
    {

        StartCoroutine(EnablePanelRoutine(panelIndex));

    }

    public IEnumerator EnablePanelRoutine(int index)
    {
        Debug.LogError("Index is" + index);
        
            if (index==2)
            {
                Debug.Log("SHOWING AD HERE");
                //AdsManager.instance.showAdmobInterstitial();
            }

            if (index!=6 && index!=2)
            {
                loadingPanel.SetActive(true);
                yield return new WaitForSecondsRealtime(waitTime);
            }
          
           
            foreach (var uiPanel in panels)
            {
                uiPanel.SetActive(false);
            }
            panels[index].SetActive(true); 
        
       
    }

    public void LoadGameScene(int index)
    {
       SetSelectedLevel(index);
        StartCoroutine(LoadingToGameRoutine(1));
    }
    /*public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }*/

    public void OnClickControllerType(int index)
    {
        for (int i = 0; i < controllerTypeButtons.Length; i++)
        {
            controllerTypeButtons[i].image.sprite = i == index ? greenMark : redMark;
            SetController(index);
            
        }
        if (GetSelectedController()==0)
        {
            RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.TouchScreen;
        }
        else if(GetSelectedController()==1)
        {
            RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.SteeringWheel;
        }
    }

    public void CheckAndApplyControlsSettingsOnStart()
    {
        for (int i = 0; i < controllerTypeButtons.Length; i++)
        {
            controllerTypeButtons[i].image.sprite = redMark;
          
            
        }
        if (GetSelectedController()==0)
        {
            RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.TouchScreen;
            controllerTypeButtons[GetSelectedController()].image.sprite = greenMark;
        }
        else if(GetSelectedController()==1)
        {
            RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.SteeringWheel;
            controllerTypeButtons[GetSelectedController()].image.sprite = greenMark;
        }
    }
    private int GetSelectedController()
    {
        return PlayerPrefs.GetInt("Controller");
    }

   
    private IEnumerator LoadingWithinMenuRoutine(bool showAd)
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(waitTime);
        if (showAd)
        {
            // Show Ad here
            Debug.Log("Calling Ad");
        }
      

    }

    private IEnumerator LoadingToGameRoutine( int sceneIndex)
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(waitTime);
        SceneManager.LoadScene(sceneIndex);

    }

    #endregion

    #region Character Selection
    public void OnClickGenderSelection(int index)
    {
        for (int i = 0; i < genderButtons.Count; i++)
        {
            genderButtons[i].image.sprite = i == index ? greenMark : redMark;
            genderPanels[i].SetActive(i == index);
        }
    }

    public float transitionDuration = 0.65f; // Duration of the transition effect

    public void OnClickCharacterSelection(int index)
    {
        var normalState = new Vector3(1, 1, 1);
        var selectedState = new Vector3(1.1f, 1.1f, 1.1f);
        
        // Iterate through all character buttons
        for (int i = 0; i < characterButtons.Count; i++)
        {
            if (i == index) // Selected character
            {
                // Set the UI elements for the selected character
                mainCharacterReferenceImage.sprite = characterButtons[i].image.sprite;
                selectedPlayerProfilePic.sprite = characterButtons[i].image.sprite;
                selectedPlayerName.text = GetPlayerName();

                // Interpolate the scale to the selected state
                characterButtons[i].gameObject.transform
                    .DOScale(selectedState, transitionDuration)
                    .SetEase(Ease.OutBack);
            }
            else // Other characters
            {
                // Interpolate the scale to the normal state
                characterButtons[i].gameObject.transform
                    .DOScale(normalState, transitionDuration)
                    .SetEase(Ease.OutBack);
            }
        }

        // Activate the selected character's child object
        for (int i = 0; i < characterChildObjects.Count; i++)
        {
            characterChildObjects[i].SetActive(i == index);
        }

        continueButton.interactable = true;
        SetPlayer(index);
        Debug.Log("Set player value is " + GetPlayer() + "Player Set As" + index);
    }

    private void CacheChildren()
    {
        foreach (var button in characterButtons)
        {
            characterChildObjects.Add(button.gameObject.transform.GetChild(0).gameObject);
        }
    }

    private void CheckForPlayerData()
    {
        EnablePanel(!PlayerPrefs.HasKey(player) ? 0 : 1);
        selectedPlayerProfilePic.sprite = characterButtons[GetPlayer()].image.sprite;
        selectedPlayerName.text = GetPlayerName();
        
        
    }

  

    #endregion

    #region Data Controller

    private void SetPlayer(int index)
    {
        PlayerPrefs.SetInt(player,index);
    }

    private int GetPlayer()
    {
        return PlayerPrefs.GetInt(player);
    }

    public void SetPlayerName(string name)
    {
        name = playerNameField.text;
        PlayerPrefs.SetString(playerName,name);
    }
    private string GetPlayerName()
    {
        return PlayerPrefs.GetString(playerName);
    }

    public void SetController(int index)
    {
        PlayerPrefs.SetInt("Controller",index);
    }

    

    public void SetSelectedLevel(int index)
    {
        PlayerPrefs.SetInt("SelectedLevel", index);
    }

    public int GetSelectedLevel()
    {
        return PlayerPrefs.GetInt("SelectedLevel");
    }




    #endregion
  
    
}
