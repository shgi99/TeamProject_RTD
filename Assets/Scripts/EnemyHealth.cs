using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHp = 100;
    public int HP { get; private set; }
    public bool IsDead { get; protected set; }
    private Animator animator;
    private EnemyMovement enemyMovement;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnDamage(20);
        }
    }
    private void OnEnable()
    {
        IsDead = false;
        HP = maxHp;
    }
    public void OnDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0 && !IsDead)
        {
            Die();
        }
    }
    public void Die()
    {
        IsDead = true;
        HP = 0;
        animator.SetTrigger("Dead");
        enemyMovement.enabled = false;
        StartCoroutine(StartSinking());
    }

    public IEnumerator StartSinking()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
