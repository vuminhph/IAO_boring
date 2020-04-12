using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedestalTrigger : MonoBehaviour
{
    public GameObject outroTimeline;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            outroTimeline.SetActive(true);
            Destroy(this);
        }
        else Physics2D.IgnoreCollision(other, gameObject.GetComponent<Collider2D>());
    }
}
