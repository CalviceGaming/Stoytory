
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
          
    [SerializeField] public float damage;                   
    [SerializeField] private GameObject breakEffect;         
    [SerializeField] private GameObject rocketObjects;       
    
    private Rigidbody rb;
    private float despawnTimer = 0f;                         
    private bool toDestroy = false;                          
    private float destroyTimer = 0f;
    [SerializeField] public float speed = 10f;
    public Vector3 directionSet { private get; set; } = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
       
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
        Debug.Log("[Rocket] Starting rocket movement. Spawned at: " + transform.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Weapon" && other.gameObject.tag != "Bullets")
        {
            rb.velocity = Vector3.zero;
            rocketObjects.SetActive(false);         
            GetComponent<CapsuleCollider>().enabled = false;  

          
            if (other.gameObject.tag == "Enemy")
            {
                // Deal damage to enemy
                other.gameObject.GetComponent<EnemyHealthComponent>().DealDamage(damage, transform.position);
            }
            
            // Instantiate the break/explosion effect
            Instantiate(breakEffect, transform.position, Quaternion.identity);

            toDestroy = true;  
        }
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
            // Destroy rocket after explosion effect has played
            if (destroyTimer >= 0.6f)
            {
                Destroy(gameObject);
            }
            destroyTimer += Time.deltaTime;
        }       
    }
}
