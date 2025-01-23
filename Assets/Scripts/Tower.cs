using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public int towerId;
    public float attackRange;
    public float attackInterval;
    public int damage;
    public TowerRarity towerRarity;
    public TowerType towerType;
    public string towerName;

    public Transform currentTarget;
    private List<Transform> enemiesInRange = new List<Transform>();
    private SphereCollider sphereCollider;
    private float rotationSpeed = 5f;
    private Animator animator;
    public Transform visualParent;
    // Start is called before the first frame update
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
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
            var enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += HandleEnemyDeath;
            }

            enemiesInRange.Add(other.transform);
            UpdateTarget();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath -= HandleEnemyDeath;
            }

            enemiesInRange.Remove(other.transform);
            UpdateTarget();
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
            animator.SetBool("IsAttack", false);
            return;
        }

        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemiesInRange)
        {
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemyHealth.IsDead)
            {
                float distance = Vector3.Distance(transform.position, enemy.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        currentTarget = closestEnemy;
    }
    private void Attack(Transform target)
    {
        if (target == null)
        {
            animator.SetBool("IsAttack", false);
            currentTarget = null;
            return;
        }

        var attackTarget = target.GetComponent<EnemyHealth>();
        if (attackTarget != null && !attackTarget.IsDead)
        {
            animator.SetBool("IsAttack", true);
            attackTarget.OnDamage(damage);
        }
        else
        {
            animator.SetBool("IsAttack", false);
            UpdateTarget();
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
        enemiesInRange.RemoveAll(enemy =>
        {
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            return enemyHealth == null || enemyHealth.IsDead;
        });
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
    private void HandleEnemyDeath(Transform enemy)
    {
        enemiesInRange.Remove(enemy);
        UpdateTarget();
    }
    public void InitTower(TowerData towerData)
    {
        towerId = towerData.Tower_ID;
        towerName = towerData.Tower_Name;
        towerRarity = (TowerRarity)towerData.Tower_Rarity;
        towerType = (TowerType)towerData.Tower_Type;
        attackRange = towerData.AtkRng;
        attackInterval = towerData.AtkSpd;
        damage = towerData.AtkDmg;

        sphereCollider.radius = attackRange;

        ApplyVisual(towerData.Asset_Path);
    }

    private void ApplyVisual(string asset_Path)
    {
        GameObject visualPrefab = Resources.Load<GameObject>(asset_Path);
        if (visualPrefab != null)
        {
            foreach (Transform child in visualParent)
            {
                Destroy(child.gameObject);
            }

            GameObject visualInstance = Instantiate(visualPrefab, visualParent);
            visualInstance.transform.localPosition = visualPrefab.transform.localPosition;
            visualInstance.transform.localRotation = Quaternion.identity;

            animator = visualInstance.GetComponent<Animator>();
        }
    }
    public void ClearBeforeDestroy()
    {
        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                var enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.OnDeath -= HandleEnemyDeath;
                }
            }
        }

        enemiesInRange.Clear();
        StopAllCoroutines();
        currentTarget = null;
    }
}
