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
    public float damage;
    public float currentDamage;
    public TowerRarity towerRarity;
    public TowerType towerType;
    public string towerName;

    public Transform currentTarget;
    private List<Transform> enemiesInRange = new List<Transform>();
    private SphereCollider sphereCollider;
    private float rotationSpeed = 5f;
    private Animator animator;
    public Transform resourceParent;
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
        if(GameManager.instance.isGameOver)
        {
            return;
        }

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
            yield return new WaitForSeconds(attackInterval);
            animator.SetBool("IsAttack", false);
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
            attackTarget.OnDamage(currentDamage);
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
    private void HandleEnemyDeath(Transform enemy)
    {
        enemiesInRange.Remove(enemy);
        UpdateTarget();
    }
    public void InitTower(TowerData towerData)
    {
        if (towerData == null)
        {
            Debug.LogError("InitTower() 오류: towerData가 null입니다!");
            return;
        }

        Debug.Log($"InitTower 호출 - ID: {towerData.Tower_ID}, Name: {towerData.Tower_Name}");


        towerId = towerData.Tower_ID;
        towerName = towerData.Tower_Name;
        towerRarity = (TowerRarity)towerData.Tower_Rarity;
        towerType = (TowerType)towerData.Tower_Type;
        attackRange = towerData.AtkRng;
        attackInterval = towerData.AtkSpd;
        damage = towerData.AtkDmg;

        UpgradeManager upgradeManager = FindObjectOfType<UpgradeManager>();

        ApplyUpgrade(upgradeManager.GetUpgradeLevel(towerType));

        sphereCollider.radius = attackRange;

        ApplyResource(towerData.Asset_Path);
    }

    private void ApplyResource(string asset_Path)
    {
        GameObject towerResource = Resources.Load<GameObject>(asset_Path);
        if (towerResource != null)
        {
            foreach (Transform child in resourceParent)
            {
                Destroy(child.gameObject);
            }

            GameObject towerInstance = Instantiate(towerResource, resourceParent);
            towerInstance.transform.localPosition = towerResource.transform.localPosition;
            towerInstance.transform.localRotation = Quaternion.identity;

            animator = towerInstance.GetComponent<Animator>();
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

    public void ApplyUpgrade(int upgradeLevel)
    {
        if (upgradeLevel == 0)
        {
            currentDamage = damage;
        }
        else
        {
            UpgradeData upgradeData = DataTableManager.UpgradeTable.Get(upgradeLevel);
            if (upgradeData != null)
            {
                currentDamage = damage + damage * (upgradeData.Change_Stat / 100);
            }
        }
    }
}
