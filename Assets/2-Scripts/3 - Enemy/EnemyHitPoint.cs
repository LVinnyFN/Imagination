using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPoint : MonoBehaviour
{
    public GameObject myself;
    private Enemy me;
    void Start()
    {
        me = myself.GetComponent<Enemy>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && me.isAttacking && me.canDamage) me.player.TakeDamage(me.DealDamage());
        me.canDamage = false; //Prevent from dealing damage multiple times in the same attack
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && me.isAttacking && me.canDamage) me.player.TakeDamage(me.DealDamage());
        me.canDamage = false; //Prevent from dealing damage multiple times in the same attack
    }
}
