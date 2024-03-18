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
    [Serializable]
    public enum MissionType
    {
        CutsceneAtStart,
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

/*[System.Serializable]
public class Levels
{
    [Serializable]
    public enum CuttSenEnum
    {
        Yes,
        No,
    }

    public Enemy[] enemies;
    public CuttSenEnum cutSceneEnum;
    public Transform startPos, endPos;
   // public GameObject  levels; 
    public GameObject cutsceen;
    public float cutsceneDuration;

}

[Serializable]
public class Enemy
{
    public Transform position;
    public GameObject weapon;
    
}

[Serializable]
public class Weapon
{
    public GameObject weaponMesh;
    seria
    public enum MyEnum
    {
        
    }
}*/


/*
[System.Serializable]*/
/*public class Levels
{
    [Serializable]
    public enum levelLogic
    {
        hasCutscene,
        noCutscene,
    }

    public levelLogic LevelLogic;
    public GameObject cutscene;
    public Transform pickPos;
    public Transform dropPos;
    public PickupPeople[] pickpPeople;
    public GameObject[] dropPeople;
    

}

[Serializable]
public class PickupPeople
{
    public Transform thisPersonTransform;
    public Transform[] waypoints;
}*/