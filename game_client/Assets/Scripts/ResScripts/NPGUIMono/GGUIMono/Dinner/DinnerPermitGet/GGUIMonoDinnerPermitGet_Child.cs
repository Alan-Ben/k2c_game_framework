using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会凭证获得界面（子嗣庆功宴）
/// </summary>
public class GGUIMonoDinnerPermitGet_Child : _AALBasicUIWndMono
{
    [ALHeader("前往举办宴会按钮")]
    public GameObject btnGo;
    [ALHeader("席位人数")]
    public TextEx txtSeatCount;
    [ALHeader("宴会名字")]
    public TextEx txtName;
    [ALHeader("宴会描述")]
    public TextEx txtDesc;
    [ALHeader("凭证倒计时")]
    public TextEx txtLifeTime;
    [ALHeader("子嗣形象")]
    public GGUIMonoChildInfo childCardItem;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2927); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2927); } }
}
