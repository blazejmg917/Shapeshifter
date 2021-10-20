using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game settings")]
    [Tooltip("The player character GameObject")]
    public GameObject player;
    [Tooltip("Location where the player character spawns and respawns")]
    public Vector3 spawnPos;
    [Tooltip("whether the game is paused or not")]
    public bool isPaused;
    [Header("Scene Names")]
    [Tooltip("The name of the main menu scene")]
    public string mainMenuScene = "MainMenu";
    [Header("UI pieces")]
    [Tooltip("the Pause Button Canvas")]
    public Canvas pauseButtonCanvas;
    [Tooltip("the Pause Screen Canvas")]
    public Canvas pausedCanvas;
    [Tooltip("the Game Over Canvas")]
    public Canvas gameOverCanvas;
    [Tooltip("the Game Over text box")]
    public Text gameOverMessage;
    [Tooltip("the message to display when you win")]
    public string winMessage = "You Win!";
    [Tooltip("the message to display when you lose")]
    public string loseMessage = "Game Over";


    //instance of the GameManager to return
    private static GameManager instance;


    public static GameManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        //Before enabling the object, we need to make sure that there are no other GameManagers.
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
        
    }

    // fully setup the scene with anything that won't spawn naturally
    public void SetupScene()
    {
        SetupSceneForSpawn();
        pauseButtonCanvas.gameObject.SetActive(true);
        pausedCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    //setup the scene for the player to spawn
    public void SetupSceneForSpawn()
    {
        player.GetComponent<Health>().Reset();
        player.transform.position = spawnPos;

    }

    public void Pause()
    {
        if (isPaused)
        {
            pauseButtonCanvas.gameObject.SetActive(true);
            pausedCanvas.gameObject.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
        else
        {
            pauseButtonCanvas.gameObject.SetActive(false);
            pausedCanvas.gameObject.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void GameOver(bool win)
    {
        Time.timeScale = 0;
        gameOverCanvas.gameObject.SetActive(true);
        pauseButtonCanvas.gameObject.SetActive(false);
        pausedCanvas.gameObject.SetActive(false);
        if (win)
        {
            gameOverMessage.text = winMessage;
        }
        else
        {
            gameOverMessage.text = loseMessage;
        }
    }

    //used to exit to the main menu
    public void QuitToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    //used to fully exit the game
    public void QuitToDesktop()
    {
        Application.Quit();
    }


}
