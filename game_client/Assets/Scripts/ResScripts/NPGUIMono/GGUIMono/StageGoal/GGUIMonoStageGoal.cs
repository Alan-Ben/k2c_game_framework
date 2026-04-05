using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum GGUIMonoStageGoalTabType
{
    [InspectorName("OVERVIEW === 总览")]
    OVERVIEW = 0,
    [InspectorName("TASK === 当前任务")]
    TASK = 1,
    [InspectorName("PEAK === 时代之巅")]
    PEAK = 2,
}
/// <summary>
/// 阶段目标界面
/// </summary>
public class GGUIMonoStageGoal : _AALBasicUIWndMono
{
    [ALHeader("页签按钮")]
    public GGUIMonoStageGoalPageTabList monoPageTabList;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("总览界面simpleUnlockId，用于判断有新阶段解锁是否能默认进总览界面")]
    public long overviewSimpleUnlockId;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5000); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5000); } }
}