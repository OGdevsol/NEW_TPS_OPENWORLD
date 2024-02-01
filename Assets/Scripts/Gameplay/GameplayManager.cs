using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        public enum EnemyType
        {
            Enemy_ak47,
            Enemy_m16,
            Enemy_Pistol,




        }

        public static GameplayManager instance;
        public GameObject[] enemyVariantsPrefab;
        public Mission[] missions;

        [SerializeField] private List<Transform> enemiesInLevel;


        private void Awake()
        {
            instance = this;
            TestFunction();
        }


        private int y;

        int checkEnemyType(int enemyIndex)
        {

            if (missions[0].enemiesInLevel[enemyIndex].enemyType == EnemyType.Enemy_ak47)
            {
                y = 0;
                Debug.Log("YOU SELECTED ENEMY NO" + enemyIndex);
            }

            if (missions[0].enemiesInLevel[enemyIndex].enemyType == EnemyType.Enemy_m16)
            {
                y = 1;
                Debug.Log("YOU SELECTED ENEMY NO" + enemyIndex);
            }

            if (missions[0].enemiesInLevel[enemyIndex].enemyType == EnemyType.Enemy_Pistol)
            {
                y = 2;
                Debug.Log("YOU SELECTED ENEMY NO" + enemyIndex);
            }

            return y;
        }

        private void TestFunction()
        {



            for (int j = 0; j < missions[0].enemiesInLevel.Length; j++)
            {
                Debug.LogError("selected variant enemy Index is:" + j);
                Debug.LogError(missions[0].enemiesInLevel[j].enemyType);
                var g = Instantiate(enemyVariantsPrefab[checkEnemyType(j)]);
                enemiesInLevel.Add(g.transform);


            }

        }







        [Serializable]
        public class Mission
        {
            //   public MissionType missionType;
            public Transform playerPosition;
            public string missionObjective;
            public EnemiesInLevel[] enemiesInLevel;


            public enum MissionType
            {
                Shooting,
                Driving,
                ReachDestination,
            }
        }
    }

    [System.Serializable]
    public class EnemiesInLevel
    {
        public GameplayManager.EnemyType enemyType;
        public Transform enemyPosition;
        public Transform[] enemyWaypoints;
        public string waveObjective;
        public GameObject waveMissionIndicator;



    }
}