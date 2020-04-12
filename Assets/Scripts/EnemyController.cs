using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    protected GameObject target;
    protected Rigidbody2D rb;
    protected Animator animator;
    public String currentState;
    protected AIPath aiPath;
    protected AIDestinationSetter PathSetter;
    protected Vector2 UpperCorner;
    protected Vector2 DownCorner;

    public int isFacingRight = 1;
    protected float Scale;
    public bool allowMovement = true;

    protected virtual void Start()
    {
        Scale = transform.localScale.x;
        if (currentState != "Spawning") currentState = "LookingForPlayer";
    }

    protected void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        chasingTime = startChasingTime;
        PathSetter = GetComponent<AIDestinationSetter>();
        UpperCorner = GameObject.Find("UpperCorner").transform.position;
        DownCorner = GameObject.Find("DownCorner").transform.position;
        DownCorner.y -= 2f;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected void Update()
    {
        handleSpawn();
        findPlayer();

        if (currentState != "Dead")
        {
            Vector2 target_pos = new Vector2(target.transform.position.x, target.transform.position.y);
            movement = target_pos - (Vector2)transform.position;
            movement *= 1 / movement.magnitude;

            if (EnemyType == "Shooter") handleEvade();
        }

        if (currentState == "Moving") animator.SetFloat("Speed", movement.sqrMagnitude);
        else animator.SetFloat("Speed", 0);

        handleDeath();

    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, 0f);

        if (movement.x > 0) isFacingRight = 1;
        else if (movement.x < 0) isFacingRight = -1;
        Vector3 facing = transform.localScale;
        facing.x = isFacingRight * Scale;
        transform.localScale = facing;

        if (currentState != "Dead" && allowMovement == true)
        {
            handleIdle();
            handleCharge();
            handleHitted();
            handleMovement();
            handleAttack();
        }
        if (falling == true) handleFall();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
    }

    protected Vector2 aim()
    {
        Vector2 target_pos = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 aimDirection = target_pos - (Vector2)attackPos.position;
        aimDirection *= 1 / aimDirection.magnitude;
        return aimDirection;
    }

    protected Vector2 aimAway()
    {
        return -1 * aim();
    }

    public void reset(String resetStateFrom)
    {
        if (currentState == resetStateFrom)
            currentState = "Idle";
    }

    public void resetfromSpawn()
    {
        currentState = "LookingForPlayer";
    }

    //chasing player
    public Vector2 movement;
    public float detectionRange;
    public float chasingTime;
    public float startChasingTime;

    void findPlayer()
    {
        if (currentState == "LookingForPlayer")
        {
            disableMovement();
            // chasing player
            Collider2D detection = Physics2D.OverlapCircle(transform.position, detectionRange, whatIsEnemies);
            if (detection != null)
            {
                Vector3 position = transform.position;
                position.y += 1f;
                GameObject Exclamation = (GameObject)Instantiate(Resources.Load("Particles/Exclamation"), position, transform.rotation);
                Exclamation.transform.parent = transform;
                Destroy(Exclamation, 1f);
                detection.gameObject.transform.parent.GetComponent<PlayerController>().inCombat = true;
                currentState = "Idle";
            }
        }
    }

    protected void handleIdle()
    {
        if (currentState == "Idle") disableMovement();
    }

    public void setHealth(float Health)
    {
        health = Health;
    }

    public void setMovementSpeed(float Speed)
    {
        aiPath.maxSpeed = Speed;
    }

    public void setBulletSpeed(float BulletSpeed)
    {
        bulletSpeed = BulletSpeed;
    }

    public void setAttacKPow(float AttacKPow)
    {
        attackPow = AttacKPow;
    }

    protected bool chasingTimeResetted = false;

    protected virtual void handleMovement()
    {
        if (chasingTime > 0 && currentState == "Idle") currentState = "Moving";
        if (chasingTime <= 0)
        {
            reset("Moving");
            if (chasingTimeResetted == false)
            {
                chasingTimeResetted = true;
                Invoke("resetChasingTime", 3f);
            }
        }

        if (currentState == "Moving")
        {
            if (PathSetter.target == null) reset("Moving");
            if (gameObject.name.Contains("Ghost")) GhostMove();
            else enableMovement();
            chasingTime -= Time.fixedDeltaTime;
        }
    }

    public float safeZone;
    public bool evading = false;

    protected virtual void handleEvade()
    {
        if (evading == false)
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, safeZone, whatIsEnemies);
            if (player != null)
            {
                evading = true;
                evade();
            }
        }
        else if ((PathSetter.target.position - transform.position).magnitude <= 2)
        {
            evading = false;
            PathSetter.target = null;
            Destroy(newTarget);
        }
    }

    public int closestCorner;
    protected GameObject newTarget;
    protected virtual void evade()
    {
        Vector2[] Corners = { UpperCorner, new Vector2(DownCorner.x, UpperCorner.y), new Vector2(UpperCorner.x, DownCorner.y), DownCorner };
        Vector2 target_pos = target.transform.position;

        closestCorner = 0;
        float minDis = (Corners[0] - target_pos).magnitude;
        for (int i = 1; i < 4; i++)
        {
            float dis = (Corners[i] - target_pos).magnitude;
            if (dis < minDis)
            {
                minDis = dis;
                closestCorner = i;
            }
        }
        int pivotCorner = (closestCorner + 2) % 4;
        int check = -1;
        int checkX = -1;
        if (pivotCorner > 1) check = 1;
        if (pivotCorner % 2 == 0) checkX = 1;
        float RanX = checkX * UnityEngine.Random.Range(0f, Math.Abs(Math.Abs(Corners[1].x - Corners[0].x) - 4f)) + Corners[pivotCorner].x;
        float RanY = check * UnityEngine.Random.Range(0f, Math.Abs(0.57f * RanX)) + Corners[pivotCorner].y;

        if (Math.Abs(RanX - Corners[0].x) <= 1.5f) RanX = Corners[0].x + 1.5f;
        else if (Math.Abs(RanX - Corners[1].x) <= 1.5f) RanX = Corners[1].x - 1.5f;
        if (Math.Abs(RanY - Corners[0].y) <= 1.5f) RanY = Corners[0].y - 1.5f;
        else if (Math.Abs(RanY - Corners[2].y) <= 1.5f) RanY = Corners[2].y + 1.5f;

        newTarget = new GameObject();
        newTarget.transform.position = new Vector2(RanX, RanY);
        PathSetter.target = newTarget.transform;
    }

    protected void resetChasingTime()
    {
        chasingTimeResetted = false;
        chasingTime = startChasingTime;
    }

    public float startDazedTime;
    protected float dazedTime;
    public float health;
    public float knockedBackSpeed;
    [HideInInspector]
    public Vector2 knockedBackDirection;

    //take damage; called by attacker
    public virtual void TakeDamage(float damage, Vector2 direction)
    {
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

    // Getting hitted
    protected virtual void handleHitted()
    {
        if (dazedTime > 0) currentState = "TakingDamage";
        else if (dazedTime <= 0) reset("TakingDamage");

        if (currentState == "TakingDamage")
        {
            disableMovement();
            dazedTime -= Time.fixedDeltaTime;
            rb.MovePosition(rb.position + (Vector2)knockedBackDirection * knockedBackSpeed * Time.deltaTime);
        }
        else dazedTime = 0;
    }

    public LayerMask whatIsEnemies;
    public Transform attackPos;
    public float attackRadius;
    public float attackRange;
    protected float attackPow;
    // public float destroyDelay;
    public bool dead;
    protected bool HPDropped = false;

    protected void handleDeath()
    {
        if (currentState == "Dead")
        {
            if (HPDropped == false)
            {
                GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().scoring(gameObject.name);
                transform.parent = null;
                HPDropped = true;
                System.Random r = new System.Random();
                int ranInt = (r.Next(1, 5));
                if (ranInt == 3)
                {
                    GameObject HPDrop = (GameObject)Instantiate(Resources.Load("SystemObjects/HP_pickup"), transform.position, transform.rotation);
                    HPDrop.GetComponent<Animator>().Play("HPFly");
                    HPDrop.transform.parent = transform;
                }
                gameObject.layer = LayerMask.NameToLayer("Corpse");
                disableMovement();
                animator.CrossFade("Die", 0, 0);
            }
            // Destroy(gameObject, destroyDelay);
        }
    }


    public string EnemyType;
    protected Vector3 HitDirection;
    [HideInInspector]
    public float timeBtwAttack = 0f;
    public float startBtwAttack;
    //attack player
    public float chargeSpeed;

    protected virtual void handleAttack()
    {
        Collider2D playerInRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsEnemies);
        if ((playerInRange && currentState != "Charging" && currentState != "Spawning" && evading == false)) currentState = "Attacking";
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            reset("Attacking");
            timeBtwAttack = 0;
        }

        if (currentState == "Attacking")
        {
            disableMovement();
            if (timeBtwAttack <= 0)
            {
                timeBtwAttack = startBtwAttack;
                StartCoroutine(playAttackAnimation());
            }
            else timeBtwAttack -= Time.deltaTime;
        }
    }

    protected IEnumerator playAttackAnimation()
    {
        animator.CrossFade("Attack", 0, 0);
        animator.SetTrigger("Attack");

        bool done = true;
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            done = false;
            yield return null;
        }
        if (done == true)
        {
            reset("Attacking");
        }
    }

    protected virtual void dealDamage()
    {
        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsEnemies);
        if (playerToDamage)
        {
            playerToDamage.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage(attackPow, aim());
        }
    }

    Vector2 chargeMovement;
    void startCharging()
    {
        currentState = "Charging";
        chargeMovement = aim() * chargeSpeed;
    }

    void handleCharge()
    {
        if (currentState == "Charging")
            rb.MovePosition(rb.position + chargeMovement * Time.fixedDeltaTime);
    }



    public string bulletType;
    Vector2 FirstShotDirection;
    float bulletSpeed;

    void spawnProjectile()
    {
        GameObject bullet = (GameObject)Instantiate(Resources.Load("Projectiles/" + bulletType), attackPos.position, attackPos.rotation);
        FirstShotDirection = bullet.GetComponent<ProjectileController>().movement = aim();
        bullet.GetComponent<ProjectileController>().Speed = bulletSpeed;
        bullet.GetComponent<ProjectileController>().attackPow = attackPow;
    }

    void spawnProjectile_2ndShot()
    {
        GameObject bullet = (GameObject)Instantiate(Resources.Load("Projectiles/" + bulletType), attackPos.position, attackPos.rotation);
        bullet.GetComponent<ProjectileController>().movement = FirstShotDirection;
        bullet.GetComponent<ProjectileController>().Speed = bulletSpeed;
        bullet.GetComponent<ProjectileController>().attackPow = attackPow;
    }

    protected void disableMovement()
    {
        aiPath.canMove = false;
    }

    protected void enableMovement()
    {
        aiPath.canMove = true;
    }

    protected void spawnParticle_step()
    {
        GameObject smoke = (GameObject)Instantiate(Resources.Load("Particles/Particle_step"));
        Vector3 position = transform.position;
        position.y -= 0.3f;
        position.x -= 0.3f * isFacingRight;
        smoke.transform.position = position;
        Destroy(smoke, 1f);

    }

    public void spawn()
    {
        currentState = "Spawning";
        disableMovement();
    }

    protected void handleSpawn()
    {
        if (currentState == "Spawning")
        {
            animator.CrossFade("Spawn", 0, 0);
        }
    }

    //// REDUNDANT
    //  Ghost
    void GhostMove()
    {
        float Speed = aiPath.maxSpeed / 4;
        Vector2 target_pos = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 moveDirection = target_pos - (Vector2)transform.position;
        moveDirection.x = Convert.ToSingle(moveDirection.x / Math.Sqrt(2));
        moveDirection.y = Convert.ToSingle(moveDirection.y / Math.Sqrt(2));

        rb.MovePosition(rb.position + moveDirection * Speed * Time.fixedDeltaTime);
    }

    float startFallTime = 0.25f;
    private float fallTime;
    bool falling = false;
    public void Fall()
    {
        falling = true;
        fallTime = startFallTime;
        rb.gravityScale = 5f;
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
            dead = true;
            falling = false;
            Destroy(gameObject);
            Destroy(splashFX, 0.3f);
        }
        else fallTime -= Time.deltaTime;
    }
    ////

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, safeZone);
    }
}
