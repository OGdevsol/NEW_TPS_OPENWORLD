using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Gameplay
{
    public class EnemyManager : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("EnemyManager is being called, please check");
        }
    }
}

[Serializable]
public class Mission
{
    //   public MissionType missionType;
    public string missionObjective;
    public Transform playerPosition;
    public List<Wave> waves;
}

[Serializable]
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