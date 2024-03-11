using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelData", order = 1)]
public class Scriptable : ScriptableObject
{
   

 public bool bShouldPlayCutscene;

      //  public static GameplayManager instance;
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
        public List<Transform> enemiesInLevel;

        public List<Transform> hudElement;
        [SerializeField]private GameObject postProcessVolume;
        private RCC_CarControllerV3 rccCar;

        public enum EnemyType
        {
            Enemy_ak47,
            Enemy_m16,
            Enemy_Pistol,
            Interactable,
        }


     //   private StateController stateController;
        private DataController dataController;
        private GameUIManager gameUIManager;
        private GameplayCarController gameplayCarController;
        private InteractiveWeapon interactiveWeapon;
       
        private int y;

        private int
            waveToKeepActive; //Introduce removeAt0 functionality by adding the wave to a list and then initializing waves again if waves are >0

}
