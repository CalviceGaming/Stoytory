
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] public int demagetoplayer;

    [SerializeField] public float damage;                   
    [SerializeField] private GameObject breakEffect;         
    [SerializeField] private GameObject rocketObjects;  
    
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioSource audioSource;
    
    private Rigidbody rb;
    private float despawnTimer = 0f;                         
    private bool toDestroy = false;                          
    private float destroyTimer = 0f;
    [SerializeField] public float speed = 10f;
    public Vector3 directionSet { private get; set; } = Vector3.zero;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        
       
        if (directionSet == Vector3.zero)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point; 
            }
            else
            {
                targetPoint = ray.origin + ray.direction * 200;
            }

            Vector3 direction = targetPoint - transform.position;
            rb.AddForce(direction.normalized * speed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(directionSet.normalized * speed, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Weapon" && other.gameObject.tag != "Bullets")
        {
            rb.velocity = Vector3.zero;
            rocketObjects.SetActive(false);         
            GetComponent<CapsuleCollider>().enabled = false; 
            
            float explosionRadius = 5f;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    var enemyHealth = hitCollider.GetComponent<EnemyHealthComponent>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.DealDamage(damage, transform.position);
                        player.GetComponent<StyleMeter>().AddStyleEvent.Invoke(damage/3);
                    }
                }

                if (hitCollider.CompareTag("Player"))
                {
                    var playerhealth = hitCollider.GetComponent<HealthComponent>();
                    if (playerhealth != null)
                    {
                        playerhealth.DealDamage(demagetoplayer);
                    }
                }
            }
            
            Instantiate(breakEffect, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().PlaySound(explosionSound, UnityEngine.Random.Range(1.1f, 1.1f));
            toDestroy = true;  
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    void Update()
    {
        RocketDespawn();
        toDestroyRocket();
    }

    private void RocketDespawn()
    {
        if (!toDestroy)
        {
            
            if (despawnTimer >= 10f)
            {
                Destroy(gameObject);
            }
            despawnTimer += Time.deltaTime;
        }
    }

    private void toDestroyRocket()
    {
        if (toDestroy)
        {
            if (destroyTimer >= 0.6f)
            {
                Destroy(gameObject);
            }
            destroyTimer += Time.deltaTime;
        }       
    }
}
