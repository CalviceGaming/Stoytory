using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    private float currentHealth;
    [SerializeField] private GameObject damageText;
    private EndArena endArena;

    void Start()
    {
        currentHealth = maxHealth;
        endArena = FindObjectOfType<EndArena>();
    }

    public void DealDamage(float damage, Vector3 hitPoint)
    {
        currentHealth -= damage;
        Instantiate(damageText).GetComponent<DamageTextScript>().DamageText(damage, hitPoint);
        if (currentHealth <= 0)
        {
            endArena.enemyDied.Invoke();
            Destroy(gameObject);
        }
    }
}
