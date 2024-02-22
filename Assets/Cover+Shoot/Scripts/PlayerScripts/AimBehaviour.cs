using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2;

// AimBehaviour inherits from GenericBehaviour. This class corresponds to aim and strafe behaviour.
public class AimBehaviour : GenericBehaviour
{
    public static AimBehaviour instance;
    public string aimButton = "Aim", shoulderButton = "Aim Shoulder"; // Default aim and switch shoulders buttons.
    public Texture2D crosshair; // Crosshair texture.
    public float aimTurnSmoothing = 0.15f; // Speed of turn response when aiming to match camera facing.
    public Vector3 aimPivotOffset = new Vector3(0.5f, 1.2f, 0f); // Offset to repoint the camera when aiming.
    public Vector3 aimCamOffset = new Vector3(0f, 0.4f, -0.7f); // Offset to relocate the camera when aiming.

    private int aimBool; // Animator variable related to aiming.
    private bool aim; // Boolean to determine whether or not the player is aiming.
    private int cornerBool; // Animator variable related to cover corner..
    private bool peekCorner; // Boolean to get whether or not the player is on a cover corner.
    private Vector3 initialRootRotation; // Initial root bone local rotation.
    private Vector3 initialHipsRotation; // Initial hips rotation related to the root bone.
    private Vector3 initialSpineRotation; // Initial spine rotation related to the root bone.

    private ShootBehaviour shootBehavior;
    private InteractiveWeapon interactiveWeapon;

    private WeaponUIManager weaponUIManager;

    // Start is always called after any Awake functions.
    void Start()
    {
        // Set up the references.
        aimBool = Animator.StringToHash("Aim");

        cornerBool = Animator.StringToHash("Corner");

        // Get initial bone rotation values.
        Transform hips = behaviourManager.GetAnim.GetBoneTransform(HumanBodyBones.Hips);
        Transform spine = behaviourManager.GetAnim.GetBoneTransform(HumanBodyBones.Spine);
        Transform root = hips.parent;
        // Correctly set the hip and root bones.
        if (spine.parent != hips)
        {
            root = hips;
            hips = spine.parent;
        }

        initialRootRotation = (root == transform) ? Vector3.zero : root.localEulerAngles;
        initialHipsRotation = hips.localEulerAngles;
        initialSpineRotation = behaviourManager.GetAnim.GetBoneTransform(HumanBodyBones.Spine).localEulerAngles;
        shootBehavior = ShootBehaviour.instance;
        if (shootBehavior == null)
        {
            Debug.LogWarning(
                "ShootBehaviour not found or not initialized properly. Make sure ShootBehaviour is present in the scene.");
        }

        interactiveWeapon = InteractiveWeapon.instance;
        weaponUIManager = WeaponUIManager.instance;
    }

    // Update is used to set features regardless the active behaviour.
    void Update()
    {
        peekCorner = behaviourManager.GetAnim.GetBool(cornerBool);

        // Activate/deactivate aim by input.
        if (ControlFreak2.CF2Input.GetAxisRaw(aimButton) != 0 && !aim)
        {
            StartCoroutine(ToggleAimOn());
        }
        else if (aim && ControlFreak2.CF2Input.GetAxisRaw(aimButton) == 0)
        {
            StartCoroutine(ToggleAimOff());
        }

        // No sprinting while aiming.
        canSprint = !aim;

        // Toggle camera aim position left or right, switching shoulders.
        if (aim && ControlFreak2.CF2Input.GetButtonDown(shoulderButton) && !peekCorner)
        {
            aimCamOffset.x = aimCamOffset.x * (-1);
            aimPivotOffset.x = aimPivotOffset.x * (-1);
        }

        // Set aim boolean on the Animator Controller.
        behaviourManager.GetAnim.SetBool(aimBool, aim);
    }

    // Co-rountine to start aiming mode with delay.
    public IEnumerator ToggleAimOn()
    {
        yield return new WaitForSeconds(0.05f);
        // Aiming is not possible.
        if (behaviourManager.GetTempLockStatus(this.behaviourCode) || behaviourManager.IsOverriding(this))
            yield return false;

        // Start aiming.
        else
        {
            aim = true;
            int signal = 1;
            if (peekCorner)
            {
                signal = (int)Mathf.Sign(behaviourManager.GetH);
            }

            aimCamOffset.x = Mathf.Abs(aimCamOffset.x) * signal;
            aimPivotOffset.x = Mathf.Abs(aimPivotOffset.x) * signal;
            yield return new WaitForSeconds(0.1f);
            behaviourManager.GetAnim.SetFloat(speedFloat, 0);
            // This state overrides the active one.
            behaviourManager.OverrideWithBehaviour(this);
        }
    }

