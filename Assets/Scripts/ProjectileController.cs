using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [HideInInspector]
    public float Speed;

    private GameObject target;
    Animator animator;
    public float attackPow;
    private float homingTime;
    public float startHomingTime;
    Rigidbody2D rb;
    private int isFacingRight;
    private bool allowMovement = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player");
        homingTime = startHomingTime;

        if (movement.x > 0) isFacingRight = 1;
        else if (movement.x < 0) isFacingRight = -1;

        Vector3 facing = transform.localScale;
        facing.x *= isFacingRight;
        transform.localScale = facing;

    }

    public Vector2 movement = new Vector2(0f, 0f);

    void FixedUpdate()
    {
        if (allowMovement == true)
        {
            if (homingTime > 0)
            {
                movement = target.transform.position - (Vector3)rb.position;
                movement *= 1 / movement.magnitude;
                homingTime -= Time.fixedDeltaTime;
            }

            rb.MovePosition(rb.position + Speed * movement * Time.fixedDeltaTime);
        }
        if (hitted == true)
        {
            allowMovement = false;
            StartCoroutine(playAnim("hit", playerToDamage));
        }

    }

    public LayerMask whatIsTarget;
    bool hitted = false;
    Collider2D playerToDamage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((whatIsTarget.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            hitted = true;
            if (other.gameObject.transform.parent.GetComponent<PlayerController>())
            {
                if (other.gameObject.transform.parent.GetComponent<PlayerController>().dashTime > 0)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other);
                    hitted = false;
                }
            }
            else if (other.gameObject.transform.parent.GetComponent<EnemyController>())
            {
                if (other.gameObject.transform.parent.GetComponent<EnemyController>().dead == true)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other);
                    hitted = false;
                }
            }

            playerToDamage = other;
        }
        else Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other);
    }


    IEnumerator playAnim(string AnimationCall, Collider2D playerToDamage)
    {
        animator.CrossFade(AnimationCall, 0, 0);

        //Wait until Animator is done playing
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationCall) &&
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        if (playerToDamage.gameObject.transform.parent.GetComponent<PlayerController>())
        {
            playerToDamage.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage(attackPow, movement);
        }

        if (playerToDamage.gameObject.transform.parent.GetComponent<EnemyController>() && (playerToDamage.gameObject.transform.parent.GetComponent<EnemyController>().dead == false))
            playerToDamage.gameObject.transform.parent.GetComponent<EnemyController>().TakeDamage(attackPow, movement);
    }

    void destroy()
    {
        Destroy(gameObject);
    }
}

