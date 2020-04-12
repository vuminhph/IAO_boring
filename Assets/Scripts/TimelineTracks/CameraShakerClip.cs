using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CameraShakerClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private CameraShakerBehaviour template = new CameraShakerBehaviour();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.None;}
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<CameraShakerBehaviour>.Create(graph, template);
    }
}
