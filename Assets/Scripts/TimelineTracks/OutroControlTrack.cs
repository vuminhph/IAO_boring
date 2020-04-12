using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[TrackBindingType(typeof(GameObject))]
[TrackClipType(typeof(ArtifactControlClip))]
[TrackClipType(typeof(CameraShakerClip))]
[TrackClipType(typeof(EndgameClip))]
[TrackClipType(typeof(PlayerControlClip))]

public class OutroControlTrack : TrackAsset
{
}