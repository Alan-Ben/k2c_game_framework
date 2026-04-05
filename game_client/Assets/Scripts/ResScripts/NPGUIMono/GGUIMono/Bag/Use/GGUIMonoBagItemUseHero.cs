using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对伙伴使用道具
    /// </summary>
    public class GGUIMonoBagItemUseHero : _AALBasicUIWndMono
    {
        [ALHeader("物品")]
        public NPGGUIMonoCommonItem monoCommonItem;
        [ALHeader("增加的属性类型图标")]
        public RawImage imgTypeIcon;
        [ALHeader("使用道具增加的值")]
        public Text txtAddValue;
        [ALHeader("骑士列表")]
        public GGUIMonoBagItemUseHeroGrid monoHeroGrid;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1915); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1915); } }
    }
}
