/************************************************************
* COPYRIGHT:  2026
* PROJECT:    ShmupGame
* FILE NAME:  PoolBall.cs
* DESCRIPTION: Initialization logic for Pool Ball
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2026/11/04 | Elijah Ciali | Created class
*
************************************************************/

using UnityEngine;


/// <summary>
/// Adds material and points for pool ball lifecycle
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