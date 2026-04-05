using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟副本日志
/// </summary>
public class GGUIMonoGuildDungeonSelectHero : _AALBasicUIWndMono
{
    public GGUIMonoGuildDungeonSelectHeroGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    // </AutoGen:MonoDeclaration>
    [ALHeader("当前选择大臣信息")] 
    public GGUIMonoHeroIconNullableItem curSelectedHero;
    [ALHeader("确认按钮")]
    public GameObject btnConfirm;
    [ALHeader("恢复按钮")]
    public GameObject btnRecover;
    [ALHeader("大臣不可用按钮")]
    public GameObject btnInValid;
    [ALHeader("取消按钮")]
    public GameObject btnCancel;
    [ALHeader("剩余总战力")]
    public Text txtLeftTotalPower;
    
    [ALHeader("不可用时的显示列表")]
    public List<GameObject> listInValidShow;
    [ALHeader("可恢复的显示列表")]
    public List<GameObject> listCanRecoverShow;
    [ALHeader("可出战的显示列表")]
    public List<GameObject> listCanFightShow;
    
    [ALHeader("消耗道具mono")]
    public NPGGUIMonoCommonItem recoverCostItem;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6906); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6906);} }
}