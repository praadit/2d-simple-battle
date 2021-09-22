using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public float waitTime = 0.5f;
    public float cooldownTime = 1f;
    private float timeLeft;
    private float cooldownLeft;
    private PlatformEffector2D platform;
    
    void Start()
    {
        platform = GetComponent<PlatformEffector2D>();
        timeLeft = waitTime;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S) && cooldownLeft <= 0)
        {
            if(timeLeft <= 0)
            {
                platform.surfaceArc = 0;
                timeLeft = waitTime;
                cooldownLeft = cooldownTime;
                Invoke("CollideWithTop", .3f);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }

        if(cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
        }
    }

    private void CollideWithTop()
    {
        platform.surfaceArc = 180;
    }
}
