using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHp;
    public float HP { get; private set; }
    public bool IsDead { get; protected set; }
    
    public Slider HpSlider;
    public Image HpFillImage;
    public Canvas hpCanvas;
    public UIBossHpBar bossHpBar;
    public EnemyType enemyType;
    public ResourceType resourceType;
    public int resourceAmount;
    public event Action<Transform> OnDeath;

    private Animator animator;
    private EnemyMovement enemyMovement;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    private void Start()
    {
        HP = maxHp;

        if (hpCanvas != null && hpCanvas.GetComponent<Billboard>() == null)
        {
            hpCanvas.gameObject.AddComponent<Billboard>();
        }

        hpCanvas.enabled = false;
        HpSliderUpdate();
    }
    public void Init(EnemyData enemyData, EnemyType type)
    {
        maxHp = enemyData.Enemy_HP;
        HP = maxHp;

        resourceType = (ResourceType)enemyData.Drop;
        resourceAmount = enemyData.Drop_Amount;
        enemyType = type;
        bossHpBar = FindObjectOfType<UIBossHpBar>();
        if(enemyType == EnemyType.Boss)
        {
            bossHpBar.UpdateHpBar(maxHp, HP, HpFillImage.color);
        }
    }
    private void OnEnable()
    {
        IsDead = false;
        HP = maxHp;
        HpSliderUpdate();
    }
    public void OnDamage(float damage)
    {
        if (IsDead) return;

        HP -= damage;
        HP = Mathf.Clamp(HP, 0, maxHp);

        if (!hpCanvas.enabled)
        {
            hpCanvas.enabled = true;
        }

        if (HP <= 0)
        {
            Die();
        }
        HpSliderUpdate();
    }
    private void HpSliderUpdate()
    {
        if (HpSlider == null || HpFillImage == null) return;

        HpSlider.value = (float)HP / maxHp;
        if (HpSlider.value < 0.3f)
        {
            HpFillImage.color = Color.red; 
        }
        else if (HpSlider.value < 0.7f)
        {
            HpFillImage.color = Color.yellow;
        }
        else
        {
            HpFillImage.color = Color.green;
        }

        if (enemyType == EnemyType.Boss && bossHpBar != null)
        {
            bossHpBar.UpdateHpBar(maxHp, HP, HpFillImage.color);
        }
    }
    public void Die()
    {
        IsDead = true;
        HP = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemyMovement.enabled = false;
        animator.SetTrigger("Dead");
        OnDeath?.Invoke(transform);
        if(bossHpBar != null)
        {
            bossHpBar.UpdateHpBar(maxHp, HP, HpFillImage.color);
            bossHpBar = null;
        }
        GameManager gameManager = FindObjectOfType<GameManager>();
        if(enemyType != EnemyType.MissionBoss)
        {
            gameManager.CheckClear(gameObject);
        }
        gameManager.AddResource(resourceType, resourceAmount);
        StartCoroutine(StartSinking());
    }
    public IEnumerator StartSinking()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
