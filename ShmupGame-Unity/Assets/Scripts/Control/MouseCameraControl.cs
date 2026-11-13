/************************************************************
* COPYRIGHT:  Year
* PROJECT:    Name of Project or Assignment
* FILE NAME:  MouseCameraControl.cs
* DESCRIPTION: What project or system does this file belong to?
*                    
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2000/01/01 | Your Name | Created class
*
************************************************************/

using UnityEngine;

using UnityEngine.InputSystem;

/// <summary>
/// What does this specific class do when I reference it in C#?
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