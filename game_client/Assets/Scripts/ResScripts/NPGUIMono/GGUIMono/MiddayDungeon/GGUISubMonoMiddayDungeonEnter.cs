using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 午间活动入口子窗口
/// </summary>
public class GGUISubMonoMiddayDungeonEnter : _AALBasicUIWndMono
{
    [ALHeader("前往战斗按钮")]
    public GameObject btnBattle;
    [ALHeader("午间副本活动持续时间")]
    public TextEx txtMiddayDungeonDurationTime;
    [ALHeader("Mini宝箱窗口")]
    public GGUIMonoMiddayDungeonMiniBox monoMiniBox;
    [ALHeader("宝箱窗口")]
    public GGUIMonoMiddayDungeonBox monoBox;
    [ALHeader("副本开启倒计时")]
    public Text txtOpenCd;
    [ALHeader("副本结束倒计时")]
    public Text txtEndCd;
    [ALHeader("活动在准备中要显示的go")]
    public List<GameObject> goShowInPrepare;
    [ALHeader("活动进行中要显示的go")]
    public List<GameObject> goShowInBattle;
}