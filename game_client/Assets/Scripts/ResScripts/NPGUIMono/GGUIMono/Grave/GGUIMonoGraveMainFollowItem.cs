using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 杰出者大厅主界面跟随窗口
/// </summary>
public class GGUIMonoGraveMainFollowItem : ALGGUIMonoCommonFollowItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("有杰出者时需要显示的Gos")]
    public List<GameObject> hasPlayerShowGos;
    [ALHeader("无杰出者时需要显示的Gos")]
    public List<GameObject> noPlayerShowGos;
    [ALHeader("杰出者类型名")]
    public Text txtName;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("杰出者详情按钮")]
    public GameObject btnDetail;
    [ALHeader("称号图标")]
    public RawImage titleIcon;
    // </AutoGen:MonoDeclaration>
}