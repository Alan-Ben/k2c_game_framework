using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoEarningGoalSelf : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("步骤列表")]
    public GGUIMonoAchieveStepGrid monoStepGrid;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5704); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5704);} }
}