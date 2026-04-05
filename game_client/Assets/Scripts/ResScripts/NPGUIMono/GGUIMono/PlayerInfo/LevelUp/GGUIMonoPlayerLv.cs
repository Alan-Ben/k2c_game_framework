using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

// 玩家等级
public class GGUIMonoPlayerLv : _AALBasicUIWndMono
{
    [ALHeader("等级图片")]
    public RawImage lvImg;

    [ALHeader("等级数值（带Lv.）")]
    public Text lvTxt;

    [ALHeader("等级数值（纯数字）")]
    public Text lvNumTxt;

    [ALHeader("等级名称")]
    public Text lvNameTxt;

    [ALHeader("等级数值和名称")]
    public Text lvNumWithNameTxt;

    [ALHeader("说明按钮")]
    public GameObject explainBtn;
}
