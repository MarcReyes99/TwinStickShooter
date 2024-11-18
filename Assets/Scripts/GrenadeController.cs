using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrenadeController : MonoBehaviour
{
    Rigidbody rb;
    public LayerMask mask;
    public float launchForce;
    public float timer;
    public float radius;
    public float explosionForce;
    public GameObject particles;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * launchForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        rb.AddTorque(randomTorque * launchForce, ForceMode.Impulse);
        StartCoroutine(ExplodeAfterDelay());
    }
    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(timer);
        Explode();
    }
    private void Explode()
    {
        if (particles != null)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask);

        foreach (Collider collider in colliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Kill();
            }

            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius, 1f, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
