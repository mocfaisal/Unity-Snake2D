using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    public static ScoreWindow instance;
    public GameHandler gameHandler;

    private Text score_count_txt;
    private Text high_score_count_txt;
    private Text level_text;

    private Text panel_title_text;
    private Text panel_subtitle_text;
    GameObject panel_title;
    GameObject panel_subtitle;
    GameObject panelScore;
    GameObject success_panel;
    GameObject failed_panel;
    private Button btn_nextLevel;

    private static int HighScoreInt;
    int currLevel;
    public int CurrScoreInt;
    int req_score;

    void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        score_count_txt = transform.Find("score_count").GetComponent<Text>();
        high_score_count_txt = transform.Find("high_score_count").GetComponent<Text>();
        level_text = transform.Find("level_label").GetComponent<Text>();
        HighScoreInt = getHighScore();
        currLevel = gameHandler.curr_level;

        level_text.text = "Level : " + currLevel.ToString();
        high_score_count_txt.text = "" + HighScoreInt;
        req_score = gameHandler.req_score;
        //Debug.Log("req_score : " + req_score.ToString());
        PlayerPrefs.SetInt("level", currLevel);

        // Panel Score
        panelScore = FindInActiveObjectByName("panel_finish");
        success_panel = FindInActiveObjectByName("success_panel");
        failed_panel = FindInActiveObjectByName("failed_panel");

        panel_title_text = FindInActiveObjectByName("panel_title").GetComponent<Text>();
        panel_subtitle_text = FindInActiveObjectByName("panel_subtitle").GetComponent<Text>();
        btn_nextLevel = FindInActiveObjectByName("btn_next_level").GetComponent<Button>();
        //panel_title = FindInActiveObjectByName("panel_title");
        //panel_subtitle = FindInActiveObjectByName("panel_subtitle");

        panelScore.SetActive(false);

        //panel_title_text = panel_title.GetComponent<Text>();
        //panel_subtitle_text = panel_subtitle.GetComponent<Text>();

        success_panel.SetActive(false);
        failed_panel.SetActive(false);
        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {
        // static method function
        CurrScoreInt = GameHandler.GetScore();
        score_count_txt.text = CurrScoreInt.ToString();
    }

    private static int getHighScore()
    {
        return PlayerPrefs.GetInt(GameHandler.indexHighscore, 0); ;
    }

    public bool showScorePanel(bool isFinish = false, int LastScore = 0)
    {
        bool is_finish = false;
        string txtTitle = "";
        string txtSubtitle = "";

        //Debug.Log("CurrLevel : " + currLevel.ToString());
        //Debug.Log("CurrScore : " + CurrScoreInt.ToString());

        if (isFinish)
        {


            if (LastScore == req_score)
            {

                //Debug.Log("LastScore : " + LastScore.ToString());
                //Debug.Log("CurrScore : " + CurrScoreInt.ToString());

                panelScore.SetActive(true);

                //panel_title_text = panel_title.GetComponent<Text>();
                //panel_subtitle_text = panel_subtitle.GetComponent<Text>();

                success_panel.SetActive(true);
                failed_panel.SetActive(false);

                // Game Finish
                txtTitle = "Level " + currLevel.ToString() + " Complete!";
                txtSubtitle = "Score " + LastScore.ToString();
                panel_title_text.text = txtTitle;
                panel_subtitle_text.text = txtSubtitle;
                //GetComponent<AudioSource>().PlayOneShot(gameCompleteSound);
                Time.timeScale = 0;
                is_finish = true;
                int next_level = gameHandler.next_level;
                PlayerPrefs.SetInt("level", next_level);

                if (next_level == 0)
                {
                    //btn_nextLevel.Text
                }

            }
        }
        else
        {
            // Game Over
            panelScore.SetActive(true);

            success_panel.SetActive(false);
            failed_panel.SetActive(true);

            txtTitle = "Level " + currLevel.ToString() + " Failed!";
            txtSubtitle = "Score " + LastScore.ToString();

            panel_title_text.text = txtTitle;
            panel_subtitle_text.text = txtSubtitle;
            //GetComponent<AudioSource>().PlayOneShot(gameOverSound);

            Time.timeScale = 0;
            is_finish = true;
        }

        return is_finish;
    }

    GameObject FindInActiveObjectByName(string name)
    {
        // Reff : https://stackoverflow.com/a/44456334/10351006
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
