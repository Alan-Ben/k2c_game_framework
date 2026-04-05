using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟火星互助详情
/// </summary>
public class GGUIMonoGuildMarsHelp : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    public GGUIMonoGuildMarsHelpGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    [ALHeader("奖励进度")]
    public NPGGUIMonoProgress rewardProgress;
    // </AutoGen:MonoDeclaration>
    [ALHeader("帮助按钮")]
    public GameObject btnHelp;

    [ALHeader("有需要处理的帮助需要显示GO列表")]
    public List<GameObject> hasHelpToDealShowGos;

    [ALHeader("权益卡剩余时间")]
    public Text txtPrivilegeCardLeftTime;
    [ALHeader("今日自动帮助次数")]
    public Text txtAutoHelpCount;
    [ALHeader("特权提示按钮")]
    public GameObject btnPrivilegeTip;
    [ALHeader("特权提示tip的偏移量")]
    public Vector2 privilegeTipInterval;
    [ALHeader("激活特权卡时需要显示的GO列表")]
    public List<GameObject> goActivePrivilegeShowList;
    [ALHeader("激活特权卡时需要隐藏的GO列表")]
    public List<GameObject> goActivePrivilegeHideList;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(4950); } }
    public static string objName { get { return UIResPathAssistant.getObjName(4950);} }
}