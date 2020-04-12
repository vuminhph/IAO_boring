using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using EZCameraShake;


[Serializable]
public class CameraShakerBehaviour : PlayableBehaviour
{
    [SerializeField]
    private bool shaking;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {   
        CameraShaker camShaker = GameObject.Find("OutroCam").GetComponent<CameraShaker>();
        if (camShaker != null && shaking == true)
        {
            camShaker.ShakeOnce(1.5f, 1.5f, .1f, .5f);
        }   
    }
}
