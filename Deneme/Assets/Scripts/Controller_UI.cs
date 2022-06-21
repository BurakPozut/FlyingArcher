using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller_UI : MonoBehaviour
{
    public TMP_Text scoreTxt;
    public TMP_Text FinalscoreTxt;
    public TMP_Text[] GameOverTexts;
    public Text [] GameWon;
    public Button TryAgain;
    System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
    int Score = 0; 
    bool NotFinished = true;
    void Start()
    {

    }

    public void Died()
    {
        int i = rnd.Next(0,5);
        GameOverTexts[i].gameObject.SetActive(true);
        TryAgain.gameObject.SetActive(true);
    }
    public void ScoreTextSet()
    {
        Score++;
        scoreTxt.text = ""+Score;
    }
    public void Won(int multiplier)
    {
        if(NotFinished)
        {
            Score = Score*multiplier;
            FinalscoreTxt.text = "Score : " + Score;
            FinalscoreTxt.gameObject.SetActive(true);
            TryAgain.gameObject.SetActive(true);
            NotFinished = false;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
