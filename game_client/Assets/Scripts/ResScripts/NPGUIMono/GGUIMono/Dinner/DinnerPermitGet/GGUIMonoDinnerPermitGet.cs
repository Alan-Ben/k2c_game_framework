using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会凭证获得界面
/// </summary>
public class GGUIMonoDinnerPermitGet : _AALBasicUIWndMono
{
    [ALHeader("前往举办宴会按钮")]
    public GameObject btnGo;
    [ALHeader("席位人数")]
    public TextEx txtSeatCount;
    [ALHeader("宴会名字")]
    public TextEx txtName;
    [ALHeader("宴会描述")]
    public TextEx txtDesc;
    [ALHeader("凭证倒计时")]
    public TextEx txtLifeTime;
    [ALHeader("妃子卡片形象")]
    public GGUIMonoConsortIconItem consortCardItem;
    [ALHeader("妃子开宴凭证显示Go")]
    public List<GameObject> familyPermitShowGos; 
    [ALHeader("妃子开宴凭证隐藏Go")]
    public List<GameObject> familyPermitHideGos; 
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2922); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2922);} }
}