using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using CommonEnum;

/// <summary>
/// 征收银币
/// </summary>
public class GGUIMonoLevySilver: _AGGUIMonoLevyBase
{
    [ALHeader("粮食累计文本")]
    public Text productTxt;

    [ALHeader("剩余时间")]
    public NPGGUIMonoCommonCountDown countDownMono;

    [ALHeader("征收按钮按钮")]
    public GameObject levyBtn;

    [ALHeader("征收弹出tip位置")]
    public RectTransform tipStartPos;

    [ALHeader("不可征收需要置灰的List")]
    public List<MaskableGraphic> grayImgList;

    [ALHeader("最低可领取数量绝对值")]
    public float canLevyMinCount;
    
    [ALHeader("不可征收提示key")]
    public string canNotLevyShowTipKey;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1602); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1602);} }
}