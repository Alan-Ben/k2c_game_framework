using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum EDinnerListStatType
{
    [InspectorName("NORMAL：常规宴会列表状态")]
    NORMAL,
    [InspectorName("FRIEND：好友宴会列表状态")]
    FRIEND,
    [InspectorName("GUILD：公会宴会列表状态")]
    GUILD,
}


/// <summary>
/// 宴会列表
/// </summary>
public class GGUIMonoDinnerListMain : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("每次刷新显示的宴会数量")]
    public int perPageItemNum = 6;
    [ALHeader("邀请服务器推荐玩家")]
    public NPGGUIMonoCommonTab tabServer;
    [ALHeader("邀请好友")]
    public NPGGUIMonoCommonTab tabFriend;
    [ALHeader("邀请联盟玩家")]
    public NPGGUIMonoCommonTab tabGuild;
    [ALHeader("宴会列表")]
    public GGUIMonoDinnerItemGrid dinnerItemGrid;
    [ALHeader("列表显示状态")]
    public List<NPCommonEnumStatInfo<EDinnerListStatType>> statInfos;
    [ALHeader("列表为空时显示的物体")]
    public List<GameObject> emptyShowGos;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2918); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2918);} }
}