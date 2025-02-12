//
//
//NOTES:
//
//This script is used for DEMONSTRATION porpuses of the Projectiles. I recommend everyone to create their own code for their own projects.
//THIS IS JUST A BASIC EXAMPLE PUT TOGETHER TO DEMONSTRATE VFX ASSETS.
//
//




#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveScript : MonoBehaviour
{
    public bool rotate = true;
    public float rotateAmount = 45;
    public bool bounce = false;
    public float bounceForce = 10;
    public float speed;
    [Tooltip("From 0% to 100%")]
    public float accuracy;
    public float fireRate;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public List<GameObject> trails;
    public GameObject target;

    private Rigidbody rb;
    private string assetPath;
    private ObjectPoolingManager poolManager;
    private Transform firePoint;
    private bool isActive = false;
    private bool isFired = false;
    private EnemyHealth targetHealth;
    private Vector3 targetPos;
    private float rotationSpeed = 50f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        poolManager = FindObjectOfType<ObjectPoolingManager>();

        if (hitPrefab != null)
        {
            hitPrefab.SetActive(false);
        }
    }

    public void SetTarget(GameObject trg, string key, Transform firePos)
    {
        if (trg == null || trg.GetComponent<EnemyHealth>().IsDead)
        {
            ResetProjectile();
            return;
        }

        target = trg;
        targetHealth = target.GetComponent<EnemyHealth>();
        targetPos = target.transform.position;
        assetPath = key;
        firePoint = firePos;
        isActive = true;
        isFired = false;

        transform.position = firePoint.position + Vector3.up * 2f;
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        if (target == null || targetHealth == null || targetHealth.IsDead)
        {
            MoveProjectile();
            if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            {
                PlayHitEffect(transform.position, Vector3.up);
                StartCoroutine(DestroyProjectile(0.2f));
            }
            return;
        }

        targetPos = target.transform.position;
        MoveProjectile();
    }
    private void MoveProjectile()
    {
        if (!isActive) return;

        isFired = true;

        Vector3 direction = (targetPos - transform.position).normalized;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);  
        transform.position += direction * speed * Time.deltaTime;
    }

    private void FireProjectile()
    {
        isFired = true;

        if (target == null || targetHealth == null || targetHealth.IsDead)
        {
            transform.position += transform.forward * 0.5f; 
            StartCoroutine(DestroyProjectile(0.1f)); 
            return;
        }

        StartCoroutine(DestroyProjectile(0.5f));
    }
    void OnCollisionEnter(Collision co)
    {
        if (!bounce && isActive)
        {
            if (co.gameObject.CompareTag("Bullet") || co.gameObject.CompareTag("BuildableTile") || co.gameObject.CompareTag("Tower"))
            {
                return;
            }

            isActive = false;
            rb.isKinematic = true;

            if (!isFired)
            {
                FireProjectile();

                return;
            }

            PlayHitEffect(co.contacts[0].point, co.contacts[0].normal);
            StartCoroutine(DestroyProjectile(0.2f));
        }
    }
    private void PlayHitEffect(Vector3 position, Vector3 normal)
    {
        if (hitPrefab != null)
        {

            hitPrefab.transform.position = position;
            hitPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
            hitPrefab.SetActive(true);

            ParticleSystem[] particles = hitPrefab.GetComponentsInChildren<ParticleSystem>(true);
            float maxDuration = 0f;

            foreach (ParticleSystem ps in particles)
            {
                ps.Play();
                float duration = ps.main.duration + ps.main.startLifetime.constantMax;
                if (duration > maxDuration)
                {
                    maxDuration = duration;
                }
            }

            if (maxDuration > 0f)
            {
                StartCoroutine(DisableHitEffect(maxDuration));
            }
        }
    }

    private IEnumerator DisableHitEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (hitPrefab != null)
        {
            ParticleSystem[] particles = hitPrefab.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem ps in particles)
            {
                ps.Stop();
            }

            hitPrefab.SetActive(false);
        }
    }


    private IEnumerator DestroyProjectile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ResetProjectile();
    }

    private void ResetProjectile()
    {
        isActive = false;
        isFired = false;
        target = null;
        targetHealth = null;

        transform.position = firePoint != null ? firePoint.position + Vector3.up * 2f : Vector3.zero;
        transform.rotation = Quaternion.identity;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (hitPrefab != null)
        {
            hitPrefab.SetActive(false);
        }

        gameObject.SetActive(false);
        poolManager.ReturnObject(assetPath, gameObject);
    }
}