using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private GameObject target;
    public Transform box;
    public float length;
    public LayerMask whatisEnemies;
    public int attackPow;
    public Animator animator;
    // Update is called once per frame
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Delay());
    }

    public float startDelay;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(startDelay);
        animator.SetTrigger("Spike");
    }

    public void dealDamage()
    {
        Vector3 HitDirection = target.transform.position - transform.position;
        HitDirection *= (1 / HitDirection.magnitude);

        Collider2D playerToDamage = Physics2D.OverlapBox(box.position, new Vector2(length, length), whatisEnemies);
        if (playerToDamage)
        {
            if (playerToDamage.GetComponent<PlayerController>())
                playerToDamage.GetComponent<PlayerController>().TakeDamage(attackPow, HitDirection);
            else if (playerToDamage.GetComponent<EnemyController>())
                playerToDamage.GetComponent<EnemyController>().TakeDamage(attackPow, HitDirection);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(box.position, new Vector2(length, length));

    }
}
