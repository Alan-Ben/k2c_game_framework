
using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

public class GTDTowerBattleStageMono : MonoBehaviour
{
    public Transform playerParent;
    public Transform bossParent;
    [ALHeader("战斗特效")]
    public List<GameObject> sfxList;
    [ALHeader("战斗10倍速特效")]
    public List<GameObject> sfxList10x;

}
