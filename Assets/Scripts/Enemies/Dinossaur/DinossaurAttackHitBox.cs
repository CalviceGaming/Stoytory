using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinossaurAttackHitBox : MonoBehaviour
{
    [SerializeField] private int damage;

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 2)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>().DealDamage(damage);
            timer = 0;
            gameObject.SetActive(false);
        }
    }
}
