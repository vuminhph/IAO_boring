using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    public LayerMask whatIsEnemies;
    public int damage = 0;
    public int knockBack = 5;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((whatIsEnemies.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Vector3 HitDirection = other.transform.position - transform.position;
            HitDirection *= 5 / HitDirection.magnitude;

            if (other.transform.parent.gameObject.CompareTag("Player"))
            {
                other.transform.parent.gameObject.GetComponent<PlayerController>().TakeDamage(damage, HitDirection);
            }
            else if (other.transform.parent.gameObject.CompareTag("Enemies") && other.transform.parent.gameObject.GetComponent<EnemyController>().dead == false)
            {
                other.transform.parent.gameObject.GetComponent<EnemyController>().TakeDamage(damage, HitDirection);
            }
        }
    }

    void disable()
    {
        gameObject.SetActive(false);
    }
}
