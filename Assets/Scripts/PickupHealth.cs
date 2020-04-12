using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int HealthValue;
    public LayerMask whatIsPlayer;
    // Update is called once per frame
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("HPFly"))
        {
            GetComponent<Collider2D>().enabled = false;
        }
        else GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().health < other.GetComponent<PlayerController>().MaxHealth)
            {
                other.GetComponent<PlayerController>().health += HealthValue;
                GameObject.Find("Canvas").GetComponent<UIController>().gainHealth(HealthValue);
                GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().health += HealthValue;
                Destroy(gameObject);
            }
        }
    }
}
