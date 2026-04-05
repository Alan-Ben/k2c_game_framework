using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

// 杰出者大厅主界面FollowMono
public class GTDGraveMainFollowMono : MonoBehaviour
{
    [ALHeader("ID")]
    public int graveTypeId;
    [ALHeader("窗口 Mono")]
    public GGUIMonoGraveMainItem itemMono;
    [ALHeader("弃用 ui跟随节点")]
    public Transform uiFollowParent;

    [ALHeader("弃用 ui跟随窗口路径ID")]
    public int uiFollowResPathId = -1;
    [ALHeader("弃用 ")]
    public bool needLoadPlayerGo = false;
    [ALHeader("弃用 ")]
    public Transform playerGoParent;

    // <AutoGen:MonoDeclaration>
    // </AutoGen:MonoDeclaration>
}