    // Co-rountine to end aiming mode with delay.
    public IEnumerator ToggleAimOff()
    {
        Debug.Log("Turning Off Aim");
        aim = false;
        yield return new WaitForSeconds(0.3f);
        behaviourManager.GetCamScript.ResetTargetOffsets();
        behaviourManager.GetCamScript.ResetMaxVerticalAngle();
        yield return new WaitForSeconds(0.05f);
        behaviourManager.RevokeOverridingBehaviour(this);
    }

    // LocalFixedUpdate overrides the virtual function of the base class.
    public override void LocalFixedUpdate()
    {
        // Set camera position and orientation to the aim mode parameters.
        if (aim)
            behaviourManager.GetCamScript.SetTargetOffsets(aimPivotOffset, aimCamOffset);
    }

    // LocalLateUpdate: manager is called here to set player rotation after camera rotates, avoiding flickering.
    public override void LocalLateUpdate()
    {
        AimManagement();
    }

    private int weapon;

    private bool Boolean;

    private List<InteractiveWeapon> weapons;


    // Handle aim parameters when aiming is active.
    void AimManagement()
    {
        // Deal with the player orientation when aiming.
        Rotating();

        // Target was hit.


        SetShootBehavior();
        AutoFire();
    }

    void SetShootBehavior()
    {
        if (shootBehavior == null)
        {
            shootBehavior = ShootBehaviour.instance;

            if (shootBehavior == null)
            {
                Debug.LogError(
                    "ShootBehaviour not found or not initialized properly. Make sure ShootBehaviour is present in the scene.");
                return; // Exit the method if shootBehavior is still null
            }
        }
    }

    public void AutoFire()
    {
        Ray ray = new Ray(behaviourManager.playerCamera.position, behaviourManager.playerCamera.forward * 1000);
        RaycastHit hit;
        if (shootBehavior == null)
        {
            Debug.LogError(
                "ShootBehaviour not found or not initialized properly. Make sure ShootBehaviour is present in the scene.");
        }

        if (Physics.Raycast(ray, out hit, 500f))
        {
            if (hit.collider.transform != this.transform)
            {
                if (hit.collider.tag == "Enemy" && shootBehavior != null)
                {
                    if (!shootBehavior.isShooting && shootBehavior.activeWeapon > 0 &&
                        shootBehavior.burstShotCount == 0)
                    {
                        shootBehavior.isShooting = true;
                        shootBehavior.ShootWeapon(shootBehavior.activeWeapon);

                        if (weaponUIManager.bulletsLeftInMag <= 0)
                        {
                            if (shootBehavior.weapons[shootBehavior.activeWeapon].StartReload())
                            {
                                AudioSource.PlayClipAtPoint(
                                    shootBehavior.weapons[shootBehavior.activeWeapon].reloadSound,
                                    shootBehavior.gunMuzzle.position, 0.5f);
                                behaviourManager.GetAnim.SetBool(shootBehavior.reloadBool, true);
                            }
                        }
                    }
                }
            }
        }
    }

    // Rotate the player to match correct orientation, according to camera.
    void Rotating()
    {
        Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Always rotates the player according to the camera horizontal rotation in aim mode.
        Quaternion targetRotation = Quaternion.Euler(0, behaviourManager.GetCamScript.GetH, 0);

        float minSpeed = Quaternion.Angle(transform.rotation, targetRotation) * aimTurnSmoothing;

        // Peeking corner situation.
        if (peekCorner)
        {
            // Rotate only player upper body when peeking a corner.
            transform.rotation = Quaternion.LookRotation(-behaviourManager.GetLastDirection());
            targetRotation *= Quaternion.Euler(initialRootRotation);
            targetRotation *= Quaternion.Euler(initialHipsRotation);
            targetRotation *= Quaternion.Euler(initialSpineRotation);
            Transform spine = behaviourManager.GetAnim.GetBoneTransform(HumanBodyBones.Spine);
            spine.rotation = targetRotation;
        }
        else
        {
            // Rotate entire player to face camera.
            behaviourManager.SetLastDirection(forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minSpeed * Time.deltaTime);
        }
        //Debug.Log("CALLED");
    }

    private void FixedUpdate()
    {
    }

    // Draw the crosshair when aiming.
    /*void OnGUI()
    {
        if (crosshair)
        {
            float mag = behaviourManager.GetCamScript.GetCurrentPivotMagnitude(aimPivotOffset);
            if (mag < 0.05f)
                GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshair.width * 0.75f),
                    Screen.height / 2 - (crosshair.height * 0.75f),
                    crosshair.width, crosshair.height), crosshair);
            //	GUI.color=Color.red;
        }
    }*/
    void OnGUI()
    {
        if (shootBehavior==null)
        {
            shootBehavior=ShootBehaviour.instance;
        }

        if (shootBehavior.activeWeapon==0) return;
        float mag = behaviourManager.GetCamScript.GetCurrentPivotMagnitude(aimPivotOffset);
            
                GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshair.width * 0.75f),
                    Screen.height / 2 - (crosshair.height * 0.75f),
                    crosshair.width, crosshair.height), crosshair);
            //	GUI.color=Color.red;
        
    }
}