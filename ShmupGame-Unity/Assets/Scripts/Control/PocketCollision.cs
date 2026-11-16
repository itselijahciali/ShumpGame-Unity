/************************************************************
* COPYRIGHT:  Year
* PROJECT:    Name of Project or Assignment
* FILE NAME:  PocketCollision.cs
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