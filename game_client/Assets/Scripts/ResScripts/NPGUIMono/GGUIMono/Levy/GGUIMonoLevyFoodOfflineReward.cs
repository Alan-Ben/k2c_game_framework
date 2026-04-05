using GOE;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 征收离线奖励弹窗
/// </summary>
///
public class GGUIMonoLevyFoodOfflineReward: _ANPBasicUIWndResBarMono
{
    [ALHeader("离线时长")]
    public Text offlineTimeTxt;

    [ALHeader("离线时长上限")]
    public Text offlineTimeMaxTxt;

    [ALHeader("离线奖励数量")]
    public Text offlineCountTxt;

    [ALHeader("执政官形象")]
    public GGUIMonoCommonShowCase assignShowcase;

    [ALHeader("自动关闭窗口延迟时间, 只有数值大于0时生效")]
    public float autoCloseWndDelay;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1612); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1612); } }
}
