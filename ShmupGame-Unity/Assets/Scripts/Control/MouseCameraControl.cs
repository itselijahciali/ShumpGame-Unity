/************************************************************
* COPYRIGHT:  2026
* PROJECT:    ShmupGame
* FILE NAME:  MouseCameraControl.cs
* DESCRIPTION: Control logic for Mouse Control
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2026/11/04 | Elijah Ciali | Created class
*
************************************************************/

using UnityEngine;

using UnityEngine.InputSystem;

/// <summary>
/// Moves camera with slight parralax according to mouse
/// </summary>
public class MouseCameraControl : MonoBehaviour
{
    #region Fields & Properties

    // Declare serialized fields and private variables here
    [SerializeField] private float strength = 0.3f;
    [SerializeField] private float smoothSpeed = 5f;

    #endregion

    private Vector3 startPosition;
    
   #region Initialization & Activation

    // Start is called once before the first Update
    private void Start()
    {
        startPosition = transform.position;
    }//end Start()
    
   #endregion
   
   #region Updates

    // LateUpdate is called last per frame (to avoid conflict with cue positioning)
    private void LateUpdate()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Normalize mouse position
        float x = (mousePos.x / Screen.width - 0.5f) * 2f;
        float y = (mousePos.y / Screen.height - 0.5f) * 2f;

        x = Mathf.Clamp(x, -1f, 1f);
        y = Mathf.Clamp(y, -1f, 1f);

        // Offset relative to the camera's orientation
        Vector3 offset = transform.right * x * strength + transform.up * y * strength;

        Vector3 targetPosition = startPosition + offset;

        // Move to target
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime));
    }//end Update()

    #endregion
    
}//end MouseCameraControl