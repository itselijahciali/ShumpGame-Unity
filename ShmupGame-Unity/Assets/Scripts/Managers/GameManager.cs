/************************************************************
* COPYRIGHT:  2026
* PROJECT:    ShmupGame
* FILE NAME:  GameManager.cs
* DESCRIPTION: Game loop management
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2026/11/04 | Elijah Ciali | Created class
*
************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Manages points and game state
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields & Properties

    // References for UI
    [SerializeField] private Text gameText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private GameObject instructionsText;
    
    // Reference to BallSpawn.cs script on same GameManager object
    private BallSpawn ballSpawner;

    // Initial properties
    private float timeLeft = 60f;
    private int score = 0;
    private bool started = false;
    
    public bool IsGameOver { get; private set; }
    
    #endregion
    
   #region Initialization & Activation

    // Awake is called once on initialization
    private void Awake()
    {
        ballSpawner = GetComponent<BallSpawn>();
    }//end Awake()

    // Start is called once before the first Update
    private void Start()
    {
        gameOverPanel.SetActive(false);

    }//end Start()
    
   #endregion
   
   #region Updates

    // Update is called once per frame
    private void Update()
    {
        // Update timer
        if (started && !IsGameOver)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                EndGame();
            }
        }

        // Update timer UI
        gameText.text = "Time: " + Mathf.CeilToInt(timeLeft) + " | Score: " + score;
        
    }//end Update()

    #endregion
    
    public void StartGame()
    {
        // Probably inefficient this is called by PoolCueControl on every hit but I was in a hurry to program this
        if (started || IsGameOver)
            return;
        
        started = true;

        // Hide instructions
        instructionsText.SetActive(false);

        ballSpawner.StartSpawn();
    }//end StartGame()

    // Called by PocketCollision
    public void AddPoints(int points)
    {
        if (timeLeft > 0)
            score += points;
    }//end AddPoints()
    
    // Called by PocketCollision
    public void EndGame()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        // Disable spawning
        ballSpawner.CancelInvoke();

        finalScoreText.text = "Final Score: " + score;
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }


}//end GameManager