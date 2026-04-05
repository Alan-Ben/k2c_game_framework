using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对情人使用道具
    /// </summary>
    public class GGUIMonoBagItemUseConsort : _AALBasicUIWndMono
    {
        [ALHeader("物品")]
        public NPGGUIMonoCommonItem monoCommonItem;
        [ALHeader("增加的属性类型图标")]
        public RawImage imgTypeIcon;
        [ALHeader("使用道具增加的值")]
        public Text txtAddValue;
        [ALHeader("情人列表")]
        public GGUIMonoBagItemUseConsortGrid monoHeroGrid;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1921); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1921); } }
    }
}
