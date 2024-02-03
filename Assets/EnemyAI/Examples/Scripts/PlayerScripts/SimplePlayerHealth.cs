using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        gameUIManager=GameUIManager.instance;
        
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart,
        GameObject origin)
    {
        health -= damage;

        if (hurtPrefab && canvas)
            hurtUI.DrawHurtUI(origin.transform, origin.GetHashCode());
    }

    public void OnGUI()
    {
        if (health > 0f)
        {
            GUIStyle textStyle = new GUIStyle
            {
                fontSize = 50
            };
            textStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(0, Screen.height - 60, 30, 30), health.ToString(), textStyle);
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
        playerAnimator.Play("Death");
    }
    
    public void FailMission()
    {
        // Trigger the event when the level is complete
        
        Debug.Log("Mission Failed, You Died");
        StartCoroutine(KillPlayer());
        gameUIManager.DisableObjectsOnPlayerDeath();
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