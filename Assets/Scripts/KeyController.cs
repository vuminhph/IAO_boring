using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("KeyFly"))
        {
            GetComponent<Collider2D>().enabled = false;
        }
        else GetComponent<Collider2D>().enabled = true;
    }
}
