using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    private float currentHealth;
    [SerializeField] private GameObject damageText;
    private EndArena endArena;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        endArena = gameObject.GetComponentInParent<EndArena>();
    }

    public void DealDamage(float damage, Vector3 hitPoint)
    {
        currentHealth -= damage;
        Instantiate(damageText).GetComponent<DamageTextScript>().DamageText(damage, hitPoint);
        if (currentHealth <= 0 && !isDead)
        {
            Destroy(gameObject);
            endArena.enemyDied.Invoke();
            isDead = true;
        }
    }
}
