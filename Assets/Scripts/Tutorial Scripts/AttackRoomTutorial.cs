using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRoomTutorial : MonoBehaviour
{
    public GameObject enemy;
    UIController ui;
    GameObject message;
    FindChildrenWithTag ChildrenGetter;
    void OnEnable()
    {
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        message = ChildrenGetter.GetChildWithName(ui.transform, "Attack Tutorial");
    }

    public bool triggered = false;

    void Update()
    {
        if (enemy.active == true) Destroy(ChildrenGetter.GetChildWithName(ui.transform, "Movement Tutorial"));
        if (enemy.GetComponent<EnemyController>().currentState == "Moving" && triggered == false)
        {
            triggered = true;
            Invoke("Trigger", 1.5f);
        }

        if (triggered == true && Input.GetButtonDown("Attack"))
        {
            Time.timeScale = 1;
            Destroy(message);
            this.enabled = false;
        }
    }

    void Trigger()
    {
        message.SetActive(true);
        Time.timeScale = 0;
    }
}
