using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    public float speed = 7f;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGroud;
    public float jumpTime = 0.4f;
    public float jumpForce = 10f;

    [Header("Attack Settings")]
    public int attackDamage = 1;
    public float attackCooldown = 1f;
    public Transform rangeCenter;
    public float attackRadius = 2f;
    public LayerMask whatIsEnemy;
    public Color hitColor;
    public float knockbackTime = 0.2f;
    private float knockcountdown;

    [Header("Sound Settings")]
    public AudioSource sfxAttack;

    public bool isTesting = false;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumping;
    private float attackTime;
    private bool isAttackHit;
    private SpriteRenderer spriteRenderer;

    private bool isOnGame;
    private Animator camAnim;

    private Animator bodyAnim;
    private Collider2D[] enemyHitted;

    public delegate void OnPlayerDie();
    public event OnPlayerDie onPlayerDie;

    public delegate void OnPlayerTakingDamage(int _healthLeft);
    public event OnPlayerTakingDamage onPlayerTakingDamage;
    //in short
    //public event Action playerDieEvent;

    void Start()
    {
        transform.GetComponent<CharacterHealthSystem>().onDieEvent += Die;
        transform.GetComponent<CharacterHealthSystem>().onTakingDamageEvent += takingDamage;

        rb = GetComponent<Rigidbody2D>();
        bodyAnim = GetComponent<Animator>();
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        if (isTesting)
        {
            isOnGame = true;
        }
        camAnim = Camera.main.transform.GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (isOnGame)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

            #region Walk Animation
            if (moveInput != 0)
            {
                bodyAnim.SetBool("iswalk", true);
            }
            else
            {
                bodyAnim.SetBool("iswalk", false);
            }
            #endregion
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void Update(){
        if (isOnGame)
        {
            #region Jumping
            isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGroud);

            if (isGrounded)
            {
                bodyAnim.SetBool("isjump", false);
            }
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                bodyAnim.SetBool("isjump", true);
                jumpTimeCounter = jumpTime;
                rb.velocity = Vector2.up * jumpForce;
            }

            if (Input.GetKey(KeyCode.Space) && isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.velocity = Vector2.up * jumpForce;
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
            #endregion

            #region Rotate Body
            if (moveInput > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (moveInput < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            #endregion

            #region Attack Move
            if (Input.GetMouseButtonDown(0) && attackTime <= 0 && isGrounded && moveInput == 0)
            {
                sfxAttack.Play();
                camAnim.SetBool("isattack", false);
                bodyAnim.SetBool("isattack", true);
                attackTime = attackCooldown;
            }
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0))
            {
                bodyAnim.SetBool("isattack", false);
            }
            #endregion

            #region Knockback
            if (knockcountdown > 0)
            {
                knockcountdown -= Time.deltaTime;
            }
            else
            {
                backToNormal();
            }
            #endregion
        }
    }

    public void Die()
    {
        if(onPlayerDie != null)
        {
            onPlayerDie();
        }

        Destroy(gameObject);
    }

    public void DamageEnemy()
    {
        camAnim.SetBool("isattack", true);
        #region Give Enemy Damage
        enemyHitted = Physics2D.OverlapCircleAll(rangeCenter.position, attackRadius, whatIsEnemy);
        for (int i = 0; i < enemyHitted.Length; i++)
        {
            //Decrese enemy health here
            enemyHitted[i].GetComponent<CharacterHealthSystem>().TakeDamage(attackDamage);
        }
        #endregion
    }

    private void takingDamage(int _healthLeft)
    {
        knockcountdown = knockbackTime;
        spriteRenderer.color = hitColor;

        /** Triggering Event */
        if(onPlayerTakingDamage != null)
        {
            onPlayerTakingDamage(_healthLeft);
        }
    }

    private void backToNormal()
    {
        spriteRenderer.color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rangeCenter.position, attackRadius);
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
    }

    public void GameStart()
    {
        isOnGame = true;
    }
}
