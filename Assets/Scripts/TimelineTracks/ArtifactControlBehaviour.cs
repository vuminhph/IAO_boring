using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ArtifactControlBehaviour : PlayableBehaviour
{
    [SerializeField]
    private bool exploded;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {   
        bool alreadyExploded = false;

        GameObject ARTIFACT = GameObject.Find("ARTIFACT");
        var explodable = ARTIFACT.GetComponent<Explodable>();
        if (explodable != null && exploded == true && alreadyExploded == false)
        {
            alreadyExploded = true;
            explodable.explode();
            ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
		    ef.doExplosion(ARTIFACT.transform.position);
        }
    }
}
