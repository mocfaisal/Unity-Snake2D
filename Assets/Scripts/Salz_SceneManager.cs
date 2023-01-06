using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Salz_SceneManager : MonoBehaviour
{
    public bool isEscapeToExit;
    public static Salz_SceneManager instance;
    public GameHandler gameHandler;

    void Awake()
    {
        instance = this;
    }


    public void StartNewGame()
    {
        SceneManager.LoadScene("Scene_Level_1");
    }

    public void ContinueGame()
    {
        int intLevel = PlayerPrefs.GetInt("level", 1);

        SceneManager.LoadScene("Scene_Level_" + intLevel.ToString());
    }

    public void StartGame()
    {
        int levelGame = gameHandler.next_level;

        switch (levelGame)
        {
            default:
            case 1: SceneManager.LoadScene("Scene_Level_1"); break;
            case 2: SceneManager.LoadScene("Scene_Level_2"); break;
            case 3: SceneManager.LoadScene("Scene_Level_3"); break;
            case 0: SceneManager.LoadScene("Scene_MainMenu"); break;
        }
    }

    public void RestartGame()
    {
        int levelGame = gameHandler.curr_level;

        switch (levelGame)
        {
            default:
            case 1: SceneManager.LoadScene("Scene_Level_1"); break;
            case 2: SceneManager.LoadScene("Scene_Level_2"); break;
            case 3: SceneManager.LoadScene("Scene_Level_3"); break;
        }
    }

    public void tutorScene()
    {
        SceneManager.LoadScene("Scene_Tutor");
    }

    public void infoScene()
    {
        SceneManager.LoadScene("Scene_Info");
    }

    public void KembaliKeMainMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void exitGame()
    {
        // will work on build version
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isEscapeToExit)
            {
                // will work on build version
                Application.Quit();
            }
            else
            {
                KembaliKeMainMenu();
            }
        }
    }
}
