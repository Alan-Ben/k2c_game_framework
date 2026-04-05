using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;


/// <summary>
/// 周卡主界面
/// </summary>
///
public class GGUIMonoWeekCard : _AALBasicUIWndMono
{
    [ALHeader("倒计时文本")]
    public Text txtTimeDown;
    [ALHeader("拥有周卡显示，否则隐藏")]
    public List<GameObject> hasWeekCardShow;
    [ALHeader("拥有周卡隐藏，否则显示")]
    public List<GameObject> hasWeekCardHide;
    [ALHeader("委派按钮")]
    public GameObject btnAssign;
    [ALHeader("免费周卡按钮")]
    public GameObject btnFreeWeekCard;
    [ALHeader("付费周卡按钮")]
    public GameObject btnCostWeekCard;
    [ALHeader("可购买免费周卡显示")]
    public List<GameObject> freeWeekCardShow;
    [ALHeader("可购买付费周卡显示")]
    public List<GameObject> costWeekCardShow;
    [ALHeader("免费订阅按钮")]
    public GameObject btnFreeSubscribe;
    [ALHeader("付费订阅按钮")]
    public GameObject btnCostSubscribe;
    [ALHeader("可购买免费订阅显示")]
    public List<GameObject> freeSubscribeShow;
    [ALHeader("可购买付费订阅显示")]
    public List<GameObject> costSubscribeShow;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(4500); } }
    public static string objName { get { return UIResPathAssistant.getObjName(4500); } }
}
