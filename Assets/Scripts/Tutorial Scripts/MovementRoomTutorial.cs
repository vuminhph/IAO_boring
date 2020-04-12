using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRoomTutorial : MonoBehaviour
{
    UIController ui;
    GameObject message;
    Animator animator;
    FindChildrenWithTag ChildrenGetter;

    void Start()
    {
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        message = ChildrenGetter.GetChildWithName(ui.transform, "Movement Tutorial");
        animator = message.GetComponent<Animator>();
        Invoke("Trigger", 1f);
    }

    bool triggered = false;

    void Trigger()
    {
        triggered = true;
        message.SetActive(true);
        Invoke("fadeOut", 3f);
    }

    void fadeOut()
    {
        if (message)
        {
            animator.Play("fadeOut");
            this.enabled = false;
        }
    }
}