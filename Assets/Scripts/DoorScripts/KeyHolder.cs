using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    UIController ui;
    RoomController roomController;
    void Awake()
    {
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    bool HasKey = false;
    public void AddKey()
    {
        HasKey = true;
    }

    public void RemoveKey()
    {
        HasKey = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Contains("Key"))
        {
            ui.foundKey();
            GameObject key = collider.gameObject;
            AddKey();
            Destroy(key);
        }

        KeyDoor keyDoor = collider.GetComponent<KeyDoor>();
        if (keyDoor != null)
        {
            roomController = GameObject.FindGameObjectWithTag("CurrentRoom").GetComponent<RoomController>();
            if (HasKey == true && (roomController.allDead == true || roomController.enemyCount == 0))
            {
                ui.useKey();
                RemoveKey();
                keyDoor.OpenDoor();
            }
            else
            {
                keyDoor.PlayOpenFailAnim();
            }
        }
    }

}
