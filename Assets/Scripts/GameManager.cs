using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Pathfinding;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float restartDelay;
    bool gameHasEnded = false;
    [HideInInspector]
    public static GameManager instance;
    UIController ui;
    FindChildrenWithTag ChildrenGetter;

    public bool boringModeOn;
    public bool adaptiveModeOn;
    enemyAttributes Melee;
    enemyAttributes Shooter;
    enemyAttributes QuickShooter;
    enemyAttributes Swordsman;
    public enemyAttributes Boss;

    public bool inMenu = true;
    public bool metBoss = false;

    void Awake()
    {
        playerSkill = 1f;
        skillFactor = 1f;
        health = 250f;

        Melee = new enemyAttributes(150f, 0f, 4.5f, 50);
        Shooter = new enemyAttributes(150f, 17f, 3f, 50);
        QuickShooter = new enemyAttributes(100f, 18f, 5f, 25);
        Swordsman = new enemyAttributes(200f, 0f, 3f, 50);
        Boss = new enemyAttributes(1000f, 15f, 5f, 25);

        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            if (instance.inMenu == false)
            {
                // instance.playerSkill = 1f;
                // instance.skillFactor = 1f;
                // instance.health = 250f;

                // instance.Melee = new enemyAttributes(150f, 0f, 4.5f, 50);
                // instance.Shooter = new enemyAttributes(150f, 17f, 3f, 50);
                // instance.QuickShooter = new enemyAttributes(100f, 18f, 5f, 25);
                // instance.Swordsman = new enemyAttributes(200f, 0f, 4f, 50);
                // instance.Boss = new enemyAttributes(1000f, 15f, 5f, 50);
            }
            instance.ui = GameObject.Find("Canvas").GetComponent<UIController>();
            instance.ChildrenGetter = GetComponent<FindChildrenWithTag>();
            Destroy(gameObject);
        }
    }

    public void resetGameManager()
    {
        playerSkill = 1f;
        skillFactor = 1f;
        health = 250f;

        Melee = new enemyAttributes(150f, 0f, 4.5f, 50);
        Shooter = new enemyAttributes(150f, 17f, 3f, 50);
        QuickShooter = new enemyAttributes(100f, 18f, 5f, 25);
        Swordsman = new enemyAttributes(200f, 0f, 3f, 50);
        Boss = new enemyAttributes(1000f, 15f, 5f, 25);

        score = combo = highestCombo = 0;
    }

    public float health;
    public int score = 0;
    public int combo = 0;
    public int highestCombo = 0;

    public void takeDamage(float damage)
    {
        ui.takeDamage(damage);
        health -= damage;
    }

    public void comboHit()
    {
        combo += 1;
        if (combo > highestCombo) highestCombo = combo;
        ui.UpdateCombo(combo);
    }

    public void comboBreak()
    {
        level_hitTaken += 1;
        combo = 0;
        ui.ComboBreak();
    }

    public void scoring(string enemyType)
    {
        level_enemiesKilled += 1;

        int enemyPoint = 0;

        if (enemyType.Contains("Melee")) enemyPoint = 100;
        if (enemyType.Contains("Shooter")) enemyPoint = 150;
        if (enemyType.Contains("QuickShooter")) enemyPoint = 200;
        if (enemyType.Contains("Swordsman")) enemyPoint = 300;
        if (enemyType.Contains("Boss")) enemyPoint = 1000;

        int multiplier = (int)combo / 10 + 1;

        score += enemyPoint * multiplier;
        ui.UpdateScore(score);
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            Time.timeScale = 0;
            gameHasEnded = true;
            ui.endGame(highestCombo, score);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().allowMovement = false;
            }

            // Invoke("Restart", restartDelay);
        }
    }

    [HideInInspector]
    public Vector2 lastCheckpointPos;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        health = 250f;
        score = level_score;
        combo = 0;
        calculatePlayerSkill();

        Invoke("restartCallback", 1f);
    }

    void restartCallback()
    {
        gameHasEnded = false;
    }

    // Save level data
    int level_score;
    int level_combo;

    // Adaptive mode
    int level_hitTaken;
    int level_enemiesKilled;
    float playerSkill;
    float level_health;
    public float skillFactor;


    public void Adapt()
    {
        level_health = health;
        level_score = score;
        level_combo = combo;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health = health;

        level_hitTaken = 0;
        level_enemiesKilled = 0;
        playerSkill = 0;
    }

    [HideInInspector]
    public float maxSkillFactor = 1.15f;
    [HideInInspector]
    public float minSkillFactor = 0.85f;

    public void calculatePlayerSkill()
    {
        // Debug.Log("health " + level_health);
        // Debug.Log("hit taken:" + level_hitTaken);
        // Debug.Log("enemies killed:" + level_enemiesKilled);
        playerSkill = 1 - level_hitTaken / ((int)level_health / 50 + 0.25f * level_enemiesKilled);
        skillFactor = playerSkill / 0.5f;
        if (skillFactor >= maxSkillFactor) skillFactor = maxSkillFactor;
        else if (skillFactor <= minSkillFactor) skillFactor = minSkillFactor;
        meleeAdapted = ShooterAdapted = QuickShooterAdapted = false;
        // Debug.Log(playerSkill);
    }

    bool meleeAdapted = false;
    bool ShooterAdapted = false;
    bool QuickShooterAdapted = false;
    bool SwordsmanAdapted = false;
    bool BossAdapted = false;

    public void enemyAdapt(EnemyController enemy)
    {

        if (enemy.gameObject.name.Contains("Melee"))
        {
            float Health = Melee.health;
            float Speed = Melee.speed;
            float AttackPow = Melee.attackPow;

            if (meleeAdapted == false)
            {
                Health *= skillFactor;
                Speed *= skillFactor;
                AttackPow *= skillFactor;
                meleeAdapted = true;
                Melee = new enemyAttributes(Health, 0f, Speed, AttackPow);
            }
            enemy.setHealth(Health);
            enemy.setMovementSpeed(Speed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("Shooter"))
        {
            float Health = Shooter.health;
            float BulletSpeed = Shooter.bulletSpeed;
            float AttackPow = Shooter.attackPow;

            if (ShooterAdapted == false)
            {
                Health *= skillFactor;
                BulletSpeed *= skillFactor;
                AttackPow *= skillFactor;
                ShooterAdapted = true;
                Shooter = new enemyAttributes(Health, BulletSpeed, 0f, AttackPow);
            }
            enemy.setHealth(Health);
            enemy.setBulletSpeed(BulletSpeed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("QuickShooter"))
        {
            float Health = QuickShooter.health;
            float BulletSpeed = QuickShooter.bulletSpeed;
            float AttackPow = QuickShooter.attackPow;

            if (QuickShooterAdapted == false)
            {
                Health *= skillFactor;
                BulletSpeed *= skillFactor;
                AttackPow *= skillFactor;
                QuickShooterAdapted = true;
                QuickShooter = new enemyAttributes(Health, BulletSpeed, 0f, AttackPow);
            }
            enemy.setHealth(Health);
            enemy.setBulletSpeed(BulletSpeed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("Swordsman"))
        {
            float Health = Swordsman.health;
            float Speed = Swordsman.speed;
            float AttackPow = Swordsman.attackPow;

            if (SwordsmanAdapted == false)
            {
                Health *= skillFactor;
                Speed *= skillFactor;
                AttackPow *= skillFactor;
                SwordsmanAdapted = true;
                Swordsman = new enemyAttributes(Health, 0f, Speed, AttackPow);
            }
            enemy.setHealth(Health);
            enemy.setMovementSpeed(Speed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("Boss"))
        {
            float Health = Boss.health;
            float BulletSpeed = Boss.bulletSpeed;
            float AttackPow = Boss.attackPow;

            if (BossAdapted == false)
            {
                Health *= skillFactor;
                BulletSpeed *= skillFactor;
                AttackPow *= skillFactor;
                BossAdapted = true;
                Boss = new enemyAttributes(Health, BulletSpeed, 0f, AttackPow);
            }
            enemy.setHealth(Health);
            enemy.setBulletSpeed(BulletSpeed);
            enemy.setAttacKPow(AttackPow);
        }


    }

    [HideInInspector]
    public float boringFactor = 0.3f;
    public void setEnemyStats(EnemyController enemy)
    {
        if (boringModeOn == false)
            boringFactor = 1;

        if (enemy.gameObject.name.Contains("Melee"))
        {
            float Health = Melee.health * boringFactor;
            float Speed = Melee.speed * boringFactor;
            float AttackPow = Melee.attackPow;

            enemy.setHealth(Health);
            enemy.setMovementSpeed(Speed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("Shooter"))
        {
            float Health = Shooter.health * boringFactor;
            float Speed = Shooter.speed * boringFactor;
            float BulletSpeed = Shooter.bulletSpeed * boringFactor;
            float AttackPow = Shooter.attackPow;

            enemy.setHealth(Health);
            enemy.setMovementSpeed(Speed);
            enemy.setBulletSpeed(BulletSpeed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("QuickShooter"))
        {
            float Health = QuickShooter.health * boringFactor;
            float Speed = QuickShooter.speed * boringFactor;
            float BulletSpeed = QuickShooter.bulletSpeed * boringFactor;
            float AttackPow = QuickShooter.attackPow;

            enemy.setHealth(Health);
            enemy.setMovementSpeed(Speed);
            enemy.setBulletSpeed(BulletSpeed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("Swordsman"))
        {
            float Health = Swordsman.health;
            float Speed = Swordsman.speed * boringFactor;
            float AttackPow = Swordsman.attackPow;

            enemy.setHealth(Health);
            enemy.setMovementSpeed(Speed);
            enemy.setAttacKPow(AttackPow);
        }

        else if (enemy.gameObject.name.Contains("Boss"))
        {
            float Health = Boss.health * 0.5f;
            float Speed = Boss.speed * boringFactor;
            float BulletSpeed = Boss.bulletSpeed * boringFactor;
            float AttackPow = Boss.attackPow * boringFactor;

            enemy.setHealth(Health);
            enemy.setMovementSpeed(Speed);
            enemy.setBulletSpeed(BulletSpeed);
            enemy.setAttacKPow(AttackPow);
        }
    }

      // Saving Scores
    const string privateCode = "ZHJOuW9FfUe2zUGkefuYIwnyQuesu8O0KotHMISv54fQ";
    const string publicCode = "5e57dab2fe232612b86b559c";
    const string webURL = "https://www.dreamlo.com/lb/";

    public void AddNewScore(string username, int score)
    {
        StartCoroutine(UploadNewScore(username, score));
    }

    IEnumerator UploadNewScore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/"+ WWW.EscapeURL(username) + "/" + score + "/" + (int) Time.time + "/" + "Boring");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            print ("Upload Successful");
        else
        {
            print ("Error uploading: " + www.error);
        }
    }

    public string Username = "";
    public bool scoreSaved = false;

    float startUpdateCooldown = 5f;
    public float updateCooldown = 0f;
    
    void Update()
    {
        if (updateCooldown <= 0)
        {
            updateCooldown = startUpdateCooldown;
            if (Username == "")
            {
                Username = System.Guid.NewGuid().ToString();
                scoreSaved = true;
            }
            AddNewScore(Username, score);
        }
        else
        {
            updateCooldown -= Time.deltaTime;
        }
    }

}
