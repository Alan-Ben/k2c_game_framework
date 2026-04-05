using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用奖励预览界面
/// </summary>
public class NPGGUIMonoCommonRewardPreview : _AALBasicUIWndMono
{
    [ALHeader("宝箱图标")]
    public RawImage imgBoxIcon;
    [ALHeader("标题")]
    public TextEx txtTitle;
    [ALHeader("描述")]
    public TextEx txtContentDesc;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("奖励列表")]
    public GGUIMonoCommonRewardContainer itemContainer;
    
}