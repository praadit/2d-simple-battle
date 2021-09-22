using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public bool takingDamage = true;
    public int maxHealth = 10;
    public GameObject bloodEffect;
    public GameObject bloodMask;
    public AudioSource sfxHit;

    private int health;
    private List<GameObject> effects;

    public delegate void OnHealthEmpty();
    public event OnHealthEmpty onDieEvent;

    public delegate void OnTakingDamage(int _healthLeft);
    public event OnTakingDamage onTakingDamageEvent;
    //In short
    //public event Action dieEvent;

    void Start()
    {
        health = maxHealth;
        if (bloodMask != null)
        {
            bloodMask.transform.localScale = new Vector3(0f, 1f, 1f);
        }
    }

    public void TakeDamage(int damagePoin)
    {
        if (takingDamage)
        {
            sfxHit.Play();
            health -= damagePoin;
            if(bloodMask != null)
            {
                bloodMask.transform.localScale = new Vector3((1 - ((float) health / maxHealth)), 1f, 1f);
            }

            #region Destroy Particles when it Done
            GameObject darah = Instantiate(bloodEffect, transform.position, Quaternion.Euler(-90, 0, 0));
            darah.transform.parent = transform;
            ParticleSystem particle = darah.GetComponent<ParticleSystem>();
            float destroyTime = particle.main.duration + particle.main.startLifetimeMultiplier;
            Destroy(darah, destroyTime);
            #endregion

            if (onTakingDamageEvent != null)
            {
                onTakingDamageEvent(health);
            }
            if (health <= 0)
            {
                if (onDieEvent != null)
                {
                    onDieEvent();
                }
            }
        }
    }
}
