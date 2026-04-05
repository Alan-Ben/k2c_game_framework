using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

//提示文字窗口
public class NPGGUIMonoCenterTipItem : _AALBasicUIWndMono{
    public Animation anim;//动画
    public Text text;// 提示信息

    //无效的时间间隔
    public float disableTime;
}

