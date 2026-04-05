using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum ESevenDayLoginBtnState
{
    [InspectorName("未达成")]
    None = 2,
    [InspectorName("可领取")]
    CanGet = 0,
    [InspectorName("已领取")]
    HasGet = 1,
    [InspectorName("明日可领取")]
    NextGet = 3,
}

/// <summary>
/// 七日登录
/// </summary>
public class GGUIMonoSevenDayLogin : _AALBasicUIWndMono
{
    [ALHeader("倒计时")]
    public Text txtCD;
    [ALHeader("有倒计时时需要显示的GO")]
    public List<GameObject> hasCDShowGos = new List<GameObject>();
    [ALHeader("有倒计时时需要隐藏的GO")]
    public List<GameObject> hasCDHidewGos = new List<GameObject>();
    public List<SevenDayLoginBtnMono> btnList = new List<SevenDayLoginBtnMono>();
    
    [ALHeader("7天奖励达成并领取完成后，需要显示的Go")]
    public List<GameObject> hasGetSevenDayRewardShowList = new List<GameObject>();
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5600); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5600);} }
}