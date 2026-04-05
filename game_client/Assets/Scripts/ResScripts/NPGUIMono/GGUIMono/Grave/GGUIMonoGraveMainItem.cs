using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// 杰出者主界面item
/// </summary>
public class GGUIMonoGraveMainItem : _AALBasicUIWndMono
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("有杰出者时需要显示的Gos")]
    public List<GameObject> hasPlayerShowGos;
    [ALHeader("无杰出者时需要显示的Gos")]
    public List<GameObject> noPlayerShowGos;
    [ALHeader("杰出者类型名")]
    public Text txtName;
    [ALHeader("杰出者类型名TextMeshPro")]
    public TextMeshProUGUI txtMeshProName;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("杰出者详情按钮")]
    public GameObject btnDetail;
    [ALHeader("称号图标")]
    public RawImage titleIcon;
    [ALHeader("点击脚本")]
    public GTDCommonPosClickMono clickMono;
    
    // </AutoGen:MonoDeclaration>
    
}