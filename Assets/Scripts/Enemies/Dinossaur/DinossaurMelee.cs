using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinossaurMelee : MonoBehaviour
{
    [SerializeField] private GameObject hitBox;
    private GameObject player;
    private float attackTimer;
    [SerializeField] private float cooldown;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
        CheckPlayerRange();
    }

    void CheckPlayerRange()
    {
        Vector3 distance = player.transform.position - transform.position;
        if (distance.magnitude < 5)
        {
            Attack();
        }
        attackTimer += Time.deltaTime;
    }

    void Attack()
    {
        if (attackTimer > cooldown)
        {
            attackTimer = 0;
            hitBox.SetActive(true);
        }
    }
}
