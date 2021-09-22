using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 3f;
    public Transform projectileOrigin;
    public GameObject projectile;
    public float fireRate = 2f;
    public LayerMask whatIsTarget;
    private Transform projectileParent;
    private float fireTime;

    [Header("Damage Settings")]
    public Color damagedColor;
    public float stunTime = 0.25f;
    private float stunLast;
    private bool stunned;

    [Header("Sound Settings")]
    public AudioSource sfxAttack;

    private EnemyPlatformPatrol mControl;
    private SpriteRenderer spriteRenderer;
    private ScoreManager score;
    private FieldOfView fow;
    private Transform target;

    public bool isPlayerInRange;

    private void Start()
    {
        mControl = GetComponent<EnemyPlatformPatrol>();
        projectileParent = GameObject.Find("_ProjectileParent").transform;
        score = ScoreManager.instance;
        fow = GetComponent<FieldOfView>();
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        #region Event Subcribes
        GetComponent<CharacterHealthSystem>().onDieEvent += Die;
        GetComponent<CharacterHealthSystem>().onTakingDamageEvent += takingDamage;

        fow.onPlayerInRange += CanShoot;
        target = FindObjectOfType<PlayerController>().transform;
        #endregion
    }

    void Update()
    {
        #region Attack Player
        //RaycastHit2D isPlayerSeen = Physics2D.Raycast(transform.position, transform.right, attackRange, whatIsTarget);
        
        if (isPlayerInRange)
        {
            #region Weapon Rotate
            Vector2 weaponPos = transform.position;
            if (target != null)
            {
                Vector2 targetPos = target.position;
                Vector2 direction = targetPos - weaponPos;
                projectileOrigin.right = direction;
            }
            #endregion

            if (fireTime <= 0)
            {
                Shot();
                fireTime = fireRate;
            }
            mControl.isMoving = false;
        }
        else
        {
            mControl.isMoving = true;
        }

        if (fireTime > 0 && !stunned)
        {
            fireTime -= Time.deltaTime;
        }
        #endregion

        #region Stun
        if (stunLast > 0)
        {
            stunLast -= Time.deltaTime;
        }
        else
        {
            backToNormal();
        }
        #endregion
    }

    private void Shot()
    {
        sfxAttack.Play();
        GameObject newProjectile = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
        newProjectile.GetComponent<Projectile>().range = attackRange;
        newProjectile.transform.parent = projectileParent;
    }

    private void Die()
    {
        score.AddScore();
        Destroy(gameObject);
    }

    private void takingDamage(int _healthLeft)
    {
        spriteRenderer.color = damagedColor;
        stunLast = stunTime;
        stunned = true;
        mControl.stunned = true;
    }

    private void backToNormal()
    {
        stunned = false;
        mControl.stunned = false;
        spriteRenderer.color = Color.white;
    }

    private void CanShoot(bool inRange)
    {
        isPlayerInRange = inRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, transform.right * (attackRange - 0.5f));
    }
}
