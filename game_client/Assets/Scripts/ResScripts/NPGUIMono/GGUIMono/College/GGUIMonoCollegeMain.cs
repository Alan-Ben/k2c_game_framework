using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 大学主页面
/// </summary>
///
public class GGUIMonoCollegeMain : _AALBasicUIWndMono
{
    [ALHeader("进修名额文本")]
    public Text learnNumTxt;

    [ALHeader("进修名额扩充按钮")]
    public GameObject addLearnNumBtn;

    [ALHeader("进修名额满了需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;

    [ALHeader("一键学习显示的GoList")]
    public List<GameObject> onceLearnShowGoList;

    [ALHeader("一键完成显示的GoList")]
    public List<GameObject> onceFinishShowGoList;

    [ALHeader("一键按钮")]
    public GameObject onceBtn;

    [ALHeader("解锁位置的音效id")]
    public long addPosAudioId;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1801); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1801); } }
}
