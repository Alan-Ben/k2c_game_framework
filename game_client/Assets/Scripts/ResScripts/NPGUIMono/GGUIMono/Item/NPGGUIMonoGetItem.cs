using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public class NPGGUIMonoGetItem : _AALBasicUIWndMono
{
    [ALHeader("标题文本")]
    public TextMeshProUGUIEx titleTxt;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("物品列表")]
    public NPGGUIMonoGetItemContainer monoItemContainer;

    [ALHeader("外层窗体")]
    [ALInfo("根据获得奖励的个数，调整物品容器和外层窗体大小，additionHeight是 外层窗体 相对 物品容器 的额外高度")]
    public RectTransform wndRect;
    [ALHeader("需要根据item数量自适应大小的RectTransform")]
    public RectTransform contentRectTrans;
    [ALHeader("最大行数")]
    public int maxLine = 5;
    [ALHeader("外层附加高度")]
    public float additionHeight = 200;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_GET_ITEM); } }
    public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_GET_ITEM); } }
}
