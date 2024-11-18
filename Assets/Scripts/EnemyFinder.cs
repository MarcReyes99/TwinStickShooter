using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    public float rotationSpeed;
    public Material targetMaterial;
    private GameObject targetQuad;
    private Transform nearestEnemy;

    private void Start()
    {
        targetQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        targetQuad.transform.localScale = new Vector3(1, 1, 1);
        targetQuad.GetComponent<Renderer>().material = targetMaterial;
        targetQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        targetQuad.SetActive(false);
    }

    private void Update()
    {
        nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            LookAtTarget(nearestEnemy.position);
            PlaceQuadAtTarget(nearestEnemy.position);
        }
        else
        {
            LookAtTarget(transform.position);
            PlaceQuadAtTarget(transform.position);
        }
    }

    private Transform FindNearestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0;

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void PlaceQuadAtTarget(Vector3 targetPosition)
    {
        targetQuad.SetActive(true);
        targetQuad.transform.position = new Vector3(targetPosition.x, 0.1f, targetPosition.z);
    }
}
