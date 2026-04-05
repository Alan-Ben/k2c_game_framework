using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 宴会参与结算
/// </summary>
public class GGUIMonoDinnerJoinResult : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("关闭按钮")]
    public GameObject btnClose2;
    [ALHeader("开宴玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("宴会名称")]
    public Text txtDinnerName;
    [ALHeader("宴会积分")]
    public Text txtScore;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer itemContainer;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2911); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2911);} }
}