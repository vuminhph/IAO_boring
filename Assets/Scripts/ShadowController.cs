using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    Transform parent;
    void Start()
    {
        parent = transform.parent;
    }

    void Update()
    {
        if (parent.GetComponent<EnemyController>())
        {
            if (parent.GetComponent<EnemyController>().dead == true)
                Destroy(gameObject);   
        }
    }
}
