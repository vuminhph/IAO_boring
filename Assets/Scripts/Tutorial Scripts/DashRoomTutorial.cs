using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRoomTutorial : MonoBehaviour
{
    UIController ui;
    GameObject message;
    FindChildrenWithTag ChildrenGetter;

    void Start()
    {
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        message = ChildrenGetter.GetChildWithName(ui.transform, "Dash Tutorial");
    }

    bool fired = false;

    void Update()
    {
        if (fired == false)
        {
            GameObject bullet = GameObject.FindGameObjectWithTag("Projectile");
            if (bullet != null)
            {
                fired = true;
                Invoke("Trigger", 0.3f);
            }
        }
        if (fired == true && Input.GetButton("Dash") && Time.timeScale == 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().forcedDash = true;
            Invoke("stopDash", 0.35f);
            Time.timeScale = 1;
            Destroy(message);
            this.enabled = false;
        }
    }

    void stopDash()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().forcedDash = false;
    }
    void Trigger()
    {
        message.SetActive(true);
        Time.timeScale = 0;
    }
}
