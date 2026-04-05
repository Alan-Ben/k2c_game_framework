using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟PVE开启副本
/// </summary>
public class GGUIMonoGuildDungeonStart : _AALBasicUIWndMono
{

    // <AutoGen:MonoDeclaration>
    [ALHeader("使用联盟财富开启")]
    public GameObject btnGuildWealth;
    [ALHeader("钻石开启")]
    public GameObject btnGem;
    [ALHeader("联盟财富")]
    public NPGGUIMonoCommonItem itemGuild;
    [ALHeader("钻石")]
    public NPGGUIMonoCommonItem itemGem;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    [ALHeader("不能使用联盟财富显示物体")]
    public List<GameObject> guildWealthShowGos;
    [ALHeader("不能使用联盟财富隐藏物体")]
    public List<GameObject> guildWealthHideGos;
    [ALHeader("不能使用联盟财富置灰物体")]
    public List<MaskableGraphic> guildWealthGrayGraphics;
    [ALHeader("不能使用钻石显示物体")]
    public List<GameObject> cantUseGemShowGos;
    [ALHeader("不能使用钻石隐藏物体")]
    public List<GameObject> cantUseGemHideGos;
    [ALHeader("不能使用钻石置灰物体")]
    public List<MaskableGraphic> cantUseGemGrayGraphics;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6901); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6901);} }
}
