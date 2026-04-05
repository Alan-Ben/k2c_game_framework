using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoChapterEventReward : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("确认按钮")]
    public GameObject btnConfirm;
    [ALHeader("标题")]
    public TextEx txtTitle;
    [ALHeader("描述")]
    public TextEx txtDesc;
    [ALHeader("奖励列表")]
    public GGUIMonoCommonRewardContainer itemContainer;
    [ALHeader("粒子开始位置")]
    public RectTransform particleStartRectTransform;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2111); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2111);} }
}