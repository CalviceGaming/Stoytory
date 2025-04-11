
using UnityEngine;

public class Explosion : MonoBehaviour
{
     
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded && collision.gameObject.CompareTag("Bullets"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            Debug.Log("Explosion overlap hit: " + hit.name);
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealthComponent health = hit.GetComponentInParent<EnemyHealthComponent>();
                if (health != null)
                {
                    Debug.Log("Applying damage to: " + hit.name);
                    health.DealDamage(explosionDamage, transform.position);
                }
                else
                {
                    Debug.Log("EnemyHealthComponent not found on " + hit.name);
                }
            }
            
            if (hit.CompareTag("Barrel"))
            {
                Explosion otherBarrel = hit.GetComponentInParent<Explosion>();
                if (otherBarrel != null && !otherBarrel.hasExploded)
                {
                    otherBarrel.Explode(); 
                    Debug.Log("im also exploding");
                }
            }
            
        }
        Debug.Log("im killing myself");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Show explosion radius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
