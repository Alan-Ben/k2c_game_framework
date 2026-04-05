using ALPackage;
using GOE;
using UnityEngine.UI;

/// <summary>
/// 物品点击查看详情弹窗
/// </summary>
public class NPGGUIMonoCommonItemDetail : _AALBasicUIWndMono
{
    [ALHeader("物品图标")]
    public RawImage imgItemIcon;

    [ALHeader("物品品质底图")]
    public Image imgQualityBg;

    [ALHeader("物品名称")]
    public Text txtItemName;

    [ALHeader("物品持有数量文本")]
    public Text txtNum;

    [ALHeader("物品产出文本")] 
    public Text txtAccess;

    [ALHeader("物品详细描述文本")] 
    public Text txtItemDesc;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_RESOURCES_TIP); } }
    public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_RESOURCES_TIP); } }
}
