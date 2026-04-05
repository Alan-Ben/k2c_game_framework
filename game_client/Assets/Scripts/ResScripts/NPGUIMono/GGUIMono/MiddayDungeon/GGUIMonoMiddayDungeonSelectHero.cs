using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


public enum EMiddayDungeonSelectHeroType
{
    Self = 0,
    Guild = 1,
}
/// <summary>
/// 
/// </summary>
public class GGUIMonoMiddayDungeonSelectHero : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("确认按钮")]
    public GameObject btnConfirm;
    [ALHeader("当前选择大臣信息")] 
    public GGUIMonoHeroIconNullableItem curSelectedHero;
    [ALHeader("大臣选择")]
    public GGUIMonoMiddayDungeonHeroSelectItemGrid monoHeroSelectWnd;
    
    [ALHeader("我的大臣页签")]
    public NPGGUIMonoCommonTab tabMyHero;
    [ALHeader("联盟大臣页签")]
    public NPGGUIMonoCommonTab tabGuildHero;
    [ALHeader("联盟剩余协助次数")]
    public TextEx txtGuildAssistCount;

    [ALHeader("我的大臣页签会显示的go")]
    public List<GameObject> goShowInMyHero;
    [ALHeader("联盟大臣页签会显示的go")]
    public List<GameObject> goShowInGuildHero;

    [ALHeader("联盟大臣页签 未加入联盟时显示的GO列表")]
    public List<GameObject> goNoJoinGuildShowList;
    [ALHeader("联盟大臣页签 未加入联盟时隐藏的GO列表")]
    public List<GameObject> goNoJoinGuildHideList;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5405); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5405);} }
}