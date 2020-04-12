using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class PlayerControlBehaviour : PlayableBehaviour
{
    [SerializeField]
    private bool freeze;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {   
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player != null && freeze == true) 
        {
            player.allowInput = false;
            player.allowMovement = false;
        }
        else if (freeze == false)
        {
            player.allowInput = true;
            player.allowMovement = true;
        }
    }
}
