/************************************************************
* COPYRIGHT:  Year
* PROJECT:    Name of Project or Assignment
* FILE NAME:  BallSpawn.cs
* DESCRIPTION: What project or system does this file belong to?
*                    
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2000/01/01 | Your Name | Created class
*
************************************************************/

using UnityEditor.PackageManager;
using UnityEngine;


/// <summary>
/// What does this specific class do when I reference it in C#?
/// </summary>
public class BallSpawn : MonoBehaviour
{
    #region Fields & Properties

    [SerializeField] private PoolBall ballPrefab;
    [SerializeField] private Material[] ballMaterials;
    
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxBalls = 10;

    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private LayerMask ballLayer;
    
    private int currBalls = 0;

    #endregion


   #region Initialization & Activation

    // Start is called once before the first Update
    private void Start()
    {
        InvokeRepeating(nameof(SpawnBall), 0f, spawnInterval);
    }//end Start()
    
   #endregion
   
    #region Spawning
    private void SpawnBall()
    {
        if (currBalls >= maxBalls)
            return;

        Bounds bounds = spawnArea.bounds;

        for (int i = 0; i < 20; i++)
        {
            Vector3 position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), bounds.center.y, Random.Range(bounds.min.z, bounds.max.z));
            if (Physics.CheckSphere(position, 0.055f, ballLayer, QueryTriggerInteraction.Ignore))
            {
                continue;
            }
            
            int index = Random.Range(0, ballMaterials.Length);

            PoolBall ball = Instantiate(ballPrefab, position, Quaternion.identity);

            ball.Initialize(index + 1, ballMaterials[index]);

            currBalls++;

            return;
        }
    }//end CustomMethod(int)

    #endregion
    

}//end BallSpawn