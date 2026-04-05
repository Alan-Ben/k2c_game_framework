
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/VideoVolumeConfig")]
public class GSOVideoVolumeRefSet : ScriptableObject
{
    public SerializableDictionary<GVideoClipIndex, float> videoVolumeDic =
        new SerializableDictionary<GVideoClipIndex, float>();
    public static string assetPath { get { return "video/volume.unity3d"; } }
    public static string objName { get { return "video_volume_info"; } }
}
