using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴家人使用道具完成伙伴家人列表item
    /// </summary>
    public class GGUIMonoBagItemHeroConsortUseResultContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("半身像")]
        public RawImage imgIcon;
        [ALHeader("背景图")]
        public Image imgIconBgk;
        [ALHeader("属性图标")]
        public RawImage imgAttrIcon;
        [ALHeader("属性值")]
        public Text txtAddValue;
    }
}
