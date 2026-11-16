/************************************************************
* COPYRIGHT:  2026
* PROJECT:    ShmupGame
* FILE NAME:  PocketCollision.cs
* DESCRIPTION: Pocket collision manager
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2026/11/04 | Elijah Ciali | Created class
*
************************************************************/

using UnityEngine;


/// <summary>
/// Handles ball detection handling in pocket
/// </summary>
public class PocketCollision : MonoBehaviour
{
    #region Fields & Properties

    [SerializeField] private GameManager gameManager;
    [SerializeField] private BallSpawn ballSpawner;
    [SerializeField] private Rigidbody cueBallRb;

    // Declare serialized fields and private variables here

    #endregion
   
   #region Updates

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.IsGameOver)
            return;

        // Cue ball ends the game
        if (other.attachedRigidbody == cueBallRb)
        {
            gameManager.EndGame();
            return;
        }

        // Get actual ball object
        PoolBall ball = other.GetComponentInParent<PoolBall>();

        // Eight ball ends the game
        if (ball.Number == 8)
        {
            gameManager.EndGame();
            return;
        }

        // Normal ball
        gameManager.AddPoints(ball.Number);
        ballSpawner.BallPocketed();

        Destroy(ball.gameObject);


    }//end Update()

    #endregion

}//end PocketCollision