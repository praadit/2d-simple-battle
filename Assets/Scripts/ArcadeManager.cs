using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcadeManager : MonoBehaviour
{
    public static ArcadeManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public GameObject enemyHolder;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    private Transform[] points;
    public delegate void WhenEnemyEmpty();
    public event WhenEnemyEmpty whenEnemyEmpty;

    private void Start()
    {
        GameController.instance.onGameStarted += StartGame;
        GameController.instance.onGameEnded += GameOver;

        points = spawnPoint.GetComponentsInChildren<Transform>();
        points = points.Skip(1).ToArray();
    }

    private void checkEnemy()
    {
        if (enemyHolder != null)
        {
            if (enemyHolder.transform.childCount <= 0)
            {
                GameController.instance.GameWon();
                Destroy(FindObjectOfType<PlayerController>().gameObject);
                if (whenEnemyEmpty != null)
                {
                    whenEnemyEmpty();
                }
            }
        }
    }

    private void StartGame()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        enemyHolder = new GameObject();
        enemyHolder.transform.name = "_enemyParent";
        enemyHolder.transform.parent = null;

        foreach (var item in points)
        {
            Spawn(item.position, Quaternion.identity, enemyHolder.transform);
        }

        InvokeRepeating("checkEnemy", 0f, 0.5f);
    }

    private void Spawn(Vector3 pos, Quaternion rot, Transform parent)
    {
        GameObject enemy = Instantiate(enemyPrefab, pos, rot, parent);
    }

    private void GameOver()
    {
        Destroy(enemyHolder);
    }

}
