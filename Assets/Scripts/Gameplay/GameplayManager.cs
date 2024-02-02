using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gameplay;
using OpenCover.Framework.Model;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        #region Variables

        public static GameplayManager instance;
        public GameObject[] enemyVariantsPrefab;
        public Mission[] missions;
        [SerializeField] private List<Transform> enemiesInLevel;

        public enum EnemyType
        {
            Enemy_ak47,
            Enemy_m16,
            Enemy_Pistol,
            Interactable,
        }


        private DataController dataController;
        private int y;

        private int
            waveToKeepActive; //Introduce removeAt0 functionality by adding the wave to a list and then initializing waves again if waves are >0

        #endregion


        private void Awake()
        {
            instance = this;
            DataCache();
            SpawnEnemies();
            dataController.SetSelectedLevel(1);
            Debug.Log(dataController.GetSelectedLevel());
        }

        private void DataCache()
        {
            dataController = DataController.instance;
        }

      
        private void SpawnEnemies()
        {
            var selectedWave = missions[dataController.GetSelectedLevel()].waves[waveToKeepActive];
            int j;
            var enemiesInLevelCount=selectedWave.enemiesInLevel.Count;

            for (j = 0; j < enemiesInLevelCount; j++)
            {
                var enemyType = selectedWave.enemiesInLevel[j].enemyType;
                Debug.Log(enemyType);

                var variantIndex = CheckEnemyType(j);
                var enemyPrefab = enemyVariantsPrefab[variantIndex];

                enemiesInLevel.Add(Instantiate(enemyPrefab).transform);
            }
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

        [Serializable]
        public class Mission
        {
            //   public MissionType missionType;
            public string missionObjective;
            public Transform playerPosition;
            public List<Wave> waves;
        }

        [System.Serializable]
        public class EnemiesInLevel
        {
            public GameplayManager.EnemyType enemyType;
            public Transform enemyPosition;
            public Transform[] enemyWaypoints;
        }

        [Serializable]
        public class Wave
        {
            public List<EnemiesInLevel> enemiesInLevel;
            public string waveObjective;
            public GameObject waveMissionIndicator;
        }
    }
}