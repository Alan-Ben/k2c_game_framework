using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;
using CommonEnum;

/// <summary>
/// 剧情提示窗口
/// </summary>
public class GGUIMonoChapterDialogTip : _AALBasicUIWndMono
{
    [ALHeader("图片")]
    public RawImage texIcon;
    [ALHeader("名字")]
    public Text txtName;
    
    [ALHeader("自动关闭时间")] 
    public float autoCloseTime;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2104); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2104); } }
}