using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{

    Collider damageCollider;
    public EnemyStats enemyStats;
    public int damage;
    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
        enemyStats=GetComponentInParent<EnemyStats>();
    }

    private void Start()
    {
        damage = enemyStats.damage;
    }
    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player =collision.transform.GetComponent<PlayerController>();
            print(player.name);
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
