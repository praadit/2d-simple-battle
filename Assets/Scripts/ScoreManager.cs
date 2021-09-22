using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    public static ScoreManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private int score;
    private int hscore;

    public delegate void OnScoreAdded(int _score);
    public event OnScoreAdded onScoreAdded;

    public delegate void OnHighscore();
    public event OnHighscore onHighscore;

    private void Start()
    {
        GameController.instance.onGameStarted += GameStart;
        GameController.instance.onGameEnded += GameEnd;
    }

    private void GameStart()
    {
        ResetScore();
        hscore = GameController.instance.GetHighScore();
    }

    private void GameEnd()
    {
        if(hscore < score)
        {
            hscore = score;
            GameController.instance.SetHighscore(hscore);
            if(onHighscore != null)
            {
                onHighscore();
            }
        }
    }

    public void AddScore()
    {
        score += 1;
        if(onScoreAdded != null)
        {
            onScoreAdded(score);
        }
    }

    private void ResetScore()
    {
        score = 0;
        if (onScoreAdded != null)
        {
            onScoreAdded(score);
        }
    }
}
