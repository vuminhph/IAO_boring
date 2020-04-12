using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    // Start is called before the first frame update

    // private GameObject[] enemies;
    // private List<Transform> enemiesToSpawn = new List<Transform>();
    // private List<Transform> spawnLocations = new List<Transform>();
    // GameObject spawnLocList;
    // GameObject enemiesList;

    // void Start()
    // {
    //     enemiesList = GameObject.FindGameObjectWithTag("enemiesList");
    //     foreach (Transform enemy in enemiesList.transform)
    //     {
    //         enemiesToSpawn.Add(enemy);
    //     }
    //     spawnLocList = GameObject.FindGameObjectWithTag("spawnLocation");
    //     foreach (Transform location in spawnLocList.transform)
    //     {
    //         spawnLocations.Add(location);
    //     }
    // }

    // bool allDead = false;

    // public void spawnEnemies()
    // {
    //     for (int i = 0; i < enemiesToSpawn.Count; i++)
    //     {
    //         enemiesToSpawn[i].gameObject.SetActive(true);
    //         enemiesToSpawn[i].gameObject.tag = "Enemies";
    //         enemiesToSpawn[i].gameObject.GetComponent<EnemyController>().spawn();
    //         enemiesToSpawn[i].position = spawnLocations[i].position;
    //         Destroy(spawnLocations[i].gameObject);
    //     }
    //     Destroy(gameObject);
    // }
}