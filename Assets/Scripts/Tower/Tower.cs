using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    public int towerId;
    public float attackRange;
    public float attackInterval;
    public float damage;
    public float currentDamage;
    public TowerRarity towerRarity;
    public TowerType towerType;
    public SkillData skillData;
    public string towerName;

    public float normalAttackChance;
    public float skillAttackChance;

    public Transform currentTarget;
    private List<Transform> enemiesInRange = new List<Transform>();
    private SphereCollider sphereCollider;
    private float rotationSpeed = 5f;
    private Animator animator;
    public Transform resourceParent;
    private GameManager gameManager;
    private bool isAttacking = false;
    // Start is called before the first frame update
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(AttackCoroutine());
        SetAnimationSpeed();
    }
    private void Update()
    {
        if(gameManager.isGameOver)
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
            if(currentTarget != null && !isAttacking)
            {
                StartCoroutine(PerformAttack());
            }
            yield return null;
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
    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackInterval / 2);

        if (currentTarget != null)
        {
            float randomAttack = Random.Range(0f, 100f);
            if (randomAttack <= skillAttackChance && skillData != null)
            {
                UseSkill();
            }
            else
            {
                Attack(currentTarget);
            }
        }

        yield return new WaitForSeconds(attackInterval / 2);
        isAttacking = false;
    }
    private void Attack(Transform target)
    {
        if (target == null)
        {
            currentTarget = null;
            return;
        }

        var attackTarget = target.GetComponent<EnemyHealth>();
        if (attackTarget != null && !attackTarget.IsDead)
        {
            attackTarget.OnDamage(currentDamage);
        }
        else
        {
            UpdateTarget();
        }
    }
    private void UseSkill()
    {

        if(skillData.Area == 0)
        {
            if (currentTarget != null)
            {
                ApplySkillEffect(currentTarget);
            }
        }
        else
        {
            foreach (var enemy in enemiesInRange.ToList())
            {
                if (Vector3.Distance(transform.position, enemy.position) <= skillData.Area)
                {
                    ApplySkillEffect(enemy);
                }
            }
        }
    }
    private void ApplySkillEffect(Transform target)
    {
        var attackTargetHealth = target.GetComponent<EnemyHealth>();
        if (attackTargetHealth != null && !attackTargetHealth.IsDead)
        {
            Debug.Log($"Using skill: {skillData.SkillAtk_ID}, {currentDamage * skillData.SkillDmgMul} damaged!");
            attackTargetHealth.OnDamage(currentDamage * skillData.SkillDmgMul);

            if (skillData.Enemy_Speed < 100)
            {
                var attackTargetMovement = target.GetComponent<EnemyMovement>();
                attackTargetMovement.StartCoroutine(attackTargetMovement.OnSkillEffect(skillData.Enemy_Speed, skillData.Duration));
            }
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
            Debug.LogError("InitTower() 오류: towerData가 null입니다..");
            return;
        }

        Debug.Log($"InitTower 호출 - ID: {towerData.Tower_ID}, Name: {towerData.Tower_Name}");

        gameManager = FindObjectOfType<GameManager>();
        towerId = towerData.Tower_ID;
        towerName = towerData.Tower_Name;
        towerRarity = (TowerRarity)towerData.Tower_Rarity;
        towerType = (TowerType)towerData.Tower_Type;
        attackRange = towerData.AtkRng;
        attackInterval = towerData.AtkSpd;
        damage = towerData.AtkDmg;
        normalAttackChance = towerData.Pct_1;
        skillAttackChance = towerData.Pct_2;

        if (towerData.SkillAtk_ID > 0)
        {
            skillData = DataTableManager.SkillTable.Get(towerData.SkillAtk_ID);
        }

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
            SetAnimationSpeed();
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
    private void SetAnimationSpeed()
    {
        if (animator == null) 
        {
            return;
        }

        AnimationClip attackClip = GetAttackClip();
        if (attackClip == null)
        {
            return;
        }

        float baseAnimationDuration = attackClip.length;
        float newAnimationSpeed = baseAnimationDuration / attackInterval;

        animator.SetFloat("AttackSpeed", newAnimationSpeed);
    }

    private AnimationClip GetAttackClip()
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name.Contains("Attack"))
            {
                return clip;
            }
        }
        return null;
    }
}
