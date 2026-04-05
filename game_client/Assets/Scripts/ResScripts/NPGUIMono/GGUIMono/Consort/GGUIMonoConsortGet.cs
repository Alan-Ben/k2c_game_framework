using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 妃子获得
/// </summary>
public class GGUIMonoConsortGet:_AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    
    [ALHeader("妃子详情信息子窗口")]
    public GGUISubMonoUnlockConsortDetailInfo monoConsortDetailInfo;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1417); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1417);} }
}