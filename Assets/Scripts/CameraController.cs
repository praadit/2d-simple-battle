using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton
    public static CameraController instance;
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
    public Transform target;
    public Vector3 offset = new Vector3(0,0,-10);
    private Animator anim;

    private void Start()
    {
        target.transform.GetComponent<PlayerController>().onPlayerDie += LosePlayer;
        GameController.instance.onGameStarted += GameStarted;
        anim = GetComponent<Animator>();
    }

    private void LosePlayer()
    {
        target = null;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    private void GameStarted()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    public void SetCameraIdle()
    {
        anim.SetBool("isattack", false);
    }
}
