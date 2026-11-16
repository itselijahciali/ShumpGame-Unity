/************************************************************
* COPYRIGHT:  2026
* PROJECT:    ShmupGame
* FILE NAME:  BallSpawn.cs
* DESCRIPTION: Ball Spawner
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2026/11/04 | Elijah Ciali | Created class
*
************************************************************/


using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


/// <summary>
/// Manages ball spawning in gameplay
/// </summary>
public class BallSpawn : MonoBehaviour
{
    #region Fields & Properties

    [SerializeField] private PoolBall ballPrefab;
    [SerializeField] private Material[] ballMaterials;
    
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxBalls = 10;

    [SerializeField] private BoxCollider spawnArea;
    
    private int currBalls = 0;

    #endregion


   #region Initialization & Activation

   public void StartSpawn()
   {
       InvokeRepeating(nameof(SpawnBall), 0f, spawnInterval);
   }
    
   #endregion
   
    #region Spawning
    private void SpawnBall()
    {
        if (currBalls >= maxBalls)
            return;

        Bounds bounds = spawnArea.bounds;
            
        // Spawn within bounds (just an empty with a box collider)
        Vector3 position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), bounds.center.y, Random.Range(bounds.min.z, bounds.max.z));
        // Originally checked if colliding with ball by using a ball layer but they all spawn above so it'll never spawn inside another ball
        
        int index = Random.Range(0, ballMaterials.Length);

        PoolBall ball = Instantiate(ballPrefab, position, Quaternion.identity);

        ball.Initialize(index + 1, ballMaterials[index]);

        currBalls++;
    }//end CustomMethod(int)

    #endregion
    
    // Called by PocketCollision to keep track of how many balls are present. Maybe number of balls should be inside GameManager?
    public void BallPocketed()
    {
        currBalls--;
    }//end BallPocketed()
    

}//end BallSpawn