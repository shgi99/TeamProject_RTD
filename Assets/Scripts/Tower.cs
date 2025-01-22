using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public int towerId;
    private float attackRange;
    private float attackInterval;
    private int damage;

    public Transform currentTarget;
    private List<Transform> enemiesInRange = new List<Transform>();
    private SphereCollider sphereCollider;
    private float rotationSpeed = 5f;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        attackRange = 1.5f;
        attackInterval = 0.8f;
        damage = 25;


        sphereCollider.radius = attackRange;

        StartCoroutine(AttackCoroutine());
    }
    private void Update()
    {
        CleanUpDestroyedEnemies();
        if (currentTarget != null)
        {
            RotateToTarget(currentTarget);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
            UpdateTarget();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == currentTarget)
        {
            enemiesInRange.Remove(other.transform);
            if (other.transform == currentTarget)
            {
                UpdateTarget();
            }
        }
    }
    public IEnumerator AttackCoroutine()
    {
        while(true)
        {
            if(currentTarget != null)
            {
                Attack(currentTarget);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
            yield return new WaitForSeconds(attackInterval);
        }
    }
    private void UpdateTarget()
    {
        CleanUpDestroyedEnemies();
        if (enemiesInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }

        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        currentTarget = closestEnemy;
    }
    private void Attack(Transform target)
    {
        if (target == null)
        {
            currentTarget = null;
            return;
        }

        var attackTarget = target.GetComponent<EnemyHealth>();
        if (attackTarget != null)
        {
            attackTarget.OnDamage(damage);
            animator.SetBool("IsAttack", true);
            Debug.Log($"Attacking {target.name} for {damage} damage.");
        }
    }
    public void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    private void CleanUpDestroyedEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);
        if (currentTarget != null && !enemiesInRange.Contains(currentTarget))
        {
            currentTarget = null;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
