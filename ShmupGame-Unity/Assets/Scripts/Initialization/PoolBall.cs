/************************************************************
* COPYRIGHT:  Year
* PROJECT:    Name of Project or Assignment
* FILE NAME:  PoolBall.cs
* DESCRIPTION: What project or system does this file belong to?
*                    
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2000/01/01 | Your Name | Created class
*
************************************************************/

using UnityEngine;


/// <summary>
/// What does this specific class do when I reference it in C#?
/// </summary>
public class PoolBall : MonoBehaviour
{
    #region Fields & Properties

    // Declare serialized fields and private variables here

    [SerializeField] private MeshRenderer ballRenderer; // Access materials
    
    public int Number { get; private set; } // Define the point value for ball

    #endregion

    // Called by GameManager
    public void Initialize(int number, Material material)
    {
        Number = number;
        ballRenderer.sharedMaterial = material;

        // Add slight rotation on fall
        GetComponent<Rigidbody>().AddForce(
            new Vector3(
                Random.Range(-0.3f, 0.3f),
                0,
                Random.Range(-0.3f, 0.3f)
            ),
            ForceMode.VelocityChange
        );
        GetComponent<Rigidbody>().maxAngularVelocity = 200f;
    }
    
}//end PoolBall