using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public GameObject destroyEffect;

    [HideInInspector]
    public float range;
    private Rigidbody2D rb;
    private Vector3 origin;
    private float distance;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        origin = transform.position;
    }

    void Update()
    {
        rb.velocity = transform.right * speed;
        distance = Vector3.Distance (origin, transform.position);
        if(distance >= range)
        {
            ShowEffect();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<CharacterHealthSystem>().TakeDamage(damage);
            ShowEffect();
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Ground")
        {
            ShowEffect();
            Destroy(gameObject);
        }
    }

    private void ShowEffect()
    {
        GameObject effect = Instantiate(destroyEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        effect.transform.parent = null;
        float lifeTime = effect.GetComponent<ParticleSystem>().main.duration + effect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier; ;
        Destroy(effect, lifeTime);
    }
}
