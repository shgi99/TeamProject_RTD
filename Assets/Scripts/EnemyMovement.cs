using System.Collections.Generic;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetMovePoints(Transform startPoint, List<Transform> movePoints)
    {
        this.startPoint = startPoint;
        this.movePoints = movePoints;
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
        if (movePoints.Count == 0 || movePointIdx >= movePoints.Count)
            return;

        Vector3 targetPosition = movePoints[movePointIdx].position;
        Vector3 direction = (targetPosition - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

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
            Destroy(gameObject);
        }
    }
}
