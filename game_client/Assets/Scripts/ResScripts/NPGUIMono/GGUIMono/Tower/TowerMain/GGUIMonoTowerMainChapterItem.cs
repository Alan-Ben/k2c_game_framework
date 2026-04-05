using System.Collections.Generic;
using ALPackage;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerMainChapterItem : _AALBasicUIWndMono
{
    [ALHeader("章节Id")]
    public long chapterId;
    [ALHeader("章节文本")]
    public TextEx txtName;
    [ALHeader("章节level")]
    public TextEx txtLevel;
    [ALHeader("点击按钮")]
    public GameObject btnClick;
    [ALHeader("是当前章节需要显示的Go")]
    public List<GameObject> curChapterShowGos;
    [ALHeader("下一个挖掘点需要显示的Go")]
    public List<GameObject> nextFightPointShowGos;
    [ALHeader("已通关的章节需要显示的Go")]
    public List<GameObject> alreadyCompleteShowGos;
    [ALHeader("章节解锁动画")]
    public Animation unlockAnimation;
    [ALHeader("解锁动画名")]
    public string unlockAnimName = "Unlock";
    [ALHeader("未解锁，但待挑战的状态(每章最后一关才有的状态)")]
    public string lockAndNextFightAnimName = "Unlock";

    
    [ALHeader("挑战按钮")]
    public GameObject btnChallenge;
    [ALHeader("章节详情显隐的Go")]
    public List<GameObject> chapterDetailShowGos;
}