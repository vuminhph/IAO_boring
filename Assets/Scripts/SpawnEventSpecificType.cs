using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEventSpecificType : MonoBehaviour
{
    List<GameObject> spawnLocations;
    List<GameObject> enemies = new List<GameObject>(); 
    public string typeOfEnemiesToSpawn;
    public string typeOfEnemiesToCheck;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            findEnemies();

            spawnLocations = GetComponent<FindChildrenWithTag>().GetChildren(transform, "spawnLocation");
            for (int i = 0; i < spawnLocations.Count; i++)
            {
                GameObject enemy = (GameObject)Instantiate(Resources.Load("Enemies/" + typeOfEnemiesToSpawn));
                enemy.SetActive(false);
                enemy.transform.position = spawnLocations[i].transform.position;
                enemies.Add(enemy);
            }
        }
    }

    public bool activated = false;

    public Transform rangePos;
    public float range;
    public LayerMask whatIsEnemies;
    Collider2D[] enemiesLeft;

    void findEnemies()
    {
        Vector2 a,b;
        a = b = rangePos.position;
        a.x -= range; b.x += range;
        a.y += range; b.y -= range;
        enemiesLeft = Physics2D.OverlapAreaAll(a, b, whatIsEnemies);
        activated = true;
    }
    
    bool allDead = false;
    void Update()
    {
        if (activated == true)
        {
            allDead = true;
            for (int i = 0; i < enemiesLeft.Length; i++)
            {   
                if (enemiesLeft[i]){
                    if (enemiesLeft[i].gameObject.name.Contains(typeOfEnemiesToCheck))
                    {
                        if (enemiesLeft[i].GetComponent<EnemyController>().dead == false) allDead = false;
                    }
                }
            }
        }
        
        if (allDead == true) spawnEnemies();
    }

    void spawnEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(true);
            enemies[i].GetComponent<EnemyController>().spawn();
        }
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rangePos.position, new Vector2(range, range));

    }
}
