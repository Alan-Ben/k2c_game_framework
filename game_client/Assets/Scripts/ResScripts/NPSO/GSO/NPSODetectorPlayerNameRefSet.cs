
using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

[Serializable]
public class NPDetectorPlayerNameRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public string illegal_character;
}

public class NPSODetectorPlayerNameRefSet : _TALSOBasicRefSet<NPDetectorPlayerNameRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "detector_player_name"; } }
}