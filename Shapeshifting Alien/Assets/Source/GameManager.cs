using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game settings")]
    [Tooltip("The player character GameObject")]
    public GameObject player;
    [Tooltip("Location where the player character spawns and respawns")]
    public Vector3 spawnPos;
    [Tooltip("whether the game is paused or not")]
    public static bool isPaused;
    [Header("Scene Names")]
    [Tooltip("The name of the main menu scene")]
    public string mainMenuScene = "MainMenu";
    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
    }

    // fully setup the scene with anything that won't spawn naturally
    public void SetupScene()
    {

    }

    //setup the scene for the player to spawn
    public void SetupSceneForSpawn()
    {
        player.GetComponent<Health>().Reset();
        player.transform.position = spawnPos;

    }

    public static void Pause()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public static void GameOver(bool win)
    {

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
