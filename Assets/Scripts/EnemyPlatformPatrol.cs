using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatformPatrol : MonoBehaviour
{
    [Header("Walk Settings")]
    public float speed;
    public float knockbackPower;
    public float groundDistance = 1f;
    public float wallDistance = 0.2f;
    public LayerMask whatIsGround;
    public LayerMask whatIsHidden;
    public Transform groundDetect;

    private bool movingRight;
    private Rigidbody2D rb;

    public bool stunned = false;
    public bool isMoving = true;
    private RaycastHit2D isEndOfPlatform;
    private RaycastHit2D isWallAhead;
    private RaycastHit2D isWallBehind;
    private RaycastHit2D isPlayerBehind;

    [Header("Detect Player Settings")]
    public float playerDetectDistance = 0.5f;
    public LayerMask whatIsPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        #region Walking
        if (isMoving)
        {
            if (!stunned)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        }

        isEndOfPlatform = Physics2D.Raycast(groundDetect.position, Vector2.down, groundDistance, whatIsGround);

        isWallAhead = Physics2D.Raycast(groundDetect.position, transform.right, wallDistance, whatIsGround);
        isWallBehind = Physics2D.Raycast(transform.position, -transform.right, 0.6f, whatIsGround);

        //if (stunned && !isWallBehind)
        //{
        //    transform.Translate(-Vector2.right * knockbackPower * Time.deltaTime);
        //}

        if (!isEndOfPlatform.collider || isWallAhead.collider) 
        {

            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else if(!movingRight)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
        #endregion

        #region Detect Player
        isPlayerBehind = Physics2D.Raycast(transform.position, -transform.right, playerDetectDistance, whatIsPlayer);
        if (isPlayerBehind.transform)
        {   
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
        #endregion

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(groundDetect.position, Vector2.down * groundDistance);
        Gizmos.DrawRay(groundDetect.position, transform.right * wallDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.right * playerDetectDistance);
    }
}
