using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
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
    public int sellPrice;
    public ParticleSystem rarityParticle;
    public float normalAttackChance;
    public float skillAttackChance;
    public string projectilePath;
    public Transform firePoint;

    public Transform currentTarget;
    public EffectState skillType;
    public AttackType skillAttackType;
    private List<Transform> enemiesInRange = new List<Transform>();
    private SphereCollider sphereCollider;
    private float rotationSpeed = 5f;
    [SerializeField] private Animator animator;
    public Transform resourceParent;
    private GameManager gameManager;
    private bool isAttacking = false;
    private ObjectPoolingManager poolManager;
    public string resourceKey;
    private Coroutine attackCoroutine;
    
    // Start is called before the first frame update
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    void Start()
    {
        poolManager = FindObjectOfType<ObjectPoolingManager>();
        gameManager = FindObjectOfType<GameManager>();
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
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackInterval / 2);
        if (currentTarget == null || currentTarget.GetComponent<EnemyHealth>() == null || currentTarget.GetComponent<EnemyHealth>().IsDead)
        {
            isAttacking = false;
            yield break;
        }

        float randomAttack = Random.Range(0f, 100f);
        if (randomAttack <= skillAttackChance && skillData != null)
        {
            UseSkill();
        }
        else
        {
            Attack(currentTarget);
        }

        yield return new WaitForSeconds(attackInterval / 2);
        isAttacking = false;
    }
    private void Attack(Transform target)
    {
        if (target == null || target.GetComponent<EnemyHealth>() == null || target.GetComponent<EnemyHealth>().IsDead)
        {
            currentTarget = null;
            UpdateTarget();
            return;
        }

        var attackTarget = target.GetComponent<EnemyHealth>();
        if (attackTarget != null && !attackTarget.IsDead)
        {
            if(projectilePath != "0")
            {
                Fire(false);
            }
            else
            {
                SoundManager.Instance.PlayTowerSFX(towerId.ToString());
                attackTarget.OnDamage(currentDamage);
            }
        }
        else
        {
            UpdateTarget();
        }
    }
    private void UseSkill()
    {
        bool firedProjectile = false; 

        if (skillData.Area == 0)
        {
            if (currentTarget != null)
            {
                ApplySkillEffect(currentTarget);
                Fire(true);
            }
        }
        else
        {
            if (currentTarget != null)
            {
                var enemiesInSkillRange = Physics.OverlapSphere(currentTarget.position, skillData.Area, LayerMask.GetMask("Enemy"));
                foreach (var enemy in enemiesInSkillRange)
                {
                    ApplySkillEffect(enemy.transform);

                    if (!firedProjectile)
                    {
                        Fire(true);
                        firedProjectile = true;
                    }
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

            if (skillData.Enemy_Speed < 100)
            {
                var attackTargetMovement = target.GetComponent<EnemyMovement>();
                attackTargetMovement.ApplyEffect(skillData.Enemy_Speed, skillData.Duration);
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
            if (enemy == null)
            {
                return true;
            }
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
            return;
        }

        ClearBeforeDestroy();

        gameManager = FindObjectOfType<GameManager>();
        towerId = towerData.Tower_ID;
        towerName = towerData.Tower_Name;
        towerRarity = (TowerRarity)towerData.Tower_Rarity;
        towerType = (TowerType)towerData.Tower_Type;
        attackRange = towerData.AtkRng;
        attackInterval = towerData.AtkSpd;
        damage = towerData.AtkDmg;
        sellPrice = towerData.Sell_Price;
        projectilePath = towerData.Pjt_1;
        normalAttackChance = towerData.Pct_1;
        skillAttackChance = towerData.Pct_2;
        isAttacking = false;

        if (towerData.SkillAtk_ID > 0)
        {
            skillData = DataTableManager.SkillTable.Get(towerData.SkillAtk_ID);
            skillType = DataTableManager.SkillTable.GetSkillEffectState(skillData.Enemy_Speed);
            skillAttackType = DataTableManager.SkillTable.GetAttackType(skillData.Area);
        }

        UpgradeManager upgradeManager = FindObjectOfType<UpgradeManager>();
        ApplyUpgrade(upgradeManager.GetUpgradeLevel(towerType));

        sphereCollider.radius = attackRange;
        ApplyResource(towerData.Asset_Path);
        resourceKey = towerData.Asset_Path;
        if (rarityParticle != null)
        {
            ParticleSystem.MainModule mainModule = rarityParticle.main;
            mainModule.startColor = DataTableManager.TowerTable.GetRarityColor(towerRarity);

            rarityParticle.gameObject.SetActive(true);
            rarityParticle.Play();
        }
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackCoroutine());
        SetAnimationSpeed();
    }

    private void ApplyResource(string asset_Path)
    {
        ObjectPoolingManager poolManager = FindObjectOfType<ObjectPoolingManager>();
        GameObject towerResource = poolManager.GetObject(asset_Path);
        if (towerResource != null)
        {
            foreach (Transform child in resourceParent)
            {
                if(child != rarityParticle.transform)
                {
                    poolManager.ReturnObject(resourceKey, child.gameObject);
                }
            }

            towerResource.transform.SetParent(resourceParent, false);
            towerResource.transform.localRotation = Quaternion.identity;
            towerResource.SetActive(true);
            firePoint = towerResource.transform;
            animator = towerResource.GetComponent<Animator>();
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
        if (attackCoroutine != null)
        {
            StopAllCoroutines();
            attackCoroutine = null;
        }

        animator = null;
        enemiesInRange.Clear();
        currentTarget = null;

        if (rarityParticle != null)
        {
            rarityParticle.Stop();
            rarityParticle.gameObject.SetActive(false);
        }
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
    public void Fire(bool isSkillAttack)
    {
        if (currentTarget == null || currentTarget.GetComponent<EnemyHealth>().IsDead)
        {
            return;
        }

        string projectileKey = isSkillAttack ? skillData.Pjt : projectilePath;
        float fireDamage = isSkillAttack ? currentDamage * skillData.SkillDmgMul : currentDamage;
        string attackSFX = isSkillAttack ? skillData.SkillAtk_ID.ToString() : towerId.ToString();

        GameObject projectileInstance = poolManager.GetObject(projectileKey);
        projectileInstance.transform.position = firePoint.position + Vector3.up * 2f;
        projectileInstance.transform.rotation = Quaternion.identity;

        ProjectileMoveScript projectile = projectileInstance.GetComponent<ProjectileMoveScript>();

        if (projectile != null && currentTarget != null)
        {
            projectile.SetTarget(currentTarget.gameObject, projectileKey, firePoint);
            SoundManager.Instance.PlayTowerSFX(attackSFX);
            currentTarget.GetComponent<EnemyHealth>().OnDamage(fireDamage);
        }

    }
}
