using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterController : MonoBehaviour
{
    private void OnEnable() {
        startTimeExist = Random.Range(1f, 5f); 
        timeExist = startTimeExist;
    }

    public float timeExist;
    float startTimeExist; 

    void Update()
    {   
        timeExist -= Time.deltaTime;
        if (timeExist <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
            {
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
            }
    } 
}
