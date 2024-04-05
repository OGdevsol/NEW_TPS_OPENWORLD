using System;
using System.Collections;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This class is created for the example scene. There is no support for this script.
public class SimplePlayerHealth : HealthManager
{
    private AimBehaviour aimBehaviour;
    private GameUIManager gameUIManager;
    public float health = 100f;

    public Transform canvas;
    public GameObject hurtPrefab;
    public float decayFactor = 0.8f;

    private HurtHUD hurtUI;
    private MissionFailManager missionFailManager;
    private Rigidbody[] playerRigidBodies;
    private Animator playerAnimator;
    public ControlFreak2.TouchButton aim;
    private ShootBehaviour shootBehaviour;
    private GameplayManager gameplayManager;
    public Slider healthSlider;
    public Image bloodSplatter;

    private void OnEnable()
    {
        MissionFailManager.onMissionFail += FailMission;
    }

    private void OnDisable()
    {
        MissionFailManager.onMissionFail -= FailMission;
    }

   

    private void Awake()
    {
        AudioListener.pause = false;
        hurtUI = this.gameObject.AddComponent<HurtHUD>();
        hurtUI.Setup(canvas, hurtPrefab, decayFactor, this.transform);
        missionFailManager = MissionFailManager.Instance;

        playerRigidBodies = gameObject.GetComponentsInChildren<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
        aimBehaviour = gameObject.GetComponent<AimBehaviour>();
        shootBehaviour=ShootBehaviour.instance;
        gameUIManager=GameUIManager.instance;
        gameplayManager=GameplayManager.instance;
        
        
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart,
        GameObject origin)
    {
        health -= damage;

        float maxOpacity = 255f; // Maximum opacity value
        float currentOpacity = Mathf.Lerp(0f, maxOpacity, 1f - (health / 500)); // Adjust opacity based on health
        byte opacityByte = (byte)Mathf.Clamp(currentOpacity, 0f, maxOpacity); // Convert opacity to byte (0-255 range)

        // Set blood splatter opacity
        Color bloodSplatterColor = bloodSplatter.color;
        bloodSplatterColor.a = opacityByte / 255f; // Set alpha value
        bloodSplatter.color = bloodSplatterColor;
        /*if (hurtPrefab && canvas)
            hurtUI.DrawHurtUI(origin.transform, origin.GetHashCode());*/
    }

    public void OnGUI()
    {
        if (health > 0f)
        {
            GUIStyle textStyle = new GUIStyle
            {
                fontSize = 1
            };
            textStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(0, Screen.height - 60, 30, 30), health.ToString(), textStyle);
            healthSlider.value = health;
        }
        else if (!dead)
        {
            dead = true;
            FailMission();
           
            //StartCoroutine(nameof(ReloadScene));
        }
    }

    private IEnumerator KillPlayer()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        aim.toggle = false;
        if (!shootBehaviour)
        {
            shootBehaviour = ShootBehaviour.instance;
            if (shootBehaviour.activeWeapon!=0)
            {
                ShootBehaviour.instance.DropWeaponOnDeath();
                playerAnimator.Play("Death");

                StartCoroutine(GameplayManager.instance.LevelFailRoutine(gameUIManager.levelFailPanelDeath));
            }
            else
            {
                playerAnimator.Play("Death");

                StartCoroutine(GameplayManager.instance.LevelFailRoutine(gameUIManager.levelFailPanelDeath));
            }
           
        }
       
     
    }
    
    public void FailMission()
    {
        // Trigger the event when the level is complete
        
        Debug.Log("Mission Failed, You Died");
        StartCoroutine(KillPlayer());
        gameUIManager.DisableObjectsOnPlayerDeath();
        
        StartCoroutine(PlayerDeathSlowmotion());
    }

    private IEnumerator PlayerDeathSlowmotion()
    {
        Time.timeScale = 0.45f;
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 1;
    }

    private IEnumerator ReloadScene()
    {
        SendMessage("PlayerDead", SendMessageOptions.DontRequireReceiver);
        yield return new WaitForSeconds(0.5f);
        canvas.gameObject.SetActive(false);
        AudioListener.pause = true;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black;
        Camera.main.cullingMask = LayerMask.GetMask();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(0);
    }
}