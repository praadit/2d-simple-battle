using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public Transform playerSpawnPoint;
    public GameObject playerPrefabs;

    [HideInInspector]
    public GameObject activePlayer;
    private bool isGameOver;
    private bool isGameNotStarted;
    private bool isGamePaused;
    private int hscore;

    private SaveManager save;

    #region Event Declaration
    public delegate void OnGameStarted();
    public event OnGameStarted onGameStarted;

    public delegate void OnGameEnded();
    public event OnGameEnded onGameEnded;

    public delegate void OnGamePaused();
    public event OnGamePaused onGamePaused;

    public delegate void OnResumeGame();
    public event OnResumeGame onGameResume;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        isGameNotStarted = true;
        save = SaveManager.instance;

        activePlayer = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameNotStarted && Input.GetKeyDown(KeyCode.Space))
        {
            GameStarted();
        }

        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            GameStarted();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !isGameNotStarted)
        {
            isGamePaused = !isGamePaused;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isGameOver)
        {
            Application.Quit();
        }

        if (isGamePaused)
        {
            PauseGame();
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            ResumeGame();
        }
    }

    private void GameStarted()
    {
        LoadGame();

        isGameNotStarted = false;
        isGameOver = false;
        if (activePlayer == null)
        {
            activePlayer = Instantiate(playerPrefabs, playerSpawnPoint.position, Quaternion.identity);
        }

        activePlayer.GetComponent<PlayerController>().onPlayerDie += GameEnded;
        activePlayer.GetComponent<PlayerController>().GameStart();

        if (onGameStarted != null)
        {
            onGameStarted();
        }
    }

    private void GameEnded()
    {
        isGameOver = true;
        if (onGameEnded != null)
        {
            onGameEnded();
        }
        SaveGame();
    }

    private void PauseGame()
    {
        isGamePaused = true;
        if (onGamePaused != null)
        {
            onGamePaused();
        }
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        if (onGameResume != null)
        {
            onGameResume();
        }
    }

    public void GameWon()
    {
        isGameOver = true;
        if (onGameEnded != null)
        {
            onGameEnded();
        }
    }

    #region Save & Load
    public void LoadGame()
    {
        save.LoadData();
        hscore = save.GetHscore();
    }

    public void SaveGame()
    {
        save.SetHscore(hscore);
        save.SaveData();
    }
    #endregion

    #region Setter & Getter
    public void SetHighscore(int _score)
    {
        hscore = _score;
    }

    public int GetHighScore()
    {
        return hscore;
    }
    #endregion
}
