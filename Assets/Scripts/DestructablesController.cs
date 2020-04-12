using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablesController : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 100;
    public Animator animator;
    public bool containsHealth = false;
    public void TakeDamage(int damage, Vector3 direction)
    {
        health -= damage; 
        if (health <= 0) {
            animator.CrossFade("break", 0, 0);
            
            GetComponent<BoxCollider2D>().enabled = false;
            if (containsHealth == true)
            {
                GameObject HP_pickup = (GameObject) Instantiate(Resources.Load("HP_pickup"));
                Vector3 position = transform.position;
                position.x += 0.3f;
                HP_pickup.transform.position = position;
            }
            Destroy(gameObject,1f);
        }
        else animator.CrossFade("hit", 0, 0);
    }
}
