using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoChapterEventDispatch : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("派遣按钮")]
    public GameObject btnDispatch;
    [ALHeader("一键派遣按钮")]
    public GameObject btnAutoDispatch;
    [ALHeader("标题")]
    public TextEx txtTitle;
    [ALHeader("描述")]
    public TextEx txtDesc;
    [ALHeader("条件Container")]
    public GGUIMonoChapterEventDispatchCondItemContainer monoCondContainer;
    [ALHeader("条件达成状态")]
    public GGUIMonoChapterEventDispatchCondStateItemContainer monoCondStateContainer;
    [ALHeader("已选择大臣列表")]
    public GGUIMonoHeroIconNullableItemContainer monoSelectedHeroContainer;
    [ALHeader("大臣选择")]
    public GGUIMonoHeroCommonSelect monoHeroSelectWnd;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2114); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2114);} }
}