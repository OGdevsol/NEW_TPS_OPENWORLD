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
 
    [Serializable]
    public enum MissionType
    {
        ThirdPersonAtStart,
        CarAtStart,
        Other,
        
    }

    public MissionType missionType;
    public bool weaponNeeded;
    public string missionObjective;
    public Transform playerPosition;
    public List<Wave> waves;
    public List<Transform> enemiesInLevel;
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
    public GameObject waveCutscene;
    public float cutsceneDuration;
    public string waveObjective;
    public GameObject waveMissionIndicator;
}

[Serializable]
public class EnemiesInLevelTransforms
{
    public List<Transform>  enemiesInLevels;
}

