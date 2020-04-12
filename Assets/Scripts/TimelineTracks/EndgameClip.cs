using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class EndgameClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private EndgameBehaviour template = new EndgameBehaviour();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.None;}
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<EndgameBehaviour>.Create(graph, template);
    }
}
