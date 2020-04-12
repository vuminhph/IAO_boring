using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController

{
    FindChildrenWithTag ChildrenGetter;
    public bool invincible = false;

    bool moveDecided = false;
    bool moveRoll = false;
    Vector2 rollDirection;
    public float rollSpeed;

    public bool secondStage = false;
    public int specialDamage = 50;
    GameObject AreaFX;

    public bool recovered = false;

    protected override void Start()
    {
        Scale = transform.localScale.x;
        currentState = "Idle";
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        AreaFX = ChildrenGetter.GetChildWithName(transform, "AreaFX");
    }

    protected override void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, 0f);

        if (currentState != "Transitioning")
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
            handleRoll();
            handleAttack();
        }
        
        if (recovered == true)
        {
            animator.CrossFade("TransitionEnd", 0, 0);
            recovered = false;
        }
    }

    protected override void handleEvade()
    {
        if (evading == false && specialInit == false && spawnATKInit == false)
        {
            evading = true;
            evade();
        }

        if (PathSetter.target != null && (PathSetter.target.position - transform.position).magnitude <= 2)
        {
            evading = false;
            PathSetter.target = null;
            Destroy(newTarget);
        }

        if (PathSetter.target == null)
        {
            evading = false;
        }
    }

    protected override void handleMovement()
    {

        if (chasingTime > 0 && currentState == "Idle")
        {
            currentState = "Moving";
            moveDecided = false;
        }
        if (chasingTime <= 0)
        {
            reset("Moving");

            if (chasingTimeResetted == false)
            {
                chasingTimeResetted = true;
                if (moveRoll == true)
                {
                    moveRoll = false;
                    Invoke("resetChasingTime", 2f);
                }
                Invoke("resetChasingTime", 1f);
            }

        }

        if (currentState == "Moving")
        {
            chasingTime -= Time.fixedDeltaTime;
            if (moveDecided == false)
            {
                moveDecided = true;
                enableMovement();
                if (PathSetter.target != null && (transform.position - PathSetter.target.position).magnitude > 8f)
                {
                    disableMovement();
                    startRoll();
                }
            }
        }
    }

    void startRoll()
    {
        moveRoll = true;
        animator.CrossFade("StartRoll", 0, 0);
    }

    void handleRoll()
    {
        if (moveRoll == true)
        {
            invincible = true;
            if (evading == false && specialInit == false)
            {
                animator.CrossFade("EndRoll", 0, 0);
                chasingTime = -1f;
            }
            if (PathSetter.target != null)
            {
                rollDirection = PathSetter.target.position - transform.position;
                rollDirection *= 1 / rollDirection.magnitude;
                rb.MovePosition(rb.position + rollDirection * rollSpeed * Time.fixedDeltaTime);
            }
        }
    }

    bool doneAttacking = false;
    public float specialAtkCooldown;
    public float StartSpecialCooldown = 10f;
    public float spawnAtkCooldown;
    public float StartSpawnCooldown = 10f;

    protected override void handleAttack()
    {
        if (currentState == "Idle" && timeBtwAttack <= 0 || specialInit == true || spawnATKInit == true)
            currentState = "Attacking";

        if (currentState == "Attacking")
        {
            disableMovement();
            int rand = UnityEngine.Random.Range(0, 4);
            if ((rand == 0 && specialAtkCooldown <= 0 && secondStage == true) || specialInit == true)
            {
                playSpecialAttack();
            }
            else if (rand == 1 && spawnAtkCooldown <= 0 || spawnATKInit == true)
            {
                playSpawnAttack();
            }
            else StartCoroutine(playAttackAnimation());


            if (specialAtkCooldown > 0)
            {
                specialAtkCooldown -= Time.fixedDeltaTime;
            }
            if (spawnAtkCooldown > 0)
            {
                spawnAtkCooldown -= Time.fixedDeltaTime;
            }

        }
        else timeBtwAttack -= Time.fixedDeltaTime;
    }

    public bool specialInit = false;
    public bool specialPlayed = false;

    void playSpecialAttack()
    {
        if (specialInit == false)
        {
            invincible = true;
            specialInit = true;
            newTarget = new GameObject();
            newTarget.transform.position = target.transform.position;
            PathSetter.target = newTarget.transform;
            moveRoll = true;
            rollSpeed = 25;
            disableMovement();
            animator.CrossFade("StartRoll", 0, 0);
        }

        if (evading == false && specialPlayed == false)
        {
            animator.CrossFade("SpecialAttack", 0, 0);
            specialPlayed = true;
            moveRoll = false;
        }
    }

    void spawnSpecialFX()
    {
        if (gameObject.active == true)
        {
            AreaFX.SetActive(true);
            AreaFX.GetComponent<AreaDamage>().damage = 50;
            AreaFX.GetComponent<AreaDamage>().knockBack = 3;
        }
    }

    public bool spawnATKInit = false;
    public int spawnNumber = 3;
    public string enemyType = "Melee";

    void playSpawnAttack()
    {
        if (spawnATKInit == false)
        {
            invincible = true;
            spawnATKInit = true;
            disableMovement();
            animator.CrossFade("UseConsole", 0, 0);
        }
    }

    void spawnEnemy()
    {
        List<Vector2> spawnLocations = locationPicker(spawnNumber);
        for (int i = 0; i < spawnLocations.Count; i++)
        {
            GameObject Enemy = (GameObject)Instantiate(Resources.Load("Enemies/" + enemyType), spawnLocations[i], transform.rotation);
            EnemyController enemy = Enemy.GetComponent<EnemyController>();
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().setEnemyStats(enemy);
            enemy.detectionRange += 10f;
            enemy.spawn();
        }
    }

    List<Vector2> locationPicker(int numberOfLocation)
    {
        List<Vector2> locations = new List<Vector2>();

        Vector2[] Corners = { UpperCorner, new Vector2(DownCorner.x, UpperCorner.y), new Vector2(UpperCorner.x, DownCorner.y), DownCorner };

        for (int i = 0; i < numberOfLocation; i++)
        {
            int pivotCorner = UnityEngine.Random.Range(0, 4);
            int check = -1;
            int checkX = -1;
            if (pivotCorner > 1) check = 1;
            if (pivotCorner % 2 == 0) checkX = 1;
            float RanX = checkX * UnityEngine.Random.Range(0f, Math.Abs(Math.Abs(Corners[1].x - Corners[0].x) - 4f)) + Corners[pivotCorner].x;
            float RanY = check * UnityEngine.Random.Range(0f, Math.Abs(Math.Abs(Corners[2].y - Corners[0].y) - 4f)) + Corners[pivotCorner].y;

            if (Math.Abs(RanX - Corners[0].x) <= 1.5f) RanX += 1.5f;
            else if (Math.Abs(RanX - Corners[1].x) <= 1.5f) RanX -= 1.5f;
            if (Math.Abs(RanY - Corners[0].y) <= 1.5f) RanY -= 1.5f;
            else if (Math.Abs(RanY - Corners[2].y) <= 1.5f) RanY += 1.5f;

            Vector2 location = new Vector2(RanX, RanY);
            locations.Add(location);
        }

        return locations;
    }

    public Transform KeyDrop;
    public override void TakeDamage(float damage, Vector2 direction)
    {
        if (invincible == false)
        {
            health -= damage;
            GameObject.Find("Canvas").GetComponent<BossRoomUIController>().bossDamage(damage);
            evade();
            dazedTime = startDazedTime;
            animator.SetTrigger("Hitted");
            animator.CrossFade("Hitted", 0, 0);

        }
        if (health <= 0)
        {
            dead = true;
            currentState = "Dead";
            GameObject Key = (GameObject)Instantiate(Resources.Load("SystemObjects/Key"), KeyDrop);
            Key.transform.parent = KeyDrop;
        }
    }
}

