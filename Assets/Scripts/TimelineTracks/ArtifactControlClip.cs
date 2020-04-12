using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ArtifactControlClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private ArtifactControlBehaviour template = new ArtifactControlBehaviour();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.None;}
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<ArtifactControlBehaviour>.Create(graph, template);
    }
}
