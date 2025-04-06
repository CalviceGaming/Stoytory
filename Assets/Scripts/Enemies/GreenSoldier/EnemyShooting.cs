using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float range;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawn;
    private float shootingTimer;
    [SerializeField] private int shotAmount;
    [SerializeField] private GameObject bulletsParent;

    public bool shooting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<PathFinding>().newTile.AddListener(IsPlayerInRange);
        bulletsParent = GameObject.FindGameObjectWithTag("BulletParent");
    }

    void Update()
    {
        StartShooting();
    }

    void IsPlayerInRange()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= range)
        {
            CheckForWall();
        }
        else
        {
            if (shooting)
            {
                player.GetComponent<TilePlayerOn>().playerChangedTile.Invoke();
            }
            shooting = false;
        }
    }

    void CheckForWall()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "Bullet")
            {
                shooting = true;
            }
            else
            {
                if (shooting)
                {
                    player.GetComponent<TilePlayerOn>().playerChangedTile.Invoke();
                }
                shooting = false;
            }
        }
    }

    void StartShooting()
    {
        if (shooting)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction); 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 100 * Time.deltaTime);

            if (shootingTimer >= 1.5)
            {
                IsPlayerInRange();
                shootingTimer = 0;
                Instantiate(bullet, bulletSpawn.transform.position, Quaternion.identity, bulletsParent.transform);
            }
        }
        shootingTimer += Time.deltaTime;
    }
}
