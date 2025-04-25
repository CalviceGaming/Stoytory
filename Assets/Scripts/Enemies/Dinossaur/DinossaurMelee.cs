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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
    }
    

    public void Attack()
    {
        if (attackTimer > cooldown)
        {
            attackTimer = 0;
            hitBox.SetActive(true);
        }
        attackTimer += Time.deltaTime;
    }
}
