using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_old : MonoBehaviour
{
    public float Speed = 5f;
    private float initialSpeed;

    public Rigidbody2D rb;
    public Animator animator;
    private int isFacingRight = 1;
    public bool allowInput = true;

    Vector2 movement;
    void Start()
    {
        // if (GameObject.Find("GameManager").GetComponent<GameManager>().lastCheckpointPos != new Vector2(-1000, -1000))
        //     resetPosition();
        initialSpeed = Speed;
    }

    void Update()
    {
        if (allowInput == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            handleMovement();
            handleHealthRegen();
            handleDeath();

            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        if (allowInput == true)
        {
            handleAttack();
            handleDash();
            handleHitted();

            rb.MovePosition(rb.position + movement * Speed * Time.fixedDeltaTime);
        }
    }

    public float dashSpeed = 50f;
    public float startDashTime = 0.1f;
    private float dashTime = 0f;

    void handleMovement()
    {
        if (movement.x > 0) isFacingRight = 1;
        else if (movement.x < 0) isFacingRight = -1;

        Vector3 facing = transform.localScale;
        facing.x = isFacingRight;
        transform.localScale = facing;
        if (movement.x != 0 && movement.y != 0)
        {
            movement.x = Convert.ToSingle(movement.x * 1 / Math.Sqrt(2));
            movement.y = Convert.ToSingle(movement.y * 1 / Math.Sqrt(2));
        }
    }


    public bool inCombat = false;
    public float regenRate;

    void handleHealthRegen()
    {
        if (inCombat == false && health < MaxHealth)
        {
            health += regenRate * Time.deltaTime;
            // Debug.Log(Time.deltaTime);
        }
    }

    void handleDash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            animator.SetTrigger("Dashing");
            animator.CrossFade("Dash", 0, 0);
            dashTime = startDashTime;
            GameObject[] Edges = GameObject.FindGameObjectsWithTag("Edge");
            for (int i = 0; i < Edges.Length; i++)
            {
                Edges[i].GetComponent<Collider2D>().enabled = false;
            }
        }
        if (dashTime > 0)
        {
            isInvincible = true;
            if (movement != new Vector2(0, 0))
            {
                transform.position += new Vector3(movement.x, movement.y, 0) * dashSpeed;
            }
            else
            {
                transform.position += new Vector3(isFacingRight, 0, 0) * dashSpeed;
            }
            dashTime -= Time.deltaTime;
        }
        else
        {
            GameObject[] Edges = GameObject.FindGameObjectsWithTag("Edge");
            for (int i = 0; i < Edges.Length; i++)
            {
                Edges[i].GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public float startBtwAttack;
    private float btwAttack = 0;
    public int attackPow;
    private int direction = 1; // 1 : side, 2: up, 3 : down

    void handleAttack()
    {
        if (btwAttack <= 0)
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (movement.y > 0) direction = 2;
                else if (movement.y < 0) direction = 3;
                else direction = 1;

                attackPos.position = transform.position;
                Vector3 attackVec = attackPos.position;
                Vector3 HitDirection = new Vector3(isFacingRight, 0, 0);
                switch (direction)
                {
                    case 1:
                        attackVec.x += 0.7f * isFacingRight;
                        attackVec.y -= 0.2f;
                        animator.SetTrigger("AttackingSide");
                        animator.CrossFade("AttackRight", 0, 0);
                        break;
                    case 2:
                        attackVec.y += 0.5f;
                        animator.SetTrigger("AttackingUp");
                        animator.CrossFade("AttackUp", 0, 0);
                        break;
                    case 3:
                        attackVec.y -= 0.9f;
                        animator.SetTrigger("AttackingDown");
                        animator.CrossFade("AttackDown", 0, 0);
                        break;
                }

                HitDirection.x = movement.x;
                HitDirection.y = movement.y;
                if (movement.x == 0 && movement.y == 0) HitDirection.x = isFacingRight;

                attackPos.position = attackVec;
                Vector3 a, b;
                a = b = attackPos.position;
                a.x -= attackRange; a.y += attackRange;
                b.x += attackRange; b.y -= attackRange;

                Collider2D[] enemiesToDamage = Physics2D.OverlapAreaAll(a, b, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i])
                    {
                        if (enemiesToDamage[i].GetComponent<EnemyController>())
                            enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(attackPow, HitDirection);
                        else if (enemiesToDamage[i].GetComponent<DestructablesController>())
                            enemiesToDamage[i].GetComponent<DestructablesController>().TakeDamage(attackPow, HitDirection);
                    }
                }
                btwAttack = startBtwAttack;
            }
        }
        else btwAttack -= Time.deltaTime;
    }
    public int MaxHealth;
    public float health;
    private float dazedTime;
    public float startDazedTime;
    public float knockedBackSpeed;
    private Vector3 knockedBackDirection;
    private bool isInvincible = false;

    public void TakeDamage(int damage, Vector3 direction)
    {
        if (isInvincible == false)
        {
            health -= damage;
            dazedTime = startDazedTime;
            invincibilityTime = startInvincibilityTime;
            animator.SetTrigger("Hitted");
            animator.CrossFade("Hitted", 0, 0);
            knockedBackDirection = direction;
            isInvincible = true;
        }
    }

    private float invincibilityTime;
    public float startInvincibilityTime;
    private float flashTime;

    void handleHitted()
    {
        if (dazedTime > 0)
        {
            Speed = 0;
            dazedTime -= Time.deltaTime;
            transform.position += knockedBackDirection * knockedBackSpeed * Time.fixedDeltaTime;
        }
        else
        {
            Speed = initialSpeed;
        }

        if (invincibilityTime > 0)
        {
            invincibilityTime -= Time.deltaTime;
        }
        else isInvincible = false;
    }

    void handleDeath()
    {
        if (health <= 0)
        {
            animator.CrossFade("Die", 0, 0);
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame();
        }
    }

    void spawnParticle_step()
    {

        GameObject smoke = (GameObject)Instantiate(Resources.Load("Particles/Particle_step"));
        Vector3 position = transform.position;
        position.y -= 0.3f;
        smoke.transform.position = position;
        Destroy(smoke, 1f);

    }

    public void fallThroughHole()
    {
        health -= 50;
        Invoke("resetPosition", 1.5f);
    }

    void resetPosition()
    {
        transform.position = GameObject.Find("GameManager").GetComponent<GameManager>().lastCheckpointPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRange, attackRange));

    }
}
