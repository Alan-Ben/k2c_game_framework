using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoEarningGoalGlobal : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    public GGUIMonoEarningGoalGlobalItemGrid globalItemGrid;
    public GGUIMonoEarningGoalHonorItemContainer honorItemContainer;
    [ALHeader("全民奖励")]
    public NPGGUIMonoCommonTab tabGlobal;
    [ALHeader("荣誉奖励")]
    public NPGGUIMonoCommonTab tabHonor;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5701); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5701);} }
}