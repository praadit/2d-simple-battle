using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    [Range(0, 360)]
    public float angleOffset;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> visibleTarget = new List<Transform>(); //Testing Only

    public delegate void OnPlayerInRange(bool isInRange);
    public event OnPlayerInRange onPlayerInRange;

    private void Start()
    {
        StartCoroutine(FindTargetWithDelay(0.2f));
    }
    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }
    public void FindVisibleTarget()
    {
        visibleTarget.Clear();
        if (onPlayerInRange != null)
        {
            onPlayerInRange(false);
        }

        Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        for(int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2) //Kalo tarlihat di dalam view field
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) //Target gak kehalangan
                {
                    //Shoot here
                    visibleTarget.Add(target); //Testing only
                    if(onPlayerInRange != null)
                    {
                        onPlayerInRange(true);
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y + angleOffset;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
