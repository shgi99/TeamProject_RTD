using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform startPoint;          
    public List<Transform> movePoints;    
    private Rigidbody rb;                 
    private int movePointIdx = 0;         
    public float speed = 1.5f;            
    public float rotationSpeed = 5.0f;    
    public float waypointThreshold = 0.1f;
    private int damage;
    private GameManager gameManager;
    private EffectState currentEffect = EffectState.None;
    private float originalSpeed;
    private float effectEndTime = 0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Init(Transform startPoint, List<Transform> movePoints, int damage)
    {
        gameManager = FindObjectOfType<GameManager>();
        this.startPoint = startPoint;
        this.movePoints = movePoints;
        this.damage = damage;
        originalSpeed = speed;
    }
    private void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
        if (movePoints.Count > 0)
        {
            LookAtNextWaypoint();
        }
    }

    private void FixedUpdate()
    {
        if (movePoints.Count == 0 || movePointIdx >= movePoints.Count || gameManager.isGameOver)
            return;

        if (currentEffect != EffectState.None && Time.time >= effectEndTime)
        {
            speed = originalSpeed;
            currentEffect = EffectState.None;
        }

        Vector3 targetPosition = movePoints[movePointIdx].position;

        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));

        rb.MovePosition(rb.position + direction * (speed * Time.deltaTime));

        if (Vector3.Distance(transform.position, targetPosition) <= waypointThreshold)
        {
            movePointIdx++;
        }
    }

    private void LookAtNextWaypoint()
    {
        if (movePointIdx < movePoints.Count)
        {
            Vector3 direction = (movePoints[movePointIdx].position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "End")
        {
            gameManager.DamageToLife(damage);
            gameManager.CheckClear(gameObject);
            Destroy(gameObject);
        }
    }

    public IEnumerator OnSkillEffect(float slowPct, float duration)
    {
        float baseSpeed = speed;
        speed *= (slowPct / 100f);
        yield return new WaitForSeconds(duration);
        speed = baseSpeed;
    }
    public void ApplyEffect(float slowPct, float duration)
    {
        if (slowPct <= 0)
        {
            if (currentEffect == EffectState.Stun)
                return;

            effectEndTime = Time.time + duration;
            speed = 0;
            currentEffect = EffectState.Stun;
        }
        else if (slowPct < 100) 
        {
            if (currentEffect == EffectState.Stun)
                return;

            float newEffectEndTime = Time.time + duration;

            if (currentEffect == EffectState.None || newEffectEndTime > effectEndTime)
            {
                effectEndTime = newEffectEndTime;
                speed = originalSpeed * (slowPct / 100f);
                currentEffect = EffectState.Slow;
            }
        }
    }
}
