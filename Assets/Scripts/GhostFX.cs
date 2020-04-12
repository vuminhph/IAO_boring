using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFX : MonoBehaviour
{
    public float startDelayTime;
    private float delayTime;
    public GameObject ghost;
    public bool makeGhost = false;

    void Start()
    {
        delayTime = startDelayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost == true)
        {
            if (delayTime > 0) delayTime -= Time.deltaTime;
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                delayTime = startDelayTime;
                Destroy(currentGhost, 1f);
            }
        }
    }
}
