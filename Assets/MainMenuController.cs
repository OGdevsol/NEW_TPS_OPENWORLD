using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button continueButton;
    public Image mainCharacterReferenceImage;
    public List<Button> genderButtons;
    public List<Button> characterButtons;
    public Sprite redMark;
    public Sprite greenMark;
    public GameObject[] genderPanels;

    private List<GameObject> characterChildObjects = new List<GameObject>();

    private void Start()
    {
       
        CacheChildren();
    }

    public void OnClickGenderSelection(int index)
    {
        for (int i = 0; i < genderButtons.Count; i++)
        {
            genderButtons[i].image.sprite = i == index ? greenMark : redMark;
            genderPanels[i].SetActive(i == index);
        }
    }

    public void OnClickCharacterSelection(int index)
    {
        Vector3 normalState=new Vector3(1,1,1);
        Vector3 selectedState=new Vector3(1.1f,1.1f,1.1f);
        foreach (var button in characterButtons)
        {
            button.gameObject.transform.localScale = normalState;
        }
      
        foreach (var childObject in characterChildObjects)
        {
            childObject.SetActive(false);
        }

      
        characterChildObjects[index].SetActive(true);

        mainCharacterReferenceImage.sprite = characterButtons[index].image.sprite;
        characterButtons[index].gameObject.transform.localScale = selectedState;
        continueButton.interactable = true;
    }

    private void CacheChildren()
    {
        foreach (var button in characterButtons)
        {
            characterChildObjects.Add(button.gameObject.transform.GetChild(0).gameObject);
        }
    }
}
