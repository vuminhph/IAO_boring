using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossEnterTrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (c.gameObject.tag == "Player" && GM.metBoss == false)
        {
            timeline.Play();
            GM.metBoss = true;
        }
    }
}
