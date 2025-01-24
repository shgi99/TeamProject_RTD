using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHp;
    public int HP { get; private set; }
    public bool IsDead { get; protected set; }
    public Slider HpSlider;
    public Image HpFillImage;
    public Canvas hpCanvas;
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
    public void Init(int hpData)
    {
        maxHp = hpData;
        HP = maxHp;
    }
    private void OnEnable()
    {
        IsDead = false;
        HP = maxHp;
        HpSliderUpdate();
    }
    public void OnDamage(int damage)
    {
        if (IsDead) return;

        HP -= damage;
        HP = Mathf.Clamp(HP, 0, maxHp);

        if (!hpCanvas.enabled)
        {
            hpCanvas.enabled = true;
        }

        HpSliderUpdate();

        if (HP <= 0)
        {
            Die();
        }
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
    }
    public void Die()
    {
        IsDead = true;
        HP = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemyMovement.enabled = false;
        animator.SetTrigger("Dead");
        OnDeath?.Invoke(transform);
        GameManager.instance.CheckClear(gameObject);
        StartCoroutine(StartSinking());
    }
    public IEnumerator StartSinking()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
