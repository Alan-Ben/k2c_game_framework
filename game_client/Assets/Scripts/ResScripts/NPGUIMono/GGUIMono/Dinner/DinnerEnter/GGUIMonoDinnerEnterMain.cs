using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会入口界面
/// </summary>
public class GGUIMonoDinnerEnterMain : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("我的宴会")]
    public GameObject btnMyDinner;
    [ALHeader("宴会参加按钮")]
    public GameObject btnJoinDinner;
    [ALHeader("宴会排行榜按钮")]
    public GameObject btnRank;
    [ALHeader("宴会记录按钮")]
    public GameObject btnRecord;
    [ALHeader("我的宴会开启状态下展示Go")]
    public List<GameObject> myDinnerOpenShowList;
    [ALHeader("我的宴会开启状态下隐藏Go")]
    public List<GameObject> myDinnerOpenHideList;
    [ALHeader("我的宴会名称")]
    public TextEx txtMyDinnerName;
    [ALHeader("我的宴会座椅数量")]
    public TextEx txtMyDinnerSeat;
    [ALHeader("我的宴会倒计时")]
    public TextEx txtMyDinnerCd;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2901); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2901);} }
}