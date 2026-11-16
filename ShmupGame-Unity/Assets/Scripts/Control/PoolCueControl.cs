/************************************************************
* COPYRIGHT:  2026
* PROJECT:    ShmupGame
* FILE NAME:  PoolCueControl.cs
* DESCRIPTION: Control logic for Pool Cue
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2026/11/04 | Elijah Ciali | Created class
*
************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Controls the pool cue.
/// </summary>
public class PoolCueControl : MonoBehaviour
{
    #region Fields & Properties

    // Declare serialized fields and private variables here
    [Header("References")]
    [SerializeField] private Rigidbody cueBallRb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameManager gameManager;

    [Header("Aiming")]
    [SerializeField] private float maxPitch = 45f;
    [SerializeField] private float maxCueDistance = 0.8f;
    [SerializeField] private float followSpeed = 15f;

    [Header("Shooting")]
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float maxPullback = 1f;
    [SerializeField] private float maxShotSpeed = 6f;
    [SerializeField] private float shotCooldown = 0.35f;

    private enum State
    {
        Aiming,
        Charging,
        Shooting
    }

    private State state = State.Aiming;

    private float ballRadius;
    private float charge;
    private float chargeStartDistance;

    private Vector3 BallPosition => cueBallRb.position;
    
    #endregion


   #region Initialization & Activation

    // Awake is called once on initialization
    private void Awake()
    {
        // Get references
        SphereCollider ballCollider =
            cueBallRb.GetComponent<SphereCollider>();

        Vector3 scale = ballCollider.transform.lossyScale;

        // I scale the ball so I have to do this
        ballRadius = ballCollider.radius *
                     Mathf.Max(
                         Mathf.Abs(scale.x),
                         Mathf.Abs(scale.y),
                         Mathf.Abs(scale.z)
                     );

    }//end Awake()
    
   #endregion
   
   #region Updates

    // Update is called once per frame
    private void Update()
    {
        if (gameManager.IsGameOver || cueBallRb == null) // Pockets delete ball
            return;

        switch (state)
        {
            // Keep state so that only one action is being takes at once
            case State.Aiming:
                Aim();

                if (Mouse.current.leftButton.wasPressedThisFrame)
                    ChargeInit();

                break;
            case State.Charging:
                Charge();
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                    StartCoroutine(Shoot());
                
                break;
            case State.Shooting:
                break;
        }
    }// end Update()
    
    #endregion
    
    #region Aiming Routine

    private void Aim()
    {
        // Project the mouse onto a plane where the ball is using the camera
        Plane plane = new Plane(Vector3.up, BallPosition);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!plane.Raycast(ray, out float dist))
            return;

        Vector3 offset = ray.GetPoint(dist) - BallPosition;
        offset.y = 0;

        // If the mouse is over the ball don't continue
        if (offset.sqrMagnitude < 0.001f)
            return;

        float t = Mathf.InverseLerp(0.1f, 2f, offset.magnitude);

        float pitch = Mathf.Lerp(maxPitch, 4f, t);

        float cueDistance = Mathf.Lerp(ballRadius + 0.02f, maxCueDistance, t);

        Quaternion rotation = Quaternion.LookRotation(offset.normalized) * Quaternion.Euler(-pitch, 0f, 0f);

        transform.rotation = rotation;
        
        Vector3 targetPosition = BallPosition + transform.forward * cueDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition,
            1f - Mathf.Exp(-followSpeed * Time.deltaTime));

    }// end Aim()
    #endregion
    
    #region Charging Routine

    private void ChargeInit()
    {
        // Set up charge
        state = State.Charging;
        charge = 0f;

        chargeStartDistance = Vector3.Distance(transform.position, BallPosition);
    }

    private void Charge()
    {
        // Pull back
        charge = charge + Time.deltaTime / maxChargeTime;
        float pullback = charge * maxPullback;
        transform.position = BallPosition + transform.forward * (chargeStartDistance + pullback);
    }

    #endregion
    
    #region Shooting

    private IEnumerator Shoot()
    {
        state = State.Shooting;

        // Determine target for ball (where cue is pointing)
        Vector3 forward = transform.forward;
        
        Vector3 shotDirection = -forward;
        shotDirection.y = 0;
        shotDirection.Normalize();
        Vector3 hitPosition = BallPosition + forward * ballRadius;

        // Move towards ball
        yield return MoveCue(hitPosition, Mathf.Lerp(0.15f, 0.05f, charge)
        );
        
        gameManager.StartGame(); // Only runs once, game starts on first hit
        
        // Hit ball
        cueBallRb.AddForce(shotDirection * charge * maxShotSpeed, ForceMode.VelocityChange
        );

        // Move more
        yield return MoveCue(hitPosition - forward * 0.1f, 0.1f);

        // Wait before resuming aim
        yield return new WaitForSeconds(shotCooldown);

        state = State.Aiming;
    }
    
    private IEnumerator MoveCue(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            
            transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }

        transform.position = target;
    } //end MoveCue()
    
    #endregion
    
}//end PoolCueControl