using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    private float initialSpeed;

    Rigidbody2D rb;
    Animator animator;
    Camera cam;
    CameraShaker cs;
    public bool switchingRoom = false;

    private int isFacingRight = 1;
    public bool allowInput;
    private Vector2 lastDirection;
    [HideInInspector]   
    public Vector2 movement;

    FindChildrenWithTag ChildrenGetter;

    void Awake()
    {
        // if (GameObject.Find("GameManager").GetComponent<GameManager>().lastCheckpointPos != new Vector2(-1000, -1000))
        //     resetPosition();
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        allowInput = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialSpeed = Speed;
        health = (float)MaxHealth;

        Time.timeScale = 1;
        gameManager.Adapt();
    }

    Vector3 mousePos;
    Vector2 lookDirection;

    void Update()
    {
        if (allowInput == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            lookDirection = mousePos - transform.position;
            lookDirection *= 1 / lookDirection.magnitude;
            animator.SetFloat("LookHorizontal", lookDirection.x);
            animator.SetFloat("LookVertical", lookDirection.y);
            animator.SetFloat("LastHorizontal", lastDirection.x);
            animator.SetFloat("LastVertical", lastDirection.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Horizontal", movement.x);
            if (movement != new Vector2(0, 0)) lastDirection = movement;
        }
        handleDeath();
        // handleHealthRegen();
    }

    [HideInInspector]   
    public bool allowMovement = true;

    void FixedUpdate()
    {
        handleHitted();
        
        if (allowMovement == true)
        {
            handleMovement();
        }

        handleAttackForward(); //ALWAYS ACTIVE
        if (allowInput == true)
        {
            handleAttack();
            handleDash();
        }
        if (falling == true) handleFall();
    }

    public float dashSpeed = 50f;
    public float startDashTime = 0.1f;
    [HideInInspector]
    public float dashTime = 0f;

    public void switchRoom()
    {
        GameObject currentRoom = GameObject.FindGameObjectWithTag("CurrentRoom");
        GameObject camera = ChildrenGetter.GetChildWithName(currentRoom.transform, "CM vcam");
        cam = camera.GetComponent<Camera>();
        cs = camera.GetComponent<CameraShaker>();
    }

    void handleMovement()
    {
        rb.velocity = new Vector2(0, 0);
        if (movement.x > 0) isFacingRight = 1;
        else if (movement.x < 0) isFacingRight = -1;

        if (movement.x != 0 && movement.y != 0)
        {
            movement.x = Convert.ToSingle(movement.x / Math.Sqrt(2));
            movement.y = Convert.ToSingle(movement.y / Math.Sqrt(2));
        }
        rb.MovePosition(rb.position + movement * Speed * Time.fixedDeltaTime);
    }

    public bool inCombat = false;
    public bool forcedDash = false;
    float timeBtwDash;
    public float startBtwDash;

    void handleDash()
    {
        if (Input.GetButtonUp("Dash")) forcedDash = false;
        if ((Input.GetButtonDown("Dash") || forcedDash == true) && timeBtwDash <= 0)
        {
            timeBtwDash = startBtwDash;
            attacking = false;
            GetComponent<GhostFX>().makeGhost = true;
            animator.SetTrigger("Dashing");
            animator.CrossFade("Dash", 0, 0);
            dashTime = startDashTime;
            GameObject[] Edges = GameObject.FindGameObjectsWithTag("Edge");
            GameObject[] Bullets = GameObject.FindGameObjectsWithTag("Projectile");
            // for (int i = 0; i < Edges.Length; i++)
            // {
            //     Edges[i].GetComponent<Collider2D>().enabled = false;
            // }
            // for (int i = 0; i < Bullets.Length; i++)
            // {
            //     Bullets[i].GetComponent<Collider2D>().enabled = false;
            // }
        }
        else timeBtwDash -= Time.deltaTime;

        if (dashTime > 0)
        {
            isInvincible = true;
            Vector3 dashDirection = new Vector3(lastDirection.x, lastDirection.y, 0);
            dashDirection *= 1 / dashDirection.magnitude;
            transform.position += dashDirection * dashSpeed;
            dashTime -= Time.deltaTime;
        }
        else
        {
            isInvincible = false;
            GetComponent<GhostFX>().makeGhost = false;
            GameObject[] Edges = GameObject.FindGameObjectsWithTag("Edge");
            GameObject[] Bullets = GameObject.FindGameObjectsWithTag("Projectile");
            for (int i = 0; i < Edges.Length; i++)
            {
                Edges[i].GetComponent<Collider2D>().enabled = true;
            }
            for (int i = 0; i < Bullets.Length; i++)
            {
                Bullets[i].GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    public Transform attackPos;
    public float attackRange;
    public int attackPow;
    private int direction;
    public LayerMask whatIsEnemies;
    public float startBtwAttack;
    public float btwAttack = 0;
    public float startBtwCombo = 3f;
    public float btwCombo;

    private int combo = -1;
    private Vector2 HitDirection;
    public float AttackFwd;
    private bool attacking = false;
    public bool forcedAttack = false;

    void handleAttack()
    {
        if (Input.GetButtonUp("Attack")) forcedAttack = false;
        if ((Input.GetButtonDown("Attack") || forcedAttack == true) && btwCombo <= 0)
        {
            attackPos.position = transform.position;
            Vector2 attackVec = attackPos.position;
            HitDirection = lookDirection;

            disableInput();
            attacking = true;

            if (combo == -1) combo = 0;
            if (combo == 0)
            {
                if (lookDirection.y < 0) attackPos.position = transform.position + (Vector3)lookDirection;
                else attackPos.position = transform.position + (Vector3)lookDirection * 0.1f;
                animator.CrossFade("Combo1", 0, 0);
                btwAttack = startBtwAttack;
                combo = 1;
            }
            else if (btwAttack >= 0 && combo == 1)
            {
                if (lookDirection.y < 0) attackPos.position = transform.position + (Vector3)lookDirection;
                else attackPos.position = transform.position + (Vector3)lookDirection * .7f;
                animator.CrossFade("Combo2", 0, 0);
                btwAttack = startBtwAttack;
                combo = 2;
            }
            else if (btwAttack >= 0 && combo == 2)
            {
                if (lookDirection.y < 0) attackPos.position = transform.position + (Vector3)lookDirection;
                else attackPos.position = transform.position + (Vector3)lookDirection * .7f;
                animator.CrossFade("Combo3", 0, 0);
                btwAttack = startBtwAttack;
                btwCombo = startBtwCombo;
                combo = 0;
            }
            Invoke("dealDamage", 0.035f);
        }
        else if (btwAttack < 0 && (animator.GetCurrentAnimatorStateInfo(0).IsName("Combo1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Combo2") || animator.GetCurrentAnimatorStateInfo(0).IsName("Combo3")))
        {
            combo = -1;
            animator.CrossFade("Idle", 0, 0);
        }
        else if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Combo1")) && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Combo2")) && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Combo3")))
            combo = -1;
        else
        {
            btwAttack -= Time.fixedDeltaTime;
        }

        if (combo == -1)
        {
            btwCombo -= Time.fixedDeltaTime;
        }

    }

    void handleAttackForward()
    {
        if (attacking == true)
        {
            rb.MovePosition(rb.position + lookDirection * AttackFwd * Time.fixedDeltaTime);
        }
    }

    bool hit = false;
    public void dealDamage()
    {
        Vector2 a, b;
        a = b = attackPos.position;
        a.x -= attackRange; a.y += attackRange;
        b.x += attackRange; b.y -= attackRange;

        Collider2D[] enemiesToDamage = Physics2D.OverlapAreaAll(a, b, whatIsEnemies);
        hit = false;
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i])
            {
                if (hit == false) hit = true;
                cs.ShakeOnce(1.5f, 1.5f, .1f, .5f);
                if (enemiesToDamage[i].gameObject.transform.parent.GetComponent<EnemyController>() && enemiesToDamage[i].gameObject.transform.parent.GetComponent<EnemyController>().dead == false)
                    enemiesToDamage[i].gameObject.transform.parent.GetComponent<EnemyController>().TakeDamage(attackPow, HitDirection);
                // else if (enemiesToDamage[i].GetComponent<DestructablesController>())
                //     enemiesToDamage[i].GetComponent<DestructablesController>().TakeDamage(attackPow, HitDirection);
            }
        }

        if (hit == true)
        {
            gameManager.comboHit();
        }

        attacking = false;
        enableInput();
    }


    public int MaxHealth;
    public float health;
    private float dazedTime;
    public float startDazedTime;
    public float knockedBackSpeed;
    private Vector3 knockedBackDirection;
    private bool isInvincible = false;
    GameManager gameManager;

    public void TakeDamage(float damage, Vector3 direction)
    {
        if (isInvincible == false)
        {
            gameManager.comboBreak();
            attacking = false;
            health -= damage;
            gameManager.takeDamage(damage);
            dazedTime = startDazedTime;
            animator.SetTrigger("Hitted");
            animator.CrossFade("Hitted", 0, 0);
            knockedBackDirection = direction;
        }
    }

    void handleHitted()
    {
        if (dazedTime > 0)
        {
            Speed = 0;
            // disableInput();
            dazedTime -= Time.deltaTime;
            transform.position += knockedBackDirection * knockedBackSpeed * Time.fixedDeltaTime;
        }
        else if (dead == false)
        {
            // enableInput();
            Speed = initialSpeed;
        }
    }

    bool dead = false;
    void handleDeath()
    {
        if (health <= 0)
        {
            dead = true;
            disableInput();
            isInvincible = true;
            gameManager.EndGame();
            animator.CrossFade("Die", 0, 0);
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

    float startFallTime = 0.25f;
    private float fallTime;
    bool falling = false;

    public void Fall()
    {
        health -= 50;
        falling = true;
        fallTime = startFallTime;
        rb.gravityScale = 50f;
    }

    bool Splashed = false;
    GameObject splashFX;
    void handleFall()
    {
        if (fallTime <= 0)
        {
            if (Splashed == false)
            {
                splashFX = (GameObject)Instantiate(Resources.Load("Particles/Splash"), transform.position, transform.rotation);
                Splashed = true;
            }
            Invoke("resetPosition", 0.5f);
            falling = false;
            gameObject.SetActive(false);
        }
        else fallTime -= Time.deltaTime;
    }

    void resetPosition()
    {
        Splashed = false;
        gameObject.SetActive(true);
        Destroy(splashFX);
        rb.gravityScale = 0f;
        transform.position = gameManager.lastCheckpointPos;
    }

    PlayerStats currentStats = new PlayerStats();
    public void savePlayerStats()
    {

    }

    void disableInput()
    {
        allowInput = false;
    }

    void enableInput()
    {
        allowInput = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRange, attackRange));

    }
}
