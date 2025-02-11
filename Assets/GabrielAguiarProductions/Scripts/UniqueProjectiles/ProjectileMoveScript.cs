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
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        poolManager = FindObjectOfType<ObjectPoolingManager>();
    }

    public void SetTarget(GameObject trg, string key, Transform firePos)
    {
        target = trg;
        assetPath = key;
        firePoint = firePos;
        isActive = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (!isActive || target == null || !target.activeSelf)
        {
            ResetProjectile();
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision co)
    {
        if (!bounce && isActive)
        {
            if (target == null || !target.activeSelf)
            {
                ResetProjectile();
                return;
            }

            if (co.gameObject.tag != "Bullet" && co.gameObject.tag != "BuildableTile" && co.gameObject.tag != "Tower" && co.gameObject.tag != "Enemy")
            {
                isActive = false;

                ContactPoint contact = co.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                if (hitPrefab != null)
                {
                    GameObject hitVFX = Instantiate(hitPrefab, pos, rot);
                    hitVFX.transform.parent = null;

                    var ps = hitVFX.GetComponent<ParticleSystem>();
                    if (ps == null)
                    {
                        var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitVFX, psChild.main.duration);
                    }
                    else
                    {
                        Destroy(hitVFX, ps.main.duration);
                    }
                }

                StartCoroutine(DestroyProjectile(0.3f));
            }
        }
    }

    private IEnumerator DestroyProjectile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isActive = false;
        ResetProjectile();
    }

    private void ResetProjectile()
    {
        isActive = false;
        target = null;

        transform.position = firePoint != null ? firePoint.position + Vector3.up * 2f : Vector3.zero;
        transform.rotation = Quaternion.identity;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(assetPath))
        {
            poolManager.ReturnObject(assetPath, gameObject);
        }
    }
}

