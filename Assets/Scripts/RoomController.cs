using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    GameObject[] Doors;
    FindChildrenWithTag ChildrenFinder;
    GameManager GM;
    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        ChildrenFinder = GetComponent<FindChildrenWithTag>();
        VirtualCam = ChildrenFinder.GetChildWithName(transform, "CM vcam");
        EnemiesParent = ChildrenFinder.GetChildWithName(transform, "EnemiesParent");
        DoorsParent = ChildrenFinder.GetChildWithName(transform, "DoorsParent");
        Corners = ChildrenFinder.GetChildren(transform, "Corners");
    }

    [HideInInspector]
    public bool allDead = false;
    bool DoorOpened = false;

    void Update()
    {
        if (allDead == false)
        {
            allDead = true;
            if (EnemiesChildren.Count == 0) allDead = false;
            else
            {
                for (int i = 0; i < enemyCount; i++)
                {
                    if (EnemiesChildren[i].GetComponent<EnemyController>().dead == false)
                        allDead = false;
                }
            }
        }
        else
        {
            if (DoorOpened == false && allDead == true && EnemiesChildren.Count != 0)
            {
                for (int i = 0; i < DoorsChildren.Count; i++)
                {
                    DoorsChildren[i].GetComponent<Animator>().Play("Open");
                }
                DoorOpened = true;
            }
        }
    }

    public int boringModeEnemyCount = 2;
    [HideInInspector]
    public int enemyCount = 1;

    GameObject VirtualCam;
    GameObject EnemiesParent;
    List<GameObject> EnemiesChildren = new List<GameObject>();
    GameObject DoorsParent;
    List<GameObject> DoorsChildren = new List<GameObject>();
    bool visited = false;
    List<GameObject> Corners = new List<GameObject>();
    GameObject[] Rooms;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(true);
            gameObject.tag = "CurrentRoom";
            VirtualCam.gameObject.name += Guid.NewGuid();
            VirtualCam.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().switchRoom();
            Rooms = GameObject.FindGameObjectsWithTag("Room");
            for (int i = 0; i < Rooms.Length; i++)
            {
                Rooms[i].SetActive(false);
            }
            Invoke("disableLastRoom", 0.1f);

            for (int i = 0; i < Corners.Count; i++)
            {
                Corners[i].SetActive(true);
            }

            if (visited == false)
            {
                visited = true;

                DoorsChildren = ChildrenFinder.GetChildren(DoorsParent.transform, "Door");
                EnemiesParent.SetActive(true);
                EnemiesChildren = ChildrenFinder.GetChildren(EnemiesParent.transform, "Enemies");

                // BORING MODE
                if (GM.boringModeOn == true && GetComponent<AttackRoomTutorial>() == null && GetComponent<DashRoomTutorial>() == null && GameObject.Find("Canvas").GetComponent<BossRoomUIController>() == null)
                {
                    if (EnemiesChildren.Count == 0) enemyCount = 0;
                    else enemyCount = UnityEngine.Random.Range(1, boringModeEnemyCount + 1);
                    for (int i = 0; i < EnemiesChildren.Count; i++)
                        EnemiesChildren[i].SetActive(false);
                }
                else
                {
                    enemyCount = EnemiesChildren.Count;
                }

                if (enemyCount != 0) Invoke("CloseDoors", 1f);

                for (int i = 0; i < enemyCount; i++)
                {
                    if (GM.boringModeOn == true)
                        EnemiesChildren[i].SetActive(true);

                    EnemyController enemy = EnemiesChildren[i].GetComponent<EnemyController>();
                    if (!enemy.gameObject.name.Contains("Boss"))
                    {
                        enemy.spawn();
                    }
                    else if (GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().metBoss == false)
                        enemy.gameObject.SetActive(false);
                    else
                        ChildrenFinder.GetChildWithName(GameObject.Find("Canvas").transform, "BossHealth").SetActive(true);

                    if (GM.adaptiveModeOn == true)
                        GM.enemyAdapt(enemy);
                    else
                        GM.setEnemyStats(enemy);
                }
            }
        }
        else
        {
            Physics2D.IgnoreCollision(other, gameObject.GetComponent<Collider2D>());
        }
    }

    void CloseDoors()
    {
        for (int i = 0; i < DoorsChildren.Count; i++)
        {
            DoorsChildren[i].GetComponent<Animator>().Play("Close");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.tag = "LastRoom";
            for (int i = 0; i < Rooms.Length; i++)
            {
                Rooms[i].SetActive(true);
            }
            if (lastRoom)
                lastRoom.SetActive(true);

            for (int i = 0; i < Corners.Count; i++)
            {
                Corners[i].SetActive(false);
            }
        }
    }

    GameObject lastRoom;
    void disableLastRoom()
    {
        lastRoom = GameObject.FindGameObjectWithTag("LastRoom");
        if (lastRoom)
        {
            lastRoom.tag = "Room";
            lastRoom.SetActive(false);
        }
    }


}
