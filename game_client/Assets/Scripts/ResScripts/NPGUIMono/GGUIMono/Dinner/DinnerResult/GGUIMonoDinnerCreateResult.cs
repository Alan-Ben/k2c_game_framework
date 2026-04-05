using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 举办宴会结算
/// </summary>
public class GGUIMonoDinnerCreateResult : _AALBasicUIWndMono
{
    [ALHeader("确认按钮")]
    public GameObject btnClose;
    [ALHeader("参会者详情按钮")]
    public GameObject btnJoinerDetail;
    [ALHeader("宴会人气")]
    public TextEx txtDinnerScore;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer itemContainer;
    [ALHeader("参宴列表")]
    public GGUIMonoDinnerCreateResultJoinItemContainer joinItemContainer;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2904); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2904);} }
}