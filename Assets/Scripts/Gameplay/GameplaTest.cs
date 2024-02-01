using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaTest : MonoBehaviour
{
    [HideInInspector] public int
        _currentWaveToKeepActiveIndex;

    private int x;
    public Level[] level;
    public GameObject[] enemiesVariantsPrefabs;

    private void Start()
    {
        for (int j = 0;
             j < level[0].wavesInLevel[_currentWaveToKeepActiveIndex].enemyType
                 .Length;
             j++)
        {
            var E = Instantiate(enemiesVariantsPrefabs[CheckEnemiesType(j)],
                level[0].wavesInLevel[_currentWaveToKeepActiveIndex].enemyPosition[j]
                    .position,
                level[0].wavesInLevel[_currentWaveToKeepActiveIndex].enemyPosition[j]
                    .rotation);
            
            
        }
    }

    private int
        CheckEnemiesType(int i) // Method to check which enemy type is selected in the editor in a level wave so THAT particular enemy can be instantiated in its respective position using InitializeEnemies() method
    {
       
            switch (level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel[_currentWaveToKeepActiveIndex].enemyType[i])
            {
                case EnemyType.AlienSoldier:
                    x = 0;
                    return x;
                case EnemyType.TurnedSoldier:
                    x = 1;
                    return x;
                case EnemyType.Mutant:
                    x = 2;
                    return x;
                case EnemyType.Fly:
                    x = 3;
                    return x;
                case EnemyType.Phone:
                    x = 4;
                    return x;
                case EnemyType.ActionCollider:
                    x = 5;
                    return x;
                default:
                    return x;
            }

            
        

       
    }


    public enum EnemyType
    {
        AlienSoldier,
        TurnedSoldier,
        Mutant,
        Fly,
        Phone,
        ActionCollider
    }  
    
}
[Serializable]
public class Wave
{
    [Header("____Wave Objective Text to Display____")]
    public string waveObjective;

    [Header("____Wave Weapon____")] public GameObject waveWeapon;
    [Header("____Wave Enemies____")] public GameplaTest.EnemyType[] enemyType;

    [Header("____Wave Enemies Positions____")]
    public Transform[] enemyPosition;

    [HideInInspector] public List<Transform>
        enemiesGameObjectInWave; // Each level's waves' enemies gameobjects will be placed in this list according to their waves placement. 
}

[Serializable]
public class Level
{
  //  public MainMenuManager.LevelMap levelMap;

    [Header("____Level Cutscene____")] [Space(10)]
    public GameObject CutsceneGameobject;

    [Header("____Player Details____")] [Space(10)]
    public Transform playerPosition; // Add PlayerPosition for level

    [Header("____Waves Details____")] [Space(10)]
    public Wave[] waves; // Add Enemies Details in this array

    [HideInInspector] public List<Wave>
        wavesInLevel; //Total waves in each level will be added to this list to add and maintain functionality control over each wave's properties
}