/*private int CheckEnemyType(int enemyIndex) //Method to check and return which enemytype is selected
        {
            if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                    .enemyType == EnemyType.Enemy_ak47)
            {
                y = 0;
            }

            else if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                         .enemyType == EnemyType.Enemy_m16)
            {
                y = 1;
            }

            else if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                         .enemyType == EnemyType.Enemy_Pistol)
            {
                y = 2;
//                
            }

            else if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[enemyIndex]
                         .enemyType == EnemyType.Interactable)
            {
                y = 3;
            }

            return y;
        }
    }*/
    
    
    
    
/*private void SpawnEnemies()
{
    for (int j = 0;
         j < missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel.Count;
         j++)
    {
//                Debug.Log("selected variant enemy Index is:" + j);
        Debug.Log(missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
            .enemyType);
        var g = Instantiate(enemyVariantsPrefab[CheckEnemyType(j)]);
        enemiesInLevel.Add(g.transform);
    }
}*/


/*var selectedWave = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive];
          int j;
          var enemiesInLevelCount=selectedWave.enemiesInLevel.Count;

          for (j = 0; j < enemiesInLevelCount; j++)
          {
              var enemyType = selectedWave.enemiesInLevel[j].enemyType;
              Debug.Log(enemyType);

              var variantIndex = CheckEnemyType(j);
              var enemyPrefab = enemyVariantsPrefab[variantIndex];

              enemiesInLevel.Add(Instantiate(enemyPrefab).transform);
          }*/








/*private void SpawnEnemies()
{
           
    for (int j = 0;
         j < missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel.Count;
         j++)
    {
//                Debug.Log("selected variant enemy Index is:" + j);
        /*Debug.Log(missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
            .enemyType);#1#
        var enemy = Instantiate(enemyVariantsPrefab[CheckEnemyType(j)],
            missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
                .enemyPosition);
        enemiesInLevel.Add(enemy.transform);
                
        for (int i = 0; i < missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j].enemyWaypoints.Length; i++)
        {
                  
            enemiesInLevel[j].gameObject.GetComponent<StateController>().patrolWayPoints.Add(
                missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].enemiesInLevel[j]
                    .enemyWaypoints[i]);

        }
            
               
                
    }
}*/





/*private void SpawnEnemies()
{
    var selectedWave = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive];

    for (int j = 0; j < selectedWave.enemiesInLevel.Count; j++)
    {
        var enemyData = selectedWave.enemiesInLevel[j];
        var enemyTypeIndex = CheckEnemyType(j);

        var enemy = Instantiate(enemyVariantsPrefab[enemyTypeIndex], enemyData.enemyPosition);
        enemiesInLevel.Add(enemy.transform);

        if (selectedWave.enemiesInLevel[j].enemyType==EnemyType.Enemy_ak47||selectedWave.enemiesInLevel[j].enemyType==EnemyType.Enemy_m16||selectedWave.enemiesInLevel[j].enemyType==EnemyType.Enemy_Pistol)
        {
            var stateController = enemy.GetComponent<StateController>();
                
            if (stateController != null)
            {
                if (enemyData.enemyWaypoints!=null)
                {
                    stateController.patrolWayPoints.AddRange(enemyData.enemyWaypoints);
                    Debug.Log("Enemies With Waypoints Spawned");
                }

                       
                  
            }
        }
        else
        {
            Debug.Log("Interactable Spawned");
        }
               
    }
}*/


/*void SetShootBehavior()
{
    if (shootBehavior == null)
    {
        shootBehavior = ShootBehaviour.instance;

        if (shootBehavior == null)
        {
            Debug.LogError("ShootBehaviour not found or not initialized properly. Make sure ShootBehaviour is present in the scene.");
            return; // Exit the method if shootBehavior is still null
        }
    }
}

public void AutoFire()
{
    Ray ray = new Ray(behaviourManager.playerCamera.position, behaviourManager.playerCamera.forward *1000);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 1000f))
    {
        if (hit.collider.transform != this.transform)
        {
            if (hit.collider.tag=="Enemy")
            {
                //	Debug.Log("calling " + hit.collider.name);
					
                if (shootBehavior == null)
                {
                    Debug.LogError("ShootBehaviour not found or not initialized properly. Make sure ShootBehaviour is present in the scene.");
                }
                shootBehavior.ShootWeapon(shootBehavior.activeWeapon);
            }
			
        }
    }
}*/


/*
public void AutoFire()
{
    Ray ray = new Ray(behaviourManager.playerCamera.position, behaviourManager.playerCamera.forward *1000);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 500f))
    {
        if (hit.collider.transform != this.transform)
        {
            if (hit.collider.tag=="Enemy")
            {
				
					
                if (shootBehavior == null)
                {
                    Debug.LogError("ShootBehaviour not found or not initialized properly. Make sure ShootBehaviour is present in the scene.");
                }
					
                if (! shootBehavior.isShooting && shootBehavior. activeWeapon > 0 && shootBehavior. burstShotCount == 0)
                {
                    shootBehavior. isShooting = true;
                    shootBehavior. ShootWeapon(shootBehavior. activeWeapon);

                    if (weaponUIManager.bulletsLeftInMag<=0)
                    {
                        if (shootBehavior.weapons[shootBehavior.activeWeapon].StartReload())
                        {
                            AudioSource.PlayClipAtPoint(shootBehavior.weapons[shootBehavior.activeWeapon].reloadSound, shootBehavior.gunMuzzle.position, 0.5f);
                            behaviourManager.GetAnim.SetBool(shootBehavior.reloadBool, true);
                        }
                    }

						
						
                }
					
					
            }
			
        }
    }
}*/

/*
using System.Collections;
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

    public void OnClickGenderSelection(int index)
    {
        int i;
        int genderButtonsLegth=genderButtons.Count;
        for (i = 0; i < genderButtonsLegth; i++)
        {
            genderButtons[i].image.sprite = redMark;
            genderPanels[i].SetActive(false);
        }

        genderButtons[index].image.sprite = greenMark;
        genderPanels[index].SetActive(true);
       
    }

    public void OnClickCharacterSelection(int index)
    {
        int i;
        int characterSelectionButtonsLength = characterButtons.Count;
        for (i = 0; i < characterSelectionButtonsLength; i++)
        {
            characterButtons[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        characterButtons[index].gameObject.transform.GetChild(0).gameObject.SetActive(true);
        mainCharacterReferenceImage.sprite = characterButtons[index].image.sprite;
        continueButton.interactable = true;
    }
}
*/

/*
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<GameObject>
        characterChildObjects = new List<GameObject>(); // Cache for child objects of character buttons

    private void Start()
    {
        StoreChildren();
    }

    public void OnClickGenderSelection(int index)
    {
        for (var i = 0; i < genderButtons.Count; i++)
        {
            genderButtons[i].image.sprite = redMark;
            genderPanels[i].SetActive(false);
        }
        genderButtons[index].image.sprite = greenMark;
        genderPanels[index].SetActive(true);
    }

    public void OnClickCharacterSelection(int index)
    {
        foreach (var childObject in characterChildObjects)
        {
            childObject.SetActive(false);
        }
        characterChildObjects[index].SetActive(true);
        mainCharacterReferenceImage.sprite = characterButtons[index].image.sprite;
        continueButton.interactable = true;
    }

    private void StoreChildren()
    {
        foreach (var childObject in characterButtons.Select(button => button.gameObject.transform.GetChild(0).gameObject))
        {
            characterChildObjects.Add(childObject);
        }
    }
}
*/
