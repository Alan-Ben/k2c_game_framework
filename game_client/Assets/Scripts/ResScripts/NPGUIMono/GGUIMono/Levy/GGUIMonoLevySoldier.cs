using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 士兵征收
/// </summary>
public class GGUIMonoLevySoldier: _AGGUIMonoLevyBase
{
    [ALHeader("次数")]
    public Text productTxt;

    [ALHeader("剩余时间")]
    public NPGGUIMonoCommonCountDown countDownMono;

    [ALHeader("获取烘焙次数按钮")]
    public GameObject getTimesBtn;

    [ALHeader("征收按钮按钮")]
    public GameObject levyBtn;

    [ALHeader("征收消耗")]
    public NPGGUIMonoCommonItem costItemMono;

    [ALHeader("没有烘焙次数显示的GoList")]
    public List<GameObject> noCountShowGoList;

    [ALHeader("有烘焙次数显示的GoList")]
    public List<GameObject> hasCountShowGoList;

    [ALHeader("不可征收需要置灰的List")]
    public List<MaskableGraphic> grayImgList;

    [ALHeader("不可征收提示key- 次数不够")]
    public string canNotLevyNoCountShowTipKey;
    [ALHeader("不可征收提示key- 消耗不够")]
    public string canNotLevyNoCostShowTipKey;

    [ALHeader("弹出tip位置")]
    public RectTransform tipStartPos;

    [ALHeader("暴击动画")]
    public Animation bigCritAni;
    [ALHeader("暴击动画名称")]
    public string bigCritAniStr;
    [ALHeader("暴击文本")]
    public TextMeshProUGUIEx bigCritTxt;

    [ALHeader("暴击特效弹出位置")]
    public Transform critSfxParPos;
    [ALHeader("暴击特效缩放")]
    public float critSfxScale;
    [ALHeader("暴击特效飞行时间(秒)")]
    public float critSfxFlyTime = 3f;
    
    [ALHeader("显示大数字特效缩放")]
    public float bigTxtSfxScale;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1603); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1603);} }
}