using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinossaurMelee : MonoBehaviour
{
    [SerializeField] private GameObject hitBox;
    private GameObject player;
    private float attackTimer;
    [SerializeField] private float cooldown;
    public bool attacking;
    [SerializeField] private GameObject range;
    
    
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animator2;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
    }


    public void Attack()
    {
        if (attackTimer <= 0)
        {
            animator.SetTrigger("Attack");
            if (animator2)
            {
                animator2.SetTrigger("Attack");
            }
            attackTimer = cooldown;
            hitBox.SetActive(true);
            Vector3 direction = player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}
