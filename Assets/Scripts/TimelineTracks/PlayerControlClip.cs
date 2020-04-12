using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PlayerControlClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private PlayerControlBehaviour template = new PlayerControlBehaviour();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.None;}
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<PlayerControlBehaviour>.Create(graph, template);
    }
}
