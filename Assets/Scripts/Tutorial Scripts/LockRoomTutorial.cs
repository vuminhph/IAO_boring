using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRoomTutorial : MonoBehaviour
{
    UIController ui;
    GameObject message;
    FindChildrenWithTag ChildrenGetter;
    Animator animator;
    
    public TriggerZoneController tz;

    void OnEnable()
    {
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        message = ChildrenGetter.GetChildWithName(ui.transform, "Lock Tutorial");
        // animator = GameObject.FindGameObjectWithTag("LockedDoor").GetComponent<Animator>();
    }

    public bool triggered = false;
    bool fired = false;
    
    void Update()
    {   
        if (fired == false && tz.triggered == true)
        {
            fired = true;
            message.SetActive(true);
            Invoke("Trigger", 3f);
        }
    }

    void Trigger()
    {
        message.GetComponent<Animator>().Play("fadeOut");
    }
}
