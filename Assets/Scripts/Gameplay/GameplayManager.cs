using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using EnemyAI;
using Gameplay;
using SickscoreGames.HUDNavigationSystem;
using UnityEditor;
//using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        #region Variables

        [Tooltip("Remove this bool and functionality before final build as this is for testing purposes only")]
        public bool bShouldPlayCutscene;

        public bool bShouldSpawnEnemies;

        public static GameplayManager instance;
        public GameObject environment;

        [Header("____Player And Enemy Data____"), Space(10)]
        public GameObject player;

        public Transform impactDirection;


        public GameObject targetForEnemy;
        public GameObject[] enemyVariantsPrefab;
        public GameObject[] playerCar;
        public GameObject[] missionsGameObjects;

        [Header("______________")]
        [Header("______________")]
        [Header("______________")]
        [Header("______________")]
        [Header("____MissionsData____"), Space(10)]
        public Mission[] missions;

        [Header("______________")]
        [Header("______________")]
        [Header("______________")]
        [Header("______________")]
        [Header("____DO NOT MODIFY, DEBUG DATA____"), Space(10)]
        [SerializeField]
        //   public List<Transform> enemiesInLevel;
        //  public List<EnemiesInLevelTransforms> enemiesList;
        public List<Transform> hudElement;

        [SerializeField] private GameObject postProcessVolume;
        private RCC_CarControllerV3 rccCar;

        public enum EnemyType
        {
            Enemy_ak47,
            Enemy_m16,
            Enemy_Pistol,
            Interactable,
        }


        private StateController stateController;
        private DataController dataController;
        private GameUIManager gameUIManager;
        private GameplayCarController gameplayCarController;
        private InteractiveWeapon interactiveWeapon;

        private int y;

        private int
            waveToKeepActive; //Introduce removeAt0 functionality by adding the wave to a list and then initializing waves again if waves are >0

        #endregion


        private void Awake()
        {
            //  QualitySettings.SetQualityLevel(3);
            instance = this;
            DataCache();
            dataController.SetSelectedLevel(2);
            Time.timeScale = 1;
            environment.SetActive(true);
            ActivateCurrentLevel();
            rccCar = FindObjectOfType<RCC_CarControllerV3>();
            if (bShouldPlayCutscene)
            {
                StartCoroutine(CutsceneRoutine());
            }
            else if (!bShouldPlayCutscene)
            {
                StartCoroutine(noCutsceneRoutine());
            }

            else if (missions[dataController.GetSelectedLevel()].missionType == Mission.MissionType.CutsceneAtStart)
            {
                StartCoroutine(CutsceneRoutine());
            }
            else if (missions[dataController.GetSelectedLevel()].missionType == Mission.MissionType.CarAtStart)
            {
                StartCoroutine(CarAtStartRoutine());
            }

            if (bShouldSpawnEnemies)
            {
                SpawnEnemies();
            }
        }


        private void ActivateCurrentLevel()
        {
            int i;
            int missionLength = missionsGameObjects.Length;
            for (i = 0; i < missionLength; i++)
            {
                missionsGameObjects[i].SetActive(false);
            }

            missionsGameObjects[dataController.GetSelectedLevel()].SetActive(true);
        }


        private void DataCache()
        {
            dataController = DataController.instance;
            stateController = StateController.instance;
            gameUIManager = GameUIManager.instance;
            gameplayCarController = GameplayCarController.instance;
            interactiveWeapon = InteractiveWeapon.instance;
        }

        private void NullCheck()
        {
            dataController ??= DataController.instance;
            stateController ??= StateController.instance;
            gameUIManager ??= GameUIManager.instance;
            gameplayCarController ??= GameplayCarController.instance;
            interactiveWeapon ??= InteractiveWeapon.instance;
        }


        private void SpawnEnemies()
        {
            int j;
            var selectedWave = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive];

            for (j = 0; j < selectedWave.enemiesInLevel.Count; j++)
            {
                var enemyData = selectedWave.enemiesInLevel[j];
                var enemyTypeIndex = CheckEnemyType(j);

                var enemy = Instantiate(enemyVariantsPrefab[enemyTypeIndex], enemyData.enemyPosition);

                missions[dataController.GetSelectedLevel()].enemiesInLevel.Add(enemy.transform);
                HUDNavigationElement hudElementEnemy = missions[dataController.GetSelectedLevel()].enemiesInLevel[j]
                    .GetComponentInChildren<HUDNavigationElement>();
                hudElement.Add(hudElementEnemy.transform);


                if (IsShootingEnemy(enemyData.enemyType))
                {
                    var stateController = enemy.GetComponent<StateController>();

                    if (stateController != null && enemyData.enemyWaypoints != null)
                    {
                        stateController.patrolWayPoints.AddRange(enemyData.enemyWaypoints);
                    }
                }
                else
                {
                    Debug.Log("SpawningFile");
                }
            }
        }

        private IEnumerator CutsceneRoutine()
        {
            NullCheck();
            yield return new WaitForSeconds(0.05f);
            if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].waveCutscene != null)
            {
                gameUIManager.DisableAllCanvases();
                player.gameObject.SetActive(false);
                playerCar[dataController.GetSelectedVehicle()].SetActive(false);
                GameplayCarController.instance.playerCamera.gameObject.SetActive(false);
                GameplayCarController.instance.rccCam.gameObject.SetActive(false);
                missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].waveCutscene.SetActive(true);
                yield return new WaitForSeconds(missions[dataController.GetSelectedLevel()].waves[waveToKeepActive]
                    .cutsceneDuration);

                playerCar[dataController.GetSelectedVehicle()].SetActive(true);
                GameplayCarController.instance.playerCamera.gameObject.SetActive(true);
                GameplayCarController.instance.rccCam.gameObject.SetActive(true);
                gameUIManager.screenHudCanvas.enabled = true;
                gameUIManager.pickupCanvas.enabled = true;
                gameUIManager.gameplayUICanvas.enabled = true;
                gameUIManager.hUDNavigationCanvas.enabled = true;


                StartCoroutine(interactiveWeapon.WeaponCoroutine());
                gameUIManager.playerControllerCanvas.enabled = true;
                player.gameObject.SetActive(true);


                missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].waveCutscene.SetActive(false);
                yield return new WaitForSecondsRealtime(0.25f);

                ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon, 0);
            }
        }

        private IEnumerator noCutsceneRoutine()
        {
            NullCheck();
            yield return new WaitForSeconds(0.05f);
            if (missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].waveCutscene != null)
            {
                playerCar[dataController.GetSelectedVehicle()].SetActive(true);
                GameplayCarController.instance.playerCamera.gameObject.SetActive(true);
                GameplayCarController.instance.rccCam.gameObject.SetActive(true);
                gameUIManager.screenHudCanvas.enabled = true;
                gameUIManager.pickupCanvas.enabled = true;
                gameUIManager.gameplayUICanvas.enabled = true;
                gameUIManager.hUDNavigationCanvas.enabled = true;


                StartCoroutine(interactiveWeapon.WeaponCoroutine());
                gameUIManager.playerControllerCanvas.enabled = true;
                gameUIManager.cutSceneUICanvas.enabled = false;
                player.gameObject.SetActive(true);


                missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].waveCutscene.SetActive(false);
                yield return new WaitForSecondsRealtime(0.25f);

                ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon, 0);
            }
        }


        private IEnumerator CarAtStartRoutine()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            playerCar[dataController.GetSelectedVehicle()].SetActive(true);
            GameplayCarController.instance.playerCamera.gameObject.SetActive(false);
            GameplayCarController.instance.playerCamera.gameObject.GetComponent<Camera>().enabled = false;
            GameplayCarController.instance.rccCam.gameObject.SetActive(true);
            GameplayCarController.instance.rccCam.gameObject.GetComponentInChildren<Camera>().enabled = true;
            //   gameUIManager.screenHudCanvas.enabled = true;
            //  gameUIManager.pickupCanvas.enabled = true;
            gameUIManager.gameplayUICanvas.enabled = true;
            gameUIManager.hUDNavigationCanvas.enabled = true;


            //   StartCoroutine(interactiveWeapon.WeaponCoroutine());
            gameUIManager.playerControllerCanvas.enabled = false;
            player.gameObject.SetActive(false);


            //   missions[dataController.GetSelectedLevel()].waves[waveToKeepActive].waveCutscene.SetActive(false);
            //   yield return new WaitForSecondsRealtime(0.25f);

            //  ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon,0);
        }


        private bool IsShootingEnemy(EnemyType enemyType)
        {
            return enemyType is EnemyType.Enemy_ak47 or EnemyType.Enemy_m16 or EnemyType.Enemy_Pistol;
        }

        private int CheckEnemyType(int enemyIndex)
        {
            var enemyType = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive]
                .enemiesInLevel[enemyIndex].enemyType;

            return enemyType switch
            {
                EnemyType.Enemy_ak47 => 0,
                EnemyType.Enemy_m16 => 1,
                EnemyType.Enemy_Pistol => 2,
                EnemyType.Interactable => 3,
                _ => -1
            };
        }

        public void SetUltra()
        {
            QualitySettings.SetQualityLevel(4);
        }

        public void SetMedium()
        {
            QualitySettings.SetQualityLevel(3);
        }

        public void SetLow()
        {
            QualitySettings.SetQualityLevel(0);
        }
    }
}