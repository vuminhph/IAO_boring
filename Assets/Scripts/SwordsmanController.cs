using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanController : EnemyController
{
    public bool invincible = false;

    bool dashing = false;
    float dashSpeed = 10f;

    protected override void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, 0f);

        if (attackInit == false)
        {
            if (movement.x > 0) isFacingRight = 1;
            else if (movement.x < 0) isFacingRight = -1;
            Vector3 facing = transform.localScale;
            facing.x = isFacingRight * Scale;
            transform.localScale = facing;
        }

        if (currentState != "Dead" && allowMovement == true)
        {
            handleIdle();
            handleHitted();
            handleMovement();
            handleDash();
            handleAttack();
        }
    }

    float startDashTime = 1f;
    float dashTime = 0f;

    void handleDash()
    {
        if (dashing == true)
        {
            dashTime -= Time.fixedDeltaTime;
            invincible = true;

            if (dashTime <= 0)
            {
                curDirection = new Vector2(0, 0);
                dashing = false;
                animator.CrossFade("FinishAtk", 0, 0);
                attackInit = false;
                invincible = false;
                attackPos.position = gameObject.transform.position;
                timeBtwAttack = startBtwAttack;
                reset("Attacking");
            }

            if (curDirection == new Vector2(0, 0) && dashing == true)
            {
                curDirection = PathSetter.target.position - transform.position;
                curDirection *= 1 / curDirection.magnitude;
            }
            rb.MovePosition(rb.position + curDirection * dashSpeed * Time.fixedDeltaTime);
        }
    }

    Vector2 curDirection;
    protected override void handleAttack()
    {
        if ((currentState == "Idle" && timeBtwAttack <= 0) || attackInit == true)
            currentState = "Attacking";

        if (currentState == "Attacking")
        {
            playAttack();
        }
        else timeBtwAttack -= Time.fixedDeltaTime;
    }

    public bool attackInit = false;
    public bool dealtDamage = false;

    void playAttack()
    {
        if (attackInit == false)
        {
            resetChasingTime();
            invincible = true;
            attackInit = true;

            dashing = true;
            dealtDamage = false;
            disableMovement();
            dashTime = startDashTime;
            animator.CrossFade("StartAtk", 0, 0);
        }
        Collider2D other = Physics2D.OverlapCircle(transform.position, attackRange, whatIsEnemies);
        if (attackInit == true && dealtDamage == false && other != null)
        {
            dealDamage_first();
            dealtDamage = true;
        }
    }

    protected void dealDamage_first()
    {
        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsEnemies);
        if (playerToDamage)
        {
            playerToDamage.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage(attackPow, aim());
        }
    }

    public override void TakeDamage(float damage, Vector2 direction)
    {
        dealtDamage = true;
        currentState = "TakingDamage";
        // evade();
        health -= damage;
        dazedTime = startDazedTime;
        animator.SetTrigger("Hitted");
        animator.CrossFade("Hitted", 0, 0);
        knockedBackDirection = direction;
        if (health <= 0)
        {
            dead = true;
            currentState = "Dead";
        }
    }

}
