using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


// 列表项数据
public enum GuildDungeonHeroState
{
    CanFight,   // 可出战
    Used,       // 已出战
    CanRecover  // 可恢复
}
/// <summary>
/// 联盟副本日志
/// </summary>
public class GGUIMonoGuildDungeonSelectHeroGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("选中时的显列表")]
    public List<GameObject> listSelectShow;
    [ALHeader("选中时的隐列表")]
    public List<GameObject> listSelectHide;
    
    [ALHeader("不可用时的显示列表")]
    public List<GameObject> listInValidShow;
    [ALHeader("可恢复的显示列表")]
    public List<GameObject> listCanRecoverShow;
    [ALHeader("可出战的显示列表")]
    public List<GameObject> listCanFightShow;
    [ALHeader("大臣信息")]
    public GGUIMonoHeroCommonCardItem heroCard;
    // </AutoGen:MonoDeclaration>
    
    [ALHeader("恢复按钮")]
    public GameObject btnRecover; // 恢复按钮
    [ALHeader("选中按钮")]
    public GameObject btnSelect;
}

