using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;


public enum ETowerResearchActiveType
{
    [InspectorName("可激活")]
    CAN_ACTIVATE,
    [InspectorName("已激活")]
    HAS_ACTIVATE,
    [InspectorName("未达成")]
    NOT_ACTIVATE,
}
/// <summary>
/// item
/// </summary>
public class GGUIMonoTowerResearchLevelItem : _AALBasicUIWndMono
{
    [ALHeader("激活按钮")]
    public GameObject btnActivate;
    [ALHeader("关卡名")]
    public Text txtLevelName;

    [ALHeader("效果描述")]
    public Text txtEffectDesc;
    [ALHeader("激活动画")] 
    public Animation activateAnim;
    [ALHeader("激活动画名称")]
    public string activateAnimName;
    [ALHeader("可激活需要显示的go")]
    public List<GameObject> canActivateGoList;
    [ALHeader("已激活需要显示的go")]
    public List<GameObject> hasActivateGoList;
    [ALHeader("未达成需要显示的go")]
    public List<GameObject> notActivateGoList;
}

