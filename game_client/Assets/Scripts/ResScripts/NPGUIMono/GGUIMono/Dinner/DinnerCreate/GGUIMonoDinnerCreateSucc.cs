using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会举办成功弹窗
/// </summary>
public class GGUIMonoDinnerCreateSucc : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("宴会类型显示")]
    public List<DinnerTypeShow> dinnerTypeShowList;
    [ALHeader("妃子卡片形象")]
    public GGUIMonoConsortIconItem consortCardItem;
    [ALHeader("子嗣卡片形象")]
    public GGUIMonoChildInfo childCardItem;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2926); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2926);} }
}