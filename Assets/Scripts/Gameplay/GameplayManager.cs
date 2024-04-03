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
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [Header("____MissionsData____"), Space(10)]
        public Mission[] missions;
        [Header("______________")]
        [Header("____DO NOT MODIFY, DEBUG DATA____"), Space(10)]
        [SerializeField]
        public List<Transform> hudElement;

        [SerializeField] private GameObject postProcessVolume;
        private RCC_CarControllerV3 rccCar;

        private Coroutine myRoutine;

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
        private HUDNavigationSystem hns;

        private int y;

        private int
            waveToKeepActive; //Introduce removeAt0 functionality by adding the wave to a list and then initializing waves again if waves are >0

        #endregion


        private void Awake()
        {
            instance = this;
            DataCache();
            Debug.LogError(missions[dataController.GetSelectedLevel()].missionControllerType);

            Time.timeScale = 1;
            environment.SetActive(true);
            ActivateCurrentLevel();
            CheckForInstructions();
            rccCar = FindObjectOfType<RCC_CarControllerV3>();
            hns = FindObjectOfType<HUDNavigationSystem>();
            GameUIManager.instance.modeState = GameUIManager.ModeState.Missions;
            myRoutine = StartCoroutine(CutsceneRoutine());
            Debug.LogError("Mode is " + dataController.GetMode());
            if (dataController.GetMode() == 1)
            {
                gameUIManager.FreeModeDeactivations();
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

        public void CheckForInstructions()
        {
            if (!missionsGameObjects[dataController.GetSelectedLevel()].GetComponent<MissionInstructionsAndEffects>()
                    .enabled && missionsGameObjects[dataController.GetSelectedLevel()]
                    .GetComponent<MissionInstructionsAndEffects>().audios == null)
            {
                for (int i = 0; i < gameUIManager.mobileButton.Length; i++)
                {
                    gameUIManager.mobileButton[i].interactable = false;
                }
            }
        }


        public IEnumerator OnClickMobileRoutine()
        {
            MissionInstructionsAndEffects missionInstructions = missionsGameObjects[dataController.GetSelectedLevel()]
                .GetComponent<MissionInstructionsAndEffects>();

            if (missionInstructions.enabled && missionInstructions.audios != null &&
                missionInstructions.audios.Length > 0)
            {
                missionInstructions.audios[0].Play();
            }

            TypewriterEffectTextMeshPro phonePanelTextEffect =
                gameUIManager.phonePanelText.GetComponent<TypewriterEffectTextMeshPro>();

            if (phonePanelTextEffect != null)
            {
                phonePanelTextEffect.stringArray[0] = missionInstructions.levelObjective;
            }

            gameUIManager.phonePanel.SetActive(true);

            yield return new WaitForSecondsRealtime(4f);

            gameUIManager.phonePanel.SetActive(false);

            if (phonePanelTextEffect != null)
            {
                phonePanelTextEffect.stringArray[0] = null;
            }
        }

        public void OnClickMobile()
        {
            StartCoroutine(OnClickMobileRoutine());
        }

        #region CoRoutines

        private IEnumerator CutsceneRoutine()
        {
            NullCheck();
            //.playerCamera.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.03f);
            var selectedLevel = missions[dataController.GetSelectedLevel()];
            var selectedWave = selectedLevel.waves[waveToKeepActive];
            var waveCutscene = selectedWave.waveCutscene;
            if (waveCutscene != null)
            {
                gameUIManager.DisableAllCanvases();
                player.gameObject.SetActive(false);
                playerCar[dataController.GetSelectedVehicle()].SetActive(false);
                GameplayCarController.instance.playerCamera.gameObject.SetActive(false);
                GameplayCarController.instance.rccCam.gameObject.SetActive(false);
                waveCutscene.SetActive(true);
                yield return new WaitForSeconds(selectedWave.cutsceneDuration);

                if (waveCutscene != null)
                    waveCutscene.SetActive(false);
                StartCoroutine(selectedLevel.missionControllerType == Mission.MissionControllerType.CarAtStart
                    ? CarAtStartRoutine()
                    : PlayerAtStartRoutine());
            }
        }

        public void StopCutsceneAndGoToGame()
        {
            StopCoroutine(myRoutine);
            var selectedLevel = missions[dataController.GetSelectedLevel()];
            var selectedWave = selectedLevel.waves[waveToKeepActive];
            var waveCutscene = selectedWave.waveCutscene;
            if (waveCutscene != null)
                waveCutscene.SetActive(false);
            StartCoroutine(selectedLevel.missionControllerType == Mission.MissionControllerType.CarAtStart
                ? CarAtStartRoutine()
                : PlayerAtStartRoutine());
            gameUIManager.cutSceneUICanvas.enabled = false;
            Timer.instance.SetTimerValueOnSkip();
        }


        private IEnumerator CarAtStartRoutine()
        {
            Debug.LogError("Calling Start");
            yield return new WaitForSecondsRealtime(0.1f);
            var selectedVehicle = dataController.GetSelectedVehicle();
            var playerCarInstance = playerCar[selectedVehicle];
            var playerCamera = GameplayCarController.instance.playerCamera;
            var rccCam = GameplayCarController.instance.rccCam;
            var gameplayUICanvas = gameUIManager.gameplayUICanvas;
            var hUDNavigationCanvas = gameUIManager.hUDNavigationCanvas;
            var rccCanvas = gameUIManager.rccCanvas;
            var playerControllerCanvas = gameUIManager.playerControllerCanvas;

            playerCarInstance.SetActive(true);
            gameplayUICanvas.enabled = true;
            hUDNavigationCanvas.enabled = true;
            rccCanvas.enabled = true;
            playerCamera.gameObject.SetActive(false);
            playerCamera.GetComponent<Camera>().enabled = false;
            rccCam.gameObject.SetActive(true);
            rccCam.gameObject.GetComponentInChildren<Camera>().enabled = true;
            rccCam.GetComponentInChildren<AudioListener>().enabled = true;
            playerCamera.GetComponent<AudioListener>().enabled = false;
            playerControllerCanvas.enabled = false;
            player.gameObject.SetActive(false);
            hns.PlayerCamera = rccCam.GetComponentInChildren<Camera>();
            hns.PlayerController = playerCarInstance.transform;
            Debug.LogError("Calling End");
        }

        private IEnumerator PlayerAtStartRoutine()
        {
            var selectedVehicleIndex = dataController.GetSelectedVehicle();
            var selectedVehicle = playerCar[selectedVehicleIndex];
            var gameUIManager = GameUIManager.instance;
            var gameplayCarController = GameplayCarController.instance;
            var selectedLevel = missions[dataController.GetSelectedLevel()];
            var selectedWave = selectedLevel.waves[waveToKeepActive];

            // Enable player car, player camera, and RCC camera
            selectedVehicle.SetActive(true);
            gameplayCarController.playerCamera.gameObject.SetActive(true);
            gameplayCarController.rccCam.gameObject.SetActive(true);

            // Enable UI canvases
            gameUIManager.screenHudCanvas.enabled = true;
            gameUIManager.pickupCanvas.enabled = true;
            gameUIManager.gameplayUICanvas.enabled = true;
            gameUIManager.hUDNavigationCanvas.enabled = true;
            gameUIManager.playerControllerCanvas.enabled = true;

            // Start weapon coroutine
            StartCoroutine(interactiveWeapon.WeaponCoroutine());

            // Enable player GameObject
            player.gameObject.SetActive(true);

            // Disable wave cutscene if present
            var waveCutscene = selectedWave.waveCutscene;
            if (waveCutscene != null)
                waveCutscene.SetActive(false);

            // Wait for a short delay
            yield return new WaitForSecondsRealtime(0.25f);

            // Change weapon
            ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon, 0);
        }

        #endregion


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

        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

        #region Level Complete/LevelFail

        public IEnumerator LevelCompleteRoutine()
        {
            InGameSoundManager.instance.missionCompleteSound.Play();
            yield return new WaitForSecondsRealtime(2f);
            ShootBehaviour.instance.ChangeWeapon(ShootBehaviour.instance.activeWeapon, 0);
            gameUIManager.levelCompletePanel.SetActive(true);
            gameUIManager.pauseButton.gameObject.SetActive(false);

            if (dataController.GetMode() == 1)
            {
                foreach (var button in gameUIManager.winPanelDeactivations)
                {
                    button.interactable = false;
                }

                yield return new WaitForSecondsRealtime(3f);
                gameUIManager.loadingPanel.SetActive(true);
                yield return new WaitForSecondsRealtime(3f);

                SceneManager.LoadScene(2);
            }

            yield return new WaitForSecondsRealtime(1.5f);
            // Show Ad here
        }

        public IEnumerator LevelFailRoutine(GameObject panelToActivate)
        {
            yield return new WaitForSecondsRealtime(2f);
            panelToActivate.SetActive(true);
            gameUIManager.rccCanvas.enabled = false;
            gameUIManager.hUDNavigationCanvas.enabled = false;
            gameUIManager.playerControllerCanvas.enabled = false;
            gameUIManager.pauseButton.gameObject.SetActive(false);

            if (dataController.GetMode() == 1)
            {
                foreach (var button in gameUIManager.losePanelDeactivations)
                {
                    button.interactable = false;
                }

                yield return new WaitForSecondsRealtime(2f);
                Debug.Log("Returning to open world");
            }

            yield return new WaitForSecondsRealtime(1.5f);
            // Show Ad here
        }

        #endregion
    }
}