using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;
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

    public GameObject mainManuPanel;
    public GameObject playPanel;
    public GameObject scorePanel;
    public GameObject controlPanel;
    public GameObject winPanel;
    public GameObject pausePanel;

    public Text scoreText;
    public Text lifeText;
    public Text finalScore;
    public Text hScoreText;
    public GameObject hScoreImage;

    private int score;
    private int hscore;

    void Start()
    {
        #region Register Event
        GameController.instance.onGameStarted += OnGameStart;
        GameController.instance.onGameEnded += OnGameEnd;
        GameController.instance.onGamePaused += OnPauseGame;
        GameController.instance.onGameResume += OnResumeGame;
        ScoreManager.instance.onScoreAdded += RefreshScore;
        ScoreManager.instance.onHighscore += ShowHighscore;

        if(ArcadeManager.instance != null)
        {
            ArcadeManager.instance.whenEnemyEmpty += ShowWinPanel;
        }
        #endregion

        mainManuPanel.SetActive(true);
        controlPanel.SetActive(false);
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    private void OnGameStart()
    {
        hscore = GameController.instance.GetHighScore();
        hScoreText.text = hscore.ToString();

        hScoreImage.SetActive(false);
        mainManuPanel.SetActive(false);
        winPanel.SetActive(false);
        playPanel.SetActive(true);
        controlPanel.SetActive(true);
        controlPanel.GetComponent<Animator>().SetBool("fade_in", true);
        controlPanel.GetComponent<Animator>().SetBool("fade_out", false);
        Invoke("closeControlPanel", 8f);

        scorePanel.SetActive(false);
        GameController.instance.activePlayer.GetComponent<PlayerController>().onPlayerTakingDamage += RefreshLife;

        RefreshLife(GameController.instance.activePlayer.GetComponent<CharacterHealthSystem>().maxHealth);
    }

    private void closeControlPanel()
    {
        controlPanel.GetComponent<Animator>().SetBool("fade_out", true);
        controlPanel.GetComponent<Animator>().SetBool("fade_in", false);
        Invoke("HideControl", 1f);
    }

    private void HideControl()
    {
        controlPanel.SetActive(false);
    }

    private void OnGameEnd()
    {
        mainManuPanel.SetActive(false);
        playPanel.SetActive(false);
        finalScore.text = score.ToString();
        scorePanel.SetActive(true);
    }

    private void RefreshScore(int _score)
    {
        scoreText.text = _score.ToString();
        score = _score;
    }

    private void RefreshLife(int _life)
    {
        lifeText.text = _life.ToString();
    }

    private void OnPauseGame()
    {
        pausePanel.SetActive(true);
    }

    private void OnResumeGame()
    {
        pausePanel.SetActive(false);
    }

    private void ShowHighscore()
    {
        hScoreImage.SetActive(true);
    }

    private void ShowWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            mainManuPanel.SetActive(false);
            playPanel.SetActive(false);
        }
    }
}
