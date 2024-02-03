using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EnemyAI;
using Gameplay;
//using OpenCover.Framework.Model;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        #region Variables

        public static GameplayManager instance;

        public GameObject targetForEnemy;
        
        public GameObject[] enemyVariantsPrefab;
        public Mission[] missions;
        [SerializeField] public List<Transform> enemiesInLevel;

        public enum EnemyType
        {
            Enemy_ak47,
            Enemy_m16,
            Enemy_Pistol,
            Interactable,
        }


        private StateController stateController;
        private DataController dataController;
        private int y;

        private int
            waveToKeepActive; //Introduce removeAt0 functionality by adding the wave to a list and then initializing waves again if waves are >0

        #endregion


        private void Awake()
        {
            instance = this;

            DataCache();
            dataController.SetSelectedLevel(0);
          
            Debug.Log(dataController.GetSelectedLevel());
        }

        private void Start()
        {
            SpawnEnemies();
        }

        private void DataCache()
        {
            dataController = DataController.instance;
            stateController = StateController.instance;
           
        }


        private void SpawnEnemies()
        {
            var selectedWave = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive];

            for (int j = 0; j < selectedWave.enemiesInLevel.Count; j++)
            {
                var enemyData = selectedWave.enemiesInLevel[j];
                var enemyTypeIndex = CheckEnemyType(j);

                var enemy = Instantiate(enemyVariantsPrefab[enemyTypeIndex], enemyData.enemyPosition);
                enemiesInLevel.Add(enemy.transform);

                if (IsShootingEnemy(enemyData.enemyType))
                {
                    var stateController = enemy.GetComponent<StateController>();

                    if (stateController != null && enemyData.enemyWaypoints != null)
                    {
                        stateController.patrolWayPoints.AddRange(enemyData.enemyWaypoints);
                        Debug.Log("Enemies With Waypoints Spawned");
                    }
                }
                else
                {
                    Debug.Log("Interactable Spawned");   //Interactable logic and assignment here
                }
            }
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
    }
}