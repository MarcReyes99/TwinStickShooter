using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float speedRotation;
    public float stoppingDistance;
    private Transform player;
    private Animator animator;
    private float currentSpeed;
    private bool isDead = false;
    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        LookAtPlayer();
        FollowPlayer(distanceToPlayer);
    }
    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
        }
    }
    private void FollowPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > stoppingDistance)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, speed);
        }
        else
        {
            currentSpeed = Mathf.Max(currentSpeed - acceleration * Time.deltaTime, 0f);
        }
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        
        if (animator != null)
        {
            animator.SetFloat("Velocity", currentSpeed / speed);
        }
    }
    public void Kill()
    {
        if (isDead == false)
        {
            isDead = true;
            Rigidbody mainRigidbody = GetComponent<Rigidbody>();
            if (mainRigidbody != null)
            {
                mainRigidbody.isKinematic = true;
            }

            if (animator != null)
            {
                animator.enabled = false;
            }

            Collider mainCollider = GetComponent<Collider>();
            if (mainCollider != null)
            {
                mainCollider.enabled = false;
            }

            Rigidbody[] childRigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in childRigidbodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            Destroy(this);
            Destroy(gameObject, 3);
        }
    }
}
