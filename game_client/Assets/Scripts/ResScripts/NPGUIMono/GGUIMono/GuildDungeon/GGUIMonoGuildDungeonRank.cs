using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟副本排行
/// </summary>
public class GGUIMonoGuildDungeonRank : _AALBasicUIWndMono
{
    public GGUIMonoGuildDungeonRankGrid itemGrid;
    [ALHeader("每次刷新列表显示的数量")]
    public int perPageItemNum = 6;
    [ALHeader("列表为空时显示的物体")]
    public List<GameObject> emptyShowGos;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("我的信息")]
    public NPGGUIMonoPlayerIcon selfPlayerInfo;
    [ALHeader("我的积分")]
    public Text txtScore;
    // </AutoGen:MonoDeclaration>
    [ALHeader("我的排名")]
    public Text txtRank;
    [ALHeader("自己没有上榜时显示的GO列表")]
    public List<GameObject> goSelfNotInRankShowList;
    [ALHeader("自己没有上榜时隐藏的GO列表")]
    public List<GameObject> goSelfNotInRankHideList;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6910); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6910);} }
}