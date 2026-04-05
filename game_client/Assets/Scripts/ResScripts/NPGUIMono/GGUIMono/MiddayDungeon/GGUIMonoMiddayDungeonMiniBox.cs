using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoMiddayDungeonMiniBox : _AALBasicUIWndMono
{
    [ALHeader("打开宝箱列表按钮")]
    public GameObject btnShowBox;
    [ALHeader("变更宝箱信息的时间")]
    public float showNextBoxInfoTime = 2;
    [ALHeader("宝箱信息文本")]
    public Text txtBoxInfo;
}