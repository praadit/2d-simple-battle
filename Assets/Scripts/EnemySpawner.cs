using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Singleton
    public static EnemySpawner instance;
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
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public int maxEnemy;

    [Range(0.5f, 4f)]
    public float spawnRateMin;

    [Range(4f, 6f)]
    public float spawnRateMax;

    //[HideInInspector]
    public bool isSpawning;

    private Transform enemyParent;
    private float lastSpawnTime;
    private int enemyCount;
    void Start()
    {
        GameController.instance.onGameStarted += GameIsStarted;
        GameController.instance.onGameEnded += GameIsOver;
        enemyParent = GameObject.Find("_enemyParent").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyParent != null)
        {
            enemyCount = enemyParent.childCount;
        }
        if (isSpawning && enemyCount < maxEnemy)
        {
            if (lastSpawnTime > 0)
            {
                lastSpawnTime -= Time.deltaTime;
            }
            else
            {
                SpawnEnemy();
                lastSpawnTime = Random.Range(spawnRateMin, spawnRateMax);
            }
        }
    }

    private void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        enemy.transform.parent = enemyParent;
    }

    private void GameIsStarted()
    {
        if (enemyParent == null)
        {
            enemyParent = new GameObject("_enemyParent").transform;
            enemyParent.parent = null;
            enemyParent.transform.position = Vector3.zero;
        }
        isSpawning = true;
    }

    private void GameIsOver()
    {
        isSpawning = false;
        //for(int i = 0; i < enemyParent.childCount; i++)
        //{
        //    Destroy(enemyParent.GetChild(0).gameObject);
        //}
        Destroy(enemyParent.gameObject);
    }
}
