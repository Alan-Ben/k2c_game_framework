using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 集市升级界面
/// </summary>
///
public class GGUIMonoMarketLvUp : _AALBasicUIWndMono
{
    [ALHeader("集市繁荣点图片")]
    public RawImage marketImg;

    [ALHeader("集市繁荣点数量")]
    public Text marketMoneyTxt;

    [ALHeader("集市繁荣点获取途径按钮")]
    public GameObject marketMoneyGainBtn;

    [ALHeader("店铺列表")]
    public GGUIMonoMarketLvUpContainer lvUpContainerMono;

    [ALHeader("天赐良机文本")]
    public Text niubiTxt;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;


    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3901); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3901); } }
}
