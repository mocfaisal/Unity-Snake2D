using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    [SerializeField] private Snake snake;
    [SerializeField] private ScoreWindow scoreWindow;
    private LevelGrid levelGrid;
    private static int curr_score;
    private static int HighScoreInt;
    public int curr_level;
    public int next_level;
    public int req_score;
    //private int CurrScoreInt;
    public static string indexHighscore = "highscore";
    public AudioClip diedSound;
    public AudioClip coinSound;
    public AudioClip completeSound;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HighScoreInt = PlayerPrefs.GetInt(indexHighscore, 0);

        Debug.Log("GameHandler.Start");

        /* int number = 0;

         FunctionPeriodic.Create(() =>
         {
             CMDebug.TextPopupMouse("Ding! " + number);
             number++;
         }, .3f);*/

        // ukuran box grid game
        levelGrid = new LevelGrid(50, 50);
        snake.Setup(levelGrid);

        levelGrid.Setup(snake);
        levelGrid.setScoreWindow(scoreWindow);
        //Debug.Log("HighScoreInt : " + HighScoreInt.ToString());
        curr_score = 0;

    }


    public static int GetScore()
    {
        return curr_score;
    }

    public static void AddScore()
    {
        curr_score++;

        if (HighScoreInt <= curr_score)
        {
            PlayerPrefs.SetInt(indexHighscore, curr_score);
        }
        //Debug.Log("curr_score : " + curr_score.ToString());
        //Debug.Log("HighScoreInt : " + HighScoreInt.ToString());

    }

    public static int getHighScore()
    {
        return HighScoreInt;
    }
}
